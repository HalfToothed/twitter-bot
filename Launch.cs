using System.Runtime.InteropServices;
using System.Text.Json;
using Data;

namespace Launch
{
    public class Launcher
    {
        public async Task<string> GetLaunch()
        {
            using HttpClient client = new();

            var url = "https://fdo.rocketlaunch.live/json/launches/next/1";
            using Stream stream = await client.GetStreamAsync(url);

            Root root = await JsonSerializer.DeserializeAsync<Root>(stream);

            Result result = root.result[0];

            var launch = result.launch_description;

            string response = $"{launch}";
            return response;
        }
    }
}