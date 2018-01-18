using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ImVehicle.Core.Specifications
{
    public class DriverInTownSpecification : Specification<DriverItem>
    {
        public DriverInTownSpecification(long townId) : base(o => o.TownId == townId)
        {

        }
    }
}
