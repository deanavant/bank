using System;
using System.ComponentModel.DataAnnotations;

namespace bank.Models
{
    public class BaseEntity { }

    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Account Account { get; set; }

        public User() { }
        
        public User(string f, string l, string e, string p)
        {
            First = f;
            Last = l;
            Email = e;
            Password = p;
        }
    }

    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage = "First name required")]
        [RegularExpression(@"^[a-zA-Z\s]{2,55}$", ErrorMessage = "Alpha characters only")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string First { get; set; }

        [Required(ErrorMessage = "Last name required")]
        [RegularExpression(@"^[a-zA-Z\s]{2,55}$", ErrorMessage = "Alpha characters only")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string Last { get; set; }

        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Valid email format required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        [Display(Name="Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        [Display(Name="Confirm Password")]
        public string Pwc { get; set; }
    }

    public class LoginViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Valid email format required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password { get; set; }
    }
}