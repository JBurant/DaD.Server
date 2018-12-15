using Server.App_Config;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.DataAccessLayer
{
    public class PostsAccess : IPostsAccess
    {
        private readonly int ExtensionFileLength;
        private readonly string FileExtension;
        private readonly string PostsDirectory;

        public PostsAccess(IDataAccessLayerConfig postAccessConfiguration)
        {
            ExtensionFileLength = postAccessConfiguration.GetExtensionFileLength();
            FileExtension = postAccessConfiguration.GetFileExtension();
            PostsDirectory = postAccessConfiguration.GetPostsDirectory();
        }

        public bool FileExists(string postName)
        {
            var test = @GetFullName(postName);

            if (System.IO.File.Exists(@GetFullName(postName)))
            {
                return true;
            }

            return false;
        }

        private string GetFullName(string postName)
        {
            return PostsDirectory + postName + FileExtension;
        }

        public void WritePost(string postName, string postFile)
        {
            using (StreamWriter outputFile = new StreamWriter(GetFullName(postName)))
            {
                outputFile.Write(postFile);
            };
        }

        public void DeletePost(string postName)
        {
            System.IO.File.Delete(@GetFullName(postName));
        }

        public string GetPost(string postName)
        {
            using (StreamReader fileReader = new StreamReader(GetFullName(postName)))
            {
                return fileReader.ReadToEnd();
            };
        }

        public List<string> GetPostList()
        {
            DirectoryInfo postsDirectory = new DirectoryInfo(@PostsDirectory);
            FileInfo[] posts = postsDirectory.GetFiles("*" + FileExtension);

            return posts.Select(x => x.Name.Substring(0, x.Name.Length - ExtensionFileLength)).ToList();
        }
    }
}