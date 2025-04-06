using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IPetitionRequestRepository 
    {
        int AddPetition(PetitionRequest PetitionRequest);
        IEnumerable<PetitionRequest> GetPetitionRequests();
        PetitionRequest GetPetitionRequestById(int Id);
    }
}
