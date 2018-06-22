using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ImageResizer;


namespace MyWeb.Utility
{
  public static class FileManager
  {
    #region Fields
    private const string _imagesFolderPath = "~/Files/image";
    private const string _avatarsFolderPath = "~/Files/Avatar";
    private const string _userFileFolderPath = "~/Files/userFile";
    #endregion

    public static string UploadAvatarFile(this Controller controller, HttpPostedFileBase postedFile, string avatarName)
    {
      var fileName = avatarName +
        /*Path.GetExtension(postedFile.FileName)*/
        ".jpg";
      var imagePath = Path.Combine(controller.Server.MapPath(_avatarsFolderPath), fileName);
      postedFile.SaveAs(imagePath);
      ResizeImage(320, 240, 70, imagePath);
      return fileName;
    }

    private static void ResizeImage(int maxWidth, int maxHeight, int quality, string path)
    {
      //keep ratio
      var tempBitmap = new Bitmap(path);
      var ratioX = (double)maxWidth / tempBitmap.Width;
      var ratioY = (double)maxHeight / tempBitmap.Height;
      var ratio = Math.Min(ratioX, ratioY);

      var newWidth = (int)(tempBitmap.Width * ratio);
      var newHeight = (int)(tempBitmap.Height * ratio);

      tempBitmap.Dispose();

      var resizeSetting = new ResizeSettings
      {
        Width = newWidth,
        Height = newHeight,
        Quality = quality,
        Format = "jpg"
      };
      ImageBuilder.Current.Build(path, path, resizeSetting);
    }
  }
}
