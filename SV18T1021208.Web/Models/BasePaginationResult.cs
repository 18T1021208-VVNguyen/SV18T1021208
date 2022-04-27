using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021208.Web.Models
{
    /// <summary>
    /// Lớp cơ sở cho cácl ớp lưu trữ dữ các dữ liệu liên quan đến phân trang.
    /// </summary>
    public abstract class BasePaginationResult
    {
        /// <summary>
        /// Trang cân xem
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số dòng trên 1 trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Tổng số dòng
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 1;

                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}