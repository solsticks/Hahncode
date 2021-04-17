using Hahn.ApplicationProcess.December2020.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            applicant = new ApplicantRepository(_db);
        }
        public IApplicantRepository applicant { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
