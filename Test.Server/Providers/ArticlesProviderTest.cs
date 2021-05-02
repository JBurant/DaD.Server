using Common.DTO;
using Common.Services;
using FluentAssertions;
using Moq;
using Server.DataAccessLayer;
using Server.DTO;
using Server.Models;
using Server.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test.Server.Providers
{
    public class ArticlesProviderTest
    {
        private ArticlesProvider articlesProvider;
        private ErrorListProvider errorListProvider;

        private Mock<IArticlesAccess> articlesAccessMock;

        private readonly ArticleDTO articleDTOMock;
        private readonly Article articleMock;

        public ArticlesProviderTest()
        {
            errorListProvider = new ErrorListProvider();
            articlesAccessMock = new Mock<IArticlesAccess>();
            articlesProvider = new ArticlesProvider(articlesAccessMock.Object, errorListProvider);

            articleDTOMock = new ArticleDTO() { ArticleHeader = new ArticleHeader() { Name = "TestFileName" }, ArticleContent = "TestTextMock" };
            articleMock = new Article() { Name = "TestFileName", ArticleContent = "TestTextMock" };
        }

        [Theory]
        [InlineData("TestArticleName")]
        public void DeleteArticleAsync_ArticleNameExists_ReturnsProperResponse(string articleName)
        {
            //Arrange
            articlesAccessMock.Setup(x => x.DeleteArticleAsync(It.IsAny<string>())).ReturnsAsync(true);

            var expectedMessageResponse = new MessageResponse<string>
            {
                Message = articleName
            };

            //Act
            var messageResponse = articlesProvider.DeleteArticleAsync(articleName).Result;

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message, articleName);
        }

        [Theory]
        [InlineData("TestArticleName")]
        public void DeleteArticleAsync_ArticleNameDoesNotExist_ReturnsError(string articleName)
        {
            //Arrange
            articlesAccessMock.Setup(x => x.DeleteArticleAsync(It.IsAny<string>())).ReturnsAsync(false);

            var expectedError = errorListProvider.GetError(ErrorCode.IE0002);

            //Act
            var messageResponse = articlesProvider.DeleteArticleAsync(articleName).Result;

            //Assert
            IsMessageFail(messageResponse, expectedError);
        }

        [Theory]
        [InlineData("TestArticleName", "This is the content of the article.")]
        public void GetArticleAsync_ArticleNameExists_ReturnsArticleContent(string articleName, string articleContent)
        {
            //Arrange
            var expectedModel = new Article { Name = articleName , ArticleContent = articleContent };
            var expectedDTO = new ArticleDTO { ArticleHeader = new ArticleHeader() { Name = articleName }, ArticleContent = articleContent };

            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).Returns(Task.FromResult(expectedModel));

            //Act
            var messageResponse = articlesProvider.GetArticleAsync(articleName).Result;

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message.ArticleHeader.Name, expectedDTO.ArticleHeader.Name);
            Assert.Equal(messageResponse.Message.ArticleContent, expectedDTO.ArticleContent);
        }

        [Theory]
        [InlineData("TestArticleName")]
        public void GetArticleAsync_ArticleNameDoesNotExist_ReturnsError(string articleName)
        {
            //Arrange
            var expectedError = errorListProvider.GetError(ErrorCode.IE0002);
            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).ReturnsAsync((Article)null);

            //Act
            var messageResponse = articlesProvider.GetArticleAsync(articleName).Result;

            //Assert
            IsMessageFail(messageResponse, expectedError);
        }

        [Fact]
        public void GetArticleListAsync_ArticlesExist_ReturnsListOfArticles()
        {
            //Arrange
            var articlesMock = new List<Article>() { new Article() { Name = "TestFile1" }, new Article() { Name = "TestFile2" }, new Article() { Name = "TestFile3" } };
            var articleHeadersMock = new List<ArticleHeader>() { new ArticleHeader() { Name = "TestFile1" }, new ArticleHeader() { Name = "TestFile2" }, new ArticleHeader() { Name = "TestFile3" } };

            articlesAccessMock.Setup(x => x.GetArticleListAsync()).ReturnsAsync(articlesMock);

            //Act
            var actualArticleList = articlesProvider.GetArticlesListAsync().Result;

            //Assert
            Assert.Empty(actualArticleList.Errors);
            Assert.Empty(actualArticleList.Warnings);

            for(int i=0; i < 3; i++) 
            {
                Assert.Equal(articleHeadersMock[i].Name, actualArticleList.Message[i].Name);
            }
        }

        [Theory]
        [InlineData(false)]
        public void PostArticleAsync_ArticleExistsAndNotOverwrite_ShouldFail(bool overwrite)
        {
            //Arrange
            var expectedError = errorListProvider.GetError(ErrorCode.IE0001);

            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).ReturnsAsync(articleMock);

            //Act
            var messageResponse = articlesProvider.PostArticleAsync(overwrite, articleDTOMock).Result;

            //Assert
            IsMessageFail(messageResponse, expectedError);
        }

        [Theory]
        [InlineData(true)]
        public void PostArticleAsync_ArticleExistsAndNotOverwriteAndNotDeleted_ShouldFail(bool overwrite)
        {
            //Arrange
            var expectedError = errorListProvider.GetError(ErrorCode.IE0010);

            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).ReturnsAsync(articleMock);
            articlesAccessMock.Setup(x => x.DeleteArticleAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var messageResponse = articlesProvider.PostArticleAsync(overwrite, articleDTOMock).Result;

            //Assert
            IsMessageFail(messageResponse, expectedError);
        }

        [Theory]
        [InlineData(true)]
        public void PostArticleAsync_ArticleExistsAndOverwriteAndDeletedAndWritten_ShouldSucceed(bool overwrite)
        {
            //Arrange
            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).ReturnsAsync(articleMock);
            articlesAccessMock.Setup(x => x.DeleteArticleAsync(It.IsAny<string>())).ReturnsAsync(true);
            articlesAccessMock.Setup(x => x.WriteArticleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<object>(null));

            //Act
            var messageResponse = articlesProvider.PostArticleAsync(overwrite, articleDTOMock).Result;

            //Assert
            messageResponse.Message.Should().Be(articleDTOMock.ArticleHeader.Name);
            Assert.Empty(messageResponse.Warnings);
            Assert.Empty(messageResponse.Errors);
        }

        [Fact]
        public void PostArticleAsync_ArticleDoesNotExistAndWritten_ShouldSucceed()
        {
            //Arrange
            articlesAccessMock.Setup(x => x.GetArticleAsync(It.IsAny<string>())).ReturnsAsync((Article)null);
            articlesAccessMock.Setup(x => x.WriteArticleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<object>(null));

            //Act
            var messageResponse = articlesProvider.PostArticleAsync(false, articleDTOMock).Result;

            //Assert
            messageResponse.Message.Should().Be(articleDTOMock.ArticleHeader.Name);
        }

        private bool IsMessageSuccessful(MessageResponse<string> response, string expectedMessage)
        {
            return (!response.Errors.Any()) && (!response.Warnings.Any()) && expectedMessage.Equals(response.Message);
        }

        private void IsMessageFail<T>(MessageResponse<T> messageResponse, Error ExpectedError)
        {
            messageResponse.Message.Should().BeNull();
            Assert.Empty(messageResponse.Warnings);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }
    }
}