using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;  // Dùng chung.
using System.Data.SqlClient; // Xử lý vs SQL Server. 

namespace SV18T1021208.DataLayer.SQLServer
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string ConnectionString) : base(ConnectionString)
        {
        }

        public int Add(Category data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Categories(CategoryName,Description)
                                    VALUES (@CategoryName,@Description)	
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@Description", data.Description);
               
                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

        public int Count(string searchValue)
        {
            int count = 0;

            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT    COUNT(*)
                                    FROM    Categories
                                    WHERE    (@searchValue = N'')
                                    OR    (
                                                (CategoryName LIKE @searchValue)
                                                OR (Description LIKE @searchValue)                                          
                                            )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return count;
        }

        public bool Delete(int categoryID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Categories WHERE CategoryID = @CategoryID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public Category Get(int categoryID)
        {
            Category result = null;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Categories WHERE CategoryID = @CategoryID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@CategoryID",categoryID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Category()
                    {
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        CategoryName = Convert.ToString(dbReader["CategoryName"]),
                        Description = Convert.ToString(dbReader["Description"]),
                       
                    };
                }
                dbReader.Close();
                cn.Close();
            }
            return result;
        }

        public bool InUsed(int categoryID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT * FROM Products WHERE CategoryID = @CategoryID  ) Then 1  else 0 END";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@CategoryID",categoryID);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }

        public IList<Category> List(int page = 1, int pageSize = 0 , string searchValue = "")
        {
            List<Category> data = new List<Category>();

            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
                                        SELECT    ROW_NUMBER() OVER(ORDER BY CategoryName) AS RowNumber, *
                                        FROM    Categories
                                        WHERE(@searchValue = N'')
                                            OR(
                                                    (CategoryName LIKE @searchValue)
                                                 OR(Description LIKE @searchValue)
                                                
                                                )
                                    ) AS t
                                    WHERE (@pageSize=0) OR (t.RowNumber BETWEEN(@page -1) *@pageSize + 1 AND @page *@pageSize)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new Category()
                    {
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        CategoryName = Convert.ToString(result["CategoryName"]),
                        Description = Convert.ToString(result["Description"]),
                    });
                }
                result.Close();
                cn.Close();
            }

            return data;
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Categories SET CategoryName = @CategoryName,  Description = @Description WHERE CategoryID = @CategoryID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                              
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
    
    }
}
