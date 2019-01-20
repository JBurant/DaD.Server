using Server.App_Config;
using Server.DTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Server.DataAccessLayer
{
    public class ArticlesAccess : IArticlesAccess
    {
        private readonly int ExtensionFileLength;
        private readonly string FileExtension;
        private readonly string ArticlesDirectory;
        private readonly string DummyAuthor = "Best Author";
        //TODO: Proper author!!!

        public ArticlesAccess(IDataAccessLayerConfig articleAccessConfiguration)
        {
            ExtensionFileLength = articleAccessConfiguration.GetExtensionFileLength();
            FileExtension = articleAccessConfiguration.GetFileExtension();
            ArticlesDirectory = articleAccessConfiguration.GetArticlesDirectory();
        }

        public bool FileExists(string articleName)
        {
            if (System.IO.File.Exists(@GetFullName(articleName)))
            {
                return true;
            }

            return false;
        }

        private string GetFullName(string articleName)
        {
            return ArticlesDirectory + articleName + FileExtension;
        }

        private ArticleHeader GetArticleHeader(string fullArticleName)
        {
            var articleHeader = new ArticleHeader();

            var articleFileInfo = new FileInfo(fullArticleName);

            articleHeader.Name = articleFileInfo.Name.Substring(0, articleFileInfo.Name.Length - ExtensionFileLength);
            articleHeader.Author = DummyAuthor;
            articleHeader.TimeCreated = articleFileInfo.CreationTimeUtc;
            articleHeader.TimeModified = articleFileInfo.LastWriteTimeUtc;

            return articleHeader;
        }

        //TODO: Save the Author!!
        //TODO: Updating article vs. new one
        public bool WriteArticle(string articleName, string articleAuthor, string articleFile)
        {
            var fullArticleName = GetFullName(articleName);

            using (StreamWriter outputFile = new StreamWriter(fullArticleName))
            {
                outputFile.Write(articleName);
                outputFile.Close();
            };
             return FileExists(articleName);
        }

        public void DeleteArticle(string articleName)
        {
            System.IO.File.Delete(@GetFullName(articleName));
        }

        public ArticleModel GetArticle(string articleName)
        {
            var model = new ArticleModel();
            var fullArticleName = GetFullName(articleName);

            using (StreamReader fileReader = new StreamReader(fullArticleName))
            {
                model.ArticleContent = fileReader.ReadToEnd();
                fileReader.Close();
            };

            model.ArticleHeader = GetArticleHeader(fullArticleName);

            return model;
        }

        public List<ArticleHeader> GetArticleList()
        {
            var articleHeaders = new List<ArticleHeader>();

            DirectoryInfo articlesDirectory = new DirectoryInfo(@ArticlesDirectory);
            FileInfo[] articleFileInfos = articlesDirectory.GetFiles("*" + FileExtension);

            foreach(FileInfo articleFileInfo in articleFileInfos)
            {
                articleHeaders.Add(GetArticleHeader(GetFullName(articleFileInfo.Name.Substring(0, articleFileInfo.Name.Length - ExtensionFileLength)))); 
            }

            return articleHeaders;
        }
    }
}