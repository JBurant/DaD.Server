using Server.DTO;

namespace Server.Providers
{
    public interface IPostsProvider
    {
        MessageResponse PostPost(string postName, bool overwrite, string postFile);

        MessageResponse DeletePost(string PostName);

        MessageResponse GetPost(string PostName);

        MessageResponse GetPostsList();
    }
}