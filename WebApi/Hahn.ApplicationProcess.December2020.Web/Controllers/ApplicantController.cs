using Hahn.ApplicationProcess.December2020.Domain.DTOs;
using Hahn.ApplicationProcess.December2020.Domain.IRepository;
using Hahn.ApplicationProcess.December2020.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ApplicantController(IUnitOfWork unitOfWork, ILogger<ApplicantController> logger)
        {            
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        /// <summary>
        /// Returns a paginated list of applicants in the database 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int perPage = 6)
        {
            if (pageNumber <= 0)
            {
                _logger.LogInformation($"Bad request on get for all applicants by page number {pageNumber}.");

                return BadRequest(new { message = $"Page number {pageNumber} is invalid " });
            }
            var applicantService = new ApplicantService(_unitOfWork);
            var applicants = await applicantService.GetAllApplicants(pageNumber, perPage);

            _logger.LogInformation($"Get request for all applicants.");

            return applicants.Count == 0 ? Ok(new { message = "No applicants in the database" }) : Ok(applicants);
        }
        /// <summary>
        /// Saves an applicant to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicantDto model)
        {
            try
            {
                var applicantService = new ApplicantService(_unitOfWork);
                var response = await applicantService.SaveApplicant(model);

                if (!response.Success)
                {
                    _logger.LogInformation($"Bad request on creation of an applicant with email {model.EmailAddress}.");

                    return BadRequest(response);
                }
                _logger.LogInformation($"Applicant was created successfully with id {response.Data.ID}.");

                return Created($"http://localhost:5000/api/user/{response.Data.ID}", response);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);

                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Returns an applicant based on the applicantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var applicant = await _unitOfWork.applicant.Find(id);

            if (applicant == null)
            {
                _logger.LogInformation($"Bad request on get of an applicant with id {id}.");

                return BadRequest($"Applicant with ID {id} does not exists");
            }
            _logger.LogInformation($"Get request of an applicant with id {applicant.ID}.");

            return Ok(applicant);
        }
        /// <summary>
        /// Updates an applicant based on the applicantId
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateApplicantDto model, int id)
        {
            model.ApplicantId = id;
            var applicantService = new ApplicantService(_unitOfWork);
            var response = await applicantService.UpdateApplicant(model);

            if (!response.Success)
            {
                _logger.LogInformation($"Bad request on update of an applicant with id {id}.");

                return BadRequest(response);
            }
            _logger.LogInformation($"Update request was made by applicant with id ${response.Data.ID}.");

            return Ok(response);
        }
        /// <summary>
        /// Deletes an applicant based on the applicantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var applicant = await _unitOfWork.applicant.Exist(id);

            if (!applicant)
            {
                _logger.LogInformation($"Bad request on delete of an applicant with id {id}.");

                return BadRequest($"Applicant with applicantID:{id} does not exist");
            }

            await _unitOfWork.applicant.Delete(id);
            _logger.LogInformation($"Delete request was made by applicant with id ${id}.");

            return Ok($"Applicant with applicantId: {id} has been deleted successfully");
        }
    }
}
