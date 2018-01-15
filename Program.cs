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
                "http://google1.com",
                "http://b1bc.co.uk",
                "http://fsh1arp.org",
                "http://fsha1rpforfunandprofit.com"
            };

            // var result = Traverser.GetMaxLengthOfWebsitesContentA(goodList).Result;

            var task = TraverserAsync.GetMaxLengthOfWebsitesContentM(badList);
            var result = task.Run().Result;
            //task.Wait();

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
