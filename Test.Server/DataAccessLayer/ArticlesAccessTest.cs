using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Contexts;
using Server.DataAccessLayer;
using Server.DTO;
using Server.MapperProfiles;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Test.Server.DataAccessLayer
{
    public class ArticlesAccessTest
    {
        private readonly DateTime TimeCreated;
        private readonly DateTime TimeModified;

        private IArticlesAccess articlesAccess;
        private Mock<IArticleContextFactory> articleContextFactoryMock;
        private Mock<ArticleDbContext> articleContextMock;

        private Article articleToBeAdded;

        public ArticlesAccessTest()
        {
            TimeCreated = new DateTime(2019, 1, 1);
            TimeModified = new DateTime(2019, 2, 2);

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ArticleArticleHeaderProfile>();
                cfg.AddProfile<ArticleArticleDtoProfile>();
            });

            articleContextFactoryMock = new Mock<IArticleContextFactory>();

            var optionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionsBuilder.UseSqlServer("Test");

            articleContextMock = new Mock<ArticleDbContext>(MockBehavior.Loose, new object[] { optionsBuilder.Options });

            var data = new List<Article>()
            {
                new Article { Name = "FirstArticleName" },
            };

            PrepareMocks(data);
        }

        [Fact]
        public void GetArticleHeaderAsync_AnyInput_ShouldCreateNewContext()
        {
            //Arrange
            //Act
            articlesAccess.WriteArticleAsync("DummyName", "DummyAuthor", "DummyText");

            //Assert
            articleContextFactoryMock.Verify(x => x.CreateArticleContext(), Times.Once());
        }

        [Theory]
        [InlineData("TestTest", "TestAuthor")]
        public void GetArticleHeaderAsync_ArticleExists_ShouldReturnExpectedHeader(string articleName, string articleAuthor)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(new Article()
                {
                    Name = articleName,
                    Author = articleAuthor,
                    TimeCreated = TimeCreated,
                    TimeModified = TimeModified
                });

            var expectedResult = new ArticleHeader()
            {
                Name = articleName,
                Author = articleAuthor,
                TimeCreated = TimeCreated,
                TimeModified = TimeModified
            };

            //Act
            var result = articlesAccess.GetArticleHeaderAsync(articleName).Result;

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("TestTest")]
        public void GetArticleHeaderAsync_ArticleDoesNotExist_ShouldReturnNull(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync((Article)null);

            //Act
            var result = articlesAccess.GetArticleHeaderAsync(articleName).Result;

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public void WriteArticleAsync_AnyInput_ShouldCreateNewContext()
        {
            //Arrange
            //Act
            articlesAccess.WriteArticleAsync("DummyName", "DummyAuthor", "DummyText");

            //Assert
            articleContextFactoryMock.Verify(x => x.CreateArticleContext(), Times.Once());
        }

        [Theory]
        [InlineData("TestTest", "TestAuthor", "TestTextTestText")]
        public void WriteArticleAsync_AnyInput_ShouldCreateNewEntry(string articleName, string articleAuthor, string articleFile)
        {
            //Arrange
            var expectedResult = new Article() { Name = articleName, Author = articleAuthor, ArticleContent = articleFile };

            //Act
            articlesAccess.WriteArticleAsync(articleName, articleAuthor, articleFile);

            //Assert
            articleToBeAdded.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("TestTest", "TestAuthor", "TestTextTestText")]
        public void WriteArticleAsync_AnyInput_ShouldReturnTrue(string articleName, string articleAuthor, string articleFile)
        {
            //Arrange
            //Act
            var result = articlesAccess.WriteArticleAsync(articleName, articleAuthor, articleFile).Result;

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("TestTest", "TestAuthor", "TestTextTestText")]
        public void WriteArticleAsync_NotSaved_ShouldReturnFalse(string articleName, string articleAuthor, string articleFile)
        {
            //Arrange
            articleContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            //Act
            var result = articlesAccess.WriteArticleAsync(articleName, articleAuthor, articleFile).Result;

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteFileAsync_AnyInput_ShouldCreateNewContext()
        {
            //Arrange
            //Act
            articlesAccess.WriteArticleAsync("DummyName", "DummyAuthor", "DummyText");

            //Assert
            articleContextFactoryMock.Verify(x => x.CreateArticleContext(), Times.Once());
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileAsync_ExistingFile_ShouldCallRemove(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync(new Article() { Name = articleName });

            //Act
            articlesAccess.DeleteArticleAsync(articleName);

            //Assert
            articleContextMock.Verify(x => x.Articles.Remove(It.IsAny<Article>()), Times.Once);
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileAsync_NonExistingFile_ShouldNotCallRemove(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync((Article)null);

            //Act
            articlesAccess.DeleteArticleAsync(articleName);

            //Assert
            articleContextMock.Verify(x => x.Articles.Remove(It.IsAny<Article>()), Times.Never);
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileAsync_NonExistingFile_ShouldReturnFalse(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync((Article)null);

            //Act
            var result = articlesAccess.DeleteArticleAsync(articleName).Result;

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("DeleteTestFile")]
        public void DeleteFileAsync_ContextNotSaved_ShouldReturnFalse(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync(new Article() { Name = articleName });
            articleContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            //Act
            var result = articlesAccess.DeleteArticleAsync(articleName).Result;

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("GetTestFile")]
        public void GetArticleAsync_AnyInput_ShouldCreateNewContext(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync(new Article() { Name = articleName });

            //Act
            articlesAccess.GetArticleAsync(articleName);

            //Assert
            articleContextFactoryMock.Verify(x => x.CreateArticleContext(), Times.Once);
        }

        [Theory]
        [InlineData("GetTestFile", "GetTestAuthor", "GetTestContent")]
        public void GetArticleAsync_ArticleExists_ShouldReturnExpectedArticle(string articleName, string articleAuthor, string articleContent)
        {
            //Arrange
            var dummyArticleDto = new ArticleDTO()
            {
                ArticleHeader = new ArticleHeader()
                {
                    Name = articleName,
                    Author = articleAuthor,
                    TimeCreated = TimeCreated,
                    TimeModified = TimeModified
                },
                ArticleContent = articleContent
            };

            var dummyArticle = new Article()
            {
                Name = articleName,
                Author = articleAuthor,
                TimeCreated = TimeCreated,
                TimeModified = TimeModified,
                ArticleContent = articleContent
            };

            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync(dummyArticle);
            var expectedResult = dummyArticleDto;

            //Act
            var result = articlesAccess.GetArticleAsync(articleName).Result;

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("GetTestFile")]
        public void GetArticleAsync_ArticleDoesNotExist_ShouldReturnNull(string articleName)
        {
            //Arrange
            articleContextMock.Setup(x => x.Articles.FindAsync(It.IsAny<string>())).ReturnsAsync((Article)null);

            //Act
            var result = articlesAccess.GetArticleAsync(articleName).Result;

            //Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("Article1", "Article2", "Article3")]
        public void GetArticleListAsync_ArticlesExist_ShouldCreateNewContext(string firstArticleName, string secondArticleName, string thirdArticleName)
        {
            //Arrange
            var data = new List<Article>()
            {
                new Article { Name = firstArticleName },
                new Article { Name = secondArticleName },
                new Article { Name = thirdArticleName },
            };

            PrepareMocks(data);

            var expectedResult = new List<ArticleHeader>()
            {
                new ArticleHeader { Name = firstArticleName },
                new ArticleHeader { Name = secondArticleName },
                new ArticleHeader { Name = thirdArticleName },
            };

            //Act
            var result = articlesAccess.GetArticleListAsync().Result;

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetArticleListAsync_NoArticles_ShouldReturnNull()
        {
            //Arrange
            PrepareMocks(new List<Article>());

            //Act
            var result = articlesAccess.GetArticleListAsync().Result;

            //Assert
            result.Should().BeEmpty();
        }

        private void PrepareMocks(List<Article> data)
        {
            var mockArticlesSet = new Mock<DbSet<Article>>();
            mockArticlesSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockArticlesSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockArticlesSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockArticlesSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(data.AsQueryable().GetEnumerator());

            articleContextMock.Setup(c => c.Articles).Returns(mockArticlesSet.Object);
            articleContextMock.Setup(x => x.Articles.Add(It.IsAny<Article>())).Callback((Article x) => articleToBeAdded = x);
            articleContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            articleContextFactoryMock.Setup(x => x.CreateArticleContext()).Returns(articleContextMock.Object);
            articlesAccess = new ArticlesAccess(articleContextFactoryMock.Object);
        }
    }
}