using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Common.Dictionary.Models
{
	public class GetDictionaryListQuery : IQuery<List<DictionaryVm>>
	{
		public string? PropName { get; set; }
	}
}
