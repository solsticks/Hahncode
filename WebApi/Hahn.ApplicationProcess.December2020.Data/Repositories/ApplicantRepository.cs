using Hahn.ApplicationProcess.December2020.Domain.IRepository;
using Hahn.ApplicationProcess.December2020.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Repositories
{
    public class ApplicantRepository : BaseRepository<Applicant>, IApplicantRepository
    {
        private readonly AppDbContext _ctx;

        public ApplicantRepository(AppDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Applicant> FindByEmail(string email)
        {
            var applicant = await _ctx.Applicants.FirstOrDefaultAsync(a => a.EmailAddress == email);
            return applicant;
        }
    }
}
