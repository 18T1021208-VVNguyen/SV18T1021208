using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV18T1021208.DomainModel;

namespace SV18T1021208.Web.Models
{
    public class EmployeePaginationResult : BasePaginationResult
    {
        /// <summary>
        /// Danh sách nhân viên.
        /// </summary>
        public List<Employee> Data { get; set; }
    }
}