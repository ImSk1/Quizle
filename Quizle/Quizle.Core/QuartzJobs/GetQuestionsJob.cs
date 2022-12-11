using Microsoft.Extensions.Configuration;
using Quartz;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;

using System.Diagnostics;

namespace Quizle.Core.QuartzJobs
{
    public class GetQuestionsJob : IJob
    {
        private readonly IQuizService _triviaDataService;
        private readonly IProfileService _userService;
        
        private readonly IConfiguration _config;
        public GetQuestionsJob(IQuizService triviaDataService, IConfiguration configuration, IProfileService userService)
        {
            _triviaDataService = triviaDataService;
            _config = configuration;
            _userService = userService;           
        }
        public async Task Execute(IJobExecutionContext context)
        {
            QuizDto? quizEasy;
            QuizDto? quizMedium;
            QuizDto? quizHard;
            try
            {
				quizEasy = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlEasy"));
				quizMedium = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlMedium"));
				quizHard = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrlHard"));
			}
            catch (InvalidApiResponseException)
            {
                return;
            }
			catch (ArgumentException)
			{
                return;
			}

            try
			{
				await _triviaDataService.AddQuizRange(new List<QuizDto> { quizEasy, quizMedium, quizHard });
			}
			catch (ArgumentNullException)
            {
                return;
            }
            await _userService.UpdateAllUsersHasDoneQuestion(false);            
        }
    }
}
