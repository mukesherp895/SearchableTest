using SearchableTest.WebApp.DataAccess;
using SearchableTest.WebApp.Models;
using SearchableTest.WebApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Services
{
    public class ContestantRatingService : Service<ContestantRating>, IContestantRatingService
    {
        private readonly IRepository<Contestant> _contestantRepository;
        private readonly IRepository<ContestantRating> _contestantRatingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContestantRatingService(IRepository<ContestantRating> contestantRatingRepository, IUnitOfWork unitOfWork, IRepository<Contestant> contestantRepository) : base(contestantRatingRepository)
        {
            _contestantRatingRepository = contestantRatingRepository;
            _unitOfWork = unitOfWork;
            _contestantRepository = contestantRepository;
        }
        public List<ContestantRatingDto> GetContestantRating(ContestantRatingReqDto reqDto)
        {
            List<ContestantRatingDto> data = new List<ContestantRatingDto>();
            List<ContestantRatingDto> contestantRatingData = new List<ContestantRatingDto>();
            data = _contestantRepository.Search(s => s.IsActive == true).AsEnumerable().Select(s => new ContestantRatingDto()
            {
                ContestantId = s.Id,
                FullName = s.Firstname + " " + s.Lastname,
                DOB = s.DOB.ToString("yyyy-MM-dd"),
                DistrictName = s.District.DistrictName,
                ImagePath = s.ImgUrl
            }).ToList();

            if (!string.IsNullOrEmpty(reqDto.FromDate) && !string.IsNullOrEmpty(reqDto.ToDate))
            {
                var fromDate = DateTime.Parse(reqDto.FromDate).Date;
                var toDate = DateTime.Parse(reqDto.ToDate).Date;
                contestantRatingData = _contestantRatingRepository.Search(s => DbFunctions.TruncateTime(s.RatedDate) >= fromDate && DbFunctions.TruncateTime(s.RatedDate) <= toDate).Select(s => new ContestantRatingDto()
                {
                    ContestantId = s.ContestantId,
                    Rating = s.Rating
                }).ToList();
            }
            else
            {
                contestantRatingData = _contestantRatingRepository.GetAll().AsEnumerable().Select(s => new ContestantRatingDto() 
                { 
                    ContestantId = s.ContestantId,
                    Rating = s.Rating
                }).ToList();
            }
            data?.ForEach(item =>
            {
                if (contestantRatingData.Where(w => w.ContestantId == item.ContestantId).Any())
                {
                    item.AverageRating = Math.Round(contestantRatingData.Where(w => w.ContestantId == item.ContestantId).Sum(s => s.Rating) / Convert.ToDecimal(contestantRatingData.Where(w => w.ContestantId == item.ContestantId).Count()),2);
                }
            });
            return data;
        }
        public bool Add(ContestantRatingDto dto)
        {
            ContestantRating contestantRating = new ContestantRating()
            {
                ContestantId = dto.ContestantId,
                Rating = dto.Rating,
            };
            if (_contestantRatingRepository.Create(contestantRating))
            {
                if (_unitOfWork.Commit())
                {
                    return true;
                }
            }
            return false;
        }
    }
    public interface IContestantRatingService : IService<ContestantRating>
    {
        List<ContestantRatingDto> GetContestantRating(ContestantRatingReqDto reqDto);
        bool Add(ContestantRatingDto dto);
    }
}