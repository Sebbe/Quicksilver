using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;

namespace EPiServer.Reference.Commerce.Site.Features.Marketing.Facet.Query
{
    public class GetCampaignsByPromotionStatus : GetContentsByFacet
    {
        private readonly IContentLoader _contentLoader;

        public GetCampaignsByPromotionStatus(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            return items.Where(x => CheckStatus(x,  facets));
        }

        public bool CheckStatus(IContent content, IEnumerable<string> facets)
        {
            if (content is SalesCampaign)
            {
                return CampaignHavePromotions((SalesCampaign) content, facets);
            }

            if (content is PromotionData)
            {
                return AvailableFor((PromotionData) content, facets);
            }

            return false;
        }

        public bool CampaignHavePromotions(SalesCampaign campaign, IEnumerable<string> facets)
        {
            return _contentLoader.GetChildren<PromotionData>(campaign.ContentLink)
                .Any(x => AvailableFor(x, facets));
        }

        public bool AvailableFor(PromotionData promotion, IEnumerable<string> facets)
        {
            var isAvailable = false;
            foreach (var facet in facets)
            {
                switch (facet)
                {
                    case "active":
                        isAvailable = promotion.IsActive && !IsExpired(promotion) && !IsScheduled(promotion);
                        break;
                    case "inactive":
                        isAvailable = !promotion.IsActive && !IsExpired(promotion) && !IsScheduled(promotion);
                        break;
                    case "expired":
                        isAvailable = IsExpired(promotion);
                        break;
                    case "scheduled":
                        isAvailable = IsScheduled(promotion);
                        break;
                }

                if (isAvailable)
                    return true;
            }

            return false;
        }

        private bool IsScheduled(PromotionData promotion)
        {
            return promotion.Schedule.ValidFrom != DateTime.MinValue && promotion.Schedule.ValidFrom > DateTime.Now;
        }

        private bool IsExpired(PromotionData promotion)
        {
            return promotion.Schedule.ValidUntil != DateTime.MinValue && promotion.Schedule.ValidUntil < DateTime.Now;
        }

        public override string Key => CustomFacetConstants.PromotionStatus;
    }
}
