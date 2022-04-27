using SV18T1021208.BusinessLayer;
using SV18T1021208.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021208.Web.Controllers
{
    [Authorize]
    [RoutePrefix("shipper")]
    public class ShipperController : Controller
    {
        // GET: Shipper
        /// <summary>
        /// Tìm kiếm và hiển thị danh sách người giao hàng 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="seachValue"></param>
        /// <returns></returns>
        public ActionResult Index(int page =1, string seachValue ="")
        {
            int pageSize = 5;
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(page, pageSize, seachValue, out rowCount);

            Models.ShipperPaginationResult model = new Models.ShipperPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = seachValue,
                RowCount = rowCount,
                Data = data
            };

            return View(model);

        }

        public ActionResult Create()
        {
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = "Bổ sung người giao hàng";
            return View(model);
        }

        [Route("edit/{shipperID}")]
        public ActionResult Edit(int shipperID)
        {
            Shipper model = CommonDataService.GetShipper(shipperID);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Chỉnh sửa thông tin người giao hàng";
            return View("Create",model);
        }

        [Route("delete/{shipperID}")]
        public ActionResult Delete(int shipperID)
        {
            if(Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(shipperID);
                return RedirectToAction("Index");
            }

            Shipper model = CommonDataService.GetShipper(shipperID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Shipper model)
        {
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError("ShipperName", "Tên người giao hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
            // False lỗi 
            // True ko lỗi
            if (!ModelState.IsValid)
            {
                if (model.ShipperID == 0)
                    ViewBag.Title = "Bổ sung người giao hàng";
                else
                    ViewBag.Title = "Thay đổi thông tin người giao hàng";

                return View("Create", model);
            }

            if (model.ShipperID == 0)
                CommonDataService.AddShipper(model);  
            else
                CommonDataService.UpdateShipper(model);
            
            return RedirectToAction("Index");
        }
    }
}