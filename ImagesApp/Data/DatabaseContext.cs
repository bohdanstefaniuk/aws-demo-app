using Microsoft.EntityFrameworkCore;

namespace ImagesApp.Data
{
	public class DatabaseContext: DbContext
	{
		public DatabaseContext(DbContextOptions dbContextOptions)
			: base(dbContextOptions)
		{
		}

		public DbSet<ImageMetadataEntity> Images { get; set; }
	}
}