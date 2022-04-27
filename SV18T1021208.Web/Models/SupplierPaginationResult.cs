using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021208.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm và phân trang
    /// </summary>
    public class SupplierPaginationResult : BasePaginationResult
    {

        /// <summary>
        /// Danh sách nhà cung cấp
        /// </summary>
        public List<Supplier> Data { get; set; }

          
    }
}