using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;

namespace EPiServer.Reference.Commerce.Site.Features.Marketing.Facet.Query
{
    public class GetCampaignsByCampaign : GetContentsByFacet
    {
        public override IEnumerable<IContent> GetItems(IEnumerable<IContent> items, IEnumerable<string> facets)
        {
            return items.Where(x => AvailableFor(x, facets));
        }

        public bool AvailableFor(IContent content, IEnumerable<string> facets)
        {
            if (content is SalesCampaign)
            {
                return facets.Any(facet => content.ContentLink.ID.ToString()
                    .Equals(facet, StringComparison.InvariantCultureIgnoreCase));
            }

            if (content is PromotionData)
            {
                return facets.Any(facet => content.ParentLink.ID.ToString()
                        .Equals(facet, StringComparison.InvariantCultureIgnoreCase));
            }

            return false;
        }

        public override string Key => CustomFacetConstants.Campaign;
    }
}
