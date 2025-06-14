﻿using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface ICollegeRepository
    {
        IEnumerable<College> GetColleges();
        College GetCollegeById(int Id);
        int AddCollege(College college);
        int UpdateCollege(College college);
        int DeleteCollegeAsync(int Id);
        string GetCollageNameByStudentId(string id);
        int GetCollageIdByStudentId(string id);
        College GetCollegeByDeanId(string Id);
        int GetCollegeIdByDeanId(string Id);
    }
}
