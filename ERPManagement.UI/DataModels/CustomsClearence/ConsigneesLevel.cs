using System.Data;
using ERPManagement.UI.DataAccess;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataModels.Accounting;
using Microsoft.Data.SqlClient;
namespace ERPManagement.UI.DataModels.CustomsClearence
{
    public class ConsigneesLevel
    {

        public int LevelID { get; set; }
        public int Level { get; set; }

        public int Width { get; set; }
        public int Start { get; set; }
        public int End { get; set; }


        public bool Deleted { get; set; }

        public int? BranchID { get; set; }
        public ConsigneesLevel Clone()
        {
            return (ConsigneesLevel)MemberwiseClone();
        }

    }
    public class ConsigneesLevelsService : IScopedService
    {



        public int Insert_Update(ConsigneesLevel obj, int UserID, Main main )
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@LevelID", obj.LevelID);
            parameters[1] = new SqlParameter("@Width", obj.Width);
            parameters[2] = new SqlParameter("@Deleted", obj.Deleted);
            parameters[3] = new SqlParameter("@BranchID", obj.BranchID);
            parameters[4] = new SqlParameter("@UserID", UserID);
            DataTable dataTable =  main.ExecuteQuery_DataTable(" CST_Consignees_Levels_Insert_Update", true, parameters);
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public List<ConsigneesLevel> Select(int LevelID, string BranchIDs, bool IsArabic, Main main )
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@LevelID", LevelID);
            parameters[1] = new SqlParameter("@BranchIDs", BranchIDs);
            parameters[2] = new SqlParameter("@IsArabic", IsArabic);
            DataTable dataTable =  main.ExecuteQuery_DataTable(" CST_Consignees_Levels_Select", true, parameters);
            List<ConsigneesLevel> lst = new List<ConsigneesLevel>();
            if (dataTable != null)
            {
                lst = main.CreateListFromTable<ConsigneesLevel>(dataTable);
            }
            return lst;
        }

        public void Delete(int LevelID, int UserID, Main main)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@LevelID", LevelID);
            parameters[1] = new SqlParameter("@UserID", UserID);
            
                main.ExecuteNonQuery(" CST_Consignees_Levels_Delete", true, parameters);
        }




    }
}