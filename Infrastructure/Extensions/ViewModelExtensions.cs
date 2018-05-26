using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Tools;

namespace Socona.ImVehicle.Infrastructure.Extensions
{
    public static class ViewModelExtensions
    {
        public static IHostingEnvironment HostingEnvironment { get; set; }
        private static string[] acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", "gif", };
        public async static Task<byte[]> GetPictureByteArray(this IFormFile formFile)
        {
            if (formFile != null)
            {
                if (acceptableExt.Contains(Path.GetExtension(formFile?.FileName)?.ToLower()))
                {
                    MemoryStream stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    stream.Close();
                    return stream.ToArray();
                }
            }
            return null;
        }
        public async static Task<byte[]> GetPictureByteArray(this IFormFile formFile, string waterMark)
        {
            if (formFile != null)
            {
                if (acceptableExt.Contains(Path.GetExtension(formFile?.FileName)?.ToLower()))
                {
                    MemoryStream stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    stream.Close();
                    var outStream = WaterMark.Mark(stream, waterMark, HostingEnvironment.WebRootPath + "/fonts/AdobeHeitiStd-Regular.otf");
                    return outStream.ToArray();
                }
            }
            return null;
        }

        public static void DeleteFromServer(this UserFileItem userFile)
        {
            if (userFile?.Status != StatusType.OK)
            {
                throw new ArgumentException($"{nameof(userFile)} 是无效文件，已删除，或不存在");
            }
            string spath = Path.Combine(HostingEnvironment.WebRootPath, userFile.ServerPath);
            string recycledPath = Path.Combine(HostingEnvironment.WebRootPath, "trashes", Path.GetFileName(userFile.ServerPath));
            try
            {
                System.IO.File.Move(spath, recycledPath + ".deleted");
                using (var writer = File.CreateText(recycledPath + ".info"))
                {


                    var info = new
                    {
                        userFile.FileName,
                        userFile.ClientPath,
                        userFile.ServerPath,
                        userFile.Size,
                        userFile.Type,
                        userFile.DownloadCount,
                        userFile.Visibility,
                        userFile.TownId,
                        userFile.GroupId,
                        userFile.Id,
                        userFile.CreateBy,
                        userFile.CreationDate,
                        DeleteDate = DateTime.Now,
                    };
                    writer.Write(JsonConvert.SerializeObject(info));
                }
            }
            catch (IOException ex)
            {

            }
        }
        public static UserFileItem ToUserFile(this IFormFile formFile, string name = null)
        {

            var subfolder = (DateTime.Now.Second % 60).ToString();
            string serverFileName = Guid.NewGuid().ToString() + ".ufile";
            string serverPath = Path.Combine("upload", subfolder, serverFileName);


            FileStream fileToWrite = new FileStream(Path.Combine(HostingEnvironment.WebRootPath, serverPath), FileMode.Create, FileAccess.Write);
            formFile.CopyTo(fileToWrite);
            fileToWrite.Close();
            UserFileItem ufi = new UserFileItem()
            {
                ClientPath = formFile.FileName,
                VersionNumber = 1,
                Status = StatusType.OK,
                ContentType = formFile.ContentType,
                FileName = Path.GetFileName(formFile.FileName),
                ServerPath = serverPath,
                Size = formFile.Length,
                Type = Path.GetExtension(formFile.FileName),
                Name = name,
                CreationDate = DateTime.Now,

            };
            return ufi;
        }

        public static string ToBase64String(this UserFileItem userFile, string waterMark = null)
        {
            if (userFile != null && userFile.Status == StatusType.OK && acceptableExt.Contains(userFile.Type))
            {
                using (var stream = File.OpenRead(Path.Combine(HostingEnvironment.WebRootPath, userFile.ServerPath)))
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    if (string.IsNullOrWhiteSpace(waterMark))
                    {
                        return Convert.ToBase64String(ms.ToArray());
                    }
                    using (var outStream = WaterMark.Mark(ms, waterMark, HostingEnvironment.WebRootPath + "/fonts/AdobeHeitiStd-Regular.otf"))
                    {
                        return Convert.ToBase64String(outStream.ToArray());
                    }
                }
            }
            return "";
        }

        public static UserFileItem UpdateUserFile(this IFormFile uploadedFile, UserFileItem userFile, VehicleDbContext dbContext, VisibilityType visibility, string name = null, long? townId = null, long? groupId = null)
        {
            if (uploadedFile != null)
            {
                if (userFile != null)
                {
                    if (userFile?.Status == StatusType.OK)
                    {
                        userFile.DeleteFromServer();
                        userFile.Status = StatusType.Deleted;
                        dbContext.Entry(userFile).State = EntityState.Modified;
                    }
                }

                userFile = uploadedFile.ToUserFile(name);
                userFile.GroupId = groupId;
                userFile.TownId = townId;
                userFile.Visibility = visibility;
                dbContext.Files.Add(userFile);
            }
            return userFile;
        }
        public static byte[] GetWaterMarkImageArray(this MemoryStream stream, string waterMark)
        {
            var outStream = WaterMark.Mark(stream, waterMark, HostingEnvironment.WebRootPath + "/fonts/AdobeHeitiStd-Regular.otf");
            return outStream.ToArray();
        }
    }
}
