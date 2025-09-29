using System.Data;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using Microsoft.Data.SqlClient;
namespace ERPManagement.UI.DataModels.CustomsClearence
{
    public class Consignee
    {

        public int ConsigneeID { get; set; }

        public string? ConsigneeCode { get; set; }

        public string? ConsigneeNameAr { get; set; }

        public string? ConsigneeNameEn { get; set; }

        public int? ParentID { get; set; }

        public bool IsMain { get; set; }

        public int? LevelID { get; set; }

        public string? VATNumber { get; set; }

        public int? CountryID { get; set; }

        public int? CityID { get; set; }

        public string? Address { get; set; }

        public bool Deleted { get; set; }

        public int BranchID { get; set; }

        public Consignee Clone()
        {
            return (Consignee)MemberwiseClone();
        }
    }
    public class ConsigneesService : IScopedService
    {

        public string GetCode(string ParentID, Main main)
        {
            string strSql = string.Empty;
            strSql += "  ";
            strSql += " select dbo.CST_Consignees_GetCode(   ";
            strSql += ParentID + ") as NewCode";	//ParentID
            DataTable dataTable = main.ExecuteQuery_DataTable(strSql, false, null);
            return dataTable.Rows[0][0].ToString();
        }

        public int Insert_Update(Consignee obj, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[14];
            parameters[0] = new SqlParameter("@ConsigneeID", obj.ConsigneeID);
            parameters[1] = new SqlParameter("@ConsigneeCode", obj.ConsigneeCode);
            parameters[2] = new SqlParameter("@ConsigneeNameAr", obj.ConsigneeNameAr);
            parameters[3] = new SqlParameter("@ConsigneeNameEn", obj.ConsigneeNameEn);
            parameters[4] = new SqlParameter("@ParentID", obj.ParentID == null ? DBNull.Value : obj.ParentID);
            parameters[5] = new SqlParameter("@IsMain", obj.IsMain);
            parameters[6] = new SqlParameter("@LevelID", obj.LevelID);
            parameters[7] = new SqlParameter("@VATNumber", obj.VATNumber);
            parameters[8] = new SqlParameter("@CountryID", obj.CountryID);
            parameters[9] = new SqlParameter("@CityID", obj.CityID);
            parameters[10] = new SqlParameter("@Address", obj.Address);
            parameters[11] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[12] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[13] = new SqlParameter("@UserID", UserID);
            DataTable dataTable = main.ExecuteQuery_DataTable("CST_Consignees_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<Consignee> Select(int ConsigneeID, string BranchIDs, bool IsArabic, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ConsigneeID", ConsigneeID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable =  main.ExecuteQuery_DataTable("CST_Consignees_Select", true, parameters);
            List<Consignee> lst = new List<Consignee>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<Consignee>(dataTable);
            }
            return lst;
        }

        public void Delete(int ConsigneeID, int UserID, Main main )
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@ConsigneeID", ConsigneeID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            
                main.ExecuteNonQuery("CST_Consignees_Delete", true, parameters);
        }


    }
}