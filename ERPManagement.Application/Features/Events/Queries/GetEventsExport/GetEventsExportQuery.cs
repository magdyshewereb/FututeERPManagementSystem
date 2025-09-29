using MediatR;

namespace ERPManagement.Application.Features.Events.Queries.GetEventsExport
{
	public class GetEventsExportQuery : IRequest<EventExportFileVm>
	{
	}
}
