using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IOpentdbService
    {
        Task<List<Question>> GetQuestions();
    }
}
