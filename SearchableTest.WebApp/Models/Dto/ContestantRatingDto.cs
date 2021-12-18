using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Models.Dto
{
    public class ContestantRatingDto
    {
        public int ContestantId { get; set; }
        public string FullName { get; set; }
        public string DOB { get; set; }
        public string DistrictName { get; set; }
        public decimal AverageRating { get; set; }
        public int Rating { get; set; }
        public string ImagePath { get; set; }
    }
}