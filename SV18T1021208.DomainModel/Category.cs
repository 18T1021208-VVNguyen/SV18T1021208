using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021208.DomainModel
{
    /// <summary>
    /// Loại hàng
    /// </summary>
   public  class Category
   {
        /// <summary>
        /// Mã loại Hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Tên loại Hàng
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Mô tả hàng
        /// </summary>
        public string Description { get; set; }
   }
}
