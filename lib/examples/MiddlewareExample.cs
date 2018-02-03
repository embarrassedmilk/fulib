using System;
using System.Diagnostics;

namespace func {
    public class Connection {
        public Transaction BeginTransaction() => new Transaction();

        public int Execute(string sql, object param, Transaction tr = null) => 1;
    }
    
    public class Transaction : IDisposable
    {
        public void Dispose()
        {            
        }

        public void Commit() {

        }
    }

    public class DatabaseHelper {
        public static R Connect<R>(string connString, Func<Connection, R> f) {
            return f(new Connection()); 
        }

        public static R Transact<R>(Connection conn, Func<Transaction, R> f)
        {
            R r = default(R);
            using (var tran = conn.BeginTransaction())
            {
                r = f(tran);
                tran.Commit();
            }
            return r;
        }
    }

    public class Logger {
        public void Log(string message) {
            Console.WriteLine(message);
        }
    }

    public static class Instrumentation
    {
        public static T Time<T>(Logger log, string op, Func<T> f)
        {
            var sw = new Stopwatch();
            sw.Start();
            T t = f();
            sw.Stop();
            log.Log($"{op} took {sw.ElapsedMilliseconds}ms");
            return t;
        }
    }

    public class Orders {
        private readonly string _connectionString;
        private readonly Logger _logger;
        private string _deleteLines = "DELETE OrderLines WHERE OrderId = @Id";
        private string _deleteOrder = "DELETE Orders WHERE OrderId = @Id";

        public Orders(string connectionString)
        {
            _logger = new Logger();
            _connectionString = connectionString;
        }

        private Middleware<Unit> Time(string msg) 
            => f => Instrumentation.Time(_logger, msg, f.ToNullary());

        private Middleware<Connection> Connect
            => f => DatabaseHelper.Connect(_connectionString, f);

        private Middleware<Transaction> Transact(Connection conn)
            => f => DatabaseHelper.Transact(conn, f);

        public void DeleteOrder(Guid id)
            => DeleteOrder(new { Id = id }).Run();

        private Middleware<int> DeleteOrder(object param) =>
            from _ in Time("Deleting...")
            from conn in Connect
            from trans in Transact(conn)
            select conn.Execute(_deleteLines, param, trans) 
                    + conn.Execute(_deleteOrder, param, trans);
    }
}