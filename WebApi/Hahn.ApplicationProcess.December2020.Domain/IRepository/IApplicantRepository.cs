using Hahn.ApplicationProcess.December2020.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain.IRepository
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        Task<Applicant> FindByEmail(string email);
    }
}
