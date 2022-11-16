﻿namespace NewsParserApi.Models.NewsDto
{
    public class NewsPreviewList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string Url { get; set; }

        public int LikesCount { get; set; } = 0;
        public int DislikesCount { get; set; } = 0;

        public bool? likedByCurrentUser { get; set; } = null;
    }
}
