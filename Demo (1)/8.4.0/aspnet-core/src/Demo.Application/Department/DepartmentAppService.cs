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
    [AbpAuthorize(PermissionNames.Pages_Department)]
    public class DepartmentAppService :DemoAppServiceBase
    {
        private readonly IRepository<Department, int> _departmentRepository;
        public DepartmentAppService(IRepository<Department, int> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<PagedResultDto<DepartmentDto>> GetAll(GetAllDepartmetInput input)
        {
            try
            {
                var filteredDepartment = _departmentRepository.GetAll()

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter));
                       
                var pagedAndFilteredDepartment = filteredDepartment
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

              
                var totalCount = await filteredDepartment.CountAsync();

                var dbList = await pagedAndFilteredDepartment.ToListAsync();
                var results = new List<DepartmentDto>();
                foreach (var o in dbList)
                {
                    var res = new DepartmentDto()
                    {
                        Id = o.Id,
                        Name = o.Name,
                    };

                    results.Add(res);
                }

                return new PagedResultDto<DepartmentDto>(
                    totalCount,
                    results
                );

            }
            catch (Exception ex)
            {
                return new PagedResultDto<DepartmentDto>(0, null);
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Department_Create)]
        public async Task CreateOrEdit(CreateOrEditDepartmentDto input)
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
        private async Task Create(CreateOrEditDepartmentDto input)
        {
            var country = ObjectMapper.Map<Department>(input);
            await _departmentRepository.InsertAsync(country);
        }

        private async Task Update(CreateOrEditDepartmentDto input)
        {
            var country = await _departmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, country);
        }
        [AbpAuthorize(PermissionNames.Pages_Department_Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            try
            {
                await _departmentRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Department_Edit)]
        public async Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto<int> input)
        {
            var country = await _departmentRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetDepartmentForEditOutput { CreateOrEditDepartmentDto = ObjectMapper.Map<CreateOrEditDepartmentDto>(country) };
            return output;
        }


    }
}
