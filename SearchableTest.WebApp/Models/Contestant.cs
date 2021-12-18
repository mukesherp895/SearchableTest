using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Models
{
    public class Contestant
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public int DistrictId { get; set; }
        public string Gender { get; set; }
        public string ImgUrl { get; set; }
        public string Address { get; set; }
        public virtual District District { get; set; }
    }
}