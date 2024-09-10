using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests
{
    public class StoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public StoriesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
         
        [Fact]
        public async Task Get_ReturnsOkResult_WithCachedValue()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
                }
                );
            }).CreateClient();

            var cache = _factory.Services.GetRequiredService<IDistributedCache>();

            var randomizer = new Randomizer();
            var number = randomizer.Int(0, 10);


            var faker = new Faker<Story>()
                .RuleFor(s => s.Title, f => f.Lorem.Sentence())
                .RuleFor(s => s.Uri, f => f.Internet.Url())
                .RuleFor(s => s.PostedBy, f => f.Internet.UserName())
                .RuleFor(s => s.Time, f => f.Date.Past())
                .RuleFor(s => s.Score, f => f.Random.Int(0, 1000))
                .RuleFor(s => s.CommentCount, f => f.Random.Int(0, 100));


            var initialStories = JsonConvert.SerializeObject(faker.Generate(number));

            await cache.SetStringAsync($"beststories:{number}", initialStories);
            
            // Act
            var stringJson = await client.GetAsync($"/stories/best/{number}");

            var stories = JsonConvert.DeserializeObject<List<Story>>(stringJson.Content.ToString());

            // Assert

            Assert.Equal(number, stories.Count);
        }
    }
}