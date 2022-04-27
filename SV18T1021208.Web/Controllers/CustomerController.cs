using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021208.BusinessLayer;
using SV18T1021208.DataLayer;
using SV18T1021208.DomainModel;

namespace SV18T1021208.Web.Controllers
{
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        /// <summary>
        /// Tìm kiếm , hiển thị danh sách khách hàng.
        /// </summary>
        /// <returns></returns>
        // GET: Customer
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(page, pageSize, searchValue, out rowCount);

            Models.CustomerPaginationResult model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };

            return View(model);


        }

        /// <summary>
        /// Giao diện bổ sung khách hàng 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Customer model = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }

        [Route("edit/{customerID}")]
        public ActionResult Edit(int customerID)
        {
            Customer model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin khách hàng";
            return View("Create", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("delete/{customerID}")]
        public ActionResult Delete(int customerID)
        {
            if(Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(customerID);
                return RedirectToAction("Index");
            }

            Customer model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Customer model)
        {
            // TODO : Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                // Tên thông báo lỗi( duy nhất ) , 
                ModelState.AddModelError("CustomerName","Tên khách hàng không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.Country))
            {
                ModelState.AddModelError("Country", "Phải chọn quốc gia");
            }

            //Xử lý giá trị null
            if (string.IsNullOrWhiteSpace(model.City))
            {
                model.City = "";
            }
            if (string.IsNullOrWhiteSpace(model.PostalCode))
            {
                model.PostalCode = "";
            }

            if (!ModelState.IsValid)
            {
                if (model.CustomerID == 0)
                    ViewBag.Title = "Bổ sung thông tin khách hàng";
                else
                    ViewBag.Title = "Thay đổi thông tin khách hàng";

                return View("Create", model);
            }



            if (model.CustomerID == 0)
            {

                CommonDataService.AddCustomer(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCustomer(model);
                return RedirectToAction("Index");
            }
        }


    }
}