

using System;
using System.Linq.Expressions;
using ImVehicleCore.Data;

namespace ImVehicleCore.Specifications
{

    public class GroupsInTownSpecification : BaseSpecification<GroupItem>
    {
        public GroupsInTownSpecification(long townId) : base(o => o.TownId == townId)
        {

        }
    }
}
