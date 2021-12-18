using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Models.Dto
{
    public class ContestantDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        [Display(Name = "First Name")]
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum {1} characters allowed")]
        public string Firstname { get; set; }
        [Display(Name = "Last Name")]
        [MaxLength(20, ErrorMessage = "Maximum {1} characters allowed")]
        public string Lastname { get; set; }
        [Display(Name = "Date Of Birth")]
        [Required]
        public string DOB { get; set; }
        public bool IsActive { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        [Display(Name = "Gender")]
        [MaxLength(10, ErrorMessage = "Maximum {1} characters allowed")]
        public string Gender { get; set; }
        [Display(Name = "Image")]
        public HttpPostedFileBase ImgUrl { get; set; }
        public string ImgUrlPath { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum {1} characters allowed")]
        public string Address { get; set; }
        public string ButtonAction { get; set; }
    }
}