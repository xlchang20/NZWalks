using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (existWalkDifficulty == null) { return null; }
            // Delete the existWalkDifficulty
            nZWalksDbContext.WalkDifficulty.Remove(existWalkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return existWalkDifficulty;
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        
        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty == null) { return null; }
            existingWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
