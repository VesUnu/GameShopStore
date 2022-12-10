using System.ComponentModel.DataAnnotations;

namespace GameShopStore.Core.Dtos.UserDtos
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage ="The username must be between 3 and 30 characters.")]
        public string Username { get; set; } = null!;
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage ="The surname must be between 3 and 40 characters.")]
        public string Surname { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage ="Password must be between 4 and 10 characters.")]
        public string Password { get; set; } = null!;

        private string _phone = null!;
        [Phone]
        public string Phone { get { return _phone; } set { _phone = string.IsNullOrWhiteSpace(value)? null : value; } }
    }
}
