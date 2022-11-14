using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.DB
{
    public class QuizleDbContext : IdentityDbContext<ApplicationUser>
    {
        public QuizleDbContext(DbContextOptions<QuizleDbContext> options)
           : base(options)
        {

        }
        public DbSet<Quiz> QuizzesHistory { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
