using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchableTest.WebApp.Services;
using SearchableTest.WebApp.Models.Dto;
using System.IO;

namespace SearchableTest.WebApp.Controllers
{
    public class PhotoGalleryController : Controller
    {
        private readonly IContestantRatingService _contestantRatingService;
        public PhotoGalleryController(IContestantRatingService contestantRatingService)
        {
            _contestantRatingService = contestantRatingService;
        }
        public ActionResult Index()
        {
            var reqDto = new ContestantRatingReqDto();
            var model = _contestantRatingService.GetContestantRating(reqDto);
            return View(model);
        }
        public ActionResult Download(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(Server.MapPath("~/img"), "NoImg.jpg");
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            catch
            {
            }
            return new EmptyResult();
        }
    }
}