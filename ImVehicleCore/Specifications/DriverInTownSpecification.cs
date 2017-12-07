using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ImVehicle.Core.Specifications
{
    public class DriverInTownSpecification : BaseSpecification<DriverItem>
    {
        public DriverInTownSpecification(long townId) : base(o => o.TownId == townId)
        {

        }
    }
}
