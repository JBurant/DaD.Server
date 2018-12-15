using Moq;
using Server.Common;
using Server.DataAccessLayer;
using Server.DTO;
using Server.Providers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test.Server.Providers
{
    public class PostsProviderTest
    {
        private PostsProvider postsProvider;
        private Mock<IPostsAccess> postsAccessMock;

        public PostsProviderTest()
        {
            postsAccessMock = new Mock<IPostsAccess>();
            postsAccessMock.Setup(x => x.DeletePost(It.IsAny<string>()));
            postsProvider = new PostsProvider(postsAccessMock.Object);
        }

        [Theory]
        [InlineData("TestPostName")]
        public void OnPostNameExistsDeletePostReturnsProperResponse(string PostName)
        {
            //Arrange
            var expectedMessageResponse = new MessageResponse
            {
                Message = PostName
            };

            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            //Act
            var messageResponse = postsProvider.DeletePost(PostName);

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message, PostName);
        }

        [Theory]
        [InlineData("TestPostName")]
        public void OnPostNameDoesNotExistDeletePostReturnsError(string PostName)
        {
            //Arrange
            var ExpectedError = new Error(ErrorCode.IE0002);
            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            //Act
            var messageResponse = postsProvider.DeletePost(PostName);

            //Assert
            Assert.Empty(messageResponse.Message);
            Assert.Empty(messageResponse.Warnings);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }

        [Theory]
        [InlineData("TestPostName", "This is the content of the post.")]
        public void OnPostNameExistsGetPostReturnsPostContent(string PostName, string PostContext)
        {
            //Arrange
            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            postsAccessMock.Setup(x => x.GetPost(It.IsAny<string>())).Returns(PostContext);

            //Act
            var messageResponse = postsProvider.GetPost(PostName);

            //Assert
            Assert.Empty(messageResponse.Errors);
            Assert.Empty(messageResponse.Warnings);
            Assert.Equal(messageResponse.Message, PostContext);
        }

        [Theory]
        [InlineData("TestPostName", "This is the content of the post.")]
        public void OnPostNameDoesNotExistGetPostReturnsError(string PostName, string PostContext)
        {
            //Arrange
            var ExpectedError = new Error(ErrorCode.IE0002);
            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
            postsAccessMock.Setup(x => x.GetPost(It.IsAny<string>())).Returns(PostContext);

            //Act
            var messageResponse = postsProvider.GetPost(PostName);

            //Assert
            Assert.Empty(messageResponse.Message);
            Assert.Empty(messageResponse.Warnings);
            Assert.Single(messageResponse.Errors);
            Assert.Equal(messageResponse.Errors[0].ErrorCode, ExpectedError.ErrorCode);
            Assert.Equal(messageResponse.Errors[0].ErrorMessage, ExpectedError.ErrorMessage);
        }

        [Fact]
        public void GetPostListReturnsListOfFiles()
        {
            //Arrange
            var postsListMock = new List<string>() { "TestFile1", "TestFile2", "TestFile3" };
            postsAccessMock.Setup(x => x.GetPostList()).Returns(postsListMock);
            var postsLickMockString = postsListMock[0] + "," + postsListMock[1] + "," + postsListMock[2] + ",";

            //Act
            var actualPostList = postsProvider.GetPostsList();

            //Assert
            Assert.Empty(actualPostList.Errors);
            Assert.Empty(actualPostList.Warnings);
            Assert.Equal(postsLickMockString, actualPostList.Message);
        }

        [Theory]
        [InlineData(true, false)]
        public void OnPostPostShouldFail(bool fileExists, bool overwrite)
        {
            //Arrange
            var ExpectedError = new Error(ErrorCode.IE0001);
            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(fileExists);
            postsAccessMock.Setup(x => x.WritePost(It.IsAny<string>(), It.IsAny<string>()));

            //Act
            var messageResponse = postsProvider.PostPost("TestFileName", overwrite, "Generic post text.");

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
        public void OnPostPostShouldSucceed(bool fileExists, bool overwrite)
        {
            //Arrange
            postsAccessMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(fileExists);
            postsAccessMock.Setup(x => x.WritePost(It.IsAny<string>(), It.IsAny<string>()));

            //Act
            var messageResponse = postsProvider.PostPost("TestFileName", overwrite, "Generic post text.");

            //Assert
            Assert.True(IsMessageSuccessful(messageResponse, "TestFileName"));  //Todo more tests
        }

        private bool IsMessageSuccessful(MessageResponse response, string expectedMessage)
        {
            return (!response.Errors.Any()) && (!response.Warnings.Any()) && expectedMessage.Equals(response.Message);
        }
    }
}