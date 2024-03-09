using Launch;
using Twitter;

namespace Program
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();

            // StellarOasis API credentials
            // string consumerKey = "vJZnkOXyeliNBi8St5Jypy1vA";
            // string consumerSecret = "oZWELwZ82DqKJXXmwE7eVXZzvQlc9eEIODWixD9i5urQG9tKDa";
            // string accessToken = "1693999721071931392-OQBzUqs8pio15Z3PaHQMV9UJwjGMR9";
            // string tokenSecret = "UNZzCb3YcMqxf3mwhtjhIMTYX19XaG1Qrkl2PLZaCbCss";

            // RocketWatcher Api credentials
            string consumerKey = "3JfqfL8fSspOMECPSLpSVnK7z";
            string consumerSecret = "Ah9l9YaY8u8oJJuajH7PpXZc4UKEIb8IXHM3pj5UciKCGH2haK";
            string accessToken = "1697685343011524608-AZzzoj26SrKy1BWrIRoVw6zy5wklzJ";
            string tokenSecret = "oCbQ5ynudmqBTaFhO9owiQzQNayhTQLNlVp4eEA0WebH1";

            string response = new string("");

            var launchClient = new Launcher();
            response = await launchClient.GetLaunch();
            Console.WriteLine(response);
            var twitterClient = new TwitterClient(httpClient, consumerKey, consumerSecret, accessToken, tokenSecret);

            try
            {
                // Call the PostTweet method
                await twitterClient.PostTweet(response);
                Console.WriteLine("Tweet posted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error posting tweet: " + ex);
            }
        }
    }
}