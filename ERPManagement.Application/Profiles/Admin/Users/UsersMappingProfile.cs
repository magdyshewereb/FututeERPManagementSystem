using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUserDetail;
using ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUsersList;
using ERPManagement.Application.Features.Admin.UsersIndex.Commands.Models;
using ERPManagement.Application.Features.Admin.UsersIndex.Queries.Models;
using ERPManagement.Domain.Entities;

namespace RakezIntelliERP.Account.Application.Profiles
{
    public partial class MapperProfile
    {

        public void UsersMapping()
        {

            CreateMap<User, UserDetailVm>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UsersListVm>().ReverseMap();
            CreateMap<UserIndex, UserPageColumns>().ForMember(i => i.ColumnsName
          , i => i.MapFrom(i => i.VisibleColumns)).ForMember(
            i => i.PageId
          , i => i.MapFrom(i => i.PageId)).ReverseMap();
            CreateMap<UserIndex, UsersIndexVm>().ForMember(i => i.ColumnsName
                , i => i.MapFrom(i => i.VisibleColumns)).ReverseMap();
        }
    }
}
