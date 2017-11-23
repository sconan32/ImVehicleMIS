using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImVehicleCore.Data;

namespace ImVehicleCore.Specifications
{
  public  class TownWithUserSpecification:BaseSpecification<TownItem>
    {
        public TownWithUserSpecification(string userName) : base(o=>o.Users.Any(t=>t.UserName==userName))
        { }
    }
}
