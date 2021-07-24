using System;

namespace ImagesApp.Data
{
	public class ImageMetadataEntity
	{
		public Guid Id { get; set; }
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}