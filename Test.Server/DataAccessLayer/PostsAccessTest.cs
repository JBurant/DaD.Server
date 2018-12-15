using Server.DataAccessLayer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Test.Server.Mocks;
using Xunit;

namespace Test.Server.DataAccessLayer
{
    public class PostsAccessTest
    {
        private const int ExtensionFileLength = 4;
        private const string ExtensionFile = ".txt";
        private const string PostsDirectory = "TestPosts/";
        private const string ExistingTestFile = "ExistingTestFile";

        private IPostsAccess postsAccess;

        public PostsAccessTest()
        {
            postsAccess = new PostsAccess(new DataAccessLayerConfigMock());

            if (Directory.Exists(PostsDirectory))
            {
                Directory.Delete(PostsDirectory, true);
            }
            Directory.CreateDirectory(PostsDirectory);
        }

        [Theory]
        [InlineData("TestTest", "TestTextTestText")]
        public void CreateFile(string postName, string postFile)
        {
            //Arrange
            if (File.Exists(PostsDirectory + postName + ExtensionFile))
            {
                File.Delete(PostsDirectory + postName + ExtensionFile);
            }

            //Act
            postsAccess.WritePost(postName, postFile);

            //Assert
            Assert.True(File.Exists(PostsDirectory + postName + ExtensionFile));
        }

        [Theory]
        [InlineData("NoNExistingTestFile")]
        public void TestFileDoesNotExistReturnsFalse(string postName)
        {
            //Arrange
            File.Delete(PostsDirectory + postName + ExtensionFile);

            //Act & Assert
            Assert.False(postsAccess.FileExists(postName));
        }

        [Theory]
        [InlineData("ExistingTestFile")]
        public void TestFileExistsReturnsTrue(string postName)
        {
            //Arrange
            File.CreateText(PostsDirectory + ExistingTestFile + ExtensionFile).Close();

            //Act & Assert
            Assert.True(postsAccess.FileExists(postName));
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileDeletesFile(string postName)
        {
            //Arrange
            File.CreateText(PostsDirectory + postName + ExtensionFile).Close();

            //Act
            postsAccess.DeletePost(postName);

            //Assert
            Assert.False(File.Exists(PostsDirectory + postName + ExtensionFile));
        }

        [Theory]
        [InlineData("GetTestFile")]
        public void GetFileReturnsString(string postName)
        {
            //Arrange
            const string testContent = "This is a Content of a test file.";
            var writer = File.CreateText(PostsDirectory + postName + ExtensionFile);
            writer.Write(testContent);
            writer.Close();

            //Act
            var postContent = postsAccess.GetPost(postName);

            //Assert
            Assert.Equal(postContent, testContent);
        }

        [Fact]
        public void GetPostListReturnsActualListInString()
        {
            //Arrange
            var expectedPostList = new List<string>() { "TestFile1", "TestFile2", "TestFile3" };

            File.CreateText(PostsDirectory + expectedPostList[0] + ExtensionFile).Close();
            File.CreateText(PostsDirectory + expectedPostList[1] + ExtensionFile).Close();
            File.CreateText(PostsDirectory + expectedPostList[2] + ExtensionFile).Close();

            //Act
            var postList = postsAccess.GetPostList();

            //Assert
            Assert.True(postList.SequenceEqual(expectedPostList));
        }
    }
}