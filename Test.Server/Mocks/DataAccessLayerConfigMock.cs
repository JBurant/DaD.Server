using Server.App_Config;

namespace Test.Server.Mocks
{
    public class DataAccessLayerConfigMock : IDataAccessLayerConfig
    {
        public const int ExtensionFileLength = 4;
        public const string FileExtension = ".txt";
        public const string PostsDirectory = "TestPosts/";

        public int GetExtensionFileLength()
        {
            return ExtensionFileLength;
        }

        public string GetFileExtension()
        {
            return FileExtension;
        }

        public string GetPostsDirectory()
        {
            return PostsDirectory;
        }
    }
}