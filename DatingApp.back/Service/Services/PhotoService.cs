using Service.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Service.Helpers;
using Microsoft.Extensions.Options;

namespace Service.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            // Recupération des informations depuis le fichier env.developpement
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            this.cloudinary = new Cloudinary(acc);
        }

        /// <summary>
        /// Ajout et Crop de la photo vers le visage de la personne
        /// </summary>
        /// <param name="file">Fichier Upload</param>
        /// <returns> Return de l'image après le crop </returns>
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        /// <summary>
        /// Suppression de l'image dans Cloudinary
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
