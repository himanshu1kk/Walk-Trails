namespace NzWalks.Models.Domain
{
    public class Difficulty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Toughness? Toughness { get; set; }
    }

    public enum Toughness
    {
        EASY = 100,
        MEDIUM = 200,
        HARD = 300
    }
}