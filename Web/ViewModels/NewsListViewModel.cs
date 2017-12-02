﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class NewsListViewModel
    {

        public long Id { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [DataType( DataType.Date)]
        [Display(Name="日期")]
        public DateTime Date { get; set; }

        [Display(Name = "顺序")]
        public int Order { get; set; }
    }
}