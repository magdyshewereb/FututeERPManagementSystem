using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UsersIndex.Queries.Models;
using ERPManagement.Application.Responses;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;

namespace ERPManagement.Application.Features.Admin.UsersIndex.Queries.Handller
{
    public class UsersIndexQueryHandler : BaseResponseHandler
        , IQuerytHandler<GetUsersIndexQuery, BaseResponse<UsersIndexVm>>

    {
        private readonly IMapper _mapper;

        private readonly IQueryGenericRepository<UserIndex> _repousersIndex;
        private readonly IQueryGenericRepository<BusinessObject> _repoQueryBussinessObject;


        ICurrentUserService _currentUser;

        public UsersIndexQueryHandler(
        IMapper mapper,
        UserManager<User> userManager,
        IQueryGenericRepository<UserIndex> repousersIndex,
        IQueryGenericRepository<BusinessObject> repoQueryBussinessObject,
        ICurrentUserService currentUser)
        {
            _mapper = mapper;

            _repousersIndex = repousersIndex;
            _currentUser = currentUser;
            _repoQueryBussinessObject = repoQueryBussinessObject;

        }
        public async Task<BaseResponse<UsersIndexVm>> Handle(GetUsersIndexQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var pagId = await _repoQueryBussinessObject.GetSingleAsync(i => i.WebUrl == request.PageUrl, i => i.Id);
            var usersIndex = await _repousersIndex.GetSingleAsync(i => i.PageId == pagId && i.UserId == userId);
            var usersIndexDetails = _mapper.Map<UsersIndexVm>(usersIndex);
            return Success(usersIndexDetails);
        }




    }
}
