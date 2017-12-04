﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Specifications;

namespace Web.ViewModels.Specifications
{
    public class TownListVmQueryStringSpecification : QueryStringSpecification<TownItemListViewModel>
    {
        public TownListVmQueryStringSpecification(string queryString) : base(queryString) { }



      

    }
}