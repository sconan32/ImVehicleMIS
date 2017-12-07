using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Specifications;

namespace Socona.ImVehicle.Web.ViewModels.Specifications
{
    public class GroupListVmQueryStringSpecification : QueryStringSpecification<GroupListViewModel>
    {
        public GroupListVmQueryStringSpecification(string queryString) : base(queryString) { }



       

    }
}
