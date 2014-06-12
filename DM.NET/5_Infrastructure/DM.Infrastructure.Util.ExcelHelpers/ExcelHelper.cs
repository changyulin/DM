using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DM.Infrastructure.Util.ExcelHelpers
{
    public static class ExcelHelper
    {
        // Fields
        public static int m_SheetMaxLen;

        // Methods
        static ExcelHelper()
        {
            m_SheetMaxLen = 65535;
        }

        /// <summary>
        /// 创建Execl
        /// </summary>
        /// <param name="pDt">数据</param>
        /// <param name="pFileName">文件名</param>
        /// <param name="pSheet">Sheet名</param>
        /// <param name="pSheetIdx">Sheet索引</param>
        /// <param name="pStart">行索引</param>
        /// <returns></returns>
        public static string CreateExcel(this DataTable pDt, string pFileName, string pSheet, int pSheetIdx, int pStart)
        {
            string result = string.Empty;
            pFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Temp\" + pFileName);
            if (!Directory.Exists(Path.GetDirectoryName(pFileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pFileName));
            }
            if (File.Exists(pFileName))
            {
                File.Delete(pFileName);
            }
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pFileName + ";Extended Properties='Excel 8.0;HDR=YES'";
            StringBuilder SbSql = new StringBuilder(1000);
            string sql = string.Empty;
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            OleDbTransaction transaction = connection.BeginTransaction();
            OleDbCommand command = new OleDbCommand("", connection);
            command.Transaction = transaction;
            pSheet = pSheet + ((pSheetIdx == 0) ? "" : ("(" + pSheetIdx.ToString() + ")"));
            SbSql.Append(" Create table " + pSheet + " (");
            for (int i = 0; i < pDt.Columns.Count; i++)
            {
                pDt.Columns[i].ColumnName = pDt.Columns[i].Caption;
            }
            for (int j = 0; j < pDt.Columns.Count; j++)
            {
                SbSql.Append("[" + pDt.Columns[j].Caption + "] NTEXT,");
            }

            SbSql.Remove(SbSql.Length - 1, 1);
            SbSql.Append(")");
            try
            {
                command.CommandText = SbSql.ToString();
                command.ExecuteNonQuery();
                SbSql.Clear();
                int Count = (pDt.Rows.Count > (pStart + m_SheetMaxLen)) ? (pStart + m_SheetMaxLen) : pDt.Rows.Count;
                for (int k = pStart; k < Count; k++)
                {
                    SbSql.Append("insert into [" + pSheet + "] values(");
                    for (int m = 0; m < pDt.Columns.Count; m++)
                    {
                        string value = "";
                        if (pDt.Rows[k][m] != DBNull.Value)
                        {
                            if (pDt.Columns[m].DataType == Type.GetType("System.DateTime"))
                            {
                                value = DateTime.Parse(pDt.Rows[k][m].ToString()).ToShortDateString();
                            }
                            else if ((pDt.Columns[m].DataType == Type.GetType("System.Decimal")) || (pDt.Columns[m].DataType == Type.GetType("System.Double")))
                            {
                                value = decimal.Parse(pDt.Rows[k][m].ToString()).ToString("N3");
                            }
                            else
                            {
                                value = Convert.ToString(pDt.Rows[k][m]);
                            }
                        }
                        SbSql.Append("'" + value.ToString().Replace("'", "''") + "',");
                    }
                    SbSql.Remove(SbSql.Length - 1, 1);
                    SbSql.Append(")");
                    command.CommandText = SbSql.ToString();
                    command.ExecuteNonQuery();
                    SbSql.Clear();
                }
                transaction.Commit();
                connection.Close();
                if (pDt.Rows.Count > (pStart + m_SheetMaxLen))
                {
                    pDt.CreateExcel(pFileName, pSheet, pSheetIdx + 1, Count);
                }
                result = pFileName;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new Exception(exception.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <param name="serverfilpath">文件路径</param>
        /// <param name="fileName">下载后的文件名</param>
        public static void DownExcel(string serverfilpath, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (File.Exists(serverfilpath))
            {
                FileInfo info = new FileInfo(serverfilpath);
                HttpContext current = HttpContext.Current;
                try
                {
                    current.Response.Clear();
                    current.Response.ClearHeaders();
                    current.Response.ClearContent();
                    current.Response.Buffer = false;
                    current.Response.Charset = "utf-8";
                    current.Response.ContentEncoding = Encoding.UTF8;
                    current.Response.ContentType = "application/vnd.ms-excel";
                    if (current.Request.Browser.Browser == "IE")
                    {
                        current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                    }
                    else
                    {
                        current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                    }
                    current.Response.AddHeader("Content-Length", info.Length.ToString());
                    current.Response.WriteFile(info.FullName, 0, info.Length);
                    current.Response.Flush();
                    current.Response.End();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    File.Delete(serverfilpath);
                }
            }
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <returns></returns>
        internal static OleDbConnection CreateConnection(string excelPath)
        {
            return new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", excelPath));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelPath"></param>
        /// <param name="sheetIndex"></param>
        public static void DataTableToExcel(DataTable dt, string excelPath, int sheetIndex)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            ArrayList sheetNames = GetSheetNames(conn);
            if (sheetNames.Count <= sheetIndex)
            {
                throw new ArgumentOutOfRangeException();
            }
            string sheetName = sheetNames[sheetIndex].ToString();
            DataTableToExcel(conn, dt, sheetName);
            conn.Close();
        }


        public static void DataTableToExcel(DataTable dt, string excelPath, string sheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            DataTableToExcel(conn, dt, sheetName + "$");
            conn.Close();
        }

        private static void DataTableToExcel(OleDbConnection conn, DataTable dt, string sheetName)
        {
            if (!GetSheetNames(conn).Contains(sheetName))
            {
                string str = ("Create table [" + sheetName.Trim(new char[] { '$' }) + "]\n") + "(";
                List<string> list = new List<string>();
                foreach (DataColumn column in dt.Columns)
                {
                    list.Add(column.ColumnName + " text");
                }
                str = str + string.Join(",", list.ToArray()) + ")";
                using (OleDbCommand command = conn.CreateCommand())
                {
                    command.CommandText = str;
                    command.ExecuteNonQuery();
                }
            }
            OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + sheetName + "]", conn);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            builder.QuotePrefix = "[";
            builder.QuoteSuffix = "]";
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Table1");
            CopyDataTable(dt, dataSet.Tables[0]);
            adapter.Update(dataSet, "Table1");
        }

        private static void CopyDataTable(DataTable srcTable, DataTable destTable)
        {
            foreach (DataRow row in srcTable.Rows)
            {
                DataRow row2 = destTable.NewRow();
                for (int i = 0; i < destTable.Columns.Count; i++)
                {
                    row2[i] = row[i];
                }
                destTable.Rows.Add(row2);
            }
        }

        private static ArrayList GetSheetNames(OleDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DataTable oleDbSchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ArrayList list = new ArrayList();
            foreach (DataRow row in oleDbSchemaTable.Select("TABLE_NAME like '%$'"))
            {
                list.Add(row[2]);
            }
            return list;
        }

        public static ArrayList GetSheetNames(string excelPath)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            ArrayList sheetNames = GetSheetNames(conn);
            conn.Close();
            return sheetNames;
        }



        public static DataTable Query(string excelPath)
        {
            return Query(excelPath, 0);
        }

        public static DataTable Query(string excelPath, int sheetIndex)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            ArrayList sheetNames = GetSheetNames(conn);
            if (sheetNames.Count <= sheetIndex)
            {
                throw new ArgumentOutOfRangeException();
            }
            string sheetName = sheetNames[sheetIndex].ToString();
            DataTable table = QueryBySheetName(conn, sheetName);
            conn.Close();
            return table;
        }

        public static DataTable Query(string excelPath, string sheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            DataTable table = new DataTable();
            table = QueryBySheetName(conn, sheetName + "$");
            conn.Close();
            return table;
        }

        private static DataTable QueryBySheetName(OleDbConnection conn, string sheetName)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + sheetName + "]", conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable QueryEx(string excelPath, string rawSheetName)
        {
            OleDbConnection conn = CreateConnection(excelPath);
            conn.Open();
            DataTable table = new DataTable();
            table = QueryBySheetName(conn, rawSheetName);
            conn.Close();
            return table;
        }

        private static string ToNumberString(string s)
        {
            if (!Regex.IsMatch(s, @"^[\-\+]?[0-9]+(\.[0-9]+)?[eE]+[\+\-][0-9]+$"))
            {
                return s;
            }
            return decimal.Parse(s, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture).ToString();
        }
    }
}