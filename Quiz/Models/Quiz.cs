using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class QuizSession
    {
        public int CurrentQuestion { get; set; }
        public int Score { get; set; }
        public bool IsFinished { get; set; }
        public List<Question> Questions { get; set; }
    }
}
