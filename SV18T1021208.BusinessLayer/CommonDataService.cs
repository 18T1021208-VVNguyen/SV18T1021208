using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV18T1021208.DataLayer;
using SV18T1021208.DomainModel;
using System.Configuration;
using System.Web;


namespace SV18T1021208.BusinessLayer
{
    /// <summary>
    /// Cung cấp chức năng xử lý dữ liệu chung. Được chạy đầu tiên luôn.
    /// </summary>
    /// // 
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Country>  countryDAL;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;

        static CommonDataService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            switch (provider)
            {
                case "SQLServer":
                    categoryDB = new DataLayer.SQLServer.CategoryDAL(connectionString);
                    customerDB = new DataLayer.SQLServer.CustomerDAL(connectionString);
                    supplierDB = new DataLayer.SQLServer.SupplierDAL(connectionString);
                    shipperDB = new DataLayer.SQLServer.ShipperDAL(connectionString);
                    employeeDB = new DataLayer.SQLServer.EmployeeDAL(connectionString);
                    countryDAL = new DataLayer.SQLServer.CountryDAL(connectionString);
                    break;

                default:
                    categoryDB = new DataLayer.FakeDB.CategoryDAL();
                    break;
            }    

        }

        public static List<Customer> List()
        {
            return customerDB.List().ToList();
        }

      
       /// <summary>
       /// Tìm kiếm và lấy ds loại hàng dưới dạng phân trang
       /// </summary>
       /// <param name="page"></param>
       /// <param name="pageSize"></param>
       /// <param name="seachValue"></param>
       /// <param name="rowCount"></param>
       /// <returns></returns>
        public static List<Category> ListOfCategories(int page, int pageSize, string seachValue, out int rowCount)
        {
            rowCount = categoryDB.Count(seachValue);
            return categoryDB.List(page, pageSize, seachValue).ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy ds khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="seachValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page, int pageSize , string seachValue , out int rowCount)
        {
            rowCount = customerDB.Count(seachValue);
            return customerDB.List(page, pageSize, seachValue).ToList();
        }

        
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="seachValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page , int pageSize , string seachValue, out int rowCount)
        {
            rowCount = supplierDB.Count(seachValue);
            return supplierDB.List(page, pageSize, seachValue).ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy ds người giao hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="seachValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string seachValue, out int rowCount)
        {
            rowCount = shipperDB.Count(seachValue);
            return shipperDB.List(page, pageSize, seachValue).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="seachValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page,int pageSize, string seachValue, out int rowCount)
        {
            rowCount = employeeDB.Count(seachValue);
            return employeeDB.List(page, pageSize, seachValue).ToList();
        }

        /// <summary>
        /// Danh sách Đất nước
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListOfCountries()
        {
            return countryDAL.List().ToList();
        }
        //Customer

        /// <summary>
        /// Thêm khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }

        /// <summary>
        /// Xóa khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int customerID)
        {
            if (customerDB.InUsed(customerID))
                    return false;
            return customerDB.Delete(customerID);
        }


        /// <summary>
        /// Lấy thông tin 1 khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(int customerID)
        {
            return customerDB.Get(customerID);
        }

        /// <summary>
        /// Kiểm tra có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool InUsedCustomer(int customerID)
        {
            return customerDB.InUsed(customerID);
        }
        
        /// Supplier ///
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }

        public static bool DeleteSupplier(int supplierID)
        {
            if (supplierDB.InUsed(supplierID))
                return false;
            return supplierDB.Delete(supplierID);
        }

        public static Supplier GetSupplier(int supplierID)
        {
            return supplierDB.Get(supplierID);
        }

        public static bool InUsedSupplier(int supplierID)
        {
            return supplierDB.InUsed(supplierID);
        }

        // Category
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        public static bool DeleteCategory(int categoryID)
        {
            if (categoryDB.InUsed(categoryID))
                return false;
            return categoryDB.Delete(categoryID);
        }

        public static Category GetCategory(int categoryID)
        {
            return categoryDB.Get(categoryID);
        }

        public static bool InUsedCategory(int categoryID)
        {
            return categoryDB.InUsed(categoryID);
        }

        /// Shipper
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }

        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }

        public static bool DeleteShipper(int shipperID)
        {
            if (shipperDB.InUsed(shipperID))
                return false;
            return shipperDB.Delete(shipperID);
        }

        public static Shipper GetShipper(int shipperID)
        {
            return shipperDB.Get(shipperID);
        }

        public static bool InUsedShipper(int shipperID)
        {
            return shipperDB.InUsed(shipperID);
        }

        //Employee
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        public static bool DeleteEmployee(int employeeID)
        {
            if (employeeDB.InUsed(employeeID))
                return false;
            return employeeDB.Delete(employeeID);
        }

        public static Employee GetEmployee(int employeeID)
        {
            return employeeDB.Get(employeeID);
        }

        public static bool InUsedEmployee(int employeeID)
        {
            return employeeDB.InUsed(employeeID);
        }


    }
}
