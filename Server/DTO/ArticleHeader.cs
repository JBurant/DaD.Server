using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Server.DTO
{
    [DataContract]
    public class ArticleHeader
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public DateTime TimeCreated { get; set; }

        [DataMember]
        public DateTime TimeModified { get; set; }
    }
}
