using SaasFeeGuides.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Helpers
{
    public static class Mapping
    {
        public static Models.CustomerBooking Map(this ViewModels.HistoricCustomerBooking booking)
        {
            return new Models.CustomerBooking()
            {
                ActivitySkuName = booking.ActivitySkuName,
                AmountPaid = booking.AmountPaid,
                CustomerEmail = booking.CustomerEmail,
                Date = booking.Date,
                NumPersons = booking.NumPersons
            };
        }

        public static ViewModels.Customer Map(this Models.Customer customer)
        {
            return new ViewModels.Customer()
            {
                Id = customer.Id,
                Address = customer.Address,
                FirstName = customer.FirstName,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
   
        public static Models.Customer Map(this ViewModels.Customer customer)
        {
            return new Models.Customer()
            {
                Address = customer.Address,
                FirstName = customer.FirstName,
                Email = customer.Email,
                DateOfBirth= customer.DateOfBirth,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
        public static ViewModels.ActivityLoc Map(this Models.ActivityLoc activity)
        {
            return new ViewModels.ActivityLoc()
            {
                Id = activity.Id,
                Description = activity.Description,
                Images = activity.Images,
                MenuImage = activity.MenuImage,
                Name = activity.Name,
                Skus = activity.Skus.Select(Map).ToList(),
                Equiptment = activity.Equiptment.Select(Map).ToList(),
                Title = activity.Title,
                Videos = activity.Videos
            };
        }
        private static ViewModels.EquiptmentLoc Map(this Models.EquiptmentLoc e)
        {
            return new ViewModels.EquiptmentLoc()
            {
                RentalPrice = e.RentalPrice,
                Title = e.Title,
                Name = e.Name,
                CanRent = e.CanRent,
                Id = e.Id
            };
        }

        private static ViewModels.ActivitySkuLoc Map(this Models.ActivitySkuLoc s)
        {
            return new ViewModels.ActivitySkuLoc()
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
            };
        }
        public static Models.Equiptment Map(this ViewModels.ActivityEquiptment equiptment, IContentRepository contentRepository)
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
                IsActive = activity.IsActive,
                CategoryName = activity.CategoryName
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
