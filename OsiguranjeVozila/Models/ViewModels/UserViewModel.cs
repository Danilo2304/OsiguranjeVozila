using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public List<User>? Users { get; set; }

        [Required (ErrorMessage = "Unesi ime")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Unesi email")]
        [EmailAddress(ErrorMessage ="Unesi ispravan email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Unesi password")]
        [MinLength(6, ErrorMessage = "Password mora imati najmanje 6 karaktera")]
        public string Password { get; set; }

        public bool Admin { get; set; }
    }
}

