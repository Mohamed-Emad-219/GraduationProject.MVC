using GP.BLL.ViewModels;
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
        IEnumerable<Application> GetApplicationsPending();
        Application GetApplicationById(int id);
        void UpdateApplication(Application app);
        int GetTotalApplications(int year);
        int GetTotalApproved(int year);
        int GetTotalRejected(int year);
        List<CollegeApplicationStats> GetApplicationsByCollege(int year);
        GenderDistribution GetGenderDistribution(int year);
    }
}
