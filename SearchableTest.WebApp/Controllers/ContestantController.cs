using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchableTest.WebApp.Services;
using SearchableTest.WebApp.Models;
using SearchableTest.WebApp.Models.Dto;
using System.IO;
using OfficeOpenXml;

namespace SearchableTest.WebApp.Controllers
{
    public class ContestantController : Controller
    {
        private readonly IContestantService _contestantService;
        private readonly IService<District> _districtService;
        public ContestantController(IContestantService contestantService, IService<District> districtService)
        {
            _contestantService = contestantService;
            _districtService = districtService;
        }
        private void init()
        {
            ViewBag.Districts = new SelectList(_districtService.GetAll().Select(s => new { Id = s.Id, Name = s.DistrictName }).ToList(), "Id", "Name");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetContestantRecords(DataTablesParam param)
        {
            List<ContestantDto> data = new List<ContestantDto>();
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

                IQueryable<Contestant> dataQuery;

                if (param.sSearch != null)
                {
                    dataQuery = _contestantService.Search(s => s.Firstname.Contains(param.sSearch) || s.Lastname.Contains(param.sSearch) || s.District.DistrictName.Contains(param.sSearch) || s.Gender.Contains(param.sSearch) || s.Address.Contains(param.sSearch));

                    totalCount = dataQuery.Count();
                }
                else
                {
                    dataQuery = _contestantService.GetAll().OrderBy(o => o.Id);
                    totalCount = dataQuery.Count();
                }

                if (string.Equals("desc", sortDirection, StringComparison.OrdinalIgnoreCase))
                {
                    if (sortColumnIndex == 1)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Firstname);
                    }
                    else if (sortColumnIndex == 2)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Lastname);
                    }
                    else if (sortColumnIndex == 3)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.DOB);
                    }
                    else if (sortColumnIndex == 4)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Gender);
                    }
                    else if (sortColumnIndex == 5)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Address);
                    }
                    else if (sortColumnIndex == 6)
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.District.DistrictName);
                    }
                    else 
                    {
                        dataQuery = dataQuery.OrderByDescending(o => o.Id);
                    }
                }
                else
                {
                    if(sortColumnIndex == 1)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Firstname);
                    }
                    else if (sortColumnIndex == 2)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Lastname);
                    }
                    else if (sortColumnIndex == 3)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.DOB);
                    }
                    else if (sortColumnIndex == 4)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Gender);
                    }
                    else if (sortColumnIndex == 5)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Address);
                    }
                    else if (sortColumnIndex == 6)
                    {
                        dataQuery = dataQuery.OrderBy(o => o.District.DistrictName);
                    }
                    else
                    {
                        dataQuery = dataQuery.OrderBy(o => o.Id);
                    }
                }
                data = dataQuery.Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).AsEnumerable().Select(s => new ContestantDto()
                {
                    Id = s.Id,
                    FullName = s.Firstname,
                    Lastname = s.Lastname,
                    DOB = s.DOB.ToString("yyyy-MM-dd"),
                    Gender = s.Gender,
                    Address = s.Address,
                    DistrictName = s.District.DistrictName,
                    IsActive = s.IsActive,
                    ButtonAction = "<button type=\"button\" style=\"margin-right:5px;\" class=\"btn btn-primary btn-xs\" onclick=\"AddEditContestant(" + s.Id + ")\">Edit</button>" + "<button type=\"button\" class=\"btn btn-danger btn-xs\" data-id=\"" + s.Id + "\" id=\"DeleteButton\">Delete</button>"
                }).ToList();
            }
            catch(Exception ex)
            {
            }
            return Json(new { aaData = data, sEcho = param.sEcho, iTotalDisplayRecords = totalCount, iTotalRecords = totalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEdit(int id)
        {
            ContestantDto model = new ContestantDto();
            try
            {
                if (id > 0)
                {
                    model = _contestantService.Search(s => s.Id == id).AsEnumerable().Select(s => new ContestantDto()
                    {
                        Id = s.Id,
                        Firstname = s.Firstname,
                        Lastname = s.Lastname,
                        DOB = s.DOB.ToString("yyyy-MM-dd"),
                        IsActive = s.IsActive,
                        DistrictId = s.DistrictId,
                        Gender = s.Gender,
                        ImgUrlPath = s.ImgUrl,
                        Address = s.Address
                    }).FirstOrDefault();
                }
                else
                {
                    model.DOB = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                init();
                return PartialView("_AddUpdateContestant", model);
            }
            catch
            {
                return new EmptyResult();
            }
            
        }
        [HttpPost]
        public ActionResult AddEdit(ContestantDto dto)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                if (ModelState.IsValid)
                {
                    if (dto.ImgUrl != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/ContestantImage"), Guid.NewGuid().ToString() + Path.GetExtension(dto.ImgUrl.FileName));
                        dto.ImgUrl.SaveAs(path);
                        dto.ImgUrlPath = path;
                    }
                    if (dto.Id == 0)
                    {
                        if (_contestantService.Add(dto))
                        {
                            responseDto.Status = "00";
                            responseDto.Message = "Contestant Added Successfully.";
                            return Json(responseDto);
                        }
                    }
                    else
                    {
                        if (_contestantService.Edit(dto))
                        {
                            responseDto.Status = "99";
                            responseDto.Message = "Contestant Edited Successfully.";
                            return Json(responseDto);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
            }
            responseDto.Status = "99";
            responseDto.Message = "Fail to Add/Edit Contestant.";
            return Json(responseDto);
        }
        public ActionResult Delete(int id)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                if (id > 0)
                {
                    if (_contestantService.Delete(id))
                    {
                        responseDto.Status = "00";
                        responseDto.Message = "Contestant Deleted Successfully.";
                        return Json(responseDto);
                    }
                }
            }
            catch
            {
            }
            responseDto.Status = "00";
            responseDto.Message = "Fail to Delete Contestant.";
            return Json(responseDto);
        }
        [HttpPost]
        public ActionResult ExcelFile()
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                ExcelPackage excel = new ExcelPackage();

                var data = _contestantService.GetAll();

                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("FirstName");
                dt.Columns.Add("LastName");
                dt.Columns.Add("DOB");
                dt.Columns.Add("Gender");
                dt.Columns.Add("Address");
                dt.Columns.Add("IsActive");
                dt.Columns.Add("District");


                int totalCol = dt.Columns.Count;
                var excelWS = excel.Workbook.Worksheets.Add("Contestant");
                int row = 1;

                excelWS.Cells[row, 1].Value = "Id";
                excelWS.Cells[row, 1].Style.Font.Bold = true;

                excelWS.Cells[row, 2].Value = "First Name";
                excelWS.Cells[row, 2].Style.Font.Bold = true;

                excelWS.Cells[row, 3].Value = "Last Name";
                excelWS.Cells[row, 3].Style.Font.Bold = true;

                excelWS.Cells[row, 4].Value = "Date Of Birth";
                excelWS.Cells[row, 4].Style.Font.Bold = true;

                excelWS.Cells[row, 5].Value = "Gender";
                excelWS.Cells[row, 5].Style.Font.Bold = true;

                excelWS.Cells[row, 6].Value = "Address";
                excelWS.Cells[row, 6].Style.Font.Bold = true;

                excelWS.Cells[row, 7].Value = "Is Active?";
                excelWS.Cells[row, 7].Style.Font.Bold = true;

                excelWS.Cells[row, 8].Value = "District";
                excelWS.Cells[row, 8].Style.Font.Bold = true;

                row++;

                foreach (var item in data)
                {
                    dt.Rows.Add(new object[] { item.Id, item.Firstname, item.Lastname, item.DOB, item.Gender, item.Address, item.IsActive, item.District.DistrictName });
                    for (int col = 1; col <= dt.Columns.Count; col++)
                    {
                        excelWS.Cells[row, col].Value = dt.Rows[0][col - 1];
                    }
                    dt.Rows.Clear();
                    row++;
                }

                string fileName = Guid.NewGuid().ToString() + ".xlsx";
                string path = Path.Combine(Server.MapPath("~/ContestantImage"), fileName);
                FileInfo fi = new FileInfo(path);
                excel.SaveAs(fi);
                excel.Dispose();
                responseDto.Status = "00";
                responseDto.Message = path;

            }
            catch(Exception ex)
            {
                responseDto.Status = "99";
            }
            return Json(responseDto);
        }
        public ActionResult DownloadExcelFile(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
                return File(fileByteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullPath));
            }
            return new EmptyResult();
        }
    }
}