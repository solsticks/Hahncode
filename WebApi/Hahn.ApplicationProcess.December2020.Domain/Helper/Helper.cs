using Hahn.ApplicationProcess.December2020.Domain.Models;
using Hahn.ApplicationProcess.December2020.Domain.Services;
using Hahn.ApplicationProcess.December2020.Domain.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain.Helper
{
    public static class Helper
    {
        public static async Task<List<KeyValue>> ApplicantValidationErrors(Applicant applicant)
        {
            var validationErrors = new List<KeyValue>();
            var validator = new ApplicantValidator();

            var results = await validator.ValidateAsync(applicant);
            var validatedResponseForCountry = await HttpClientService.ValidateCountryName(applicant.CountryOfOrigin);

            if (!results.IsValid)
            {
                validationErrors.AddRange(results.Errors.Select(error => new KeyValue
                {
                    PropertyName = error.PropertyName,
                    PropertyValue = error.ErrorMessage,
                    HasValidationErrors = true
                }));

                return validationErrors;
            }

            if (!validatedResponseForCountry.Success)
            {

                validationErrors.Add(new KeyValue
                {
                    PropertyName = "Country",
                    PropertyValue = validatedResponseForCountry.Message,
                    HasValidationErrors = true
                });
            }

            return validationErrors;
        }
    }
}

