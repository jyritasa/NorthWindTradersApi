namespace WebApi2ElectricBoogaLoo.Controllers.shared
{
    public class EncodePictureToString
    {
        static public string FromOLE(byte[] picture)
        {
            byte[] originalPhoto = picture;
            //Trimming OLE bytes array off from the Photo
            byte[] trimmedPhoto = new byte[originalPhoto.Length - 78];
            Array.Copy(originalPhoto, 78, trimmedPhoto, 0, trimmedPhoto.Length);
            string base64EncodedImage = Convert.ToBase64String(trimmedPhoto);
            return base64EncodedImage;
        }
    }
}
