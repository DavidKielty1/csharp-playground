using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAScync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}