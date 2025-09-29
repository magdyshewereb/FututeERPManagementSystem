using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPManagement.Application.Features.Security.Authorization.Query
{
	public class MenueResult
	{

		public int ParentId { get; set; }
		public required string MenueName { get; set; }
		public required string MenueNameEn { get; set; }
		public string? Action { get; set; }
		public bool? AddData { get; set; }
		public bool? UpdateData { get; set; }
		public bool? DeleteData { get; set; }
		public bool ShowToUser { get; set; }

	}
}
