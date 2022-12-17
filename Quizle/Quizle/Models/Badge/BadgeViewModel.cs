namespace Quizle.Web.Models.Badge
{
    public class BadgeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Rarity { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public List<string>? OwnerIds { get; set; }
        public int Price { get; set; }



    }
}
