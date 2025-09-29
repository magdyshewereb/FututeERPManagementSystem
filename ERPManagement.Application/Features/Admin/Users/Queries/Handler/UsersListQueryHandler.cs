namespace ERPManagement.Application.Features.Admin.Users.Queries.Handler
{
	//public class UsersListQueryHandler : ResponseHandler
	//    , IQuerytHandler<UsersListQuery, List<UsersListVm>>,
	//    IQuerytHandler<UserDetailQuery, BaseResponse<UserDetailVm>>
	//    , IQuerytHandler<RequestsNotifecationsQuery, BaseResponse<List<RequestsNotifecationsList>>>

	//{
	//    private readonly IMapper _mapper;

	//    private readonly UserManager<User> _userManager;

	//    IQueryGenericRepository<Roles> _roles;
	//    IQueryGenericRepository<Hr_Confirmations> _repoconirmfollowquery;
	//    ICurrentUserService _currentUserService;
	//    public UsersListQueryHandler(
	//                              IMapper mapper,
	//                              UserManager<User> userManager
	//                           , IQueryGenericRepository<Roles> roles
	//        , IQueryGenericRepository<Hr_Confirmations> repoconirmfollowquery, ICurrentUserService currentUserService)
	//    {
	//        _mapper = mapper;

	//        _userManager = userManager;
	//        _roles = roles;
	//        _repoconirmfollowquery = repoconirmfollowquery;
	//        _currentUserService = currentUserService;
	//    }




	//    public async Task<List<UsersListVm>> Handle(UsersListQuery request, CancellationToken cancellationToken)
	//    {

	//        var users = _userManager.Users.AsQueryable();
	//        var userslist = await _mapper.ProjectTo<UsersListVm>(users).ToListAsync();
	//        return userslist;
	//    }



	//    public async Task<BaseResponse<UserDetailVm>> Handle(UserDetailQuery request, CancellationToken cancellationToken)

	//    {

	//        var users = _userManager.Users.Where(i => i.Id == request.UserId).AsQueryable().Include(i => i.UserForms)
	//            .ThenInclude(i => i.BusinessObjects)
	//            ;
	//        var userroles = users.FirstOrDefault() != null ? await _userManager.GetRolesAsync(users.FirstOrDefault()) : null;
	//        var roledata = userroles != null ? await _roles.GetListAsync(i => userroles.Contains(i.Name)) : null;


	//        var userdata = users.Count() == 0 ? new UserDetailVm() : _mapper.ProjectTo<UserDetailVm>(users).FirstOrDefault();
	//        userdata.RoleId = userroles == null ? new List<int>() : roledata.Select(i => i.Id).ToList();
	//        return Success(userdata);

	//    }

	//    public async Task<BaseResponse<UserDetailVm>> Handle(UserDataByNameQuery request, CancellationToken cancellationToken)

	//    {

	//        var users = _userManager.Users.Where(i => i.UserName == request.UserName).AsQueryable();


	//        var userdata = _mapper.ProjectTo<UserDetailVm>(users).FirstOrDefault();

	//        return Success(userdata);

	//    }

	//    public async Task<BaseResponse<List<RequestsNotifecationsList>>> Handle(RequestsNotifecationsQuery request, CancellationToken cancellationToken)
	//    {
	//        var user = await _currentUserService.GetUserAllDataAsync();
	//        if (user.Employee == null)
	//        {
	//            return Success(new List<RequestsNotifecationsList>() { });
	//        }
	//        var resquery = await _repoconirmfollowquery.GetListAsync(i => i.Status == 0 && i.EmpId == user.Employee.ID && i.Notify == true);
	//        //get empaid for current user

	//        var resgrouping = resquery.GroupBy(i => new { i.EmpId, i.RequestTypeId });
	//        var res = resgrouping.Select(i => new RequestsNotifecationsList()
	//        {
	//            RequestCount = i.Count(),
	//            RequestTypeName = Enum.GetName(typeof(RequestType), i.Key.RequestTypeId)
	//        ,
	//            RequestTypeId = i.Key.RequestTypeId
	//        });
	//        return Success(res.ToList());
	//    }
	//}
}
