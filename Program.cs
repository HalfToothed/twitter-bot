using Launch;
using Twitter;

namespace Program
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();

            // RocketWatcher Api credentials
            string consumerKey = "API-Key";
            string consumerSecret = "API-Secret";
            string accessToken = "Access-Token";
            string tokenSecret = "Token-Secret";

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