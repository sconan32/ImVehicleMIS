using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImVehicleCore.Data
{
    public class DriverItem : BaseEntity
    {
        public VehicleLicenseType LicenseType { get; set; }


        public string Tel { get; set; }
        public string IdCardNumber { get; set; }

        public GenderType Gender { get; set; }


        public string LicenseNumber { get; set; }
        public DateTime LicenseIssueDate { get; set; }

        public int LicenseValidYears { get; set; }


        //public DateTime LicenseExpireDate { get; set; }



        public virtual List<VehicleItem> Vehicles { get; set; }


        public byte[] PhotoIdCard1 { get; set; }

        public byte[] PhotoIdCard2 { get; set; }

        public byte[] PhotoDriverLicense { get; set; }


        public byte[] PhotoWarranty { get; set; }
    }
    public enum VehicleLicenseType
    {

        A1 = 1,
        A2 = 2,
        A3 = 3,
        B1 = 4,
        B2 = 5,
        C1 = 6,
        C2 = 7,
        C3 = 8,
        C4 = 9,
        C5 = 10,
        D = 11,
        E = 12,
        F = 13,
        M = 14,
        N = 15,
        P = 16,
    }

    public enum GenderType
    {
        [Display(Name = "男")]
        Male = 1,
        [Display(Name = "女")]
        Female = 2,
    }
}
