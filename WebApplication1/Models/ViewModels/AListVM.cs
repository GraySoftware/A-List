namespace AList.Models.ViewModels
{
    public class AListVM
    {
        public ICollection<Actor> Actors { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
