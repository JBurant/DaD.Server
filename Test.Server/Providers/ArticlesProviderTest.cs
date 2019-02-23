using Common.DTO;
using Moq;
using Newtonsoft.Json;
using Server.DataAccessLayer;
using Server.DTO;
using Server.Providers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test.Server.Providers
{
    public class ArticlesProviderTest
    {
        private ArticlesProvider articlesProvider;
        private Mock<IArticlesAccess> articlesAccessMock;

        public ArticlesProviderTest()
        {
            articlesAccessMock = new Mock<IArticlesAccess>();
            articlesAccessMock.Setup(x => x.DeleteArticle(It.IsAny<string>()));
            articlesProvider = new ArticlesProvider(articlesAccessMock.Object);
        }

        [Theory]
        [InlineData("TestArticleName")]
        public void OnArticleNameExistsDeleteArticleReturnsProperResponse(string articleName)
        {
            //Arrange
            var expectedMessageResponse = new MessageResponse
            {
                Message = articleName
            };

            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            //Act
            var messageResponse = articlesProvider.DeleteArticle(articleName);

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message, articleName);
        }

        [Theory]
        [InlineData("TestArticleName")]
        public void OnArticleNameDoesNotExistDeleteArticleReturnsError(string articleName)
        {
            //Arrange
            var ExpectedError = new Error(ErrorCode.IE0002);
            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            //Act
            var messageResponse = articlesProvider.DeleteArticle(articleName);

            //Assert
            Assert.Empty(messageResponse.Message);
            Assert.Empty(messageResponse.Warnings);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }

        [Theory]
        [InlineData("TestArticleName", "This is the content of the article.")]
        public void OnArticleNameExistsGetArticleReturnsArticleContent(string articleName, string articleContent)
        {
            //Arrange
            var expectedModel = new ArticleModel { ArticleHeader = new ArticleHeader() { Name = articleName }, ArticleContent = articleContent };
            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            articlesAccessMock.Setup(x => x.GetArticle(It.IsAny<string>())).Returns(expectedModel);

            //Act
            var messageResponse = articlesProvider.GetArticle(articleName);

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message, JsonConvert.SerializeObject(expectedModel));
        }

        [Theory]
        [InlineData("TestArticleName", "This is the content of the article.")]
        public void OnArticleNameDoesNotExistGetArticleReturnsError(string articleName, string articleContent)
        {
            //Arrange
            var ExpectedError = new Error(ErrorCode.IE0002);
            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
            articlesAccessMock.Setup(x => x.GetArticle(It.IsAny<string>())).Returns(new ArticleModel { ArticleContent = articleContent });

            //Act
            var messageResponse = articlesProvider.GetArticle(articleName);

            //Assert
            Assert.Empty(messageResponse.Message);
            Assert.Empty(messageResponse.Warnings);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }

        [Fact]
        public void GetArticleListReturnsListOfFiles()
        {
            //Arrange
            var articleHeadersMock = new List<ArticleHeader>() { new ArticleHeader() { Name = "TestFile1" }, new ArticleHeader() { Name = "TestFile2" }, new ArticleHeader() { Name = "TestFile3" } };
            articlesAccessMock.Setup(x => x.GetArticleList()).Returns(articleHeadersMock);
            string articleHeadersMockString = JsonConvert.SerializeObject(articleHeadersMock);

            //Act
            var actualArticleList = articlesProvider.GetArticlesList();

            //Assert
            Assert.Empty(actualArticleList.Errors);
            Assert.Empty(actualArticleList.Warnings);
            Assert.Equal(articleHeadersMockString, actualArticleList.Message);
        }

        [Theory]
        [InlineData(true, false)]
        public void OnPostArticleShouldFail(bool fileExists, bool overwrite)
        {
            //Arrange
            var articleMock = new ArticleModel() { ArticleHeader = new ArticleHeader() { Name = "TestFileName" }, ArticleContent = "TestTextMock" };
            var ExpectedError = new Error(ErrorCode.IE0001);
            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(fileExists);
            articlesAccessMock.Setup(x => x.WriteArticle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //Act
            var messageResponse = articlesProvider.PostArticle(overwrite, articleMock);

            //Assert
            Assert.Empty(messageResponse.Warnings);
            Assert.Empty(messageResponse.Message);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void OnPostArticleShouldSucceed(bool fileExists, bool overwrite)
        {
            //Arrange
            var articleMock = new ArticleModel() { ArticleHeader = new ArticleHeader() { Name = "TestFileName" }, ArticleContent = "TestTextMock" };
            articlesAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(fileExists);
            articlesAccessMock.Setup(x => x.WriteArticle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //Act
            var messageResponse = articlesProvider.PostArticle(overwrite, articleMock);

            //Assert
            Assert.True(IsMessageSuccessful(messageResponse, "TestFileName"));  //Todo more tests
        }

        private bool IsMessageSuccessful(MessageResponse response, string expectedMessage)
        {
            return (!response.Errors.Any()) && (!response.Warnings.Any()) && expectedMessage.Equals(response.Message);
        }
    }
}