using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace SV18T1021208.DataLayer.SQLServer
{

    /// <summary>
    /// Lớp cơ sở cho các lớp xử lý dữ liệu trên SQL Server.
    /// </summary>
   public abstract class _BaseDAL
   {
         protected string _connectionString;
            
            public _BaseDAL(string ConnectionString)
            {
                _connectionString = ConnectionString;
            }


        /// <summary>
        /// Tạo và kết nối đến CSDL;
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnecttion()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();

            return cn;
        }

    }
}
