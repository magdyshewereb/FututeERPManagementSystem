using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using System.Data;

namespace ERPManagement.UI.Shared
{
    public partial class NavTree
    {
       
        DataTable? dtforms;
        //List<UserForm> forms;
        UserLoginData loginForm;
        Dictionary<int, bool> expandedModules = new();
        Dictionary<int, bool> expandedGroups = new();
        //string ConnString="";

        bool IsModulesExpanded(int FormID) =>
        expandedModules.ContainsKey(FormID) && expandedModules[FormID];
        bool IsGroupsExpanded(int FormID) =>
            expandedGroups.ContainsKey(FormID) && expandedGroups[FormID];

        protected override async Task OnInitializedAsync()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Response.HasStarted)
            {
              dtforms  =  await protectedLocalStorageService.GetUserFormsAsync();           
            }
        }
     
      

        void ToggleModules(DataRow Modules)
        {
            if (!expandedModules.ContainsKey(int.Parse(Modules["FormID"].ToString())) )
            {
                expandedModules[int.Parse(Modules["FormID"].ToString())] = true;
            }
            else
            {
                expandedModules[int.Parse(Modules["FormID"].ToString())] = !expandedModules[int.Parse(Modules["FormID"].ToString())];
            }
            foreach (var groups in dtforms.Select(" ParentID= '" + Modules["FormID"].ToString()+"'"))
            {
                expandedGroups[int.Parse(groups["FormID"].ToString())] = false;
            }
        }
        void ToggleGroups(DataRow Modules, DataRow groups)
        {
            expandedGroups[int.Parse(groups["FormID"].ToString())] = !expandedGroups.ContainsKey(int.Parse(groups["FormID"].ToString())) || !expandedGroups[int.Parse(groups["FormID"].ToString())];
        }

        bool Collapsed=false;
        private string? Collapse => Collapsed ? "collapse" : "";
        void ToggleNavMenu()
        {
            Collapsed = !Collapsed;
        }
    }
}
