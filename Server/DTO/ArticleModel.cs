using System.Runtime.Serialization;

namespace Server.DTO
{
    [DataContract]
    public class ArticleDTO
    {
        [DataMember]
        public ArticleHeader ArticleHeader { get; set; }

        [DataMember]
        public string ArticleContent { get; set; }
    }
}