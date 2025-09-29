using ERPManagement.UI.DataModels.CustomsClearence;
using ERPManagement.UI.DataModels.General;
using ERPManagement.UI.DataModels.ProtectedLocalStorage;
using ERPManagement.UI.GeneralClasses;
using ERPManagement.UI.DataModels.Accounting;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Data;
//using System.Data.Linq.SqlClient;

namespace ERPManagement.UI.Pages.CustomClearence
{
    public partial class FrmConsigneesTree : ComponentBase
    {
        public string ConnectionString { get; set; }
        
        Consignee currentObject = new Consignee();
        Consignee selectedNode;
        List<City> lstCities { get; set; } = new();
        List<Country> lstCountries { get; set; } = new();

        List<string> invisibleColumns;
        Dictionary<string, Dictionary<string, string>> valueLists = new Dictionary<string, Dictionary<string, string>>();
        bool isNavMode = true;
        public int NodeLevel;
        GlobalVariables.States currentState = GlobalVariables.States.NavMode;
        UserLoginData userData { get; set; }
        private SystemSettings systemSettings { get; set; } = new();
        HiddenButtonsConfig hiddenButtons = new HiddenButtonsConfig();

        //Tree Table column
        string IDCol = "ConsigneeID";
        string NoCol = "ConsigneeCode";
        string NameCol = "ConsigneeNameAr";
        string NameEnCol = "ConsigneeNameEn";
        string ParentIDCol = "ParentID";
        string IsMainCol = "IsMain";
        string ItemLevelCol = "LevelID";
        string TableName = "CST_Consignees";
        //Levels Table
        string LevelsTable = "CST_Consignees_Levels";
        string LevelsCol = "LevelID";
        string LevelsWidthCol = "Width";

        protected override async Task OnInitializedAsync()
        {
            invisibleColumns = new List<string> { "ConsigneeID", "ParentID", "IsMain", "LevelID", "BranchID", "Deleted" };
            hiddenButtons.HideCopy = hiddenButtons.HideNext = hiddenButtons.HidePrevious = hiddenButtons.HideSearch  = true;
            if (ConnectionString == null || ConnectionString == "")
            {
                ConnectionString = await protectedLocalStorageService.GetConnectionStringAsync();
            }
            userData = await protectedLocalStorageService.GetUserDataAsync();
            systemSettings = await protectedLocalStorageService.GetSystemSettingsAsync();

            lstCountries = countryService.Select(-1, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString));
            valueLists.Add("CountryID", lstCountries.ToDictionary(c => c.CountryID.ToString(), c => systemSettings.IsArabic ? c.CountryNameAr : c.CountryNameEn));

            lstCities = cityService.Select(-1, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString));
            valueLists.Add("CityID", lstCities.ToDictionary(c => c.CityID.ToString(), c => systemSettings.IsArabic ? c.CityNameAr : c.CityNameEn));
        }
        private void PrepareData()
        {

        }

        private void ClearControls(Consignee item)
        {
            currentObject = item; 
            if (!isNavMode)
            {
                currentObject.ConsigneeCode = consigneeService.GetCode(selectedNode == null || selectedNode.ConsigneeID == 0 ? "Null" : selectedNode.ConsigneeID.ToString(), new DataAccess.Main(ConnectionString));
                currentObject.CountryID = selectedNode == null ? 1 : selectedNode.CountryID;
                valueLists["CityID"] = lstCities.Where(c => c.CountryID == currentObject.CountryID).ToDictionary(c => c.CityID.ToString(), c => systemSettings.IsArabic ? c.CityNameAr : c.CityNameEn);
                currentObject.CityID = selectedNode == null ? -1 : selectedNode.CityID;
            }
            else
            {
                selectedNode = null;
            }

        }
        public void SetControls(GlobalVariables.States state)
        {
            currentState = state;
            isNavMode = currentState == GlobalVariables.States.NavMode;
            if (isNavMode)
            {
                currentObject = selectedNode.Clone();
            }
        }
        private void DisplayData(Consignee item)
        {
            if (item != null)
            {
                item= consigneeService.Select(item.ConsigneeID, "-1", systemSettings.IsArabic, new DataAccess.Main(ConnectionString)).FirstOrDefault();
                currentObject = item.Clone();
                selectedNode = item;
                NodeLevel = (int)selectedNode.LevelID;
            }
        }


        private async Task<int> TreeAddData(Consignee consignee)
        {
            consignee.ConsigneeID = -1;
            consignee.ConsigneeNameEn = currentObject.ConsigneeNameEn == null ? currentObject.ConsigneeNameAr : currentObject.ConsigneeNameEn;
            consignee.VATNumber = consignee.VATNumber == null ? DBNull.Value.ToString() : consignee.VATNumber;
            consignee.Address = consignee.Address == null ? DBNull.Value.ToString() : consignee.Address;
            consignee.ParentID = selectedNode == null ? null : selectedNode.ConsigneeID;
            consignee.LevelID = selectedNode == null ? 1 : selectedNode.LevelID + 1;
            consignee.IsMain = selectedNode == null ? true : NodeLevel == 0 ? true : false;
            consignee.BranchID = int.Parse(userData.BranchID.ToString());

            return consigneeService.Insert_Update(consignee, int.Parse(userData.UserID.ToString()), new DataAccess.Main(ConnectionString));
            //FillData();
        }
        private async Task<int> TreeUpdateData(Consignee consignee)
        {
            consignee.ConsigneeNameEn = currentObject.ConsigneeNameEn == "" ? currentObject.ConsigneeNameAr : currentObject.ConsigneeNameEn;
            consignee.ParentID = consignee.ParentID == null ? null : selectedNode.ParentID;
            consignee.LevelID = NodeLevel;
            consignee.BranchID = int.Parse(userData.BranchID.ToString());
            return consigneeService.Insert_Update(consignee, 1, new DataAccess.Main(ConnectionString));
        }
        private void TreeDeleteData()
        {
            consigneeService.Delete(selectedNode.ConsigneeID, 1, new DataAccess.Main(ConnectionString));
            js.InvokeVoidAsync("TreeView.setHighlightDedault");
        }
        private void Cancel()
        {
            currentObject = selectedNode == null ? new Consignee() : selectedNode.Clone();
            isNavMode = true;
        }
        private void cboCountiesValueChanges(string cboName)
        {
            if (cboName== "CountryID")
            {
                valueLists["CityID"] = lstCities.Where(c => c.CountryID == currentObject.CountryID).ToDictionary(c => c.CityID.ToString(), c => systemSettings.IsArabic ? c.CityNameAr : c.CityNameEn);
            }
        }
    }
}

