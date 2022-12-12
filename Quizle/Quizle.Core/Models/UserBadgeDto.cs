﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Models
{
    public class UserBadgeDto
    {
        public BadgeDto Badge { get; set; } = null!;
        public string DateAcquired { get; set; } = null!;
    }
}
