using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Demo.Authorization
{
    public class DemoAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Department, L("Department"));
            context.CreatePermission(PermissionNames.Pages_Department_Create, L("CreateDepartment"));
            context.CreatePermission(PermissionNames.Pages_Department_Edit, L("EditDepartment"));
            context.CreatePermission(PermissionNames.Pages_Department_Delete, L("DeleteDepartment"));
            context.CreatePermission(PermissionNames.Pages_Designation, L("Designation"));
            context.CreatePermission(PermissionNames.Pages_Designation_Create, L("CreateDesignation"));
            context.CreatePermission(PermissionNames.Pages_Designation_Delete, L("DeleteDesignation"));
            context.CreatePermission(PermissionNames.Pages_Designation_Edit, L("EditDesignation"));
            context.CreatePermission(PermissionNames.Pages_Employee, L("Employee"));
            context.CreatePermission(PermissionNames.Pages_Employee_Create, L("CreateEmployee"));
            context.CreatePermission(PermissionNames.Pages_Employee_Edit, L("EditEmployee"));
            context.CreatePermission(PermissionNames.Pages_Employee_Delete, L("DeleteEmployee"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, DemoConsts.LocalizationSourceName);
        }
    }
}
