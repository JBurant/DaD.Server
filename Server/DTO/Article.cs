using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
