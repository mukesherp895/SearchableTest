using SearchableTest.WebApp.DataAccess;
using SearchableTest.WebApp.Models;
using SearchableTest.WebApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Services
{
    public class ContestantService : Service<Contestant>, IContestantService
    {
        private readonly IRepository<Contestant> _contestantReposiotry;
        private readonly IUnitOfWork _unitOfWork;
        public ContestantService(IRepository<Contestant> contestantReposiotry, IUnitOfWork unitOfWork) : base(contestantReposiotry)
        {
            _contestantReposiotry = contestantReposiotry;
            _unitOfWork = unitOfWork;
        }
        public bool Add(ContestantDto dto)
        {
            Contestant contestant = new Contestant()
            {
                Firstname = dto.Firstname,
                Lastname = dto.Lastname,
                DOB = DateTime.Parse(dto.DOB),
                IsActive = dto.IsActive,
                DistrictId = dto.DistrictId,
                Gender = dto.Gender,
                ImgUrl = dto.ImgUrlPath,
                Address = dto.Address
            };
            if (_contestantReposiotry.Create(contestant))
            {
                if (_unitOfWork.Commit())
                {
                    return true;
                }
            }
            return false;
        }
        public bool Edit(ContestantDto dto)
        {
            var entity = _contestantReposiotry.GetById(dto.Id);
            entity.Firstname = dto.Firstname;
            entity.Lastname = dto.Lastname;
            entity.DOB = DateTime.Parse(dto.DOB);
            entity.IsActive = dto.IsActive;
            entity.DistrictId = dto.DistrictId;
            entity.Gender = dto.Gender;
            entity.ImgUrl = string.IsNullOrEmpty(dto.ImgUrlPath) ? entity.ImgUrl : dto.ImgUrlPath;
            entity.Address = dto.Address;

            if (_contestantReposiotry.Update(entity))
            {
                if (_unitOfWork.Commit())
                {
                    return true;
                }
            }
            return false;
        }
        public bool Delete(int id)
        {
            var entity = _contestantReposiotry.GetById(id);

            if (_contestantReposiotry.DeleteObject(entity))
            {
                if (_unitOfWork.Commit())
                {
                    return true;
                }
            }
            return false;
        }
    }
    public interface IContestantService : IService<Contestant>
    {
        bool Add(ContestantDto dto);
        bool Edit(ContestantDto dto);
        bool Delete(int id);
    }
}