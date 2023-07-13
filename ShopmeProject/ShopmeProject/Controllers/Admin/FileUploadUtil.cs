using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ShopmeProject.Controllers
{
    public class FileUploadUtil
    {
        public static void SaveFile(string uploadDir, string fileName, HttpPostedFileBase file)
        {
            var uploadPath = Path.Combine(uploadDir, fileName);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            using (var inputStream = file.InputStream)
            {
                using (var fileStream = File.Create(uploadPath))
                {
                    inputStream.CopyTo(fileStream);
                }
            }
        }

        public static void CleanDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                var dirPath = new DirectoryInfo(dir);

                foreach (var file in dirPath.EnumerateFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (IOException)
                    {
                        // Handle file delete error
                    }
                }
            }
        }
    }

}
