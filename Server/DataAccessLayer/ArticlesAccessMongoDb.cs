using MongoDB.Bson;
using MongoDB.Driver;
using Server.ConfigurationDTO;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public class ArticlesAccessMongoDb : IArticlesAccess
    {
        private ArticleDatabaseSettings articleDatabaseSettings;
        
        public ArticlesAccessMongoDb(ArticleDatabaseSettings articleDatabaseSettings)
        {
            this.articleDatabaseSettings = articleDatabaseSettings;
        }

        public async Task<bool> DeleteArticleAsync(string articleName)
        {
            var filter = Builders<Article>.Filter.Eq("Name", articleName);
            var collection = GetCollection();
            
            var result = await collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }   

        public async Task<Article> GetArticleAsync(string articleName)
        {
            var filter = Builders<Article>.Filter.Eq("Name", articleName);
            var collection = GetCollection();
            return await collection.Find(filter).FirstAsync();
        }

        public async Task<List<Article>> GetArticleListAsync()
        {
            var collection = GetCollection();
            return await collection.AsQueryable().ToListAsync();
        }

        public async Task WriteArticleAsync(string articleName, string articleAuthor, string articleFile)
        {
            var collection = GetCollection();
            await collection.InsertOneAsync(new Article() { 
                Id = ObjectId.GenerateNewId(),
                Name = articleName, 
                Author = articleAuthor, 
                TimeCreated = DateTime.Now,
                TimeModified = DateTime.Now,
                ArticleContent = articleFile });
        }

        private IMongoCollection<Article> GetCollection()
        {
            var mongoClient = new MongoClient(articleDatabaseSettings.ConnectionString);
            return mongoClient.GetDatabase(articleDatabaseSettings.DatabaseName).GetCollection<Article>(articleDatabaseSettings.ArticlesCollectionName);
        }
    }
}
