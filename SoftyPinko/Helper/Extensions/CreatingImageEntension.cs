namespace SoftyPinko.Helper.Extensions
{
    public static class CreatingImageExtension
    {
        public static string CreatingImage(this IFormFile imageFile, string root, string folderName)
        {
            string filename = "";
            if (filename.Length > 100)
            {
                filename = Guid.NewGuid() + imageFile.FileName.Substring(imageFile.FileName.Length - 64);
            }
            else
            {
                filename = Guid.NewGuid() + imageFile.FileName;
            }
            string path = Path.Combine(root, folderName, filename);
            using (FileStream stream = new FileStream(path,FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return filename;

        }
        public static void DeletingImage(this string imagePath, string root, string folderName)
        {
            string path = Path.Combine(root, folderName, imagePath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
