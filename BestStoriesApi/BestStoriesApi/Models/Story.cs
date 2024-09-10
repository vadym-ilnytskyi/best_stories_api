using BestStoriesApi.Models;

public class Story
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Uri { get; set; }
    public string PostedBy { get; set; }
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }

    public Story() { }
    public Story(ExternalStory externalStory)
    {

        Title = externalStory.Title;
        Uri = externalStory.Url;
        PostedBy = externalStory.By;
        Time = DateTimeOffset.FromUnixTimeSeconds(externalStory.Time).DateTime;
        Score = externalStory.Score;
        CommentCount = externalStory.Descendants;
    }
}
