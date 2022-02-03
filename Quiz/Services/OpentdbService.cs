using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quiz.Services
{

    public class OpentdbService : IOpentdbService
    {
        private readonly IHttpClientFactory _httpFactory;
        private const string _opentdbApiUrl = @"https://opentdb.com/api.php?amount=10&type=multiple";
        private List<Question> _questions;

        public OpentdbService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<List<Question>> GetQuestions()
        {
            var response = await GetResponse();
            GenerateQuestions(response);
            return _questions;
        }

        public async Task<Response> GetResponse()
        {
            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync(_opentdbApiUrl);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response>(json);
        }

        public void GenerateQuestions(Response response)
        {
            _questions = new List<Question>();
            foreach(Result result in response.results)
            {
                var options = new List<Option>();
                foreach(var option in result.incorrect_answers)
                {
                    options.Add(new Option { Text = option , IsCorrect = false });
                }
                options.Add(new Option { Text = result.correct_answer, IsCorrect = true });
                options = options.OrderBy(t => t.Text).ToList();
                int id = 0;
                foreach(Option option in options)
                {
                    option.Id = id;
                    id++;
                }

                Question question = new Question
                {
                    Options = options,
                    Text = result.question
                };
                _questions.Add(question);
            };
        }

    }
}
