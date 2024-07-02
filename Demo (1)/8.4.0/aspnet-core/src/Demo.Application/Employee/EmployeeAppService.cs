using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Demo.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Demo
{
    [AbpAuthorize(PermissionNames.Pages_Employee)]
    public class EmployeeAppService: DemoAppServiceBase
    {
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<Department, int> _departmentRepository;
        private readonly IRepository<Designation, int> _designationRepository;
        public EmployeeAppService(IRepository<Employee, int> employeeRepository, IRepository<Department, int> departmentRepository, IRepository<Designation, int> designationRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
        }
        public async Task<PagedResultDto<EmployeeDto>> GetAll(GetAllForEmployeeInput input)
        {
            try
            {
                var filteredEmployee = _employeeRepository.GetAll().Include(e => e.DepartmentFk).Include(e => e.DesignationFk)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter)||e.LastName.Contains(input.Filter)||e.EmailAddress.Contains(input.Filter));

                var pagedAndFilteredEmployee = filteredEmployee
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);


                var totalCount = await filteredEmployee.CountAsync();

                var dbList = await pagedAndFilteredEmployee.ToListAsync();
                var results = new List<EmployeeDto>();
                foreach (var o in dbList)
                {
                    var res = new EmployeeDto()
                    {
                        Id = o.Id,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        Address = o.Address,
                        EmailAddress = o.EmailAddress,
                        DepartmentId = o.DepartmentId,
                        DesignationId = o.DesignationId,
                        DepatmentName = o.DepartmentFk.Name,
                        DesignationName = o.DesignationFk.Name,
                    };

                    results.Add(res);
                }

                return new PagedResultDto<EmployeeDto>(
                    totalCount,
                    results
                );

            }
            catch (Exception ex)
            {
                return new PagedResultDto<EmployeeDto>(0, null);
            }

        }

        public async Task CreateOrEdit(CreateOrEditEmployeeDto input)
        {
            try
            {
                if (input.Id == null)
                {
                    await Create(input);
                }
                else
                {
                    await Update(input);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Employee_Create)]
        private async Task Create(CreateOrEditEmployeeDto input)
        {
            try
            {
                var employee = ObjectMapper.Map<Employee>(input);
              var id =  await _employeeRepository.InsertAndGetIdAsync(employee);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Employee_Edit)]
        private async Task Update(CreateOrEditEmployeeDto input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employee);
        }

        [AbpAuthorize(PermissionNames.Pages_Employee_Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            try
            {
                await _employeeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Employee_Edit)]
        public async Task<GetEmplyeeForEditOutput> GetEmployeeForEdit(EntityDto<int> input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetEmplyeeForEditOutput { CreateOrEditEmployeeDto = ObjectMapper.Map<CreateOrEditEmployeeDto>(employee) };
            return output;
        }
        public async Task<List<NameValueDto<int>>> GetNamesForDepartment()
        {
            var departmantNames = _departmentRepository.GetAll().Select(x =>new NameValueDto<int>
            {
                Name = x.Name,
                Value = x.Id
            }).Distinct().ToList();
            return departmantNames;
        }
        public async Task<List<NameValueDto<int>>> GetNamesForDesignation()
        {
            var designationNames = _designationRepository.GetAll().Select(x => new NameValueDto<int>
            {
                Name = x.Name,
                Value = x.Id
            }).Distinct().ToList();
            return designationNames;
        }
    }
}
