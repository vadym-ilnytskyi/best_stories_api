using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BestStoriesApi.BackgroundServices;

public class StoriesWorker(IHttpClientFactory factory, IDistributedCache cache) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var client = factory.CreateClient();
            var bestStories = await GetBestStories(client, stoppingToken);
            await GenerateBestStoriesCache(bestStories, stoppingToken);

            await Task.Delay(120000, stoppingToken);
        }
    }

    public async Task GenerateBestStoriesCache(IList<Story> bestStories, CancellationToken stoppingToken)
    {
        bestStories = bestStories.OrderByDescending(story => story.Score).ToList();

        for (int i = 1; i <= bestStories.Count; i++)
        {
            var topStories = bestStories.Take(i);

            await cache.SetStringAsync($"beststories:{i}", JsonSerializer.Serialize(topStories), stoppingToken);
        }
    }

    public async Task<IList<Story>> GetBestStories(HttpClient client,CancellationToken stoppingToken)
    {
        var ids = await client.GetFromJsonAsync<int[]>("https://hacker-news.firebaseio.com/v0/beststories.json", cancellationToken: stoppingToken);

        var bestStories = new List<Story>();

        if (ids is not null)
        {
            foreach (var id in ids)
            {
                var externalStory = await client.GetFromJsonAsync<ExternalStory>($"https://hacker-news.firebaseio.com/v0/item/{id}.json", cancellationToken: stoppingToken);

                if (externalStory is not null)
                {
                    bestStories.Add(new Story(externalStory));
                }
            }
        }

        return bestStories;
    }
}