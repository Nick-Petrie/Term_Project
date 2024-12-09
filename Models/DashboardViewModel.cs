using System.Collections.Generic;

namespace TermProject.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<MovieReview> MovieReviews { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}