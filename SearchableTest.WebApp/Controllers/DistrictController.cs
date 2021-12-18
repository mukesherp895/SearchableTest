using SearchableTest.WebApp.Models;
using SearchableTest.WebApp.Models.Dto;
using SearchableTest.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SearchableTest.WebApp.Controllers
{
    public class DistrictController : Controller
    {
        private readonly IService<District> _districtService;
        public DistrictController(IService<District> districtService)
        {
            _districtService = districtService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDistrictRecords(DataTablesParam param)
        {
            List<DistrictDto> data = new List<DistrictDto>();
            int totalCount = 0;
            try
            {
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];
                int pageNo = 1;

                if (param.iDisplayStart >= param.iDisplayLength)
                {
                    pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;
                }

                IQueryable<District> dataQuery;

                if (param.sSearch != null)
                {
                    dataQuery = _districtService.Search(s => s.DistrictName.Contains(param.sSearch));

                    totalCount = dataQuery.Count();
                }
                else
                {
                    dataQuery = _districtService.GetAll();
                    totalCount = dataQuery.Count();
                }

                if (string.Equals("desc", sortDirection, StringComparison.OrdinalIgnoreCase))
                {
                    if (sortColumnIndex == 0)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Id);
                    }
                    else 
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.DistrictName);
                    }
                }
                else
                {
                    if (sortColumnIndex == 0)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Id);
                    }
                    else
                    {
                        dataQuery = dataQuery.OrderBy(o => o.DistrictName);
                    }
                }
                data = dataQuery.Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).AsEnumerable().Select(s => new DistrictDto()
                {
                    Id = s.Id,
                    DistrictName = s.DistrictName,
                }).ToList();
            }
            catch
            {
            }
            return Json(new { aaData = data, sEcho = param.sEcho, iTotalDisplayRecords = totalCount, iTotalRecords = totalCount }, JsonRequestBehavior.AllowGet);
        }
    }
}