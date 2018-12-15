using Server.Common;
using Server.DataAccessLayer;
using Server.DTO;

namespace Server.Providers
{
    public class PostsProvider : IPostsProvider
    {
        private IPostsAccess postsAccess;

        public PostsProvider(IPostsAccess postsAccess)
        {
            this.postsAccess = postsAccess;
        }

        public MessageResponse PostPost(string postName, bool overwrite, string postFile)
        {
            var response = new MessageResponse();

            if (!overwrite && postsAccess.FileExists(postName))
            {
                response.Errors.Add(new Error(ErrorCode.IE0001));
                return response;
            }

            postsAccess.WritePost(postName, postFile);
            response.Message = postName;

            return response;
        }

        public MessageResponse DeletePost(string PostName)
        {
            var response = new MessageResponse();

            if (postsAccess.FileExists(PostName))
            {
                postsAccess.DeletePost(PostName);
                response.Message = PostName;
            }
            else
            {
                response.Errors.Add(new Error(ErrorCode.IE0002));
            }

            return response;
        }

        public MessageResponse GetPost(string PostName)
        {
            var response = new MessageResponse();

            if (postsAccess.FileExists(PostName))
            {
                response.Message = postsAccess.GetPost(PostName);
            }
            else
            {
                response.Errors.Add(new Error(ErrorCode.IE0002));
            }

            return response;
        }

        public MessageResponse GetPostsList()
        {
            var postsList = postsAccess.GetPostList();
            string messagePostsList = "";

            foreach (string post in postsList)
            {
                messagePostsList += post + ",";
            }

            return new MessageResponse() { Message = messagePostsList };
        }
    }
}