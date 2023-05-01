using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AList.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }

        [ValidateNever]
        [DisplayName("Image")]
        public string? ImageUrl { get; set; }
    }
}
