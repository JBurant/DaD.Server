using System.Runtime.Serialization;

namespace Server.DTO
{
    [DataContract]
    public class ArticleModel
    {
        [DataMember]
        public ArticleHeader ArticleHeader { get; set; }

        [DataMember]
        public string ArticleContent { get; set; }
    }
}