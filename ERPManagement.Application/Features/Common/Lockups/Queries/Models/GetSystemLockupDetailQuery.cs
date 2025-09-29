using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Common.Lockups.Queries.Models
{
	public class GetSystemLockupDetailQuery : IQuery<List<SystemLockupDetailVm>>
	{
		public int? SystemLockupTypeID { get; set; }
	}
}
