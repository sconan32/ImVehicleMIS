using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ImVehicle.Core.Specifications
{

    public class VehicleInTownSpecification : BaseSpecification<VehicleItem>
    {
        public VehicleInTownSpecification(long townId) : base(o => o.TownId == townId)
        {

        }
    }

}
