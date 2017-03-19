using System.Collections.Generic;
using EPiServer.Commerce.Shell.Facets;
using EPiServer.Commerce.Shell.Rest.Query;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Reference.Commerce.Site.Features.Marketing.Facet.Query;
using EPiServer.Shell.Services.Rest;
using Mediachase.Commerce.Markets;

namespace EPiServer.Reference.Commerce.Site.Features.Marketing.Facet.Store
{
    [RestStore("customfacet")]
    public class CustomFacetStore : RestControllerBase
    {
        private readonly FacetFactory _facetFactory;
        private readonly IMarketService _marketService;
        private readonly LocalizationService _localizationService;
        private readonly IContentLoader _contentLoader;
        private readonly IEnumerable<GetContentsByFacet> _filters;

        public CustomFacetStore(FacetFactory facetFactory, IMarketService marketService, LocalizationService localizationService, IContentLoader contentLoader)
        {
            _facetFactory = facetFactory;
            _marketService = marketService;
            _localizationService = localizationService;
            _contentLoader = contentLoader;

            _filters = new GetContentsByFacet[]
            {
                new GetCampaignsByCampaign(),
                new GetPromotionsByStatus(_contentLoader)
            };
        }

        public RestResult Get(string id, string facetString, ContentReference parentLink)
        {
            var customFacet = new CustomFacet(_facetFactory, _marketService, _localizationService, _contentLoader);
            var facetQueryHandler = new FacetQueryHandler();
            facetQueryHandler.CalculateMatchingNumbers(
                _contentLoader.GetChildren<IContent>(parentLink),
                customFacet.Groups,
                facetString,
                _filters);

            return Rest(customFacet);
        }
    }
}
