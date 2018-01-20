using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Socona.ImVehicle.Infrastructure.Extensions
{
    public static class ViewModelExtensions
    {

        private static string[] acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };
        public async static Task<byte[]> GetPictureByteArray(this IFormFile formFile)
        {
            if (formFile != null)
            {              
                if (acceptableExt.Contains(Path.GetExtension(formFile?.FileName)?.ToLower()))
                {
                    MemoryStream stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    return stream.ToArray();
                }
            }
            return null;
        }

    }
}
