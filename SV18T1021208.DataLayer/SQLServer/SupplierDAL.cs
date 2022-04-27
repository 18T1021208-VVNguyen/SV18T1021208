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
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string ConnectionString) : base(ConnectionString)
        {
        }

        public int Add(Supplier data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Suppliers (SupplierName,ContactName,[Address],City,PostalCode,Country,Phone)
                                    VALUES (@SupplierName,@ContactName,@Address,@City,@PostalCode,@Country,@Phone)
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@SupplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@PostalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);

                result = Convert.ToInt32( cmd.ExecuteScalar());
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
                                    FROM    Suppliers
                                    WHERE    (@searchValue = N'')
                                    OR    (
                                                (SupplierName LIKE @searchValue)
                                                OR (ContactName LIKE @searchValue)
                                                OR (Address LIKE @searchValue)
                                                OR (Phone LIKE @searchValue)
                                            )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", seachValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return count;
        }

        public bool Delete(int supplierID)
        {
            bool result = false;
            using(SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                result = cmd.ExecuteNonQuery() >0;

                cn.Close();
            }
            return result;
        }

        public Supplier Get(int supplierID)
        {
            Supplier result = null;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Supplier()
                    {
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        SupplierName = Convert.ToString(dbReader["SupplierName"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        City = Convert.ToString(dbReader["City"]),
                        Country = Convert.ToString(dbReader["Country"]),
                        Phone = Convert.ToString(dbReader["Phone"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"]),
                       
                    };
                }
                dbReader.Close();
                cn.Close();
            }
            return result;
        }

        public bool InUsed(int supplierID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT * FROM Products WHERE SupplierID = @SupplierID) Then 1
                                    else 0 END";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                result = Convert.ToBoolean(cmd.ExecuteScalar());
               
                cn.Close();
            }
            return result;
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string seachValue = "")
        {
            List<Supplier> data = new List<Supplier>();

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
                                        SELECT    ROW_NUMBER() OVER(ORDER BY SupplierName) AS RowNumber, *
                                        FROM    Suppliers
                                        WHERE(@searchValue = N'')
                                            OR(
                                                    (SupplierName LIKE @searchValue)
                                                 OR(ContactName LIKE @searchValue)
                                                 OR(Address LIKE @searchValue)
                                                  OR (Phone LIKE @searchValue)
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
                    data.Add(new Supplier()
                    {
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        SupplierName = Convert.ToString(result["SupplierName"]),
                        Address = Convert.ToString(result["Address"]),
                        City = Convert.ToString(result["City"]),
                        ContactName = Convert.ToString(result["ContactName"]),
                        Country = Convert.ToString(result["Country"]),
                        Phone = Convert.ToString(result["Phone"]),
                        PostalCode = Convert.ToString(result["PostalCode"])

                    });
                }
                result.Close();
                cn.Close();

            }
            return data;
        }

        public bool Update(Supplier data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Suppliers SET SupplierName = @SupplierName , ContactName = @ContactName , [Address] = @Address , City = @City , 
                     PostalCode = @PostalCode, Country = @Country, Phone = @Phone WHERE SupplierID = @SupplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@SupplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@PostalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                result = cmd.ExecuteNonQuery() > 0 ;
                cn.Close();
            }
            return result;
        }
    }
}
