
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Socona.ImVehicle.Core.Data
{
    public class VehicleOperation : BaseEntity
    {

        public VehicleOperationEvent Event { get; set; }


        public string IpAddr { get; set; }

        public string Url { get; set; }

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