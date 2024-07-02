using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Employee: FullAuditedEntity
    {
         public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address {  get; set; } 
        
        public virtual int DesignationId { get; set; }

        [ForeignKey("DesignationId")]
        public Designation DesignationFk { get; set; }
        public virtual int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department DepartmentFk { get; set; }
    }
}
