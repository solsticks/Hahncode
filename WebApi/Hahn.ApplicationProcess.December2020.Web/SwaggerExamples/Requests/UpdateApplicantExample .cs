using Hahn.ApplicationProcess.December2020.Domain.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicationProcess.December2020.Web.SwaggerExamples.Requests
{
    public class UpdateApplicantExample : IExamplesProvider<UpdateApplicantDto>
    {
        public UpdateApplicantDto GetExamples()
        {
            return new UpdateApplicantDto()
            {
                Address = "5 asanjo ajah lagos",
                Age = 21,
                CountryOfOrigin = "Nigeria",
                EmailAddress = "applicant@gmail.com",
                FamilyName = "Onibokun",
                Hired = false,
                Name = "Adeyemi",
                ApplicantId = 1
            };
        }
    }
}
