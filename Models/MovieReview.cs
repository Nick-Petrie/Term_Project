using System.ComponentModel.DataAnnotations;

namespace TermProject.Models
{
    public class MovieReview
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide the movie title.")]
        [StringLength(100, ErrorMessage = "Movie title can't exceed 100 characters.")]
        [Display(Name = "Movie Title")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "Please provide the reviewer's name.")]
        [StringLength(50, ErrorMessage = "Reviewer name can't exceed 50 characters.")]
        [Display(Name = "Reviewer Name")]
        public string ReviewerName { get; set; }

        [Required(ErrorMessage = "Please provide a rating between 1 and 10.")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Review text can't exceed 500 characters.")]
        [Display(Name = "Review Text")]
        public string ReviewText { get; set; }

        public int SubscribersId { get; set; }
        public Subscribers Subscriber { get; set; }
        public int GenreId { get; set; } 
        public Genre Genre { get; set; } 
    }
}