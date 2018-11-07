using SaasFeeGuides.Data;
using SaasFeeGuides.Extensions;
using SaasFeeGuides.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Helpers
{
    public static class Mapping
    {
        public static ViewModels.ActivityDate Map(this Models.ActivityDate date)
        {
            return new ViewModels.ActivityDate()
            {
                ActivityId = date.ActivityId,
                ActivityName = date.ActivityName,
                ActivitySkuDateId = date.ActivitySkuDateId,
                ActivitySkuId = date.ActivitySkuId,
                ActivitySkuName = date.ActivitySkuName,
                StartDateTime =date.StartDateTime,
                EndDateTime = date.EndDateTime,
                NumPersons = date.NumPersons
            };
        }
        public static ViewModels.ActivityDateAdmin MapAdmin(this Models.ActivityDate date)
        {
            return new ViewModels.ActivityDateAdmin()
            {
                ActivityId = date.ActivityId,
                ActivityName = date.ActivityName,
                ActivitySkuDateId = date.ActivitySkuDateId,
                ActivitySkuId = date.ActivitySkuId,
                ActivitySkuName = date.ActivitySkuName,
                StartDateTime = date.StartDateTime,
                EndDateTime = date.EndDateTime,
                AmountPaid = date.AmountPaid,
                NumPersons = date.NumPersons,
                TotalPrice = date.TotalPrice,
                CustomerBookings = date.CustomerBookings?.Select(Map).ToList()
            };
        }
        public static ViewModels.CustomerBooking Map(this Models.CustomerBooking booking)
        {
            return new ViewModels.CustomerBooking()
            {
                CustomerDisplayName = booking.CustomerDisplayName,
                Id = booking.Id,
                PriceAgreed = booking.PriceAgreed,
                ActivitySkuName = booking.ActivitySkuName,
                CustomerEmail = booking.CustomerEmail,
                DateTime = booking.DateTime,
                NumPersons = booking.NumPersons,
                HasConfirmed = booking.HasConfirmed,
                HasPaid = booking.HasPaid,
                CustomerNotes = booking.CustomerNotes
            };
        }
        public static Models.CustomerBooking Map(this ViewModels.CustomerBooking booking)
        {
            return new Models.CustomerBooking()
            {
                CustomerNotes = booking.CustomerNotes,
                Id= booking.Id,
                PriceAgreed = booking.PriceAgreed ?? -1,                
                ActivitySkuName = booking.ActivitySkuName,
                CustomerEmail = booking.CustomerEmail,
                DateTime = booking.DateTime,
                NumPersons = booking.NumPersons,
                HasConfirmed = booking.HasConfirmed,
                HasCancelled = booking.HasCancelled,
                HasPaid = booking.HasPaid
            };
        }

        public static Models.CustomerBooking Map(this ViewModels.HistoricCustomerBooking booking)
        {
            return new Models.CustomerBooking()
            {
                ActivitySkuName = booking.ActivitySkuName,
                PriceAgreed = booking.AmountPaid,
                CustomerEmail = booking.CustomerEmail,
                DateTime = booking.DateTime,
                NumPersons = booking.NumPersons,
                HasConfirmed = true,
                HasPaid = true
            };
        }
        public static ViewModels.Guide Map(this Models.Guide guide)
        {
            return new ViewModels.Guide()
            {
                Id = guide.Id,
                Address = guide.Address,
                FirstName = guide.FirstName,
                Email = guide.Email,
                DateOfBirth = guide.DateOfBirth,
                LastName = guide.LastName,
                PhoneNumber = guide.PhoneNumber,
                IsActive = guide.IsActive
            };
        }

        public static Models.Guide Map(this ViewModels.Guide guide)
        {
            return new Models.Guide()
            {
                Address = guide.Address,
                FirstName = guide.FirstName,
                Email = guide.Email,
                DateOfBirth = guide.DateOfBirth,
                LastName = guide.LastName,
                PhoneNumber = guide.PhoneNumber,
                IsActive = guide.IsActive
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
        private static async Task<ViewModels.ActivitySku> Map(this Models.ActivitySku sku,IContentRepository contentRepository)
        {
            var titleContent = contentRepository.SelectContent(sku.TitleContentId);
            var descContent = contentRepository.SelectContent(sku.DescriptionContentId);
            var additionalRequirementsContent = contentRepository.SelectContent(sku.AdditionalRequirementsContentId);
            var webContent = contentRepository.SelectContent(sku.WebContentId);
            return new ViewModels.ActivitySku()
            {
                DescriptionContent = await descContent,
                TitleContent = await titleContent,
                Name = sku.Name,
                ActivityName = sku.ActivityName,
                AdditionalRequirementsContent =await additionalRequirementsContent,
                DurationDays = sku.DurationDays,
                DurationHours = sku.DurationHours,               
                MaxPersons = sku.MaxPersons,
                MinPersons = sku.MinPersons,
                PricePerPerson = sku.PricePerPerson,
                WebContent = await webContent,
                ActivityId = sku.ActivityId,
                Id = sku.Id.Value,
                PriceOptions = await Task.WhenAll(sku.PriceOptions?.Select(price => price.Map(contentRepository)))
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

        public static async Task<ViewModels.Activity> Map(this Models.Activity activity, IContentRepository contentRepository)
        {
            var titleContent = contentRepository.SelectContent(activity.TitleContentId);
            var menuImage = contentRepository.SelectContent(activity.MenuImageContentId);
            var imageContent = contentRepository.SelectContent(activity.ImageContentId);
            var videoContent = contentRepository.SelectContent(activity.VideoContentId);
            var descContent = contentRepository.SelectContent(activity.DescriptionContentId);

            return new ViewModels.Activity
            {
                Id = activity.Id,
                TitleContent = await titleContent,
                MenuImageContent = await menuImage,
                ImageContent = await imageContent,
                VideoContent = await videoContent,
                Name = activity.Name,
                DescriptionContent = await descContent,
                IsActive = activity.IsActive,
                CategoryName = activity.CategoryName,
                Skus = activity.Skus != null ? await Task.WhenAll( activity.Skus.Select(s => s.Map(contentRepository))) : null,
                Equiptment = activity.Equiptment != null ? activity.Equiptment.Select(e=> new ViewModels.ActivityEquiptment()
                { EquiptmentId = e.EquiptmentId}).ToList() : null,
                
            };
        }
        public static async Task<Models.Activity> Map(this ViewModels.Activity activity, int? activityId, IContentRepository contentRepository )
        {
            var title = contentRepository.InsertContent(activity.TitleContent);
            var menuImage = contentRepository.InsertContent(activity.MenuImageContent);
            var imageContent = contentRepository.InsertContent(activity.ImageContent);
            var videoContent = contentRepository.InsertContent(activity.VideoContent);
            var descContent = contentRepository.InsertContent(activity.DescriptionContent);
            return new Models.Activity
            {
                Id = activityId,
                TitleContentId = await title,
                MenuImageContentId =await menuImage ,
                ImageContentId = await imageContent,
                VideoContentId = await videoContent,
                Name = activity.Name,
                DescriptionContentId = await descContent,
                IsActive = activity.IsActive ?? false,
                CategoryName = activity.CategoryName
            };
        }
        public static async Task<ViewModels.ActivitySkuPrice> Map(this ActivitySkuPrice activitySkuPrice, IContentRepository contentRepository)
        {
            return new ViewModels.ActivitySkuPrice()
            {
                ActivitySkuId = activitySkuPrice.ActivitySkuId,
                DiscountCode = activitySkuPrice.DiscountCode,
                DiscountPercentage = activitySkuPrice.DiscountPercentage,
                MaxPersons = activitySkuPrice.MaxPersons,
                MinPersons = activitySkuPrice.MinPersons,
                Name = activitySkuPrice.Name,
                Price = activitySkuPrice.Price ?? 0,
                ValidFrom = activitySkuPrice.ValidFrom,
                ValidTo = activitySkuPrice.ValidTo,
                DescriptionContent = await contentRepository.SelectContent(activitySkuPrice.DescriptionContentId),
            };
        }
        public static async Task<ActivitySkuPrice> Map(this ViewModels.ActivitySkuPrice activitySkuPrice, IContentRepository contentRepository)
        {
            return new ActivitySkuPrice()
            {
                ActivitySkuId = activitySkuPrice.ActivitySkuId,
                DiscountCode = activitySkuPrice.DiscountCode,
                DiscountPercentage = activitySkuPrice.DiscountPercentage,
                MaxPersons = activitySkuPrice.MaxPersons,
                MinPersons = activitySkuPrice.MinPersons,
                Name = activitySkuPrice.Name,
                Price = activitySkuPrice.Price,
                ValidFrom = activitySkuPrice.ValidFrom,
                ValidTo = activitySkuPrice.ValidTo,
                DescriptionContentId = await contentRepository.InsertContent(activitySkuPrice.DescriptionContent),
            };
        }
        public static async Task<Models.ActivitySku> Map(this ViewModels.ActivitySku activitySku, IContentRepository contentRepository)
        {
            var title = contentRepository.InsertContent(activitySku.TitleContent);
            var add = contentRepository.InsertContent(activitySku.AdditionalRequirementsContent);
            var webContent = contentRepository.InsertContent(activitySku.WebContent);
            var descContent = contentRepository.InsertContent(activitySku.DescriptionContent);
            return new Models.ActivitySku
            {
                ActivityName = activitySku.ActivityName,
                Name = activitySku.Name,
                TitleContentId = await title,
                DescriptionContentId = await descContent,
                PricePerPerson = activitySku.PricePerPerson,
                MinPersons = activitySku.MinPersons,
                MaxPersons = activitySku.MaxPersons,
                AdditionalRequirementsContentId =await add,
                DurationDays = activitySku.DurationDays,
                DurationHours = activitySku.DurationHours,
                WebContentId = await webContent,
            };
        }
    }
}
