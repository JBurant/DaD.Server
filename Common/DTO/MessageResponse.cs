using System.Collections.Generic;

namespace Common.DTO
{
    public class MessageResponse
    {
        public MessageResponse()
        {
            Message = "";
            Warnings = new List<string>();
            Errors = new List<Error>();
        }

        public string Message { get; set; }

        public List<string> Warnings { get; set; }

        public List<Error> Errors { get; set; }
    }
}