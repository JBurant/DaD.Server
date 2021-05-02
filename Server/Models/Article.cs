using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Server.Models
{
    public class Article
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public string ArticleContent { get; set; }
    }
}