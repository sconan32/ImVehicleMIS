using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;


namespace DbTrasferTool
{
    static class Program
    {


        static string PathPrefix = "";
        static void Main(string[] args)
        {
            Console.WriteLine("INFO: INIT DBContext");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                PathPrefix = "E:/Projects";
            }
            else
            {
                PathPrefix = "/home/ubuntu";
            }
            OldDbContext oldDb = new OldDbContext();
            

            var optionsBuilder = new DbContextOptionsBuilder<VehicleDbContext>();
            optionsBuilder.UseSqlite("Data Source=.\\imvehiclemis.db.dll;");
            var newDb = new VehicleDbContext(optionsBuilder.Options);




            var optionsBuilder2 = new DbContextOptionsBuilder<VehicleDbContext>();
            optionsBuilder2.UseMySql("Server=localhost;Database=imvehicledb;Uid=root;Pwd=Imvehicle@DbSvr;");
            var mySqlDb = new VehicleDbContext(optionsBuilder2.Options);
            Console.WriteLine("INFO: Update Groups ====================");
            UpdateGroups(oldDb, mySqlDb);
            UpdateGroupRules(oldDb, mySqlDb);
            UpdateGroupGuarantee(oldDb, mySqlDb);

            Console.WriteLine("INFO: Update Drivers ====================");

            UpdateDrivers(oldDb, mySqlDb);
            UpdateDriverIdPhoto(oldDb, mySqlDb);
            UpdateDriverWarrantyPhoto(oldDb, mySqlDb);

            Console.WriteLine("INFO: Update Vehicles ====================");

            UpdateVehicle(oldDb, mySqlDb);
            UpdateVehicleWarranty(oldDb, mySqlDb);
            UpdateVehicleGps(oldDb, mySqlDb);






        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void UpdateGroups(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var secGrp in oldDb.CarsGroup)
            {

                var newItem = newDb.Groups.FirstOrDefault(t => t.Name == secGrp.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"INFO: Add New Group {secGrp.Name}");
                    newItem = new GroupItem()
                    {

                        VersionNumber = 1,
                        CreationDate = DateTime.Now,
                        CreateBy = adminUser.Id,
                        Status = StatusType.OK,

                        Code = secGrp.Code,
                        Name = secGrp.Name,

                        ChiefName = secGrp.OfficialName,
                        ChiefTel = secGrp.Phone,
                        ChiefTitle = secGrp.OfficialPosition,

                        TownId = dlw.Id,
                        Comment = secGrp.Introduce,


                    };



                    try
                    {
                        newDb.Groups.Add(newItem);
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {secGrp.Name}");
                    }

                }
                var filePath = PathPrefix+"/version4/static/img/company/" + secGrp.Photo;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: {secGrp.Photo} @ {secGrp.Name}");
                    continue;
                }



                Console.WriteLine($"INFO: Find Image :{secGrp.Photo} @ {secGrp.Name}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentGroup,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "企业图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.Id,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.MainImageId;

                    if (newItem.MainImageId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.MainImageId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.MainImage = null;
                    newItem.MainImageId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {secGrp.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {secGrp.Name}");
                }
            }
        }
        public static void UpdateGroupRules(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var secGrp in oldDb.CarsGroup)
            {
                var newItem = newDb.Groups.FirstOrDefault(t => t.Name == secGrp.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"E: Entity Not Exist: {secGrp.Name}");
                    continue;
                }
                var filePath = PathPrefix+"/version4/static/img/rule/" + secGrp.Rules;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"E: File Not Exist : {secGrp.Rules}@{secGrp.Name}");
                    if (newItem.RuleFileId != null)
                    {
                        var tufi = newDb.Files.First(t => t.Id == newItem.RuleFileId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.RuleFile = null;
                    newItem.RuleFileId = null;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                    }
                    continue;
                }



                Console.WriteLine($"I: Loading File :{secGrp.Rules}@ {secGrp.Name}");
                //    var stream = File.OpenRead(filePath);
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);
                Console.WriteLine($"I: Writing File :{secGrp.Rules} @{secGrp.Name}");
                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentGroup,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "制度文件",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.Id,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.RuleFileId;

                    if (newItem.RuleFileId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.RuleFileId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.RuleFile = null;
                    newItem.RuleFileId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                }
            }
        }

        public static void UpdateGroupGuarantee(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var secGrp in oldDb.CarsGroup)
            {
                var newItem = newDb.Groups.FirstOrDefault(t => t.Name == secGrp.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"E: Entity Not Exist: {secGrp.Name}");
                    continue;
                }
                var filePath = PathPrefix + "/version4/static/img/guarantee/" + secGrp.Guarantee;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"E: File Not Exist : {secGrp.Guarantee}@{secGrp.Name}");
                    if (newItem.GroupGuranteeFileId != null)
                    {
                        var tufi = newDb.Files.First(t => t.Id == newItem.GroupGuranteeFileId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.GroupGuranteeFile = null;
                    newItem.GroupGuranteeFileId = null;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                    }
                    continue;
                }



                Console.WriteLine($"I: Loading File :{secGrp.Guarantee}@ {secGrp.Name}");
                //    var stream = File.OpenRead(filePath);
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);
                Console.WriteLine($"I: Writing File :{secGrp.Guarantee} @{secGrp.Name}");
                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    CreateBy = adminUser.Id,
                    ClientPath = filePath,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentGroup,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "保证书",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.Id,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.GroupGuranteeFileId;

                    if (newItem.GroupGuranteeFileId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.GroupGuranteeFileId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.GroupGuranteeFile = null;
                    newItem.GroupGuranteeFileId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"E: 更新数据库时发生错误 {secGrp.Name}");
                }
            }
        }


        public static void UpdateDrivers(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var driver in oldDb.CarsDriver.Include(t => t.Group))
            {
                var newItem = newDb.Drivers.FirstOrDefault(t => t.Name == driver.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"INFO: Add New Group {driver.Name}");
                    newItem = new DriverItem()
                    {
                        CreateBy = adminUser.Id,
                        VersionNumber = 1,
                        CreationDate = DateTime.Now,
                        Status = StatusType.OK,
                        Name = driver.Name,
                        Gender = driver.Sex == "男" ? GenderType.Male : GenderType.Female,
                        LicenseIssueDate = Convert.ToDateTime(driver.LicenseStart),
                        IdCardNumber = driver.IdCard,
                        LicenseType = driver.CarLicense,
                        LicenseValidYears = (int?)driver.LicenseTime,
                        LivingAddress = driver.LiveAddress,

                        Tel = driver.Phone,
                        TownId = dlw.Id,
                    };

                    var groupName = driver.Group.Name;
                    var group = newDb.Groups.FirstOrDefault(t => t.Name == groupName);
                    if (group != null)
                    {
                        newItem.GroupId = group.Id;
                        newItem.Group = null;
                    }
                    try
                    {
                        newDb.Drivers.Add(newItem);
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERRO: 更新数据库时发生错误 {driver.Name}");
                    }

                }
                var filePath = PathPrefix + "/version4/static/img/driverlicense/" + driver.DriverPhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Fount: {driver.DriverPhoto} @ {driver.Name}");
                    continue;
                }


                Console.WriteLine($"INFO: Find Image :{driver.DriverPhoto} @ {driver.Name}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "驾驶证图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.LicenseImageId;

                    if (newItem.LicenseImageId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.LicenseImageId);
                        Console.WriteLine($"I: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.LicenseImage = null;
                    newItem.LicenseImageId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }
        public static void UpdateDriverIdPhoto(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));

            foreach (var driver in oldDb.CarsDriver.Include(t => t.Group))
            {

                var newItem = newDb.Drivers.FirstOrDefault(t => t.Name == driver.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"ERROR: Driver Not Exist:{ driver.Name}");
                }

                var filePath = PathPrefix + "/version4/static/img/idcard/" + driver.IdPhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: {driver.IdPhoto} @ {driver.Name}");
                    continue;
                }


                Console.WriteLine($"INFO: Find Image :{driver.IdPhoto} @ {driver.Name}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "身份证图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.IdCardImage1Id;

                    if (newItem.IdCardImage1Id != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.IdCardImage1Id);
                        Console.WriteLine($"INFO: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.IdCardImage1 = null;
                    newItem.IdCardImage1Id = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }

        public static void UpdateDriverWarrantyPhoto(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");
            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));

            foreach (var driver in oldDb.CarsDriver.Include(t => t.Group))
            {

                var newItem = newDb.Drivers.FirstOrDefault(t => t.Name == driver.Name);
                if (newItem == null)
                {
                    Console.WriteLine($"ERROR: Driver Not Exist:{ driver.Name}");
                }

                var filePath = PathPrefix + "/version4/static/img/title/" + driver.TitlePhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: {driver.TitlePhoto} @ {driver.Name}");
                    continue;
                }


                Console.WriteLine($"INFO: Find Image :{driver.TitlePhoto} @ {driver.Name}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);

                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "资格证图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();

                    var oldFileId = newItem.ExtraImage1Id;

                    if (newItem.ExtraImage1Id != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.ExtraImage1Id);
                        Console.WriteLine($"INFO: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                    }
                    newItem.ExtraImage1 = null;
                    newItem.ExtraImage1Id = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }


        public static void UpdateVehicle(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");

            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var vehicle in oldDb.CarsCar.Include(t => t.Group))
            {
                var newItem = newDb.Vehicles.FirstOrDefault(t => t.LicenceNumber == vehicle.Plate);
                if (newItem == null)
                {
                    Console.WriteLine($"ERROR: Add New Vehicle {vehicle.Plate}");
                    newItem = new VehicleItem()
                    {

                        VersionNumber = 1,
                        CreationDate = DateTime.Now,
                        Status = StatusType.OK,
                        Name = vehicle.Plate,
                        LicenceNumber = vehicle.Plate,

                        Brand = vehicle.Brand,
                        Color = vehicle.Color,
                        Type = ParseVehicleType(vehicle.Type),
                        Usage = ParseVehicleUsage(vehicle.Property),
                        AuditExpiredDate = Convert.ToDateTime(vehicle.InsuranceEnd),
                        DumpDate = Convert.ToDateTime(vehicle.DumpTime),
                        RealOwner = vehicle.RealOwner,
                        Agent = vehicle.RealOwner,
                        GpsEnabled = !string.IsNullOrEmpty(vehicle._3g),
                        YearlyAuditDate = Convert.ToDateTime(vehicle.ValidityInspection),
                        FirstRegisterDate = vehicle.FirstRegistered,
                        VehicleStatus = vehicle.Status,
                        TownId = dlw.Id,
                    };
                    var driverName = oldDb.CarsDriverCars.Include(t => t.Driver).FirstOrDefault(t => t.CarId == vehicle.Id)?.Driver?.Name;
                    if (driverName != null)
                    {
                        var driver = newDb.Drivers.FirstOrDefault(t => t.Name == driverName);
                        if (driver != null)
                        {
                            newItem.DriverId = driver.Id;
                        }
                    }

                    var groupName = vehicle.Group.Name;
                    var group = newDb.Groups.FirstOrDefault(t => t.Name == groupName);
                    if (group != null)
                    {
                        newItem.GroupId = group.Id;
                    }
                    try
                    {
                        newDb.Vehicles.Add(newItem);
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"E: 更新数据库时发生错误 {vehicle.Plate}");
                    }

                }
                var filePath = PathPrefix + "/version4/static/img/car_driver/" + vehicle.DrivingPhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: {vehicle.DrivingPhoto} @ {vehicle.Plate}");
                    continue;
                }



                Console.WriteLine($"INFO: Find Image :{vehicle.DrivingPhoto} @ {vehicle.Plate}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);
                Console.WriteLine($"INFO: Image Copied");
                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "行驶证图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();
                    Console.WriteLine($"INFO: Image Updated");
                    var oldFileId = newItem.LicenseImageId;

                    if (newItem.LicenseImageId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.LicenseImageId);
                        Console.WriteLine($"INFO: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                        Console.WriteLine($"INFO: Image Deleted");
                    }
                    newItem.LicenseImage = null;
                    newItem.LicenseImageId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }
        public static void UpdateVehicleWarranty(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");

            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var vehicle in oldDb.CarsCar.Include(t => t.Group))
            {
                var newItem = newDb.Vehicles.FirstOrDefault(t => t.LicenceNumber == vehicle.Plate);
                if (newItem == null)
                {
                    Console.WriteLine($"ERROR: Driver Not Exist:{ vehicle.Plate}");
                }

                var filePath = PathPrefix + "/version4/static/img/car_buss/" + vehicle.ServicePhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: { vehicle.ServicePhoto} @ { vehicle.Plate}");
                    continue;
                }
                Console.WriteLine($"INFO: Find Image :{vehicle.DrivingPhoto} @ {vehicle.Plate}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);
                Console.WriteLine($"INFO: Image Copied");
                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "营运证图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();
                    Console.WriteLine($"INFO: Image Updated");
                    var oldFileId = newItem.ExtraImage1Id;

                    if (newItem.ExtraImage1Id != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.ExtraImage1Id);
                        Console.WriteLine($"INFO: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                        Console.WriteLine($"INFO: Image Deleted");
                    }
                    newItem.LicenseImage = null;
                    newItem.LicenseImageId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }


        public static void UpdateVehicleGps(OldDbContext oldDb, VehicleDbContext newDb)
        {
            var adminUser = newDb.Users.First(t => t.UserName == "admin");

            var dlw = newDb.Towns.First(t => t.Name.Contains("大连湾"));
            foreach (var vehicle in oldDb.CarsCar.Include(t => t.Group))
            {
                var newItem = newDb.Vehicles.FirstOrDefault(t => t.LicenceNumber == vehicle.Plate);
                if (newItem == null)
                {
                    Console.WriteLine($"ERROR: Driver Not Exist:{ vehicle.Plate}");
                }

                var filePath = PathPrefix + "/version4/static/img/car_gps/" + vehicle.GpsPhoto;
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: File Not Found: { vehicle.GpsPhoto} @ { vehicle.Plate}");
                    continue;
                }
                Console.WriteLine($"INFO: Find Image :{vehicle.GpsPhoto} @ {vehicle.Plate}");
                var subfolder = (DateTime.Now.Second % 60).ToString();
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine("upload", subfolder, serverFileName);
                FileInfo fi = new FileInfo(filePath);

                File.Copy(filePath, serverPath);
                Console.WriteLine($"INFO: Image Copied");
                UserFileItem ufi = new UserFileItem()
                {
                    ClientPath = filePath,
                    CreateBy = adminUser.Id,
                    VersionNumber = 1,
                    Status = StatusType.OK,
                    ContentType = GetMimeType(Path.GetExtension(filePath)),
                    FileName = Path.GetFileName(filePath),
                    ServerPath = serverPath,
                    Visibility = VisibilityType.CurrentDriver,
                    Size = fi.Length,
                    Type = Path.GetExtension(filePath),
                    Name = "GPS图片",
                    CreationDate = DateTime.Now,
                    GroupId = newItem.GroupId,
                };
                try
                {
                    newDb.Files.Add(ufi);
                    newDb.SaveChanges();
                    Console.WriteLine($"INFO: Image Updated");
                    var oldFileId = newItem.GpsImageId;

                    if (newItem.GpsImageId != null)
                    {

                        var tufi = newDb.Files.First(t => t.Id == newItem.GpsImageId);
                        Console.WriteLine($"INFO: Deleting File :{tufi.ServerPath} @ {newItem.Name}");
                        File.Delete(tufi.ServerPath);
                        newDb.Files.Remove(tufi);
                        Console.WriteLine($"INFO: Image Deleted");
                    }
                    newItem.GpsImage = null;
                    newItem.GpsImageId = ufi.Id;
                    try
                    {

                        newDb.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        newDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: 更新数据库时发生错误 {newItem.Name}");
                }
            }
        }

        private static VehicleType ParseVehicleType(string str)
        {
            var values = Enum.GetValues(typeof(VehicleType)).Cast<VehicleType>();
            var enumDictionary = values.Select(v => new { name = GetDisplayName(v), value = v, });
            var item = enumDictionary.FirstOrDefault(t => t.name == str);
            if (item != null)
            {
                return item.value;
            }
            return VehicleType.Other;
        }

        private static UsageType ParseVehicleUsage(string str)
        {
            var values = Enum.GetValues(typeof(UsageType)).Cast<UsageType>();
            var enumDictionary = values.Select(v => new { name = GetDisplayName(v), value = v, });
            var item = enumDictionary.FirstOrDefault(t => t.name == str);
            if (item != null)
            {
                return item.value;
            }
            return UsageType.NonCommercial;
        }

        public static string GetDisplayName<T>(this T enumValue)
        {
            string returnName = null;
            try
            {
                returnName = enumValue.GetType()
                   .GetMember(enumValue.ToString())
                   ?.First()
                   ?.GetCustomAttribute<DisplayAttribute>()
                   ?.GetName();
            }
            catch (Exception)
            { }
            if (returnName == null)
            {
                returnName = enumValue?.ToString() ?? "";
            }
            return returnName;
        }
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        {".323", "text/h323"},
        {".3g2", "video/3gpp2"},
        {".3gp", "video/3gpp"},
        {".3gp2", "video/3gpp2"},
        {".3gpp", "video/3gpp"},
        {".7z", "application/x-7z-compressed"},
        {".aa", "audio/audible"},
        {".AAC", "audio/aac"},
        {".aaf", "application/octet-stream"},
        {".aax", "audio/vnd.audible.aax"},
        {".ac3", "audio/ac3"},
        {".aca", "application/octet-stream"},
        {".accda", "application/msaccess.addin"},
        {".accdb", "application/msaccess"},
        {".accdc", "application/msaccess.cab"},
        {".accde", "application/msaccess"},
        {".accdr", "application/msaccess.runtime"},
        {".accdt", "application/msaccess"},
        {".accdw", "application/msaccess.webapplication"},
        {".accft", "application/msaccess.ftemplate"},
        {".acx", "application/internet-property-stream"},
        {".AddIn", "text/xml"},
        {".ade", "application/msaccess"},
        {".adobebridge", "application/x-bridge-url"},
        {".adp", "application/msaccess"},
        {".ADT", "audio/vnd.dlna.adts"},
        {".ADTS", "audio/aac"},
        {".afm", "application/octet-stream"},
        {".ai", "application/postscript"},
        {".aif", "audio/x-aiff"},
        {".aifc", "audio/aiff"},
        {".aiff", "audio/aiff"},
        {".air", "application/vnd.adobe.air-application-installer-package+zip"},
        {".amc", "application/x-mpeg"},
        {".application", "application/x-ms-application"},
        {".art", "image/x-jg"},
        {".asa", "application/xml"},
        {".asax", "application/xml"},
        {".ascx", "application/xml"},
        {".asd", "application/octet-stream"},
        {".asf", "video/x-ms-asf"},
        {".ashx", "application/xml"},
        {".asi", "application/octet-stream"},
        {".asm", "text/plain"},
        {".asmx", "application/xml"},
        {".aspx", "application/xml"},
        {".asr", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".atom", "application/atom+xml"},
        {".au", "audio/basic"},
        {".avi", "video/x-msvideo"},
        {".axs", "application/olescript"},
        {".bas", "text/plain"},
        {".bcpio", "application/x-bcpio"},
        {".bin", "application/octet-stream"},
        {".bmp", "image/bmp"},
        {".c", "text/plain"},
        {".cab", "application/octet-stream"},
        {".caf", "audio/x-caf"},
        {".calx", "application/vnd.ms-office.calx"},
        {".cat", "application/vnd.ms-pki.seccat"},
        {".cc", "text/plain"},
        {".cd", "text/plain"},
        {".cdda", "audio/aiff"},
        {".cdf", "application/x-cdf"},
        {".cer", "application/x-x509-ca-cert"},
        {".chm", "application/octet-stream"},
        {".class", "application/x-java-applet"},
        {".clp", "application/x-msclip"},
        {".cmx", "image/x-cmx"},
        {".cnf", "text/plain"},
        {".cod", "image/cis-cod"},
        {".config", "application/xml"},
        {".contact", "text/x-ms-contact"},
        {".coverage", "application/xml"},
        {".cpio", "application/x-cpio"},
        {".cpp", "text/plain"},
        {".crd", "application/x-mscardfile"},
        {".crl", "application/pkix-crl"},
        {".crt", "application/x-x509-ca-cert"},
        {".cs", "text/plain"},
        {".csdproj", "text/plain"},
        {".csh", "application/x-csh"},
        {".csproj", "text/plain"},
        {".css", "text/css"},
        {".csv", "text/csv"},
        {".cur", "application/octet-stream"},
        {".cxx", "text/plain"},
        {".dat", "application/octet-stream"},
        {".datasource", "application/xml"},
        {".dbproj", "text/plain"},
        {".dcr", "application/x-director"},
        {".def", "text/plain"},
        {".deploy", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dgml", "application/xml"},
        {".dib", "image/bmp"},
        {".dif", "video/x-dv"},
        {".dir", "application/x-director"},
        {".disco", "text/xml"},
        {".dll", "application/x-msdownload"},
        {".dll.config", "text/xml"},
        {".dlm", "text/dlm"},
        {".doc", "application/msword"},
        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".dot", "application/msword"},
        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
        {".dsp", "application/octet-stream"},
        {".dsw", "text/plain"},
        {".dtd", "text/xml"},
        {".dtsConfig", "text/xml"},
        {".dv", "video/x-dv"},
        {".dvi", "application/x-dvi"},
        {".dwf", "drawing/x-dwf"},
        {".dwp", "application/octet-stream"},
        {".dxr", "application/x-director"},
        {".eml", "message/rfc822"},
        {".emz", "application/octet-stream"},
        {".eot", "application/octet-stream"},
        {".eps", "application/postscript"},
        {".etl", "application/etl"},
        {".etx", "text/x-setext"},
        {".evy", "application/envoy"},
        {".exe", "application/octet-stream"},
        {".exe.config", "text/xml"},
        {".fdf", "application/vnd.fdf"},
        {".fif", "application/fractals"},
        {".filters", "Application/xml"},
        {".fla", "application/octet-stream"},
        {".flr", "x-world/x-vrml"},
        {".flv", "video/x-flv"},
        {".fsscript", "application/fsharp-script"},
        {".fsx", "application/fsharp-script"},
        {".generictest", "application/xml"},
        {".gif", "image/gif"},
        {".group", "text/x-ms-group"},
        {".gsm", "audio/x-gsm"},
        {".gtar", "application/x-gtar"},
        {".gz", "application/x-gzip"},
        {".h", "text/plain"},
        {".hdf", "application/x-hdf"},
        {".hdml", "text/x-hdml"},
        {".hhc", "application/x-oleobject"},
        {".hhk", "application/octet-stream"},
        {".hhp", "application/octet-stream"},
        {".hlp", "application/winhlp"},
        {".hpp", "text/plain"},
        {".hqx", "application/mac-binhex40"},
        {".hta", "application/hta"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".htt", "text/webviewhtml"},
        {".hxa", "application/xml"},
        {".hxc", "application/xml"},
        {".hxd", "application/octet-stream"},
        {".hxe", "application/xml"},
        {".hxf", "application/xml"},
        {".hxh", "application/octet-stream"},
        {".hxi", "application/octet-stream"},
        {".hxk", "application/xml"},
        {".hxq", "application/octet-stream"},
        {".hxr", "application/octet-stream"},
        {".hxs", "application/octet-stream"},
        {".hxt", "text/html"},
        {".hxv", "application/xml"},
        {".hxw", "application/octet-stream"},
        {".hxx", "text/plain"},
        {".i", "text/plain"},
        {".ico", "image/x-icon"},
        {".ics", "application/octet-stream"},
        {".idl", "text/plain"},
        {".ief", "image/ief"},
        {".iii", "application/x-iphone"},
        {".inc", "text/plain"},
        {".inf", "application/octet-stream"},
        {".inl", "text/plain"},
        {".ins", "application/x-internet-signup"},
        {".ipa", "application/x-itunes-ipa"},
        {".ipg", "application/x-itunes-ipg"},
        {".ipproj", "text/plain"},
        {".ipsw", "application/x-itunes-ipsw"},
        {".iqy", "text/x-ms-iqy"},
        {".isp", "application/x-internet-signup"},
        {".ite", "application/x-itunes-ite"},
        {".itlp", "application/x-itunes-itlp"},
        {".itms", "application/x-itunes-itms"},
        {".itpc", "application/x-itunes-itpc"},
        {".IVF", "video/x-ivf"},
        {".jar", "application/java-archive"},
        {".java", "application/octet-stream"},
        {".jck", "application/liquidmotion"},
        {".jcz", "application/liquidmotion"},
        {".jfif", "image/pjpeg"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpb", "application/octet-stream"},
        {".jpe", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".json", "application/json"},
        {".jsx", "text/jscript"},
        {".jsxbin", "text/plain"},
        {".latex", "application/x-latex"},
        {".library-ms", "application/windows-library+xml"},
        {".lit", "application/x-ms-reader"},
        {".loadtest", "application/xml"},
        {".lpk", "application/octet-stream"},
        {".lsf", "video/x-la-asf"},
        {".lst", "text/plain"},
        {".lsx", "video/x-la-asf"},
        {".lzh", "application/octet-stream"},
        {".m13", "application/x-msmediaview"},
        {".m14", "application/x-msmediaview"},
        {".m1v", "video/mpeg"},
        {".m2t", "video/vnd.dlna.mpeg-tts"},
        {".m2ts", "video/vnd.dlna.mpeg-tts"},
        {".m2v", "video/mpeg"},
        {".m3u", "audio/x-mpegurl"},
        {".m3u8", "audio/x-mpegurl"},
        {".m4a", "audio/m4a"},
        {".m4b", "audio/m4b"},
        {".m4p", "audio/m4p"},
        {".m4r", "audio/x-m4r"},
        {".m4v", "video/x-m4v"},
        {".mac", "image/x-macpaint"},
        {".mak", "text/plain"},
        {".man", "application/x-troff-man"},
        {".manifest", "application/x-ms-manifest"},
        {".map", "text/plain"},
        {".master", "application/xml"},
        {".mda", "application/msaccess"},
        {".mdb", "application/x-msaccess"},
        {".mde", "application/msaccess"},
        {".mdp", "application/octet-stream"},
        {".me", "application/x-troff-me"},
        {".mfp", "application/x-shockwave-flash"},
        {".mht", "message/rfc822"},
        {".mhtml", "message/rfc822"},
        {".mid", "audio/mid"},
        {".midi", "audio/mid"},
        {".mix", "application/octet-stream"},
        {".mk", "text/plain"},
        {".mmf", "application/x-smaf"},
        {".mno", "text/xml"},
        {".mny", "application/x-msmoney"},
        {".mod", "video/mpeg"},
        {".mov", "video/quicktime"},
        {".movie", "video/x-sgi-movie"},
        {".mp2", "video/mpeg"},
        {".mp2v", "video/mpeg"},
        {".mp3", "audio/mpeg"},
        {".mp4", "video/mp4"},
        {".mp4v", "video/mp4"},
        {".mpa", "video/mpeg"},
        {".mpe", "video/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpf", "application/vnd.ms-mediapackage"},
        {".mpg", "video/mpeg"},
        {".mpp", "application/vnd.ms-project"},
        {".mpv2", "video/mpeg"},
        {".mqv", "video/quicktime"},
        {".ms", "application/x-troff-ms"},
        {".msi", "application/octet-stream"},
        {".mso", "application/octet-stream"},
        {".mts", "video/vnd.dlna.mpeg-tts"},
        {".mtx", "application/xml"},
        {".mvb", "application/x-msmediaview"},
        {".mvc", "application/x-miva-compiled"},
        {".mxp", "application/x-mmxp"},
        {".nc", "application/x-netcdf"},
        {".nsc", "video/x-ms-asf"},
        {".nws", "message/rfc822"},
        {".ocx", "application/octet-stream"},
        {".oda", "application/oda"},
        {".odc", "text/x-ms-odc"},
        {".odh", "text/plain"},
        {".odl", "text/plain"},
        {".odp", "application/vnd.oasis.opendocument.presentation"},
        {".ods", "application/oleobject"},
        {".odt", "application/vnd.oasis.opendocument.text"},
        {".one", "application/onenote"},
        {".onea", "application/onenote"},
        {".onepkg", "application/onenote"},
        {".onetmp", "application/onenote"},
        {".onetoc", "application/onenote"},
        {".onetoc2", "application/onenote"},
        {".orderedtest", "application/xml"},
        {".osdx", "application/opensearchdescription+xml"},
        {".p10", "application/pkcs10"},
        {".p12", "application/x-pkcs12"},
        {".p7b", "application/x-pkcs7-certificates"},
        {".p7c", "application/pkcs7-mime"},
        {".p7m", "application/pkcs7-mime"},
        {".p7r", "application/x-pkcs7-certreqresp"},
        {".p7s", "application/pkcs7-signature"},
        {".pbm", "image/x-portable-bitmap"},
        {".pcast", "application/x-podcast"},
        {".pct", "image/pict"},
        {".pcx", "application/octet-stream"},
        {".pcz", "application/octet-stream"},
        {".pdf", "application/pdf"},
        {".pfb", "application/octet-stream"},
        {".pfm", "application/octet-stream"},
        {".pfx", "application/x-pkcs12"},
        {".pgm", "image/x-portable-graymap"},
        {".pic", "image/pict"},
        {".pict", "image/pict"},
        {".pkgdef", "text/plain"},
        {".pkgundef", "text/plain"},
        {".pko", "application/vnd.ms-pki.pko"},
        {".pls", "audio/scpls"},
        {".pma", "application/x-perfmon"},
        {".pmc", "application/x-perfmon"},
        {".pml", "application/x-perfmon"},
        {".pmr", "application/x-perfmon"},
        {".pmw", "application/x-perfmon"},
        {".png", "image/png"},
        {".pnm", "image/x-portable-anymap"},
        {".pnt", "image/x-macpaint"},
        {".pntg", "image/x-macpaint"},
        {".pnz", "image/png"},
        {".pot", "application/vnd.ms-powerpoint"},
        {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
        {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
        {".ppa", "application/vnd.ms-powerpoint"},
        {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
        {".ppm", "image/x-portable-pixmap"},
        {".pps", "application/vnd.ms-powerpoint"},
        {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
        {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        {".prf", "application/pics-rules"},
        {".prm", "application/octet-stream"},
        {".prx", "application/octet-stream"},
        {".ps", "application/postscript"},
        {".psc1", "application/PowerShell"},
        {".psd", "application/octet-stream"},
        {".psess", "application/xml"},
        {".psm", "application/octet-stream"},
        {".psp", "application/octet-stream"},
        {".pub", "application/x-mspublisher"},
        {".pwz", "application/vnd.ms-powerpoint"},
        {".qht", "text/x-html-insertion"},
        {".qhtm", "text/x-html-insertion"},
        {".qt", "video/quicktime"},
        {".qti", "image/x-quicktime"},
        {".qtif", "image/x-quicktime"},
        {".qtl", "application/x-quicktimeplayer"},
        {".qxd", "application/octet-stream"},
        {".ra", "audio/x-pn-realaudio"},
        {".ram", "audio/x-pn-realaudio"},
        {".rar", "application/octet-stream"},
        {".ras", "image/x-cmu-raster"},
        {".rat", "application/rat-file"},
        {".rc", "text/plain"},
        {".rc2", "text/plain"},
        {".rct", "text/plain"},
        {".rdlc", "application/xml"},
        {".resx", "application/xml"},
        {".rf", "image/vnd.rn-realflash"},
        {".rgb", "image/x-rgb"},
        {".rgs", "text/plain"},
        {".rm", "application/vnd.rn-realmedia"},
        {".rmi", "audio/mid"},
        {".rmp", "application/vnd.rn-rn_music_package"},
        {".roff", "application/x-troff"},
        {".rpm", "audio/x-pn-realaudio-plugin"},
        {".rqy", "text/x-ms-rqy"},
        {".rtf", "application/rtf"},
        {".rtx", "text/richtext"},
        {".ruleset", "application/xml"},
        {".s", "text/plain"},
        {".safariextz", "application/x-safari-safariextz"},
        {".scd", "application/x-msschedule"},
        {".sct", "text/scriptlet"},
        {".sd2", "audio/x-sd2"},
        {".sdp", "application/sdp"},
        {".sea", "application/octet-stream"},
        {".searchConnector-ms", "application/windows-search-connector+xml"},
        {".setpay", "application/set-payment-initiation"},
        {".setreg", "application/set-registration-initiation"},
        {".settings", "application/xml"},
        {".sgimb", "application/x-sgimb"},
        {".sgml", "text/sgml"},
        {".sh", "application/x-sh"},
        {".shar", "application/x-shar"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".sitemap", "application/xml"},
        {".skin", "application/xml"},
        {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
        {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
        {".slk", "application/vnd.ms-excel"},
        {".sln", "text/plain"},
        {".slupkg-ms", "application/x-ms-license"},
        {".smd", "audio/x-smd"},
        {".smi", "application/octet-stream"},
        {".smx", "audio/x-smd"},
        {".smz", "audio/x-smd"},
        {".snd", "audio/basic"},
        {".snippet", "application/xml"},
        {".snp", "application/octet-stream"},
        {".sol", "text/plain"},
        {".sor", "text/plain"},
        {".spc", "application/x-pkcs7-certificates"},
        {".spl", "application/futuresplash"},
        {".src", "application/x-wais-source"},
        {".srf", "text/plain"},
        {".SSISDeploymentManifest", "text/xml"},
        {".ssm", "application/streamingmedia"},
        {".sst", "application/vnd.ms-pki.certstore"},
        {".stl", "application/vnd.ms-pki.stl"},
        {".sv4cpio", "application/x-sv4cpio"},
        {".sv4crc", "application/x-sv4crc"},
        {".svc", "application/xml"},
        {".swf", "application/x-shockwave-flash"},
        {".t", "application/x-troff"},
        {".tar", "application/x-tar"},
        {".tcl", "application/x-tcl"},
        {".testrunconfig", "application/xml"},
        {".testsettings", "application/xml"},
        {".tex", "application/x-tex"},
        {".texi", "application/x-texinfo"},
        {".texinfo", "application/x-texinfo"},
        {".tgz", "application/x-compressed"},
        {".thmx", "application/vnd.ms-officetheme"},
        {".thn", "application/octet-stream"},
        {".tif", "image/tiff"},
        {".tiff", "image/tiff"},
        {".tlh", "text/plain"},
        {".tli", "text/plain"},
        {".toc", "application/octet-stream"},
        {".tr", "application/x-troff"},
        {".trm", "application/x-msterminal"},
        {".trx", "application/xml"},
        {".ts", "video/vnd.dlna.mpeg-tts"},
        {".tsv", "text/tab-separated-values"},
        {".ttf", "application/octet-stream"},
        {".tts", "video/vnd.dlna.mpeg-tts"},
        {".txt", "text/plain"},
        {".u32", "application/octet-stream"},
        {".uls", "text/iuls"},
        {".user", "text/plain"},
        {".ustar", "application/x-ustar"},
        {".vb", "text/plain"},
        {".vbdproj", "text/plain"},
        {".vbk", "video/mpeg"},
        {".vbproj", "text/plain"},
        {".vbs", "text/vbscript"},
        {".vcf", "text/x-vcard"},
        {".vcproj", "Application/xml"},
        {".vcs", "text/plain"},
        {".vcxproj", "Application/xml"},
        {".vddproj", "text/plain"},
        {".vdp", "text/plain"},
        {".vdproj", "text/plain"},
        {".vdx", "application/vnd.ms-visio.viewer"},
        {".vml", "text/xml"},
        {".vscontent", "application/xml"},
        {".vsct", "text/xml"},
        {".vsd", "application/vnd.visio"},
        {".vsi", "application/ms-vsi"},
        {".vsix", "application/vsix"},
        {".vsixlangpack", "text/xml"},
        {".vsixmanifest", "text/xml"},
        {".vsmdi", "application/xml"},
        {".vspscc", "text/plain"},
        {".vss", "application/vnd.visio"},
        {".vsscc", "text/plain"},
        {".vssettings", "text/xml"},
        {".vssscc", "text/plain"},
        {".vst", "application/vnd.visio"},
        {".vstemplate", "text/xml"},
        {".vsto", "application/x-ms-vsto"},
        {".vsw", "application/vnd.visio"},
        {".vsx", "application/vnd.visio"},
        {".vtx", "application/vnd.visio"},
        {".wav", "audio/wav"},
        {".wave", "audio/wav"},
        {".wax", "audio/x-ms-wax"},
        {".wbk", "application/msword"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wcm", "application/vnd.ms-works"},
        {".wdb", "application/vnd.ms-works"},
        {".wdp", "image/vnd.ms-photo"},
        {".webarchive", "application/x-safari-webarchive"},
        {".webtest", "application/xml"},
        {".wiq", "application/xml"},
        {".wiz", "application/msword"},
        {".wks", "application/vnd.ms-works"},
        {".WLMP", "application/wlmoviemaker"},
        {".wlpginstall", "application/x-wlpg-detect"},
        {".wlpginstall3", "application/x-wlpg3-detect"},
        {".wm", "video/x-ms-wm"},
        {".wma", "audio/x-ms-wma"},
        {".wmd", "application/x-ms-wmd"},
        {".wmf", "application/x-msmetafile"},
        {".wml", "text/vnd.wap.wml"},
        {".wmlc", "application/vnd.wap.wmlc"},
        {".wmls", "text/vnd.wap.wmlscript"},
        {".wmlsc", "application/vnd.wap.wmlscriptc"},
        {".wmp", "video/x-ms-wmp"},
        {".wmv", "video/x-ms-wmv"},
        {".wmx", "video/x-ms-wmx"},
        {".wmz", "application/x-ms-wmz"},
        {".wpl", "application/vnd.ms-wpl"},
        {".wps", "application/vnd.ms-works"},
        {".wri", "application/x-mswrite"},
        {".wrl", "x-world/x-vrml"},
        {".wrz", "x-world/x-vrml"},
        {".wsc", "text/scriptlet"},
        {".wsdl", "text/xml"},
        {".wvx", "video/x-ms-wvx"},
        {".x", "application/directx"},
        {".xaf", "x-world/x-vrml"},
        {".xaml", "application/xaml+xml"},
        {".xap", "application/x-silverlight-app"},
        {".xbap", "application/x-ms-xbap"},
        {".xbm", "image/x-xbitmap"},
        {".xdr", "text/plain"},
        {".xht", "application/xhtml+xml"},
        {".xhtml", "application/xhtml+xml"},
        {".xla", "application/vnd.ms-excel"},
        {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
        {".xlc", "application/vnd.ms-excel"},
        {".xld", "application/vnd.ms-excel"},
        {".xlk", "application/vnd.ms-excel"},
        {".xll", "application/vnd.ms-excel"},
        {".xlm", "application/vnd.ms-excel"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xlt", "application/vnd.ms-excel"},
        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
        {".xlw", "application/vnd.ms-excel"},
        {".xml", "text/xml"},
        {".xmta", "application/xml"},
        {".xof", "x-world/x-vrml"},
        {".XOML", "text/plain"},
        {".xpm", "image/x-xpixmap"},
        {".xps", "application/vnd.ms-xpsdocument"},
        {".xrm-ms", "text/xml"},
        {".xsc", "application/xml"},
        {".xsd", "text/xml"},
        {".xsf", "text/xml"},
        {".xsl", "text/xml"},
        {".xslt", "text/xml"},
        {".xsn", "application/octet-stream"},
        {".xss", "application/xml"},
        {".xtp", "application/octet-stream"},
        {".xwd", "image/x-xwindowdump"},
        {".z", "application/x-compress"},
        {".zip", "application/x-zip-compressed"},
        #endregion

        };

        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
    }
}
