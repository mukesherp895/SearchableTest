using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchableTest.WebApp.Services;
using SearchableTest.WebApp.Models.Dto;

namespace SearchableTest.WebApp.Controllers
{
    public class ContestantRatingController : Controller
    {
        private readonly IContestantRatingService _contestantRatingService;
        public ContestantRatingController(IContestantRatingService contestantRatingService)
        {
            _contestantRatingService = contestantRatingService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetContestantRating(ContestantRatingReqDto reqDto)
        {
            var data = _contestantRatingService.GetContestantRating(reqDto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Rating(ContestantRatingDto dto)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                if (dto.ContestantId > 0)
                {
                    if (_contestantRatingService.Add(dto))
                    {
                        responseDto.Status = "00";
                        responseDto.Message = "Contestant Rating Added Successfully.";
                        return Json(responseDto);
                    }
                }
            }
            catch
            {
            }
            responseDto.Status = "99";
            responseDto.Message = "Fail to Add Contestant Rating.";
            return Json(responseDto);
        }
    }
}