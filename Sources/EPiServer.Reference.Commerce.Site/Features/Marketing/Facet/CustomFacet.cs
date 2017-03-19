using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Facets;
using EPiServer.Framework.Localization;
using Mediachase.Commerce.Markets;

namespace EPiServer.Reference.Commerce.Site.Features.Marketing.Facet
{
    public class CustomFacet : CampaignFacet
    {
        private readonly IContentLoader _contentLoader;
        public CustomFacet(FacetFactory facetFactory, IMarketService marketService, LocalizationService localizationService, IContentLoader contentLoader) : base(facetFactory, marketService, localizationService)
        {
            _contentLoader = contentLoader;

            // clears the builtin facets
            Groups.Clear();

            Groups.Add(facetFactory.CreateFacetGroup(
                CustomFacetConstants.Campaign,
                "Campaigns",
                GetCampaignFacets(),
                new FacetGroupSettings(FacetSelectionType.Multiple, 6, true, false, false, Enumerable.Empty<string>())));

            Groups.Add(facetFactory.CreateFacetGroup(
                CustomFacetConstants.PromotionStatus,
                "Promotion status",
                GetPromotionStatusFacetItems(),
                new FacetGroupSettings(FacetSelectionType.Multiple, 0, true, true, true, new List<string>
                {
                    // Depends on "campaign" so Matching numbers gets correct
                    CustomFacetConstants.Campaign
                })));
        }

        private IEnumerable<FacetItem> GetCampaignFacets()
        {
            var campaigns = _contentLoader.GetChildren<SalesCampaign>(SalesCampaignFolder.CampaignRoot);
            return campaigns.Select(x => new FacetItem(x.ContentLink.ID.ToString(), x.Name));
        }

        private IEnumerable<FacetItem> GetPromotionStatusFacetItems()
        {
            return new List<FacetItem>()
            {
                new FacetItem("active", "Active", "epi-statusIndicatorIcon epi-statusIndicator4"),
                new FacetItem("scheduled", "Scheduled", "epi-statusIndicatorIcon epi-statusIndicator6"),
                new FacetItem("expired", "Expired", "epi-statusIndicatorIcon epi-statusIndicator100"),
                new FacetItem("inactive", "Inactive", "epi-statusIndicatorIcon epi-statusIndicator5")
            };
        }
    }
}
