using Hahn.ApplicationProcess.December2020.Domain.DTOs;
using Hahn.ApplicationProcess.December2020.Domain.IRepository;
using Hahn.ApplicationProcess.December2020.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain.Services
{
    public class ApplicantService
    {        
        private readonly IUnitOfWork _unitOfWork;

        public ApplicantService(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Applicant>> SaveApplicant(CreateApplicantDto model)
        {
            var response = new Response<Applicant>();

            var applicant = new Applicant
            {
                Name = model.Name,
                Address = model.Address,
                Age = model.Age,
                CountryOfOrigin = model.CountryOfOrigin,
                EmailAddress = model.EmailAddress,
                FamilyName = model.FamilyName,
                Hired = model.Hired
            };

            var validationErrors = await Helper.Helper.ApplicantValidationErrors(applicant);

            var hasValidationErrors = validationErrors.FirstOrDefault(e => e.HasValidationErrors);

            if (hasValidationErrors != null)
            {
                response.Success = false;
                response.Errors = validationErrors;

                return response;
            }

            var applicantExists = await _unitOfWork.applicant.FindByEmail(model.EmailAddress);

            if (applicantExists == null)
            {

                await _unitOfWork.applicant.Save(applicant);

                var fetchApplicant = await _unitOfWork.applicant.FindByEmail(applicant.EmailAddress);

                response.Message = "Applicant saved successfully";
                response.Success = true;
                response.Data = fetchApplicant;

                return response;
            }

            response.Message = $"Applicant with these email {model.EmailAddress} already been added";
            response.Success = false;
            return response;
        }


        public async Task<PaginatedResponseDto<IEnumerable<Applicant>>> GetAllApplicants(int pageNumber, int perPage)
        {
            var applicants = await _unitOfWork.applicant.FindAll();

            var count = await _unitOfWork.applicant.Count();
            return new PaginatedResponseDto<IEnumerable<Applicant>>()
            {
                Name = "Applicants",
                Count = count,
                Data = applicants.OrderByDescending(x => x.ID).Skip((pageNumber - 1) * perPage).Take(perPage),
                PageNumber = pageNumber,
                PerPage = perPage
            };
        }

        public async Task<Response<Applicant>> UpdateApplicant(UpdateApplicantDto model)
        {
            var response = new Response<Applicant>();

            var applicant = await _unitOfWork.applicant.Find(model.ApplicantId);

            if (applicant == null)
            {
                response.Message = $"Applicant with applicantID {model.ApplicantId} does not exist";
                response.Success = false;
                return response;
            }
            applicant.Name = model.Name;
            applicant.EmailAddress = model.EmailAddress;
            applicant.FamilyName = model.FamilyName;
            applicant.Address = model.Address;
            applicant.Age = model.Age;
            applicant.Hired = model.Hired;
            applicant.CountryOfOrigin = model.CountryOfOrigin;

            var validationErrors = await Helper.Helper.ApplicantValidationErrors(applicant);

            var hasValidationErrors = validationErrors.FirstOrDefault(e => e.HasValidationErrors);

            if (hasValidationErrors != null)
            {
                response.Success = false;
                response.Errors = validationErrors;

                return response;
            }


            await _unitOfWork.applicant.Update(applicant);

            var updatedApplicant = await _unitOfWork.applicant.Find(model.ApplicantId);

            response.Success = true;
            response.Message = "Applicant updated successfully";
            response.Data = updatedApplicant;

            return response;
        }

    }
}

