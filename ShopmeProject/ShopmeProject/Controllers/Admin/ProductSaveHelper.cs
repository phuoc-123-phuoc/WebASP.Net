using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace ShopmeProject.Controllers
{
    public class ProductSaveHelper
    {
        public static void setMainImageName(HttpPostedFileBase fileImage, Product product)
        {
            if (fileImage != null && fileImage.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileImage.FileName);
                product.MainImage = fileName;
            }
        }

        public static void setNewExtraImageNames(HttpPostedFileBase[] extraImage, Product product)
        {
            string fileName;
            if (extraImage.Length > 0)
            {
                foreach (var multipartFile in extraImage)
                {
                    if (multipartFile != null && multipartFile.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(multipartFile.FileName);
                        if (!product.Images.Any(x => x.Name == fileName))
                        {
                            product.Images.Add(new ProductImage() { Name = fileName });
                        }

                    }
                }
            }
        }

        public static void saveUploadedImages(HttpPostedFileBase fileImage, HttpPostedFileBase[] extraImage, string uploadDir)
        {
            // Debug.WriteLine(uploadDir);
            string fileName;
            if (fileImage != null && fileImage.ContentLength > 0)
            {
                fileName = Path.GetFileName(fileImage.FileName);
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, fileImage);
            }
            if (extraImage.Length > 0)
            {
                uploadDir = uploadDir + "/extras";
                foreach (var multipartFile in extraImage)
                {
                    if (multipartFile != null && multipartFile.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(multipartFile.FileName);
                        FileUploadUtil.SaveFile(uploadDir, fileName, multipartFile);

                    }
                }
            }
        }

        public static void setProductDetails(string[] detailNames,
            string[] detailValues, Product product, string[] detailIDs)
        {
            if (detailNames == null || detailNames.Length == 0 ) return;
            for (int count = 0; count < detailNames.Length; count++)
            {
                if(detailNames[count] != "" || detailValues[count] != "")
                {
                    int id = Convert.ToInt32(detailIDs[count]);
                    string name = detailNames[count];
                    string value = detailValues[count];
                    product.Details.Add(new ProductDetail() { Id = id, Name = name, Value = value });
                }
               
            }
          
        }

        public static void setExistingExtraImageNames(string[] imageIDs,
            string[] imageNames, Product product)
        {
            if (imageIDs == null || imageIDs.Length == 0) return;
            product.Images.Clear();
            for (int count = 0; count < imageIDs.Length; count++)
            {
                int id = Convert.ToInt32(imageIDs[count]);
                string name = imageNames[count];

                product.Images.Add(new ProductImage() {Id = id, Name = name });
            }

           

        }

        public static void deleteExtraImagesWeredRemovedOnForm(Product product, string uploadDir)
        {
            string extraImageDir = uploadDir + "/extras";
            DirectoryInfo dirPath = new DirectoryInfo(extraImageDir);
            foreach (FileInfo file in dirPath.GetFiles())
            {
                string filename = file.Name;

                if (!product.Images.Any(x => x.Name == filename))
                {
                    file.Delete();
                }
            }
        }
    }
}