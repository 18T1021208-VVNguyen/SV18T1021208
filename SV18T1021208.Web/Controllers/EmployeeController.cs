using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021208.BusinessLayer;

namespace SV18T1021208.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // GET: Employee
        [Authorize]
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(page, pageSize, searchValue, out rowCount);

            Models.EmployeePaginationResult model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };

            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.Title = "Chỉnh sửa thông tin nhân viên";
            return View("Create");
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}