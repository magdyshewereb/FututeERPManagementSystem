using ERPManagement.Application.Features.Events.Queries.GetEventsExport;
using System.Collections.Generic;

namespace ERPManagement.Application.Contracts.Infrastructure
{
	public interface ICsvExporter
	{
		byte[] ExportEventsToCsv(List<EventExportDto> eventExportDtos);
	}
}
