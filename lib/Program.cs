using System;
using System.Linq;
using System.Collections.Generic;

namespace func
{
    class Program
    {
        static void Main(string[] args)
        {
            ReaderExample.TestWithCustomer("CX");

            // var goodList = new List<string>() {
            //     "http://google.com",
            //     "http://bbc.co.uk",
            //     "http://fsharp.org",
            //     "http://fsharpforfunandprofit.com"
            // };

            // var badList = new List<string>() {
            //     "http://google.com",
            //     "http://bbc1.co.uk",
            //     "http://fshar1p.org",
            //     "http://fsha1rpforfunandprofit.com"
            // };

            // var result = Traverser.GetMaxLengthOfWebsitesContentA(badList).Result;
            
            // result.Match(
            //     Succ: val => {
            //         Console.WriteLine($"Max content size is {val}");
            //         return val.AsResult();
            //     },
            //     Fail: errs => {
            //         Console.WriteLine("Errors: ");
            //         Console.WriteLine(string.Join(" | ", errs.Select(x=>x.Message)));
            //         return Result<int>.Failure(errs);
            //     }
            // );
        }
    }
}
