using Microsoft.Extensions.Configuration;
using Quartz;
using Quizle.Core.Contracts;
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
            var triviaData = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrl"));
            await _triviaDataService.AddQuiz(triviaData);
            await _userService.UpdateAllUsersHasDoneQuestion(false);            
        }
    }
}
