using AList.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

namespace AList.Utility
{
    public class UtilityClass
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public UtilityClass(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        //public void HandleImage(IFormFile file, ref TModel model)
        //{
        //    // get root path
        //    string wwwRootPath = _hostEnvironment.WebRootPath;
        //    if (file != null)
        //    {
        //        string fileName = Guid.NewGuid().ToString();
        //        var uploads = Path.Combine(wwwRootPath, @"images\movies");
        //        var extension = Path.GetExtension(file.FileName);

        //        // if the image already exists delete the old one
        //        if (model.ImageUrl != null)
        //        {
        //            var oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));
        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }

        //        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
        //        {
        //            file.CopyTo(fileStreams);
        //        }
        //        model.ImageUrl = @"\images\movies\" + fileName + extension;

        //    }
        //}
    }
}
