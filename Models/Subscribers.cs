using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TermProject.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class Subscribers
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30,ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public Gender? GenderIdentity { get; set; }
        [StringLength(50, ErrorMessage = "Please enter a valid address.")]
        public string? Address { get; set; }

        [StringLength(30, ErrorMessage = "Please enter a valid city.")]
        public string? City { get; set; }
        [StringLength(2, ErrorMessage ="Please enter an abbreviated state.")]
        public string? State { get; set; }

        [StringLength(10, ErrorMessage ="Please enter a valid zip code.")]
        public string? Zip {  get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Column("Subscriber_Email")]

        public string? email {  get; set; }
        [Display(Name = "Phone Number")]
        public string? phoneNumber { get; set; }

    }
}
