using System.Collections.Generic;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;

namespace EPiServer.Reference.Commerce.Site.Features.Marketing.Facet.Query
{

    [ServiceConfiguration(typeof(IContentQuery))]
    public class CustomGetSalesCampaignChildrenQuery : GetSalesCampaignChildrenQuery
    {
        public CustomGetSalesCampaignChildrenQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            LanguageSelectorFactory languageSelectorFactory,
            CampaignInfoExtractor campaignInfoExtractor,
            FacetQueryHandler facetQueryHandler)
            : base(queryHelper, contentRepository, languageSelectorFactory, campaignInfoExtractor, facetQueryHandler) { }

        public override int Rank => 1000;

        protected override IEnumerable<GetContentsByFacet> FacetFunctions => new GetContentsByFacet[] {
            new GetCampaignsByCampaign(),
            new GetCampaignsByPromotionStatus(_contentRepository)
        };
    }
}
