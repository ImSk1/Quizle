using Microsoft.Extensions.Configuration;
using Quartz;
using Quizle.Core.Contracts;
using Quizle.Core.Models;

using System.Diagnostics;

namespace Quizle.Core.QuartzJobs
{
    public class GetQuestionsJob : IJob
    {
        private readonly IQuizDataService _triviaDataService;
        private readonly IProfileService _userService;
        
        private readonly IConfiguration _config;
        public GetQuestionsJob(IQuizDataService triviaDataService, IConfiguration configuration, IProfileService userService)
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
