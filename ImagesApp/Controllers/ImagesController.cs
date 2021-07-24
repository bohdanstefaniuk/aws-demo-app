using System.Threading.Tasks;
using ImagesApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImagesApp.Controllers
{
	[Route("/api/files")]
	public class ImagesController : ControllerBase
	{
		private readonly MetadataRepository _metadataRepository;
		private readonly S3Repository _s3Repository;

		public ImagesController(MetadataRepository metadataRepository, S3Repository s3Repository)
		{
			_metadataRepository = metadataRepository;
			_s3Repository = s3Repository;
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile formFile)
		{
			await _s3Repository.UploadFileToS3(formFile.FileName, formFile.OpenReadStream());
			await _metadataRepository.CreateMetadata(formFile.FileName);

			return Ok(new {formFile.FileName});
		}

		[HttpGet("{fileName}/metadata")]
		public async Task<IActionResult> GetMetadata(string fileName)
		{
			return Ok(await _metadataRepository.GetMetadataByName(fileName));
		}

		[HttpGet("{fileName}")]
		public async Task<IActionResult> DownloadFile(string fileName)
		{
			var fileContent = await _s3Repository.GetFileFromS3(fileName);
			var contentType = "APPLICATION/octet-stream";
			return File(fileContent, contentType, fileName);
		}

		[HttpDelete("{fileName}")]
		public async Task<IActionResult> DeleteFile(string fileName)
		{
			await _s3Repository.DeleteFileFromS3(fileName);
			await _metadataRepository.DeleteMetadata(fileName);
			return Ok();
		}

		[HttpGet("allMetadata")]
		public async Task<IActionResult> GetAllMetadata()
		{
			return Ok(await _metadataRepository.GetAllMetadata());
		}
	}
}