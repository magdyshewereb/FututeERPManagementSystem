using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Common.Base.Queries;
using ERPManagement.Application.Features.Security.Authorization.Query;
using ERPManagement.Application.Responses;
using ERPManagement.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;

public class QueryHandler : CachingBaseQueryHandler<UserForm, MenueResult>,
    IQuerytHandler<MenueQuery, BaseResponse<List<MenueResult>>>,
    IQuerytHandler<MenueSingleQuery, BaseResponse<MenueResult>>
{
    public QueryHandler(
        IQueryGenericRepository<UserForm> modelRepository,
        IMapper mapper,
        IMemoryCache memoryCache)
        : base(modelRepository, mapper, memoryCache)
    {
    }

    public async Task<BaseResponse<MenueResult>> Handle(MenueSingleQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<UserForm, object>>>
        {
            x => x.User,
            x => x.BusinessObjects
        };

        var res = await GetDetailsAsync(
            i => i.User.UserName == request.UserName &&
                 i.BusinessObjects.WebUrl.ToLower() == request.WebUrl.ToLower(),
            includes: includes
        );

        return Success(res);
    }

    public async Task<BaseResponse<List<MenueResult>>> Handle(MenueQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<UserForm, object>>>
        {
            x => x.User,
            x => x.BusinessObjects
        };

        var res = await GetListAsync(
            i => i.User.UserName == request.UserName,
            includes: includes
        );

        return Success(res);
    }
}
