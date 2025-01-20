namespace CookTheWeek.Services.Data.Services.Interfaces
{
    
    public interface IImageService
    {
        public Task<string> UploadImageAsync(string externalUrl);

        public Task CleanupUnusedImagesAsync();

        public Task GenerateMissingRecipeImagesAsync();
    }
}
