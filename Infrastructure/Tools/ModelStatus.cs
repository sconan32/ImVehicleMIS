using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socona.ImVehicle.Infrastructure.Tools
{
  
    public enum   ModelStatus
    {

        [Display(Name = "全部")]
        All = 0x0,


        [Display(Name="正常")]
        Ok=0x1,
        [Display(Name = "预警")]
        Warning =0x2,
       
    }
}
