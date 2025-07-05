using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Tesisatci.Models;

namespace Tesisatci.Services
{
    public class PhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folder = null)
        {
            var uploadResult = new ImageUploadResult();
            try
            {
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };

                    if (!string.IsNullOrEmpty(folder))
                    {
                        uploadParams.Folder = folder; // örn: "urunler"
                    }

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            catch (Exception ex)
            {
                // Loglama sistemi ekleyeceksen burada kullan
                Console.WriteLine($"Cloudinary upload error: {ex.Message}");
                throw; // opsiyonel: burada handle etmek istiyorsan remove et
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        /// <summary>
        /// Cloudinary URL'den publicId'yi parse eder (silme işlemi için)
        /// </summary>
        public string ExtractPublicId(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var versionIndex = Array.FindIndex(segments, s => s.StartsWith("v"));
            var publicIdWithExtension = string.Join("/", segments.Skip(versionIndex + 1));
            return publicIdWithExtension.Split('.').First();
        }
    }
}
