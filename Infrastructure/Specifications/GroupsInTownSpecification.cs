﻿

using System;
using System.Linq.Expressions;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Specifications
{

    public class GroupsInTownSpecification : Specification<GroupItem>
    {
        public GroupsInTownSpecification(long townId) : base(o => o.TownId == townId)
        {

        }
    }
}
