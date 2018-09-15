using SaasFeeGuides.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Helpers
{
    public static class Mapping
    {
        public static ViewModels.ActivityLoc Map(this Models.ActivityLoc activity)
        {
            return new ViewModels.ActivityLoc()
            {
                Id = activity.Id,
                Description = activity.Description,
                Images = activity.Images,
                MenuImage = activity.MenuImage,
                Name = activity.Name,
                Skus = activity.Skus.Select(s => new ViewModels.ActivitySkuLoc()
                {
                    Description = s.Description,
                    Title = s.Title,
                    Name = s.Name,
                    ActivityName = s.ActivityName,
                    AdditionalRequirements = s.AdditionalRequirements,
                    DurationDays = s.DurationDays,
                    DurationHours = s.DurationHours,
                    Id = s.Id,
                    MaxPersons = s.MaxPersons,
                    MinPersons = s.MinPersons,
                    PricePerPerson = s.PricePerPerson,
                    WebContent = s.WebContent
                }).ToList(),
                Title = activity.Title,
                Videos = activity.Videos
            };
        }
        public static Models.Equiptment Map(this ViewModels.Equiptment equiptment, IContentRepository contentRepository)
        {
            return new Models.Equiptment
            {
                Name = equiptment.Name,
                TitleContentId = contentRepository.InsertContent(equiptment.TitleContent),
                RentalPrice = equiptment.RentalPrice,
                ReplacementPrice = equiptment.ReplacementPrice,
                CanRent = equiptment.CanRent
            };
        }
        public static Models.Activity Map(this ViewModels.Activity activity, int? activityId, IContentRepository contentRepository )
        {
            return new Models.Activity
            {
                Id = activityId,
                TitleContentId = contentRepository.InsertContent(activity.TitleContent),
                MenuImageContentId = contentRepository.InsertContent(activity.MenuImageContentId),
                ImageContentId = contentRepository.InsertContent(activity.ImageContentId),
                VideoContentId = contentRepository.InsertContent(activity.VideoContentId),
                Name = activity.Name,
                DescriptionContentId = contentRepository.InsertContent(activity.DescriptionContent),
                IsActive = activity.IsActive
            };
        }

        public static Models.ActivitySku Map(this ViewModels.ActivitySku activitySku, IContentRepository contentRepository)
        {
            return new Models.ActivitySku
            {
                ActivityName = activitySku.ActivityName,
                Name = activitySku.Name,
                TitleContentId = contentRepository.InsertContent(activitySku.TitleContent),
                DescriptionContentId = contentRepository.InsertContent(activitySku.DescriptionContent),
                PricePerPerson = activitySku.PricePerPerson,
                MinPersons = activitySku.MinPersons,
                MaxPersons = activitySku.MaxPersons,
                AdditionalRequirementsContentId = contentRepository.InsertContent(activitySku.AdditionalRequirementsContent),
                DurationDays = activitySku.DurationDays,
                DurationHours = activitySku.DurationHours,
                WebContentId = contentRepository.InsertContent(activitySku.WebContent),
            };
        }
    }
}
