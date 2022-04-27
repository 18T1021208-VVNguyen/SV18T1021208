using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV18T1021208.DomainModel;

namespace SV18T1021208.Web.Models
{
    public class CategoryPaginationResult : BasePaginationResult
    {
        /// <summary>
        /// Danh sách loại hàng.
        /// </summary>
        public List<Category> Data { get; set; }
    }
}