using System;
using System.Collections.Generic;

namespace func {
    public class ApiClient : IDisposable
    {
        private readonly Dictionary<string, object> _map;

        public ApiClient() {
            _map = new Dictionary<string, object>();
            Open();
        }

        public Result<T> Get<T>(string key) {
            Console.WriteLine($"[API] Getting {key}...");
            
            if (!_map.ContainsKey(key)) {
                return Result<T>.Failure($"{key} does not exist");
            }
            
            var val = _map[key];
            if (!(val is T)) {
                return Result<T>.Failure($"Value under {key} type {val.GetType().Name}");
            }

            return ((T)val).AsResult();
        }

        public Result<Unit> Set(string key, object value) {
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
}