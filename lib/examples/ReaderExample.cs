using System;
using System.Linq;
using System.Collections.Generic;

namespace func {
    public static class ApiClientReaderExtensions {
        public static Result<T> Execute<T>(this Reader<ApiClient, Result<T>> reader) {
            using (var client = new ApiClient()) {
                return reader.Run(client);
            }
        }
    }

    public class ApiClient : IDisposable
    {
        private readonly Dictionary<string, object> _map;

        public ApiClient() {
            _map = new Dictionary<string, object>();
            Open();
        }

        public Result<T> Get<T>(string key) where T: class {
            Console.WriteLine($"[API] Getting {key}...");
            
            if (!_map.ContainsKey(key)) {
                return Result<T>.Failure($"{key} does not exist");
            }
            
            var val = _map[key];
            if ((val as T) == null) {
                return Result<T>.Failure($"Value under {key} type {val.GetType().Name}");
            }

            return ((T)val).AsResult();
        }

        public Result<Unit> Set(string key, object value) {
            Console.WriteLine($"[API] Setting {key} and {value}");
            if (_map.ContainsKey(key)) {
                return Result<Unit>.Failure($"{key} already exists.");
            }

            _map.Add(key, value);

            return Unit.Default.AsResult();
        }
        
        public void Open() {
            Console.WriteLine("[API] Opening...");
        }

        public void Close() {
            Console.WriteLine("[API] Closing...");
        }

        public void Dispose()
        {
            Close();
            Console.WriteLine("[API] Disposing...");
        }
    }

    public class CustomerId {
        string Value { get; }
        public CustomerId (string value) { Value = value; }

        public static implicit operator string (CustomerId c) => c.Value;
        public static implicit operator CustomerId (string s) => new CustomerId (s);

        public override string ToString () => Value;
    }

    public class ProductId {
        string Value { get; }
        public ProductId (string value) { Value = value; }

        public static implicit operator string (ProductId c) => c.Value;
        public static implicit operator ProductId (string s) => new ProductId (s);

        public override string ToString () => Value;
    }

    public class ProductInfo {
        public string ProductName { get; }
        public ProductInfo (string productName) {
            ProductName = productName;
        }
    }

    public class ReaderExample {
        private static Reader<ApiClient, Result<IEnumerable<ProductId>>> GetProductIds(CustomerId id)
            => api
            => api.Get<IEnumerable<ProductId>>(id);

        private static Reader<ApiClient, Result<ProductInfo>> GetProductInfo(ProductId id)
            => api
            => api.Get<ProductInfo>(id);

        private static Reader<ApiClient, Result<Unit>> SetupTestData()
            => api
            => api.Set("C1", new [] { (ProductId)"P1", (ProductId)"P2" })
                .Bind(_ => api.Set("CX", new [] { (ProductId)"PX", (ProductId)"P2" }))
                .Bind(_ => api.Set("P1", new ProductInfo("P1-name")))
                .Bind(_ => api.Set("P2", new ProductInfo("P2-name")));

        public static Result<Unit> TestWithCustomer(CustomerId customerId) =>
            SetupTestData()
                .Bind(_ => GetProductIds(customerId))
                .ThenTraverseApplicativeWithLogs(GetProductInfo, errs => Console.WriteLine($"[SKIPPED]: {string.Join(", ", errs.Select(e => e.Message))}"))
                .Execute()
                .Match(
                    Succ: val => {
                        Console.WriteLine($"[SUCCESS]: Products: {string.Join(", ", val.Select(p => p.ProductName))}");
                        return Unit.Default.AsResult();
                    },
                    Fail: errs => {
                        Console.WriteLine($"[FAILURE]: {string.Join(", ", errs.Select(e => e.Message))}");
                        return Result<Unit>.Failure(errs);
                    }
                );
    }
}