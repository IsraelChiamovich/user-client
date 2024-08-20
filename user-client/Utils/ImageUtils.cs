namespace user_client.Utils
{
    public static class ImageUtils
    {
        public static byte[] ConvertFromIFormFile(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using MemoryStream stream = new();
            file.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
