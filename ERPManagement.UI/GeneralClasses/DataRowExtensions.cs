using System.Data;

namespace ERPManagement.UI.GeneralClasses
{
    public static class DataRowExtensions
    {
        public static void AddFromObject<T>(this DataTable table, T obj)
        {
            var row = table.NewRow();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (table.Columns.Contains(prop.Name))
                {
                    var value = prop.GetValue(obj) ?? DBNull.Value;
                    row[prop.Name] = value;
                }
            }

            table.Rows.Add(row);
        }
        public static void UpdateFromObject<T>(this DataRow row, T obj)
        {
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                if (row.Table.Columns.Contains(prop.Name))
                {
                    var value = prop.GetValue(obj) ?? DBNull.Value;
                    row[prop.Name] = value;
                }
            }
        }
        public static void RemoveRowFromTable<T>(
    this DataTable table,
    T obj,
    string keyColumnName,
    string keyPropertyName)
        {
            if (table == null || obj == null)
                return;

            // نجيب قيمة الـ Key من الكائن
            var property = typeof(T).GetProperty(keyPropertyName);
            if (property == null)
                throw new ArgumentException($"Property {keyPropertyName} not found on {typeof(T).Name}");

            var keyValue = property.GetValue(obj);

            // نلاقي الصف اللي يساوي القيمة
            var row = table.AsEnumerable()
                           .FirstOrDefault(r => Equals(r[keyColumnName], keyValue));

            if (row != null)
            {
                table.Rows.Remove(row); // حذف الصف
                                        // أو row.Delete(); لو عاوز تعلمه كمحذوف بس
            }
        }

    }

}
