using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using ERPManagement.UI.DataModels.Interfaces;
using ERPManagement.UI.DataAccess;
namespace ERPManagement.UI.DataModels.EInvoices
{
public class SubAccountsTypes
{

public int EINVSubAccountTypeID { get; set; }

public string? EINVSubAccountTypeCode { get; set; }

public string? EINVSubAccountTypeNameAr { get; set; }

public string? EINVSubAccountTypeNameEn { get; set; }

public bool  Deleted { get; set; }

public int BranchID { get; set; }

public SubAccountsTypes Clone()
{
return (SubAccountsTypes)MemberwiseClone();
}
}
public class SubAccountsTypesService : IScopedService
    {

        public DataTable FillCombo(int ID, bool IsArabic,Main main)
        {
            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0]=new SqlParameter("@EINVSubAccountTypeID", ID);
            parameter[1]=new SqlParameter("@IsArabic",IsArabic);
                
            DataTable dataTable = main.SyncExecuteQuery_DataTable("EINV_SubAccountsTypes_FillCombo", true, parameter);
            return dataTable;
        }

    }
}
