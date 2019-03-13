using System;

namespace Server.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public string ArticleContent { get; set; }
    }
}