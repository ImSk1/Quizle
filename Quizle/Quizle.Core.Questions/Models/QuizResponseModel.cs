using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quizle.Core.Models
{
    internal class QuizResponseModel
    {
        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; } = new List<Result>();
        public class Result
        {
            [JsonProperty("category")]
            public string Category { get; set; } = null!;

            [JsonProperty("type")]
            public string Type { get; set; } = null!;

            [JsonProperty("difficulty")]
            public string Difficulty { get; set; } = null!;

            [JsonProperty("question")]
            public string Question { get; set; } = null!;

            [JsonProperty("correct_answer")]
            public string CorrectAnswer { get; set; } = null!;

            [JsonProperty("incorrect_answers")]
            public List<string> IncorrectAnswers { get; set; } = new List<string>();
        }
    }
}
