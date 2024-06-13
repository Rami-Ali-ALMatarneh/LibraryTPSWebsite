namespace LibraryTPSWebsite.Extensions
    {
    public static class PhotoExtensions
        {
        public static string ConvertPhotoToString( IFormFile file, IWebHostEnvironment webHostEnvironment )
            {
            string uniqueFileName = null;
            string uniqueUpload = Path.Combine(webHostEnvironment.WebRootPath, "Image");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uniqueUpload, uniqueFileName);
            file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return uniqueFileName;
            }
        public static void DeletePhotoFromImageFolder( string url, IWebHostEnvironment webHostEnvironment )
            {
            string uniqueUpload = Path.Combine(webHostEnvironment.WebRootPath, "Image");
            string filePath = Path.Combine(uniqueUpload, url);
            if (File.Exists(filePath))
                {
                File.Delete(filePath);
                }
            }
        }
    }
