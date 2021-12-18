using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchableTest.WebApp.Services;
using SearchableTest.WebApp.Models.Dto;

namespace SearchableTest.WebApp.Controllers
{
    public class MapController : Controller
    {
        private readonly IContestantService _contestantService;
        public MapController(IContestantService contestantService)
        {
            _contestantService = contestantService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDistrictWiseContestant(string district)
        {
            try
            {
                var data = _contestantService.Search(s => s.IsActive == true && s.District.DistrictName == district).AsEnumerable().Select(s => new ContestantDto()
                {
                    Firstname = s.Firstname,
                    Lastname = s.Lastname,
                    Gender = s.Gender,
                    DOB = s.DOB.ToString("yyyy-MM-dd"),
                    Address = s.Address,
                    DistrictName = s.District.DistrictName
                }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return new EmptyResult();
        }
    }
}