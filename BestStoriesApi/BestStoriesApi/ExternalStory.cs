﻿namespace BestStoriesApi
{
    public class ExternalStory
    {
        public int Id { get; set; }
        public string By { get; set; }
        public int Descendants { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
