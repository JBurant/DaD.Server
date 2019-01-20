using Server.DataAccessLayer;
using Server.DTO;
using System.Collections.Generic;
using System.IO;
using Test.Server.Mocks;
using Xunit;

namespace Test.Server.DataAccessLayer
{
    public class ArticlesAccessTest
    {
        private const int ExtensionFileLength = 4;
        private const string ExtensionFile = ".txt";
        private const string ArticlesDirectory = "TestArticles/";
        private const string ExistingTestFile = "ExistingTestFile";

        private IArticlesAccess articlesAccess;

        public ArticlesAccessTest()
        {
            articlesAccess = new ArticlesAccess(new DataAccessLayerConfigMock());

            if (Directory.Exists(ArticlesDirectory))
            {
                Directory.Delete(ArticlesDirectory, true);
            }
            Directory.CreateDirectory(ArticlesDirectory);
        }

        [Theory]
        [InlineData("TestTest", "TestAuthor", "TestTextTestText")]
        public void CreateFile(string articleName, string articleAuthor, string articleFile)
        {
            //Arrange
            if (File.Exists(ArticlesDirectory + articleName + ExtensionFile))
            {
                File.Delete(ArticlesDirectory + articleName + ExtensionFile);
            }

            //Act
            articlesAccess.WriteArticle(articleName, articleAuthor, articleFile);

            //Assert
            Assert.True(File.Exists(ArticlesDirectory + articleName + ExtensionFile));
        }

        [Theory]
        [InlineData("NoNExistingTestFile")]
        public void TestFileDoesNotExistReturnsFalse(string articleName)
        {
            //Arrange
            File.Delete(ArticlesDirectory + articleName + ExtensionFile);

            //Act & Assert
            Assert.False(articlesAccess.FileExists(articleName));
        }

        [Theory]
        [InlineData("ExistingTestFile")]
        public void TestFileExistsReturnsTrue(string articleName)
        {
            //Arrange
            File.CreateText(ArticlesDirectory + ExistingTestFile + ExtensionFile).Close();

            //Act & Assert
            Assert.True(articlesAccess.FileExists(articleName));
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileDeletesFile(string articleName)
        {
            //Arrange
            File.CreateText(ArticlesDirectory + articleName + ExtensionFile).Close();

            //Act
            articlesAccess.DeleteArticle(articleName);

            //Assert
            Assert.False(File.Exists(ArticlesDirectory + articleName + ExtensionFile));
        }

        [Theory]
        [InlineData("GetTestFile")]
        public void GetFileReturnsString(string articleName)
        {
            //Arrange
            const string testContent = "This is a Content of a test file.";
            var writer = File.CreateText(ArticlesDirectory + articleName + ExtensionFile);
            writer.Write(testContent);
            writer.Close();

            //Act
            var articleContent = articlesAccess.GetArticle(articleName).ArticleContent;

            //Assert
            Assert.Equal(articleContent, testContent);
        }

        [Fact]
        public void GetArticleListReturnsActualListInString()
        {
            //Arrange
            var DummyAuthor = "Best Author";
            var expectedArticles = new List<ArticleHeader>() { new ArticleHeader() { Name = "TestFile1", Author = DummyAuthor }, new ArticleHeader() { Name = "TestFile2", Author = DummyAuthor }, new ArticleHeader() { Name = "TestFile3", Author = DummyAuthor } };

            foreach (ArticleHeader expectedArticle in expectedArticles)
            {
                var articleFullName = ArticlesDirectory + expectedArticle.Name + ExtensionFile;
                File.CreateText(articleFullName).Close();
                expectedArticle.TimeCreated = new FileInfo(articleFullName).CreationTimeUtc;
                expectedArticle.TimeModified = new FileInfo(articleFullName).LastWriteTimeUtc;
            }

            //Act
            var articleList = articlesAccess.GetArticleList();

            Assert.Equal(articleList.Count, expectedArticles.Count);

            //Assert
            for (int i=0; i < articleList.Count; i++)
            {
                Assert.Equal(articleList[i].Name, expectedArticles[i].Name);
                Assert.Equal(articleList[i].Author, expectedArticles[i].Author);
                Assert.Equal(articleList[i].TimeCreated, expectedArticles[i].TimeCreated);
                Assert.Equal(articleList[i].TimeModified, expectedArticles[i].TimeModified);
            }
        }
    }
}