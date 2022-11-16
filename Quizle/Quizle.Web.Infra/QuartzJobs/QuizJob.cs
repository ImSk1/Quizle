using Microsoft.Extensions.Configuration;
using Quartz;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using System.Diagnostics;

namespace Quizle.Web.Infra.QuartzJobs
{
    public class QuizJob : IJob
    {
        private readonly IQuizDataService _triviaDataService;
        private readonly IUserService _userService;
        
        private readonly IConfiguration _config;
        public QuizJob(IQuizDataService triviaDataService, IConfiguration configuration, IUserService userService)
        {
            _triviaDataService = triviaDataService;
            _config = configuration;
            _userService = userService;           
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var quizEasy = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlEasy"));
            var quizMedium = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlMedium"));
            var quizHard = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlHard"));

            await _triviaDataService.AddQuizRange(new List<QuizDto> { quizEasy, quizMedium, quizHard});
            await _userService.UpdateAllUsersHasDoneQuestion(false);            
        }
    }
}
