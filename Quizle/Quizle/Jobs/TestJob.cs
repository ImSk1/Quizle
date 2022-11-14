using Quartz;
using System.Diagnostics;

namespace Quizle.Web.Jobs
{
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine("bahti qkoto");
        }
    }
}
