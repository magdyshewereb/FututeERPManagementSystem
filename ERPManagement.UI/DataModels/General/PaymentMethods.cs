using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.General
{
public class PaymentMethods
{

public int PaymentMethodID { get; set; }

public string PaymentMethodNameAr { get; set; }

public string? PaymentMethodNameEn { get; set; }

public int? PaymentTime { get; set; }

public int? InstallmentsCount { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public PaymentMethods Clone()
{
return (PaymentMethods)MemberwiseClone();
}
}
public class PaymentMethodsService : IScopedService
    {

public string GetCode(Main main)
{
            DataTable dataTable = main.ExecuteQuery_DataTable("G_PaymentMethods_GetCode",true,null);
return dataTable.Rows[0][0].ToString();
}
public string GetCodeByBranchID(DateTime Date,string BranchID,Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@Date", Date);
parameters[1] = new SqlParameter("@BranchID",BranchID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_PaymentMethods_GetCodeByBranchID",true,parameters);
return dataTable.Rows[0][0].ToString();
}

public int Insert_Update(PaymentMethods obj,int UserID,Main main)
{
SqlParameter[] parameters = new SqlParameter[8];
parameters[0] = new SqlParameter("@PaymentMethodID", obj.PaymentMethodID);
parameters[1] = new SqlParameter("@PaymentMethodNameAr", obj.PaymentMethodNameAr);
parameters[2] = new SqlParameter("@PaymentMethodNameEn", obj.PaymentMethodNameEn);
parameters[3] = new SqlParameter("@PaymentTime", obj.PaymentTime);
parameters[4] = new SqlParameter("@InstallmentsCount", obj.InstallmentsCount);
parameters[5] = new SqlParameter("@Deleted", obj.Deleted);
parameters[6] = new SqlParameter("@BranchID", obj.BranchID);
parameters[7] = new SqlParameter("@UserID",UserID);
DataTable dataTable = main.ExecuteQuery_DataTable("G_PaymentMethods_Insert_Update",true,parameters);
return int.Parse(dataTable.Rows[0][0].ToString());
}

public List<PaymentMethods> Select(int PaymentMethodID, string BranchIDs, bool IsArabic, Main main)
{
SqlParameter[] parameters = new SqlParameter[3];
parameters[0] = new SqlParameter("@PaymentMethodID", PaymentMethodID);
parameters[1] = new SqlParameter("@BranchIDs",BranchIDs);
parameters[2] = new SqlParameter("@IsArabic", IsArabic);
 DataTable dataTable =main.ExecuteQuery_DataTable("G_PaymentMethods_Select",true,parameters);
List<PaymentMethods> lst = new List<PaymentMethods>();
if (dataTable != null)
{
 lst =main.CreateListFromTable<PaymentMethods>(dataTable);
}
return lst;
}

public void Delete(int PaymentMethodID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PaymentMethodID", PaymentMethodID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_PaymentMethods_Delete",true,parameters);
}
public void DeleteVirtual(int PaymentMethodID, int UserID, Main main)
{
SqlParameter[] parameters = new SqlParameter[2];
parameters[0] = new SqlParameter("@PaymentMethodID", PaymentMethodID);
parameters[1] = new SqlParameter("@UserID",UserID);
main.ExecuteNonQuery("G_PaymentMethods_DeleteVirtual",true,parameters);
}




}
}
