using System.Runtime.Serialization;

namespace Identity.DTO
{
    [DataContract]
    public class RegisterUserRequest
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}