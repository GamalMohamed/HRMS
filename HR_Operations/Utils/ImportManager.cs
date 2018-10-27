using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace CoEX_HRMS.Utils
{
    public static class ImportManager
    {
        public static int ImportExcel(HttpPostedFileBase uploadFile, string serverPath, string table, List<string> columns )
        {
            // Download the file
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            var filePath = serverPath + Path.GetFileName(uploadFile.FileName);
            var extension = Path.GetExtension(uploadFile.FileName);
            if (!(extension == ".xls" || extension == ".xlsx"))
            {
                return -1;
            }

            uploadFile.SaveAs(filePath);

            var conString = string.Empty;
            switch (extension)
            {
                case ".xls": // Excel 97-03.
                    conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": // Excel 07 and above.
                    conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }

            var dt = new DataTable();
            conString = string.Format(conString, filePath);
            using (var connExcel = new OleDbConnection(conString))
            {
                using (var cmdExcel = new OleDbCommand())
                {
                    using (var odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        // Get the name of First Sheet.
                        connExcel.Open();
                        var dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        var sheetName = dtExcelSchema?.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        // Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }
            }

            conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(con))
                {
                    // Set the database table name.
                    sqlBulkCopy.DestinationTableName = table;

                    // Map Excel columns with that of the database table
                    foreach (var column in columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(column, column);
                    }
                    
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            return 1;
        }

    }
}