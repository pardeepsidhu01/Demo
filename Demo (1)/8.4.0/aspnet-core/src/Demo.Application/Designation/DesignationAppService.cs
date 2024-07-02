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
    [AbpAuthorize(PermissionNames.Pages_Designation)]
    public class DesignationAppService: DemoAppServiceBase
    {
        private readonly IRepository<Designation, int> _designationRepository;
        public DesignationAppService(IRepository<Designation, int> designationRepository)
        {
            _designationRepository = designationRepository;
        }
        public async Task<PagedResultDto<DesignationDto>> GetAll(GetAllDepartmetInput input)
        {
            try
            {
                var filteredDesignation = _designationRepository.GetAll()

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter));

                var pagedAndFilteredDesignation = filteredDesignation
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);


                var totalCount = await filteredDesignation.CountAsync();

                var dbList = await pagedAndFilteredDesignation.ToListAsync();
                var results = new List<DesignationDto>();
                foreach (var o in dbList)
                {
                    var res = new DesignationDto()
                    {
                        Id = o.Id,
                        Name = o.Name,




                    };

                    results.Add(res);
                }

                return new PagedResultDto<DesignationDto>(
                    totalCount,
                    results
                );

            }
            catch (Exception ex)
            {
                return new PagedResultDto<DesignationDto>(0, null);
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Designation_Create)]
        public async Task CreateOrEdit(CreateOrEditDesignationDto input)
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
        private async Task Create(CreateOrEditDesignationDto input)
        {
            var designation = ObjectMapper.Map<Designation>(input);
            await _designationRepository.InsertAsync(designation);
        }

        private async Task Update(CreateOrEditDesignationDto input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, designation);
        }
        [AbpAuthorize(PermissionNames.Pages_Designation_Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            try
            {
                await _designationRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AbpAuthorize(PermissionNames.Pages_Designation_Edit)]
        public async Task<GetDesignationForEditOutput> GetDepartmentForEdit(EntityDto<int> input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetDesignationForEditOutput { CreateOrEditDesignationDto = ObjectMapper.Map<CreateOrEditDesignationDto>(designation) };
            return output;
        }
    }
}
