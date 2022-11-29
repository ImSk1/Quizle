﻿namespace Quizle.Web.Models
{
    public class BadgeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Rarity { get; set; } = null!; 
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int Price { get; set; }
        
        

    }
}
