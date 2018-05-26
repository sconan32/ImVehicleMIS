
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Socona.ImVehicle.Core.Data
{
    public class VehicleOperation : BaseEntity
    {

        public VehicleOperationEvent Event { get; set; }

        [MaxLength(128)]
        public string IpAddr { get; set; }

        [MaxLength(512)]
        public string Url { get; set; }

        [MaxLength(512)]
        public string Summary { get; set; }

        public string OldData { get; set; }

        public string NewData { get; set; }


    }


    public enum VehicleOperationEvent
    {
        LoggingIn,
        LoggedIn,
        Logoff,

        Unauthorized,
        Authorized,

        VisitPage,

        Modify,
        Create,
        Delete,

        IllegalRequest,

    }
}