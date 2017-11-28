using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImVehicleCore.Data
{
    public static class VehicleDbContextSeed
    {

        public static async Task SeedAsync(VehicleDbContext dbContext,
          ILoggerFactory loggerFactory, int? retry = 0)
        {
           
            dbContext.Newses.RemoveRange(dbContext.Newses);
            dbContext.Drivers.RemoveRange(dbContext.Drivers);
            dbContext.Vehicles.RemoveRange(dbContext.Vehicles);
            dbContext.Groups.RemoveRange(dbContext.Groups);
            dbContext.Towns.RemoveRange(dbContext.Towns);
            dbContext.Districts.RemoveRange(dbContext.Districts);

            DistrictItem district = new DistrictItem() { Name = "甘井子区", };
            await dbContext.Districts.AddAsync(district);
            TownItem[] towns =
            {
                 new TownItem(){Name="周水子街道",Address="周水子街道", District=district, Code=1,},
                 new TownItem(){Name="椒金山街道",Address="椒金山街道", District=district, Code=2,},
                 new TownItem(){Name="甘井子街道",Address="甘井子街道", District=district, Code=3,},
                 new TownItem(){Name="南关岭街道",Address="南关岭街道", District=district, Code=4,},
                 new TownItem(){Name="泡崖街道",Address="泡崖街道", District=district, Code=5,},
                 new TownItem(){Name="中华路街道",Address="中华路街道", District=district, Code=6,},
                 new TownItem(){Name="兴华街道",Address="兴华街道", District=district, Code=7,},
                 new TownItem(){Name="机场街道",Address="机场街道", District=district, Code=8,},
                 new TownItem(){Name="辛寨子街道",Address="辛寨子街道", District=district, Code=9,},
                 new TownItem(){Name="红旗街道",Address="红旗街道", District=district, Code=10,},
                 new TownItem(){Name="凌水街道",Address="凌水街道", District=district, Code=11,},
                 new TownItem(){Name="大连湾街道",Address="大连湾街道", District=district, Code=12,},
                 new TownItem(){Name="泉水街道",Address="泉水街道", District=district, Code=13,},
                 new TownItem(){Name="革镇堡街道",Address="革镇堡街道", District=district, Code=14,},
                 new TownItem(){Name="营城子街道",Address="营城子街道", District=district, Code=15,},
                 new TownItem(){Name="七贤岭街道",Address="七贤岭街道", District=district, Code=16,},
                 new TownItem(){Name="大连华侨果树农场",Address="大连华侨果树农场", District=district, Code=17,},
                 new TownItem(){Name="其它街道",Address="其它街道", District=district, Code=18,},
            };
            await dbContext.Towns.AddRangeAsync(towns);

            GroupItem[] groups =
            {

                new GroupItem(){Name="周东社区", ChiefName="张三",ChiefTel="13555654554",TownId=towns[0].Id,},
                new GroupItem(){Name="周发社区", ChiefName="李四",ChiefTel="12444424555",TownId=towns[0].Id,},
                new GroupItem(){Name="周兴社区", ChiefName="王五",ChiefTel="1884424555",TownId=towns[0].Id,},
                new GroupItem(){Name="周西社区", ChiefName="赵六",ChiefTel="124342324555",TownId=towns[0].Id,},
                new GroupItem(){Name="周南社区", ChiefName="冯七",ChiefTel="1777724555",TownId=towns[0].Id,},
                new GroupItem(){Name="周盛社区", ChiefName="刘八",ChiefTel="12488884555",TownId=towns[0].Id,},
                new GroupItem(){Name="周顺社区", ChiefName="唐九",ChiefTel="12444989766",TownId=towns[0].Id,},
                new GroupItem(){Name="周水子社区", ChiefName="滕十",ChiefTel="12448889455",TownId=towns[0].Id,},
                new GroupItem(){Name="周北社区", ChiefName="贾十一",ChiefTel="124444774435",TownId=towns[0].Id,},
            };
            await dbContext.Groups.AddRangeAsync(groups);

            DriverItem[] drivers =
            {
                new DriverItem(){Name="容可佳", Gender= GenderType.Male, IdCardNumber="111333122233445566", LicenseType= VehicleLicenseType.A1, LicenseNumber="5553445667", LicenseIssueDate=new DateTime(2003,3,3), LicenseValidYears=10,Tel="12244452677",},
                new DriverItem(){Name="禄冰露", Gender= GenderType.Male, IdCardNumber="222333122233445566", LicenseType= VehicleLicenseType.A2, LicenseNumber="6663445667", LicenseIssueDate=new DateTime(2004,3,3), LicenseValidYears=10,Tel="12344453677",},
                new DriverItem(){Name="叶饮香", Gender= GenderType.Female, IdCardNumber="333333122233445566", LicenseType= VehicleLicenseType.A3, LicenseNumber="7773445667", LicenseIssueDate=new DateTime(2005,3,3), LicenseValidYears=10,Tel="12444454677",},
                new DriverItem(){Name="程欣悦", Gender= GenderType.Male, IdCardNumber="444333122233445566", LicenseType= VehicleLicenseType.B1, LicenseNumber="8883445667", LicenseIssueDate=new DateTime(2006,3,3), LicenseValidYears=10,Tel="12564455677",},
                new DriverItem(){Name="阚琼诗", Gender= GenderType.Male, IdCardNumber="555333111233445566", LicenseType= VehicleLicenseType.B2, LicenseNumber="9991235667", LicenseIssueDate=new DateTime(2007,3,3), LicenseValidYears=10,Tel="12644456677",},
                new DriverItem(){Name="耿许洌", Gender= GenderType.Female, IdCardNumber="666333222233445566", LicenseType= VehicleLicenseType.C3, LicenseNumber="0002345667", LicenseIssueDate=new DateTime(2007,1,30), LicenseValidYears=10,Tel="12744456677",},
                new DriverItem(){Name="富秋翠", Gender= GenderType.Male, IdCardNumber="777333333233445566", LicenseType= VehicleLicenseType.M, LicenseNumber="2232345111", LicenseIssueDate=new DateTime(2008,2,22), LicenseValidYears=10,Tel="12814456677",},
                new DriverItem(){Name="贺叶飞", Gender= GenderType.Male, IdCardNumber="888333444233445566", LicenseType= VehicleLicenseType.P, LicenseNumber="2234565222", LicenseIssueDate=new DateTime(2009,3,1), LicenseValidYears=10,Tel="12392456677",},
                new DriverItem(){Name="莘山芙", Gender= GenderType.Male, IdCardNumber="222333555233445566", LicenseType= VehicleLicenseType.C1, LicenseNumber="2235675333", LicenseIssueDate=new DateTime(2000,4,2), LicenseValidYears=10,Tel="12303456677",},
                new DriverItem(){Name="卓秀华", Gender= GenderType.Female, IdCardNumber="222333666233445566", LicenseType= VehicleLicenseType.C1, LicenseNumber="2236785444", LicenseIssueDate=new DateTime(2003,5,3), LicenseValidYears=10,Tel="12344456677",},
                new DriverItem(){Name="孟滇萍", Gender= GenderType.Male, IdCardNumber="222333777233445566", LicenseType= VehicleLicenseType.A1, LicenseNumber="2237894555", LicenseIssueDate=new DateTime(2003,6,4), LicenseValidYears=10,Tel="12345456677",},
                new DriverItem(){Name="尹从南", Gender= GenderType.Male, IdCardNumber="222333888233445566", LicenseType= VehicleLicenseType.A2, LicenseNumber="2238905666", LicenseIssueDate=new DateTime(2003,7,5), LicenseValidYears=10,Tel="12346456677",},
                new DriverItem(){Name="菱芳蔼", Gender= GenderType.Male, IdCardNumber="222333999233445566", LicenseType= VehicleLicenseType.A3, LicenseNumber="2233445777", LicenseIssueDate=new DateTime(2003,8,6), LicenseValidYears=10,Tel="12347456677",},

            };
            await dbContext.Drivers.AddRangeAsync(drivers);



            VehicleItem[] vehicles =
            {
                new VehicleItem(){LicenceNumber="辽BB1123", Brand="本田", Name="不知C4", Color="黑", Type= VehicleType.HeavyAuto, LastRegisterDate=new DateTime(2015,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB1X23",Brand="本田",  Name="高级C4", Color="红",Type= VehicleType.HeavyLorry, LastRegisterDate=new DateTime(2016,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[1].Id},
                new VehicleItem(){LicenceNumber="辽BB2T23",Brand="本田", Name="不知C4",  Color="黄",Type= VehicleType.CommercialVehicle, LastRegisterDate=new DateTime(2017,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[2].Id},
                new VehicleItem(){LicenceNumber="辽BB8763",Brand="本田", Name="不知C4",  Color="蓝",Type= VehicleType.HeavyLorry, LastRegisterDate=new DateTime(2017,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[3].Id},
                new VehicleItem(){LicenceNumber="辽BBAF23",Brand="现代",  Name="不知C4", Color="绿",Type= VehicleType.SchoolBus, LastRegisterDate=new DateTime(2016,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[4].Id},
                new VehicleItem(){LicenceNumber="辽BB1444", Brand="五菱", Name="火舞C4", Color="黑",Type= VehicleType.TourBus, LastRegisterDate=new DateTime(2017,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB17U3",Brand="本田",  Name="不知C4", Color="黑",Type= VehicleType.PrivateCar, LastRegisterDate=new DateTime(2016,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[1].Id},
                new VehicleItem(){LicenceNumber="辽BB1M88",Brand="本田", Name="不知C4",  Color="黑",Type= VehicleType.DangerLorry, LastRegisterDate=new DateTime(2014,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[2].Id},
                new VehicleItem(){LicenceNumber="辽BB1T99",Brand="本田",  Name="火舞C4", Color="黑",Type= VehicleType.HeavyLorry, LastRegisterDate=new DateTime(2017,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[3].Id},
                new VehicleItem(){LicenceNumber="辽BB1225", Brand="本田", Name="高级C4", Color="黑",Type= VehicleType.HeavyAuto, LastRegisterDate=new DateTime(2016,1,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[4].Id},
                new VehicleItem(){LicenceNumber="辽BB22B4", Brand="五菱", Name="火舞C4", Color="蓝",Type= VehicleType.TourBus, LastRegisterDate=new DateTime(2017,2,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[0].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB33N4", Brand="五菱", Name="不知C4", Color="蓝",Type= VehicleType.DangerLorry, LastRegisterDate=new DateTime(2017,3,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[1].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB4T44", Brand="五菱", Name="火舞C4", Color="蓝",Type= VehicleType.SchoolBus, LastRegisterDate=new DateTime(2015,4,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[1].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB5G44", Brand="五菱", Name="不知C4", Color="黑",Type= VehicleType.TourBus, LastRegisterDate=new DateTime(2016,5,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[1].Id,DriverId=drivers[0].Id},
                new VehicleItem(){LicenceNumber="辽BB1A44", Brand="五菱", Name="不知C4", Color="黑",Type= VehicleType.SchoolBus, LastRegisterDate=new DateTime(2017,6,1), ProductionDate=new DateTime(2001,1,1),GroupId=groups[1].Id,DriverId=drivers[0].Id},
            };

            await dbContext.Vehicles.AddRangeAsync(vehicles);



            NewsItem[] newses =
            {
                 new NewsItem(){Name="你这有杂音我听不清楚",Content="你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="我刚才刚买的车在那个",Content="我刚才刚买的车在那个", Title="我刚才刚买的车在那个"},
                 new NewsItem(){ Name="在道边上底下大梁磕道牙上了",Content="在道边上底下大梁磕道牙上了", Title="在道边上底下大梁磕道牙上了"},
                 new NewsItem(){ Name="您的车辆出险了现在要报案是吗",Content="您的车辆出险了现在要报案是吗", Title="您的车辆出险了现在要报案是吗"},
                 new NewsItem(){ Name="先生你好什么时间出的事情",Content="先生你好什么时间出的事情", Title="先生你好什么时间出的事情"},
                 new NewsItem(){Name="刚刚时间刚刚出的",Content="刚刚时间刚刚出的", Title="刚刚时间刚刚出的"},
                 new NewsItem(){ Name="大概有几分钟啦就刚刚的事",Content="大概有几分钟啦就刚刚的事", Title="大概有几分钟啦就刚刚的事"},
                 new NewsItem(){ Name="半个小时是嘛就五十分钟",Content="半个小时是嘛就五十分钟", Title="半个小时是嘛就五十分钟"},
                 new NewsItem(){ Name="是在哪里出的事情是在大连嘛",Content="是在哪里出的事情是在大连嘛", Title="是在哪里出的事情是在大连嘛"},
                 new NewsItem(){ Name="是在辽宁省的大连市是吗？",Content="是在辽宁省的大连市是吗？", Title="是在辽宁省的大连市是吗？"},
                 new NewsItem(){ Name="在大连市的哪条路？甘井子革镇堡。",Content="在大连市的哪条路？甘井子革镇堡。", Title="在大连市的哪条路？甘井子革镇堡。"},
                 new NewsItem(){ Name="11你这有杂音我听不清楚",Content="11你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="22你这有杂音我听不清楚",Content="22你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="33你这有杂音我听不清楚",Content="33你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="44你这有杂音我听不清楚",Content="44你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="55你这有杂音我听不清楚",Content="55你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="66你这有杂音我听不清楚",Content="66你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
                 new NewsItem(){ Name="77你这有杂音我听不清楚",Content="77你这有杂音我听不清楚", Title="你这有杂音我听不清楚"},
            };

            await dbContext.Newses.AddRangeAsync(newses);

            await dbContext.SaveChangesAsync();
        }
    }
}
