using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImagesApp.Data
{
	public class MetadataRepository
	{
		private readonly DatabaseContext _databaseContext;

		public MetadataRepository(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public async Task CreateMetadata(string fileName)
		{
			var fileNameWithoutExt = Path.GetFileName(fileName);
			var metadata = await _databaseContext.Images.FirstOrDefaultAsync(x => x.FileName == fileNameWithoutExt);
			if (metadata == null)
			{
				metadata = new ImageMetadataEntity
				{
					Id = Guid.NewGuid(),
					CreatedAt = DateTime.UtcNow,
					FileExtension = Path.GetExtension(fileName),
					FileName = fileNameWithoutExt
				};
				_databaseContext.Images.Add(metadata);
			}
			else
			{
				// Replace file metadata if uploaded file with the same name
				metadata.CreatedAt = DateTime.UtcNow;
				metadata.FileExtension = Path.GetExtension(fileName);
				metadata.FileName = fileNameWithoutExt;
			}

			await _databaseContext.SaveChangesAsync();
		}

		public async Task<ImageMetadataEntity> GetMetadataByName(string fileName)
		{
			var fileNameWithoutExt = Path.GetFileName(fileName);
			return await _databaseContext.Images.FirstOrDefaultAsync(x => x.FileName == fileNameWithoutExt);
		}

		public async Task DeleteMetadata(string fileName)
		{
			var fileNameWithoutExt = Path.GetFileName(fileName);
			var metadata = await _databaseContext.Images.FirstOrDefaultAsync(x => x.FileName == fileNameWithoutExt);
			if (metadata != null)
			{
				_databaseContext.Images.Remove(metadata);
				await _databaseContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ImageMetadataEntity>> GetAllMetadata()
		{
			return await _databaseContext.Images.ToListAsync();
		}
	}
}