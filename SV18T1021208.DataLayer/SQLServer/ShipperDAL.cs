using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021208.DataLayer.SQLServer
{
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        public ShipperDAL(string ConnectionString) : base(ConnectionString)
        {
        }

        public int Add(Shipper data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers(ShipperName,Phone)
                                    VALUES (@ShipperName,@Phone)	
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@ShipperName", data.ShipperName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

        public int Count(string seachValue)
        {
            int count = 0;

            if (seachValue != "")
            {
                seachValue = "%" + seachValue + "%";
            }
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT    COUNT(*)
                                    FROM    Shippers
                                    WHERE    (@searchValue = N'')
                                    OR    (
                                                (ShipperName LIKE @searchValue)
                                                OR (Phone LIKE @searchValue)                                              
                                            )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", seachValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return count;
        }

        public bool Delete(int shipperID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);

                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public Shipper Get(int shipperID)
        {
             Shipper result = null;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @ShipperID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Shipper()
                    {
                        ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                        ShipperName = Convert.ToString(dbReader["ShipperName"]),
                        Phone = Convert.ToString(dbReader["Phone"])
                        
                    };
                }
                dbReader.Close();
                cn.Close();
            }
            return result;
        }

        public bool InUsed(int shipperID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT * FROM Orders WHERE ShipperID = @ShipperID ) Then 1  else 0 END";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@ShipperID", shipperID);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }

        public IList<Shipper> List(int page = 1 , int pageSize = 0, string seachValue = "")
        {
            List<Shipper> data = new List<Shipper>();

            if (seachValue != "")
            {
                seachValue = "%" + seachValue + "%";
            }
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                        SELECT    ROW_NUMBER() OVER(ORDER BY ShipperName) AS RowNumber, *
                                        FROM    Shippers
                                        WHERE(@searchValue = N'')
                                            OR(
                                                    (ShipperName LIKE @searchValue)
                                                 OR(Phone LIKE @searchValue)
                                               
                                                )
                                    ) AS t
                                   WHERE (@pageSize = 0) OR (t.RowNumber BETWEEN(@page -1) *@pageSize + 1 AND @page *@pageSize)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", seachValue);

                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new Shipper()
                    {
                        ShipperID = Convert.ToInt32(result["ShipperID"]),
                        ShipperName = Convert.ToString(result["ShipperName"]),
                        Phone = Convert.ToString(result["Phone"])
                    });
                }
                result.Close();
                cn.Close();
            }

            return data;
        }

        public bool Update(Shipper data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Shippers SET ShipperName = @ShipperName,  Phone = @Phone WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@ShipperName", data.ShipperName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);

                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
    }
}
