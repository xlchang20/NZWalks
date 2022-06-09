namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }


        public String Name { get; set; }


        public double Length { get; set; }


        public Guid RegionId { get; set; }


        public Guid WalkDifficultyId { get; set; }


        // Navigation Properties, A Walk have a many to one Region.
        public Region Region { get; set; }


        // Navigation Properties, A Walk have a many to one WalkDifficulty.
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
