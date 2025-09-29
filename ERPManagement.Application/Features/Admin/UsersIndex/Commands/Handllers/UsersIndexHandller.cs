using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Features.Admin.UsersIndex.Commands.Models;
using ERPManagement.Application.Features.Common.Base.Commands;
using ERPManagement.Domain.Common;
using ERPManagement.Domain.Entities;
using Hr.Application.Interfaces.GenericRepository.Command;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;

namespace ERPManagement.Application.Features.Admin.UsersIndex.Commands.Handllers
{
    internal class UsersIndexHandller(IBaseRepository<UserIndex> _repo, IQueryGenericRepository<BusinessObject> repoquery
        , IMapper _mapper, IUnitOfwork _unitOfwork, ICurrentUserService _currentUser, IQueryGenericRepository<UserIndex> repouserindexquery)
        : BaseCommand<UserIndex, AuditableEntity>(_repo, _mapper, _unitOfwork)
        , ICommandHandler<UserPageColumns, IMessage>

    {
        public async Task<IMessage> Handle(UserPageColumns request, CancellationToken cancellationToken)
        {
            try
            {
                var pagedata = await repoquery.GetSingleAsync(i => i.WebUrl == request.PageUrl);
                var currentuserindexdata = await repouserindexquery.GetSingleAsync(i => i.PageId == pagedata.Id);
                if (pagedata != null)
                {


                    var userindex = _mapper.Map<UserIndex>(request);
                    userindex.UserId = _currentUser.GetUserId();
                    userindex.PageId = pagedata.Id;
                    if (currentuserindexdata != null)
                    {
                        _repo.Delete(Convert.ToInt32(currentuserindexdata.Id));
                    }

                    _repo.Add(userindex);
                    await _unitOfwork.SaveAsync();

                    return Success("Ok");






                }
                else
                {
                    return BadRequest<string>("Failed");
                }
            }
            catch (Exception e)
            {


                return BadRequest<string>(e.StackTrace);
            }

        }
    }
}
