using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<Option> Options { get; set; }
    }
}
