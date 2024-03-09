
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Twitter
{
    public class TwitterClient
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private readonly string accessToken;
        private readonly string tokenSecret;

        private readonly HttpClient httpClient;

        public TwitterClient(HttpClient httpClient, string consumerKey, string consumerSecret, string accessToken, string tokenSecret)
        {
            this.httpClient = httpClient;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.accessToken = accessToken;
            this.tokenSecret = tokenSecret;
        }

        public async Task PostTweet(string text)
        {
            var timstamp = CreateTimestamp();
            var nonce = CreateNonce();
            var body = JsonSerializer.Serialize(new { text });
            var uri = new Uri("https://api.twitter.com/2/tweets");

            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Post,
                Content = new StringContent(body, Encoding.ASCII, "application/json")
            };

            var signatureBase64 = CreateSignature(uri.ToString(), "POST", nonce, timstamp);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth",
                    $@"oauth_consumer_key=""{Uri.EscapeDataString(consumerKey)}""" +
                    $@",oauth_token=""{Uri.EscapeDataString(accessToken)}""" +
                    $@",oauth_signature_method=""HMAC-SHA1"",oauth_timestamp=""{Uri.EscapeDataString(timstamp)}""" +
                    $@",oauth_nonce=""{Uri.EscapeDataString(nonce)}"",oauth_version=""1.0""" +
                    $@",oauth_signature=""{Uri.EscapeDataString(signatureBase64)}""");

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private string CreateSignature(string url, string method, string nonce, string timestamp)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("oauth_consumer_key", consumerKey);
            parameters.Add("oauth_nonce", nonce);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", timestamp);
            parameters.Add("oauth_token", accessToken);
            parameters.Add("oauth_version", "1.0");

            var sigBaseString = CombineQueryParams(parameters);

            var signatureBaseString =
                method.ToString() + "&" +
                Uri.EscapeDataString(url) + "&" +
                Uri.EscapeDataString(sigBaseString.ToString());

            var compositeKey =
                Uri.EscapeDataString(consumerSecret) + "&" +
                Uri.EscapeDataString(tokenSecret);

            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                return Convert.ToBase64String(hasher.ComputeHash(
                    Encoding.ASCII.GetBytes(signatureBaseString)));
            }
        }

        private string CreateTimestamp()
        {
            var totalSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                .TotalSeconds;

            return Convert.ToInt64(totalSeconds).ToString();
        }

        private string CreateNonce()
        {
            return Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(
                    DateTime.Now.Ticks.ToString()));
        }

        public string CombineQueryParams(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();

            var first = true;

            foreach (var param in parameters)
            {
                if (!first)
                {
                    sb.Append("&");
                }
                sb.Append(param.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(param.Value));

                first = false;
            }
            return sb.ToString();
        }
    }
}
