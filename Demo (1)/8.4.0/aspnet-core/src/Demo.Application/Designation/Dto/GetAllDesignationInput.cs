﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
   public class GetAllDesignationInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
