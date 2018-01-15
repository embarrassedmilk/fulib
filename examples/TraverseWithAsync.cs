using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LanguageExt;

namespace func {
    public class TraverserAsync {
        private static Async<Result<string>> GetUriContent(Uri uri) {
            return new Async<Result<string>>(async () => {
                using (var client = new WebClientWithTimeout(1000)) {
                    try {
                        Console.WriteLine($"Started... {uri.Host}");
                        var html = await client.DownloadStringTaskAsync(uri);
                        Console.WriteLine($"Finished... {uri.Host}");
                        return Result<string>.Success(html);
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Exception... {uri.Host}");
                        var error = $"[{uri.Host}] {ex.Message}";
                        return Result<string>.Failure(new string[] {error});
                    }
                }
            });
        }

        private static Result<int> MakeContentSize(string html) {
            if (string.IsNullOrEmpty(html)) {
                return Result<int>.Failure(new []{ "empty page" });
            }

            return Result<int>.Success(html.Length);
        }

        private static Async<Result<int>> GetUriContentSize(Uri uri) => GetUriContent(uri).Map(tr => tr.Bind(r => MakeContentSize(r)));

        public static Async<Result<int>> GetMaxLengthOfWebsitesContentM(List<string> list)
        {
            return list
                .Map(s => new Uri(s))
                .Map(u => GetUriContentSize(u))
                .SequenceAsyncResult()
                .MapAR(tr => tr.Max());
        }
    }
}