using System;
using System.Collections.Generic;

namespace PatientEHR.APISDK.FileManager
{
    public class ImageUtility
    {

        public static bool IsImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            List<string> exts = new List<string>();
            exts.AddRange(new string[] { ".png", ".jpg", ".jpeg" });
            string ext = System.IO.Path.GetExtension(fileName);
            if (exts.Contains(ext.ToLower()))
                return true;
            else
                return false;
        }

        public static bool IsVideo(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            List<string> exts = new List<string>();
            exts.AddRange(new string[] { ".mov", ".mp4" });
            string ext = System.IO.Path.GetExtension(fileName);
            if (exts.Contains(ext.ToLower()))
                return true;
            else
                return false;
        }
    }
}
