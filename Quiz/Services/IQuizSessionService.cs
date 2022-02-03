using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IQuizSessionService
    {
        Task LoadQuestions();
        Question GetQuestion();
        bool IsFinished { get; }
        void CheckAnswer(int id);
        int GetScore { get; }
        void ReviewQuestions();
        int CurrentQuestion { get; }
    }
}
