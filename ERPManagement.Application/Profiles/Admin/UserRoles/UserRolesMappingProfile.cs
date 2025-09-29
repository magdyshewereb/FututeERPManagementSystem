using ERPManagement.Application.Features.Admin.UserRoles.Command.Models;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.CreateRole;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.UpdateRole;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRolesList;
using ERPManagement.Application.Features.Admin.Users.Commands.Models;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser;
using ERPManagement.Application.Features.Security.Authorization.Query;
using ERPManagement.Domain.Entities;

namespace RakezIntelliERP.Account.Application.Profiles
{
    public partial class MapperProfile
    {
        public void UserRolesMapping()
        {
            CreateMap<Role, GetRolesListResultVm>().ForMember(i => i.RolesBusinessObjects,
               i => i.MapFrom(i => i.RolesBusinessObjects))
               .ForMember(i => i.RoleName, i => i.MapFrom(i => i.Name));

            CreateMap<UserForm, MenueResult>().ForMember(
                i => i.MenueName, i => i.MapFrom(i => i.BusinessObjects.NameAr)).
                ForMember(
                i => i.ShowToUser, i => i.MapFrom(i => i.BusinessObjects.ShowToUser))
                .ForMember(
                i => i.MenueNameEn, i => i.MapFrom(i => i.BusinessObjects.Name))
                .ForMember(
                i => i.ParentId, i => i.MapFrom(i => i.BusinessObjects.BusinessObjectID))
                .ForMember(
                i => i.Action, i => i.MapFrom(i => i.BusinessObjects.WebUrl))
                .ForMember(
                i => i.AddData, i => i.MapFrom(i => i.AddData))
                .ForMember(
                i => i.UpdateData, i => i.MapFrom(i => i.UpdateData))
            .ForMember(
                i => i.DeleteData, i => i.MapFrom(i => i.DeleteData));

            CreateMap<Role, CreateRoleCommand>().ReverseMap().ForMember(i => i.Name, i => i.MapFrom(i => i.RoleName));
            CreateMap<Role, UpdateRoleCommand>().ReverseMap().ForMember(i => i.Name, i => i.MapFrom(i => i.RoleName));

            CreateMap<RoleBusinessObject, RolesBusinessObjectsVm>().ReverseMap()
                .ForMember(i => i.businessObjectId, i => i.MapFrom(i => i.BusinessObjectId));

            CreateMap<BusinessObject, RolesBusinessObjectsVmByRoleQueryResult>().ForMember(
                i => i.NameAr, i => i.MapFrom(i => i.NameAr))
            .ForMember(
            i => i.Id, i => i.MapFrom(i => i.Id))
            .ForMember(
                i => i.Items, i => i.MapFrom(i => i.ChildObject));

            CreateMap<RoleBusinessObject, RolesBusinessObjectsVmQueryResult>()
                .ForMember(i => i.NameAr, i => i.MapFrom(i => i.BusinessObject.NameAr))
                .ForMember(i => i.NameEn, i => i.MapFrom(i => i.BusinessObject.NameAr))
                .ForMember(i => i.BusinessObjectId, i => i.MapFrom(i => i.BusinessObject.Id))
                .ForMember(i => i.Id, i => i.MapFrom(i => i.Id));
            CreateMap<BusinessObject, RolesBusinessObjectsVmQueryResult>()
           .ForMember(i => i.NameAr, i => i.MapFrom(i => i.NameAr))
           .ForMember(i => i.NameEn, i => i.MapFrom(i => i.NameAr))
           .ForMember(i => i.Id, i => i.MapFrom(i => i.Id));


            CreateMap<User, CreateUserCommand>()
                .ForMember(i => i.UserName, i => i.MapFrom(i => i.UserName))
                .ForMember(i => i.Email, i => i.MapFrom(i => i.Email))
                //.ForMember(i => i.Password, i => i.MapFrom(i => i.Password))
                .ForMember(i => i.NameAr, i => i.MapFrom(i => i.NameAr))
                .ForMember(i => i.NameEn, i => i.MapFrom(i => i.NameEn))
                .ForMember(i => i.UserForms, i => i.MapFrom(i => i.UserForms)).ReverseMap();

            ; CreateMap<User, UpdateUserCommand>()
                .ForMember(i => i.UserName, i => i.MapFrom(i => i.UserName))
                .ForMember(i => i.Email, i => i.MapFrom(i => i.Email))
                //.ForMember(i => i.Password, i => i.MapFrom(i => i.Password))
                .ForMember(i => i.NameAr, i => i.MapFrom(i => i.NameAr))
                .ForMember(i => i.NameEn, i => i.MapFrom(i => i.NameEn))
                .ForMember(i => i.UserForms, i => i.MapFrom(i => i.UserForms)).ReverseMap();

            CreateMap<UserForm, BusinessObjectsVmQueryResult>()
              .ForMember(
          i => i.AddData, i => i.MapFrom(i => i.AddData))
                  .ForMember(
          i => i.UpdateData, i => i.MapFrom(i => i.UpdateData))
                      .ForMember(
          i => i.DeleteData, i => i.MapFrom(i => i.DeleteData))
          .ForMember(
          i => i.BusinessObjectId, i => i.MapFrom(i => i.BusinessObjectsId))
          .ForMember(i => i.businessObjectsVmQueryResult, i => i.MapFrom(i => i.BusinessObjects)).ReverseMap()
          ;
            CreateMap<UserForm, BusinessObjectsVmQueryResult>()
          .ForMember(
          i => i.AddData, i => i.MapFrom(i => i.AddData))
                  .ForMember(
          i => i.UpdateData, i => i.MapFrom(i => i.UpdateData))
                      .ForMember(
          i => i.DeleteData, i => i.MapFrom(i => i.DeleteData))
          .ForMember(
          i => i.BusinessObjectId, i => i.MapFrom(i => i.BusinessObjectsId))
          .
          ForMember(
          i => i.NameAr, i => i.MapFrom(i => i.BusinessObjects.NameAr))
          .ForMember(
          i => i.NameEn, i => i.MapFrom(i => i.BusinessObjects.Name));


            CreateMap<UserForm, RolesBusinessObjectsVmQueryResult>()
                .ForMember(
          i => i.Id, i => i.MapFrom(i => i.Id))
              .ForMember(
          i => i.AddData, i => i.MapFrom(i => i.AddData))
                  .ForMember(
          i => i.UpdateData, i => i.MapFrom(i => i.UpdateData))
                      .ForMember(
          i => i.DeleteData, i => i.MapFrom(i => i.DeleteData))
          .ForMember(
          i => i.BusinessObjectId, i => i.MapFrom(i => i.BusinessObjectsId))
          .
          ForMember(
          i => i.NameAr, i => i.MapFrom(i => i.BusinessObjects.NameAr))
          .ForMember(
          i => i.NameEn, i => i.MapFrom(i => i.BusinessObjects.Name));
        }
    }
}


