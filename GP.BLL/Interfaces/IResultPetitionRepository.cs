using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IResultPetitionRepository
    {
        int AddResultPetition(ResultPetition result);
    }
}
