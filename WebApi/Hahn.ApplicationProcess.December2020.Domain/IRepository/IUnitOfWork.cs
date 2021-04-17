using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicantRepository applicant { get; }
    }
}
