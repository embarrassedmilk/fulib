using System;
using System.Collections.Generic;

namespace func
{
    class Program
    {
        static void Main(string[] args)
        {
            var goodList = new List<string>() {
                "http://google.com",
                "http://bbc.co.uk",
                "http://fsharp.org",
                "http://fsharpforfunandprofit.com"
            };

            var badList = new List<string>() {
                "http://google.com",
                "http://bbc1.co.uk",
                "http://fshar1p.org",
                "http://fsha1rpforfunandprofit.com"
            };

            var result = Traverser.GetMaxLengthOfWebsitesContentM(goodList).Result;
            
            if (result.IsSuccess) {
                Console.WriteLine($"Max content size is {result.Value}");
            }
            else {
                Console.WriteLine("Errors: ");
                Console.WriteLine(string.Join(" | ", result.Errors));
            }
        }
    }
}
