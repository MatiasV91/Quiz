using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Controllers
{
    public class QuizController : Controller
    {

        private readonly IQuizSessionService _quizService;


        public QuizController(IQuizSessionService quizService)
        {
            _quizService = quizService;
        }

        public async Task<IActionResult> Index(bool reviewResult)
        {
            if (!reviewResult)
            {
                await _quizService.LoadQuestions();
            }
            var question = _quizService.GetQuestion();
            ViewBag.IsFinished = _quizService.IsFinished;
            return View(nameof(Questions), question);
        }


        [HttpPost]
        public IActionResult Questions(int id)
        {
            _quizService.CheckAnswer(id);
            if (_quizService.IsFinished && _quizService.CurrentQuestion == 10)
            {
                return RedirectToAction(nameof(QuizResults));
            }
            ViewBag.IsFinished = _quizService.IsFinished;
            return View(nameof(Questions), _quizService.GetQuestion());
        }

        public IActionResult QuizResults()
        {
            if (_quizService.IsFinished)
            {
                ViewBag.IsFinished = true;
                _quizService.ReviewQuestions();
                return View(_quizService.GetScore);
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
