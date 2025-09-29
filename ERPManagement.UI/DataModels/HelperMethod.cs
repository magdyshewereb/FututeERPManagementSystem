using System.Data;

namespace ERPManagement.UI.DataModels
{
    public class HelperMethod
    {
        public static void PrintTableColumns(DataTable dt)
        {
            Console.WriteLine("=== Columns in DataTable ===");
            foreach (DataColumn col in dt.Columns)
            {
                Console.WriteLine(col.ColumnName);
            }
            Console.WriteLine("============================");
        }

    }
}
