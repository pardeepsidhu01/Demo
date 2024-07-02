using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class CreateOrEditDepartmentDto: EntityDto<int?>
    {
        public string Name { get; set; }
    }
}
