﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class AnswerDto
    {
        public string Answer { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
