using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IApplicationRepository
    {
        int AddApplication(Application App);
        IEnumerable<Application> GetApplications();
        Application GetApplicationById(int id);
        void UpdateApplication(Application app);
    }
}
