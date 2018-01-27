using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LanguageExt;

namespace func {
    internal class WebClientWithTimeout : WebClient {
        private readonly int _timeout;

        public WebClientWithTimeout(int timeout) {
            _timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address) {
            var result = base.GetWebRequest(address);
            result.Timeout = _timeout;
            return result;
        }
    }
    
    public class Traverser {
        private async static Task<Result<string>> GetUriContent(Uri uri) {
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
        }

        private static Result<int> MakeContentSize(string html) {
            if (string.IsNullOrEmpty(html)) {
                return Result<int>.Failure(new []{ "empty page" });
            }

            return Result<int>.Success(html.Length);
        }

        private static Task<Result<int>> GetUriContentSize(Uri uri) => GetUriContent(uri).Map(tr => tr.Bind(r => MakeContentSize(r)));

        public static Task<Result<int>> GetMaxLengthOfWebsitesContentA(List<string> list) =>
            list
                .Map(s => new Uri(s))
                .TraverseTaskResultA(u => GetUriContentSize(u))
                .Map(t => t.Map(r => r.Max()));

        public static Task<Result<int>> GetMaxLengthOfWebsitesContentM(List<string> list)
        {
            return list
                .Map(s => new Uri(s))
                .TraverseTaskResultM(u => GetUriContentSize(u))
                .MapLocal(tr => tr.Max());
        }
    }
}