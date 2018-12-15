using System.ComponentModel.DataAnnotations;

namespace Server.DTO
{
    public class AlphaNumString
    {
        [Required]
        [StringLength(60)]
        [RegularExpression("^[a-zA-Z]+$")]
        public string PostName { get; set; }
    }
}