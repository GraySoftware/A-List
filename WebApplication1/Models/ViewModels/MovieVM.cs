using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AList.Models.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; }

        [ValidateNever]
        public List<int> SelectedActorIds { get; set; }
    }
}
