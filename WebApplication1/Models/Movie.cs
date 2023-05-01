using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AList.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Director { get; set; }
        public virtual ICollection<Actor>? Actors { get; set; }

        [ValidateNever]
        [DisplayName("Image")]
        public string? ImageUrl { get; set; }
    }
}
