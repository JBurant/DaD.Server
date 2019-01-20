namespace Server.App_Config
{
    public class DataAccessLayerConfig : IDataAccessLayerConfig
    {
        public const int ExtensionFileLength = 4;
        public const string FileExtension = ".txt";
        public const string ArticlesDirectory = "Articles/";

        public int GetExtensionFileLength()
        {
            return ExtensionFileLength;
        }

        public string GetFileExtension()
        {
            return FileExtension;
        }

        public string GetArticlesDirectory()
        {
            return ArticlesDirectory;
        }
    }
}