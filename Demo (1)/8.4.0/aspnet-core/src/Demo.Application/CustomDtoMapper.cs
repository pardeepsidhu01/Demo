using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Department, CreateOrEditDepartmentDto>().ReverseMap();
            configuration.CreateMap<Designation, CreateOrEditDesignationDto>().ReverseMap();
            configuration.CreateMap<Employee, CreateOrEditEmployeeDto>().ReverseMap();
        }
    }
}
