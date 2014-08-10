using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DM.Infrastructure.Util.SqlHelpers
{
    public class SqlHelpers
    {
        /// <summary>
        /// 获取Database, 支持多数据库切换
        /// </summary>
        /// <returns></returns>
        public static Database CreateDatabase()
        {
            return DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 执行command，支持事物
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static bool Execute(DbCommand[] commands)
        {
            Database db = SqlHelpers.CreateDatabase();
            bool result;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (DbCommand command in commands)
                    {
                        db.ExecuteNonQuery(command, trans);
                    }
                    trans.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    result = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        /*example*/
        //查看Microsoft Enterprise Library 5.0 - Hands On Labs\CS\Data Access Sample文档
        private void example()
        {
            //工作单元模式
            DataSet ds = new DataSet();
            Database _db = SqlHelpers.CreateDatabase();

            System.Data.Common.DbCommand insertCommand = null;
            insertCommand = _db.GetStoredProcCommand("HOLAddProduct");
            _db.AddInParameter(insertCommand, "ProductName",
                DbType.String, "ProductName", DataRowVersion.Current);
            _db.AddInParameter(insertCommand, "CategoryID",
                DbType.Int32, "CategoryID", DataRowVersion.Current);
            _db.AddInParameter(insertCommand, "UnitPrice",
                DbType.Currency, "UnitPrice", DataRowVersion.Current);

            System.Data.Common.DbCommand deleteCommand = null;
            deleteCommand = _db.GetStoredProcCommand("HOLDeleteProduct");
            _db.AddInParameter(deleteCommand, "ProductID",
                DbType.Int32, "ProductID", DataRowVersion.Current);
            _db.AddInParameter(deleteCommand, "LastUpdate",
                DbType.DateTime, "LastUpdate", DataRowVersion.Original);

            System.Data.Common.DbCommand updateCommand = null;
            updateCommand = _db.GetStoredProcCommand("HOLUpdateProduct");
            _db.AddInParameter(updateCommand, "ProductID",
                DbType.Int32, "ProductID", DataRowVersion.Current);
            _db.AddInParameter(updateCommand, "ProductName",
                DbType.String, "ProductName", DataRowVersion.Current);
            _db.AddInParameter(updateCommand, "CategoryID",
                DbType.Int32, "CategoryID", DataRowVersion.Current);
            _db.AddInParameter(updateCommand, "UnitPrice",
                DbType.Currency, "UnitPrice", DataRowVersion.Current);
            _db.AddInParameter(updateCommand, "LastUpdate",
                DbType.DateTime, "LastUpdate", DataRowVersion.Current);

            int rowsAffected = _db.UpdateDataSet(
                ds,
                "Products",
                insertCommand,
                updateCommand,
                deleteCommand,
                UpdateBehavior.Standard);
            //Standard：如果更新失败，停止更新。
            //Continue：如果更新失败，将继续其他的更新。
            //Transactional：如果更新失败，所有的操作都将回滚。
        }
        /*example*/
    }
}
