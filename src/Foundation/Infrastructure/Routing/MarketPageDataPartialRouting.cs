using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Routing;
using EPiServer.Core.Routing.Pipeline;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;
using Mediachase.Commerce;
using Mediachase.Commerce.Extensions;
using Mediachase.Commerce.Markets;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;

namespace SonDo.Infrastructure.Routing;

public class MarketHierarchicalPageDataPartialRouting : IPartialRouter<PageData, PageData>, IPartialRouter
{
    private readonly IMarketService _marketService;
    private readonly ICurrentMarket _currentMarket;
    private readonly IContentLoader _contentLoader;
    private readonly IRoutingSegmentLoader _routingSegmentLoader;
    private readonly IContentVersionRepository _contentVersionRepository;
    private readonly IContentLanguageAccessor _contentLanguageAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RoutingOptions _routingOptions;
    private readonly IContentLanguageSettingsHandler _contentLanguageSettingsHandler;
    private const string VirtualPathCachePrefix = "EP:PageDataRouterVPath";

    [NonSerialized] private static readonly ILogger _log = LogManager.GetLogger(typeof(MarketHierarchicalPageDataPartialRouting));
    public const string SegmentName = "market";

    public MarketHierarchicalPageDataPartialRouting()
        : this(
            ServiceLocator.Current.GetInstance<IMarketService>(),
            ServiceLocator.Current.GetInstance<ICurrentMarket>(),
            ServiceLocator.Current.GetInstance<IContentLanguageAccessor>(),
            ServiceLocator.Current.GetInstance<IHttpContextAccessor>(),
            ServiceLocator.Current.GetInstance<IContentLoader>(),
            ServiceLocator.Current.GetInstance<IContentVersionRepository>(),
            ServiceLocator.Current.GetInstance<RoutingOptions>(),
            ServiceLocator.Current.GetInstance<IContentLanguageSettingsHandler>(),
            ServiceLocator.Current.GetInstance<IRoutingSegmentLoader>()
        )
    {
    }

    public MarketHierarchicalPageDataPartialRouting(
        IMarketService marketService,
        ICurrentMarket currentMarket,
        IContentLanguageAccessor contentLanguageAccessor,
        IHttpContextAccessor httpContextAccessor,
        IContentLoader contentLoader,
        IContentVersionRepository contentVersionRepository,
        RoutingOptions routingOptions,
        IContentLanguageSettingsHandler contentLanguageSettingsHandler,
        IRoutingSegmentLoader routingSegmentLoader)
    {
        _marketService = marketService;
        _currentMarket = currentMarket;
        _contentLanguageAccessor = contentLanguageAccessor;
        _httpContextAccessor = httpContextAccessor;
        _contentLoader = contentLoader;
        _contentVersionRepository = contentVersionRepository;
        _routingOptions = routingOptions;
        _contentLanguageSettingsHandler = contentLanguageSettingsHandler;
        _routingSegmentLoader = routingSegmentLoader;
    }

    protected virtual ContentReference RouteStartingPoint
    {
        get
        {
            return !ContentReference.IsNullOrEmpty(SiteDefinition.Current.StartPage)
                ? SiteDefinition.Current.StartPage
                : SiteDefinition.Current.RootPage;
        }
    }

    public virtual object RoutePartial(PageData content, UrlResolverContext urlGeneratorContext)
    {
        if (_log.IsDebugEnabled())
            _log.DebugBeginMethod(nameof(GetPartialVirtualPath), content, urlGeneratorContext);

        var currentMarket = _currentMarket.GetCurrentMarket();
        var marketId = currentMarket.MarketId.ToString().ToLower();

        var cultureInfo = urlGeneratorContext.RequestedLanguage ?? _contentLanguageAccessor.Language;
        if (!TryGetVirtualPath(_httpContextAccessor.HttpContext, content, cultureInfo?.Name, marketId, out var virtualPath))
            return null;

        var editOrPreviewMode = SegmentHelper.GetModifiedVirtualPathInEditOrPreviewMode
            (content.ContentLink, virtualPath, urlGeneratorContext.ContextMode);

        return new PartialRouteData()
        {
            BasePathRoot = SiteDefinition.Current.RootPage,
            PartialVirtualPath = !String.IsNullOrEmpty(editOrPreviewMode) ? $"{marketId}/{editOrPreviewMode}" : string.Empty
        };
    }

    public virtual PartialRouteData GetPartialVirtualPath(PageData content, UrlGeneratorContext urlGeneratorContext)
    {
        if (_log.IsDebugEnabled())
            _log.DebugBeginMethod(nameof(GetPartialVirtualPath), content, urlGeneratorContext);

        var currentMarket = _currentMarket.GetCurrentMarket();
        var marketId = currentMarket.MarketId.ToString().ToLower();

        var cultureInfo = urlGeneratorContext.Language ?? _contentLanguageAccessor.Language;
        if (!TryGetVirtualPath(_httpContextAccessor.HttpContext, content, cultureInfo?.Name, marketId, out var virtualPath))
            return null;

        var editOrPreviewMode = SegmentHelper.GetModifiedVirtualPathInEditOrPreviewMode
            (content.ContentLink, virtualPath, urlGeneratorContext.ContextMode);

        return new PartialRouteData()
        {
            BasePathRoot = SiteDefinition.Current.RootPage,
            PartialVirtualPath = !String.IsNullOrEmpty(editOrPreviewMode) ? $"{marketId}/{editOrPreviewMode}" : string.Empty
        };
    }

    private bool ProcessMarketSegment(UrlResolverContext context, Segment segment)
    {
        var marketSegment = segment.Next;
        var marketId = new MarketId(marketSegment.ToString());

        var market = _marketService.GetMarket(marketId);
        if (market == null) return false;

        // Set current market in case the market segment is not the same as current one.
        var currentMarket = _currentMarket.GetCurrentMarket();
        if (marketId != currentMarket.MarketId)
            _currentMarket.SetCurrentMarket(marketId);

        context.RouteValues[SegmentName] = marketSegment;
        context.RemainingSegments = segment.Remaining;

        return true;
    }

    protected virtual PageData GetPageContentRecursive(
        PageData pageContent,
        Segment segment,
        UrlResolverContext segmentContext,
        CultureInfo cultureInfo)
    {
        var contentInSegmentPair = FindNextContentInSegmentPair(pageContent, segment, segmentContext, cultureInfo);
        if (contentInSegmentPair == null)
        {
            var versionSpecificContent = GetVersionSpecificContent(segmentContext, pageContent);
            return versionSpecificContent.ContentLink.CompareToIgnoreWorkID(RouteStartingPoint) ? null : versionSpecificContent;
        }

        if (string.IsNullOrEmpty(segment.Remaining.ToString())
            && !IsValidLanguage(segment, segmentContext, cultureInfo.Name, contentInSegmentPair))
            return pageContent;
        if (IsValidRoutedContent(contentInSegmentPair))
            segmentContext.RemainingSegments = segment.Remaining;
        var remainingSegment = segmentContext.GetNextSegment(segment.Remaining);
        return GetPageContentRecursive(contentInSegmentPair, remainingSegment, segmentContext, cultureInfo);
    }

    protected virtual PageData FindNextContentInSegmentPair(
        PageData pageContent,
        Segment segment,
        UrlResolverContext segmentContext,
        CultureInfo cultureInfo)
    {
        return string.IsNullOrEmpty(segment.Next.ToString())
            ? null
            : _contentLoader.GetBySegment(pageContent.ContentLink, segment.Next.ToString(), cultureInfo) as PageData;
    }

    bool IsValidLanguage(Segment segment, UrlResolverContext context, string lang, IContent content)
    {
        var routable = content as IRoutable;
        if (_routingOptions.StrictLanguageRouting && !string.IsNullOrEmpty(lang) && routable != null)
        {
            if (!string.Equals(routable.RouteSegment, segment.Next.ToString(), StringComparison.OrdinalIgnoreCase)
                && !IsReplacementOrFallback(content, lang)
                && !RouteStartingPoint.CompareToIgnoreWorkID(content.ContentLink))
                return false;

            if (content is ILocalizable localizable
                && !IsReplacementOrFallback(content, lang)
                && !localizable.Language.Name.Equals(lang, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    protected virtual bool TryGetVirtualPath(
        HttpContext context,
        PageData content,
        string language,
        string marketId,
        out string virtualPath)
    {
        virtualPath = null;
        if (content == null)
            return false;
        if (content.ContentLink.CompareToIgnoreWorkID(RouteStartingPoint))
        {
            virtualPath = string.Empty;
            return true;
        }

        if (ContentReference.IsNullOrEmpty(content.ParentLink))
            return false;
        string virtualPathCacheKey = BuildVirtualPathCacheKey(content.ContentLink, language, marketId);
        if (context != null && context.Items.ContainsKey(virtualPathCacheKey) && context.Items[virtualPathCacheKey] is Tuple<bool, string> tuple1)
        {
            virtualPath = tuple1.Item2;
            return tuple1.Item1;
        }

        bool retVal = false;
        string segment;
        PageData nextContent;
        string nextVirtualPath;
        if (TryGetRouteSegment(content.ContentLink, language, out segment)
            && _contentLoader.TryGet(content.ParentLink, out nextContent)
            && TryGetVirtualPath(context, nextContent, language, marketId, out nextVirtualPath))
        {
            virtualPath = string.IsNullOrEmpty(nextVirtualPath) ? segment : nextVirtualPath + "/" + segment;
            retVal = true;
        }

        if (context != null)
        {
            Tuple<bool, string> tuple2 = new Tuple<bool, string>(retVal, virtualPath);
            context.Items[virtualPathCacheKey] = tuple2;
        }

        return retVal;
    }


    private bool TryGetRouteSegment(ContentReference contentLink, string language, out string segment)
    {
        segment = (string)null;
        try
        {
            var routable = _routingSegmentLoader.GetRoutingSegments(contentLink, language).FirstOrDefault();
            if (routable == null)
                return false;
            segment = routable.RouteSegment;
        }
        catch (ContentNotFoundException ex)
        {
            return false;
        }

        return true;
    }

    private bool IsReplacementOrFallback(IContent content, string language)
    {
        var languageSelectionSource = _contentLanguageSettingsHandler.MatchLanguageSettings(content, language);
        switch (languageSelectionSource)
        {
            case LanguageSelectionSource.Replacement:
            case LanguageSelectionSource.Fallback:
                return true;
            default:
                return languageSelectionSource == LanguageSelectionSource.ReplacementFallback;
        }
    }

    private PageData GetVersionSpecificContent(UrlResolverContext urlResolverContext, PageData pageContent)
    {
        var remainingSegment = urlResolverContext.GetNextSegment(urlResolverContext.RemainingSegments);
        if (SegmentHelper.TryGetIncomingNodeInEditOrPreviewMode(urlResolverContext.ContextMode, remainingSegment, out var versionSpecificContentReference))
        {
            var commonDraftContent = GetCommonDraftContent(versionSpecificContentReference, urlResolverContext.RequestedLanguage);
            if (commonDraftContent == null)
            {
                var settings = new LoaderOptions().Add(LanguageLoaderOption.Fallback(urlResolverContext.RequestedLanguage));
                commonDraftContent = _contentLoader.Get<PageData>(versionSpecificContentReference, settings);
            }

            pageContent = commonDraftContent;
            urlResolverContext.RemainingSegments = remainingSegment.Remaining;
        }

        return pageContent;
    }

    /// <summary>
    /// Returns the common draft catalog content if no work Id has been specified and if the draft content exists.
    /// </summary>
    /// <param name="contentLink">The content link to load common draft for.</param>
    /// <param name="requestedLanguage">The current requested language</param>
    /// <returns>Returns common draft catalog content if it exists, otherwise return null.</returns>
    private PageData GetCommonDraftContent(ContentReference contentLink, CultureInfo requestedLanguage)
    {
        if (contentLink.WorkID == 0)
        {
            var settings = new LoaderOptions().Add(LanguageLoaderOption.Fallback(requestedLanguage));
            var pageData = _contentLoader.Get<PageData>(contentLink, settings);
            var contentVersion = _contentVersionRepository.LoadCommonDraft(contentLink, pageData.Language.Name);
            if (contentVersion != null)
            {
                return _contentLoader.Get<PageData>(contentVersion.ContentLink, settings);
            }
        }

        return null;
    }

    private bool IsValidRoutedContent(PageData content) => content != null;

    private string BuildVirtualPathCacheKey(ContentReference contentLink, string marketId, string language)
        => $"{VirtualPathCachePrefix}{language}/{marketId}" +
           contentLink.ID.ToString(CultureInfo.InvariantCulture) + ":" +
           contentLink.WorkID.ToString(CultureInfo.InvariantCulture);
}
