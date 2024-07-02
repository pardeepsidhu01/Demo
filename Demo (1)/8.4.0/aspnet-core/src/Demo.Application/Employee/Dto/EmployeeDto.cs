using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class EmployeeDto: EntityDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public virtual int DesignationId { get; set; }
        public virtual int DepartmentId { get; set; }
        public string DepatmentName { get; set; }
        public string DesignationName { get; set; }
    }
}
