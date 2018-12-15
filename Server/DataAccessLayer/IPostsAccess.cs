using System.Collections.Generic;

namespace Server.DataAccessLayer
{
    public interface IPostsAccess
    {
        bool FileExists(string postName);

        void WritePost(string postName, string postFile);

        void DeletePost(string postName);

        string GetPost(string postName);

        List<string> GetPostList();
    }
}