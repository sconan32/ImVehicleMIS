using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Specifications
{
  public  class TownWithUserSpecification:Specification<TownItem>
    {
        public TownWithUserSpecification(string userName) : base(o=>o.Users.Any(t=>t.UserName==userName))
        { }
    }
}
