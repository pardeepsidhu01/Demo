using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class CreateOrEditEmployeeDto: EntityDto<int?>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public virtual int DesignationId { get; set; }
        public virtual int DepartmentId { get; set; }
    }
}
