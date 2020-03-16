class FoundationCms {
    init() {
        axios.defaults.headers.common['Accept'] = '*/*';
        window.notification = new NotifyHelper();
        feather.replace();

        var header = new Header();
        header.init();

        // Mobile Navgition
        var params = { searchBoxId: "#mobile-searchbox", openSearchBoxId: "#open-searh-box", closeSearchBoxId: "#close-search-box", sideBarId: "#offside-menu-mobile", openSideBarId: "#open-offside-menu" }
        var mobileNav = new MobileNavigation(params);
        mobileNav.Init();
        //--- End

        // Selection CM
        var selection = new Selection();
        selection.Init();
        // ---End

        // Dropdown
        var dropdowns = new Dropdown();
        dropdowns.Init();
        // --- End

        //Search Box
        var searchBox = new SearchBox();
        searchBox.Init();
        // --- End

        // Blog
        var blog = new Blog();
        blog.init();
        // --- End

        //Content Search
        var contentSearch = new ContentSearch();
        contentSearch.Init();
        // --- End

        // Pdf preview
        var pdfPreview = new PdfPreview();
        pdfPreview.Render();
        // --- End
    }
}