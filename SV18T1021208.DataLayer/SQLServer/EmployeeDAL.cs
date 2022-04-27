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
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string ConnectionString) : base(ConnectionString)
        {
        }

        public int Add(Employee data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees(LastName,FirstName,BirthDate,Photo,Notes,Email)
                                VALUES (@LastName,@FirstName,@BirthDate,@Photo,@Notes,@Email)	
                                SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@Email", data.Email);

                result = Convert.ToInt32( cmd.ExecuteScalar());

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
                                    FROM    Employees
                                    WHERE    (@searchValue = N'')
                                    OR    (
                                                (LastName LIKE @searchValue)
                                                OR (FirstName LIKE @searchValue) 
                                                OR (Email LIKE @searchValue) 
                                            )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return count;
        }

        public bool Delete(int EmployeeID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees WHERE EmployeeID = @SupplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                result = cmd.ExecuteNonQuery() > 0 ;
                cn.Close();
            }
            return result;
        }

        public Employee Get(int EmployeeID)
        {
            Employee result = null;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Employee()
                    {
                        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                        FirstName = Convert.ToString(dbReader["FirstName"]),
                        LastName = Convert.ToString(dbReader["LastName"]),
                        BirthDate = Convert.ToString(dbReader["BirthDate"]),
                        Email = Convert.ToString(dbReader["Email"]),
                        Notes = Convert.ToString(dbReader["Notes"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                    };
                }
                dbReader.Close();
                cn.Close();
            }
            return result;
        }

        public bool InUsed(int EmployeeID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT * FROM Orders WHERE EmployeeID = @EmployeeID) Then 1  else 0 END";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1 , int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();

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
                                        SELECT    ROW_NUMBER() OVER(ORDER BY EmployeeID) AS RowNumber, *
                                        FROM    Employees
                                        WHERE(@searchValue = N'')
                                            OR(
                                                    (LastName LIKE @searchValue)
                                                 OR(FirstName LIKE @searchValue)
                                                 OR(Email LIKE @searchValue)
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
                    data.Add(new Employee()
                    {
                        EmployeeID = Convert.ToInt32(result["EmployeeID"]),
                        FirstName = Convert.ToString(result["FirstName"]),
                        LastName = Convert.ToString(result["LastName"]),
                        BirthDate = DateTime.Parse( Convert.ToString(result["BirthDate"]) ).ToString("dd/MM/yyyy"),
                        Email = Convert.ToString(result["Email"]),
                        Notes = Convert.ToString(result["Notes"]),
                        Password = Convert.ToString(result["Password"]),
                        Photo = Convert.ToString(result["Photo"]),


                    });
                }
                result.Close();
                cn.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnecttion())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees SET LastName = @LastName , FirstName = @FirstName , BirthDate = @BirthDate , Photo = @Photo , 
					    Notes = @Notes, Email = @Email WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);

                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }
    }
}
