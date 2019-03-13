namespace Server.Contexts
{
    public interface IArticleContextFactory
    {
        ArticleDbContext CreateArticleContext();
    }
}