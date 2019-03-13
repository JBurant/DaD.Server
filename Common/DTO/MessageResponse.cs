using System.Collections.Generic;

namespace Common.DTO
{
    public class MessageResponse<T>
    {
        public MessageResponse()
        {
            Warnings = new List<string>();
            Errors = new List<Error>();
        }

        public T Message { get; set; }

        public List<string> Warnings { get; set; }

        public List<Error> Errors { get; set; }
    }
}