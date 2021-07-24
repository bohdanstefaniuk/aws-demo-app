using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace ImagesApp.Data
{
	public class S3Repository
	{
		private readonly AmazonS3Client _s3Client;
		private readonly string _bucketName;

		public S3Repository(IOptions<AwsSettings> options)
		{
			_bucketName = options.Value.BucketName;

			// _s3Client = new AmazonS3Client(
			// 	options.Value.AccessKey, 
			// 	options.Value.SecretKey, RegionEndpoint.EUCentral1);

			_s3Client = new AmazonS3Client(RegionEndpoint.EUCentral1);
		}

		public async Task UploadFileToS3(string fileName, Stream fileStream)
		{
			await _s3Client.PutObjectAsync(new PutObjectRequest
			{  
				InputStream = fileStream,  
				BucketName = _bucketName,  
				Key = fileName
			}); 
		}

		public async Task<Stream> GetFileFromS3(string fileName)
		{
			var response = await _s3Client.GetObjectAsync(_bucketName, fileName);
			var memoryStream = new MemoryStream();

			await using (var responseStream = response.ResponseStream)
			{
				await responseStream.CopyToAsync(memoryStream);
			}
			memoryStream.Flush();
			memoryStream.Position = 0;
			return memoryStream;
		}

		public async Task DeleteFileFromS3(string fileName)
		{
			await _s3Client.DeleteObjectAsync(_bucketName, fileName);
		}
	}
}