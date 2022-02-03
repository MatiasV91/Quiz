using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Infrastructure;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class QuizSessionService : IQuizSessionService
    {
        private IOpentdbService _opentdbService;
        public QuizSessionService(IOpentdbService opentdbService, IServiceProvider services)
        {
            _opentdbService = opentdbService;
            Session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            Quiz = Session?.GetJson<QuizSession>("QuizSession") ?? new QuizSession();
        }

        public ISession Session { get; set; }

        public QuizSession Quiz { get; set; }

        public bool IsFinished => Quiz.IsFinished;

        public int GetScore => Quiz.Score;

        public int CurrentQuestion => Quiz.CurrentQuestion;

        public async Task LoadQuestions()
        {
            Quiz = new QuizSession();
            Quiz.Questions = await _opentdbService.GetQuestions();
            Session.SetJson("QuizSession", Quiz);
        }

        public Question GetQuestion()
        {
            return Quiz.Questions[Quiz.CurrentQuestion];
        }

        public void CheckAnswer(int id)
        {
            if (!IsFinished)
            {
                Option option = Quiz.Questions[Quiz.CurrentQuestion].Options.SingleOrDefault(o => o.Id == id);
                option.WasSelected = true;
                if (option.IsCorrect)
                    Quiz.Score++;
            }
            Quiz.CurrentQuestion++;
            if (Quiz.CurrentQuestion >= 10)
                Quiz.IsFinished = true;
            Session.SetJson("QuizSession", Quiz);
        }

        public void ReviewQuestions()
        {
            Quiz.CurrentQuestion = 0;
            Session.SetJson("QuizSession", Quiz);
        }
    }
}
