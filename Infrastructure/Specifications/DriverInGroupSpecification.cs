using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ImVehicle.Core.Specifications
{
    public class DriverInGroupSpecification : BaseSpecification<DriverItem>
    {
        public DriverInGroupSpecification(int groupId) : base(t => t.GroupId == groupId) { }
    }
}
