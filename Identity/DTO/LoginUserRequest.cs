using System.Runtime.Serialization;

namespace Identity.DTO
{
    [DataContract]
    public class LoginUserRequest
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}