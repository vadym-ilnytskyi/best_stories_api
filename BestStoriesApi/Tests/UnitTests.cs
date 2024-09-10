using BestStoriesApi.BackgroundServices;
using BestStoriesApi;
using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Json;
using System.Net;
using Moq;
using Xunit;

namespace Tests
{
        namespace Tests
    {
        public class StoriesWorkerTests
        {
            private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
            private readonly Mock<IDistributedCache> _cacheMock;

            private readonly Mock<HttpClient> _httpClient;

            public StoriesWorkerTests()
            {
                _cacheMock = new Mock<IDistributedCache>();
                _httpClient = new Mock<HttpClient>();
            }

            [Fact]
            public async Task CheckIfForEveryIdsStoryExists()
            {
                //TBD
            }


            [Fact]
            public async Task CheckMappingsFromHackerApiProperlyMappedIntoExternalStory()
            {
                //TBD
            }

            [Fact]
            public async Task CheckMappingsFromExternalStoriesToStories()
            {
                //TBD
            }

            [Fact]
            public async Task CheckGetBestStoriesGeneration_GetBestStories()
            {
                // Arrange
       
                // Mock requests by Moq library

                var stoppingToken = new CancellationTokenSource().Token;
                var _storiesWorker = new StoriesWorker(_httpClientFactoryMock.Object, _cacheMock.Object);
                // Act
                var stories = await _storiesWorker.GetBestStories(_httpClient.Object, stoppingToken);
                // Assert
                    //check results
            }

            [Fact]
            public async Task CheckFullGenerationWithCache()
            {
                // Arrange
                // Arrange

                // Mock requests by Moq library


                var stoppingToken = new CancellationTokenSource().Token;
                var _storiesWorker = new StoriesWorker(_httpClientFactoryMock.Object, _cacheMock.Object);
                // Act
                var stories = await _storiesWorker.GetBestStories(_httpClient.Object,stoppingToken);
                await _storiesWorker.GenerateBestStoriesCache(stories, stoppingToken);

                // Assert
                  // Check cache by using in memeory mocked cache 
            }

        }
    }

}

