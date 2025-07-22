using System.Net;
using Carden.Api.Dtos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Carden.Api.Services;

public interface ICloudinaryService
{
    public Task<CloudinaryUploadResponse> UploadImageAsync(CloudinaryUploadRequest request);
    public Task<CloudinaryUploadResponse> DeleteImageAsync(string publicId);
}

public class CloudinaryService: ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly long _maxFileSize = 10 * 1024 * 1024;
    private readonly string[] _allowedFormats = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];

    public CloudinaryService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<CloudinaryUploadResponse> UploadImageAsync(CloudinaryUploadRequest request)
    {
        try
        {
            if (request.FormFile.Length == 0)
            {
                return new CloudinaryUploadResponse
                {
                    Success = false,
                    ErrorMessage = "No file provided"
                };
            }

            if (request.FormFile.Length > _maxFileSize)
            {
                return new CloudinaryUploadResponse
                {
                    Success = false,
                    ErrorMessage = $"File too large. Max size: {_maxFileSize / (1024 * 1024)}MB"
                };
            }

            var fileExtension = Path.GetExtension(request.FormFile.FileName).ToLowerInvariant();
            if (!_allowedFormats.Contains(fileExtension))
            {
                return new CloudinaryUploadResponse
                {
                    Success = false,
                    ErrorMessage = $"Invalid format. Allowed: {string.Join(", ", _allowedFormats)}"
                };
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.FormFile.FileName, request.FormFile.OpenReadStream()),
                Format = "webp"
            };
            if (!string.IsNullOrEmpty(request.Folder)) uploadParams.Folder = request.Folder;
            if (!string.IsNullOrEmpty(request.Tags)) uploadParams.Tags = request.Tags;

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return new CloudinaryUploadResponse
                {
                    Success = false,
                    ErrorMessage = result.Error?.Message ?? "Upload failed"
                };
            }

            return new CloudinaryUploadResponse
            {
                Success = true,
                ImageUrl = result.SecureUrl.ToString(),
                PublicId = result.PublicId,
                Format = result.Format,
                Width = result.Width,
                Height = result.Height,
                Bytes = result.Bytes,
                CreatedAt = result.CreatedAt
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async  Task<CloudinaryUploadResponse> DeleteImageAsync(string publicId)
    {
        try
        {
            await Task.Delay(100);
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}