namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Data.Repositories;
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using System.Text.RegularExpressions;

    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;
        private readonly ILogger<ImageService> logger;
        private readonly IRecipeRepository recipeRepository;

        public ImageService(Cloudinary cloudinary, 
            ILogger<ImageService> logger,
            IRecipeRepository recipeRepository)
        {
            this.cloudinary = cloudinary;
            this.recipeRepository = recipeRepository;
            this.logger = logger;
        }
        public async Task<string> UploadImageAsync(string externalUrl)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(externalUrl),
                Transformation = new Transformation()
                    .Width(800)
                    .Height(600)
                    .Crop("fill")
                    .FetchFormat("webp")
                    .Quality(80)
            };
           
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult?.SecureUrl != null)
            {
                return uploadResult.SecureUrl.ToString();
            }
          
            return "";
        }

        public async Task CleanupUnusedImagesAsync()
        {
            try
            {
                // Step 1: Fetch all Cloudinary image metadata
                var allCloudinaryImages = await GetAllCloudinaryImagesAsync();

                // Step 2: Get active image URLs from the database
                var activeImageUrls = await recipeRepository
                    .GetAllQuery()
                    .AsNoTracking()
                    .Where(r => r.InternalImageUrl != null && r.InternalImageUrl != string.Empty)
                    .Select(r => r.InternalImageUrl)
                    .ToListAsync();

                var activeImageIds = activeImageUrls
                    .Select(url => ExtractPublicIdFromUrl(url))
                    .ToHashSet();

                // Step 3: Identify unused images
                var unusedImageIds = allCloudinaryImages
                    .Where(publicId => !activeImageIds.Contains(publicId))
                    .ToList();

                // Step 4: Delete unused images
                foreach (var publicId in unusedImageIds)
                {
                    try
                    {
                        var deleteParams = new DeletionParams(publicId);
                        var deleteResult = await cloudinary.DestroyAsync(deleteParams);

                        if (deleteResult.Result != "ok")
                        {
                            logger.LogError($"Failed to delete Cloudinary image with public ID: {publicId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred while deleting the image with public ID: {publicId}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the cleanup of unused images.");
            }
        }

        private string ExtractPublicIdFromUrl(string url)
        {
            string pattern = @"(?:https://(?:[\w.-]+\/){2}image/upload/(?:v\d+/)?)([^\/\.]+)(?:\.\w+)";
            Match? match = Regex.Match(url, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private async Task<List<string>> GetAllCloudinaryImagesAsync()
        {
            var allCloudinaryImages = new List<string>();
            var listParams = new ListResourcesParams
            {
                Type = "upload",
                MaxResults = 500
            };

            do
            {
                var resources = await cloudinary.ListResourcesAsync(listParams);
                allCloudinaryImages.AddRange(resources.Resources.Select(r => r.PublicId));
                listParams.NextCursor = resources.NextCursor; // Update cursor

            } while (!string.IsNullOrEmpty(listParams.NextCursor));

            return allCloudinaryImages;
        }

       
        // One time usable for a scheduled job
        public async Task GenerateMissingRecipeImagesAsync()
        {
            // Get all recipes tracked by the Change Tracker
            var recipes = await recipeRepository.GetAllQuery()
                .Where(r => r.InternalImageUrl == null || r.InternalImageUrl == string.Empty)
                .ToListAsync();

            foreach (var recipe in recipes)
            {
                try
                {
                    recipe.InternalImageUrl = await UploadImageAsync(recipe.ExternalImageUrl);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Failed to upload image for recipe: {recipe.Title}, using external URL: {recipe.ExternalImageUrl}");
                }
            }

            await recipeRepository.SaveChangesAsync();

            
            logger.LogInformation("Finished processing recipes missing internal image URLs.");
        }
    }
}
