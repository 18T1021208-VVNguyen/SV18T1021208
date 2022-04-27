using SV18T1021208.BusinessLayer;
using SV18T1021208.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021208.DomainModel;

namespace SV18T1021208.Web.Controllers
{
    [Authorize]
    [RoutePrefix("supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index(int page = 1, string seachValue = "")
        {

            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(page, pageSize, seachValue, out rowCount);

            Models.SupplierPaginationResult model = new Models.SupplierPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = seachValue,
                RowCount = rowCount,
                Data = data
            };

            return View(model);

            /* ViewBag.seachValue = seachValue;
             ViewBag.currentPage = page;

             int pageSize = 10;
             int rowCount = 0;

             var model = CommonDataService.ListOfSupplier(page, pageSize, seachValue,out rowCount);
             ViewBag.RowCount = rowCount;

             int pageCount = rowCount / pageSize;
             if (rowCount % pageSize != 0)
                 pageCount += 1;

             ViewBag.pageCount = pageCount;
             return View(model);*/
        }

        public ActionResult Create()
        {
            Supplier model = new Supplier()
            {
                SupplierID = 0
            };
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View(model);
        }

       [Route("edit/{supplierID}")]
        public ActionResult Edit(int supplierID)
        {
            Supplier model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Thay đổi thông tin nhà cung cấp";
            return View("Create", model);
        }

        [Route("delete/{supplierID}")]
        public ActionResult Delete(int supplierID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteSupplier(supplierID);
                return RedirectToAction("Index");
            }

            Supplier model = CommonDataService.GetSupplier(supplierID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Supplier model)
        {
            if (string.IsNullOrWhiteSpace(model.SupplierName))
            {
                // Tên thông báo lỗi( duy nhất ) , 
                ModelState.AddModelError("SupplierName", "Tên nhà cung cấp không được để trống");
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

            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
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
                if (model.SupplierID == 0)
                    ViewBag.Title = "Bổ sung nhà cung cấp";
                else
                    ViewBag.Title = "Thay đổi thông tin nhà cung cấp";

                return View("Create", model);
            }

            if (model.SupplierID == 0)
            {

                CommonDataService.AddSupplier(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateSupplier(model);
                return RedirectToAction("Index");
            }
        }
    }
}