using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021208.Web.Models
{
    /// <summary>
    /// kết quả tìm kiếm , phân trang của khách hàng
    /// </summary>
    public class CustomerPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// Danh sách khách hàng.
        /// </summary>
           public List<Customer> Data { get; set; }
    }
}