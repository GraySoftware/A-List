using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AList.Models.ViewModels
{
    public class ActorVM
    {

        public Actor Actor { get; set; }

        [ValidateNever]
        public List<int> SelectedMovieIds { get; set; }
    }
}
