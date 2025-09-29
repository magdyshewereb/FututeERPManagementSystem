using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPManagement.Application.Features.Admin.Users.Queries.Models
{
	public class RequestsNotifecationsList
	{
		public required string RequestTypeName { get; set; }
		public int RequestTypeId { get; set; }
		public int RequestCount { get; set; }
		public int EmpId { get; set; }
	}
}
