using System.ComponentModel.DataAnnotations;

namespace onlineStore.Model
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
