using SdvCode.Data;
using SdvCode.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Constraints
{
    public class CreateCategoryGlobally
    {
        private readonly ApplicationDbContext db;

        public CreateCategoryGlobally(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateCategory(string name, string description)
        {
            if (this.db.Categories.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return false;
            }

            var category = new Category
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                Description = description,
            };

            this.db.Categories.Add(category);
            await this.db.SaveChangesAsync();
            return true;
        }
    }
}