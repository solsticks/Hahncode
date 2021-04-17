using Hahn.ApplicationProcess.December2020.Domain.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicationProcess.December2020.Web.SwaggerExamples.Requests
{
    public class CreateApplicantExample : IExamplesProvider<CreateApplicantDto>
    {
        public CreateApplicantDto GetExamples()
        {
            return new CreateApplicantDto()
            {
                Address = "6 asanjo ajah lagos",
                Age = 21,
                CountryOfOrigin = "Nigeria",
                EmailAddress = "applicant@gmail.com",
                FamilyName = "Clark",
                Hired = false,
                Name = "James"
            };
        }
    }
}
