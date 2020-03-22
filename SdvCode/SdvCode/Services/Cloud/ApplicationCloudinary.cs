// Copyright (c) SDV Code Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SdvCode.Services.Cloud
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class ApplicationCloudinary
    {
        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile image, string name)
        {
            if (image != null)
            {
                byte[] destinationImage;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    destinationImage = memoryStream.ToArray();
                }

                using (var ms = new MemoryStream(destinationImage))
                {
                    // Cloudinary doesn't work with [?, &, #, \, %, <, >]
                    name = name.Replace("&", "And");
                    name = name.Replace("#", "sharp");
                    name = name.Replace("?", "questionMark");
                    name = name.Replace("\\", "right");
                    name = name.Replace("%", "percent");
                    name = name.Replace(">", "greater");
                    name = name.Replace("<", "lower");

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(name, ms),
                        PublicId = name,
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    return uploadResult.SecureUri.AbsoluteUri;
                }
            }

            return null;
        }

        public static void DeleteImage(Cloudinary cloudinary, string name)
        {
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
            };

            cloudinary.DeleteResources(delParams);
        }
    }
}