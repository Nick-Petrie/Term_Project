namespace TermProject.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieReview> MovieReviews { get; set; }
    }
}