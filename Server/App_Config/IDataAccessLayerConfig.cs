namespace Server.App_Config
{
    public interface IDataAccessLayerConfig
    {
        int GetExtensionFileLength();

        string GetFileExtension();

        string GetPostsDirectory();
    }
}