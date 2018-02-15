using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fulib.Examples
{
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
                    return Result<string>.Failure(error);
                }
            }
        }

        private static Result<int> MakeContentSize(string html) {
            if (string.IsNullOrEmpty(html)) {
                return Result<int>.Failure("content is empty");
            }

            return Result<int>.Success(html.Length);
        }

        private static Task<Result<int>> GetUriContentSize(Uri uri) => GetUriContent(uri).Then(c => MakeContentSize(c));

        public static Task<Result<int>> GetMaxLengthOfWebsitesContentA(List<string> list) => list
                .TraverseTaskResultA(u => GetUriContentSize(new Uri(u)))
                .MapTaskResult(t => t.Max());

        public static Task<Result<int>> GetMaxLengthOfWebsitesContentM(List<string> list) => list
                .TraverseTaskResultM(u => GetUriContentSize(new Uri(u)))
                .MapTaskResult(tr => tr.Max());
    }
}