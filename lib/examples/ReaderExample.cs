using System;

namespace func {
    public class Api : IDisposable
    {
        public Api() {
            
        }

        public Result<T> Get<T>(string key) {
            throw new NotImplementedException();
        }
        
        public void Open() {
            Console.WriteLine("[API] Opening...");
        }

        public void Close() {
            Console.WriteLine("[API] Closing...");
        }

        public void Dispose()
        {
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