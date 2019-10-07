<%@ Application Language="C#" Inherits="EPiServer.Global" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="EPiServer.Security" %>
<%@ Import Namespace="Mediachase.Commerce.Core.Dto" %>
<%@ Import Namespace="EPiServer.Data" %>
<%@ Import Namespace="EPiServer.Logging" %>
<%@ Import Namespace="Mediachase.Commerce.Security" %>

<script RunAt="server">

    private static AuthenticationMode _authenticationMode;
    private static DatabaseMode _databaseMode;

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["ComponentArtWebUI_AppKey"] = "This edition of ComponentArt Web.UI is licensed for EPiServer Framework only.";

        string[] resolvedPaths = new string[] {
            "~/Apps/MetaDataBase/Primitives/",
            "~/Apps/MetaDataBase/MetaUI/Primitives/",
            "~/Apps/MetaUIEntity/Primitives/",
            "~/Apps/Customer/Primitives/"
        };

        Mediachase.Commerce.Manager.ControlPathResolver ctrlPathResolver = new Mediachase.Commerce.Manager.ControlPathResolver();

        ctrlPathResolver.Init(resolvedPaths);


        Mediachase.Commerce.Manager.ControlPathResolver.Current = ctrlPathResolver;

        Mediachase.Ibn.Web.UI.Layout.DynamicControlFactory.ControlsFolderPath = "~/Apps/";
        Mediachase.Ibn.Web.UI.Layout.WorkspaceTemplateFactory.ControlsFolderPath = "~/Apps/";

        var configuration = WebConfigurationManager.OpenWebConfiguration("/");
        var authenticationSection = (AuthenticationSection)configuration.GetSection("system.web/authentication");
        _authenticationMode = authenticationSection.Mode;
        _databaseMode = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IDatabaseMode>().DatabaseMode;
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError().GetBaseException();

        if (ex != null)
        {
            if (typeof(AccessDeniedException) == ex.GetType())
            {
                Response.Redirect(String.Format("~/Apps/Shell/Pages/Unauthorized.html"));
            }
            else if (typeof(HttpException) == ex.GetType())
            {
                int errorCode = ((HttpException)ex).ErrorCode;
                if (errorCode == 500) // consider 500 a fatal exception
                {
                    // Log the exception
                    LogManager.GetLogger(GetType()).Critical("Backend encountered unhandled error.", ex);
                    return;
                }
            }
        }

        // Code that runs when an unhandled error occurs
        // Log the exception
        LogManager.GetLogger(GetType()).Error("Backend encountered unhandled error.", ex);

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

        //Unlock all user locked objects
        Mediachase.Commerce.Orders.Managers.OrderGroupLockManager.UnlockAllUserLocks(EPiServer.Security.PrincipalInfo.CurrentPrincipal.GetContactId());

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        if (_databaseMode == DatabaseMode.ReadOnly)
        {
            Response.Redirect("~/Apps/Shell/Pages/Readonly.html");
        }
        // Bug fix for MS SSRS Blank.gif 500 server error missing parameter IterationId
        else if (HttpContext.Current.Request.Url.PathAndQuery.StartsWith("/Reserved.ReportViewerWebControl.axd") &&
            !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ResourceStreamID"]) &&
            HttpContext.Current.Request.QueryString["ResourceStreamID"].ToLower().Equals("blank.gif"))
        {
            Context.RewritePath(String.Concat(HttpContext.Current.Request.Url.PathAndQuery, "&IterationId=0"));
        }
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthorizeRequest(object sender, EventArgs e)
    {
        var httpApplication = sender as HttpApplication;

        if (!Request.IsAuthenticated || Request.Url.AbsoluteUri.IndexOf("logout", StringComparison.OrdinalIgnoreCase) >= 0 || Request.Url.AbsoluteUri.IndexOf("login", StringComparison.OrdinalIgnoreCase) >= 0 )
        {
            return;
        }

        // Check current 
        var fullName = User.Identity.Name;
        var appName = string.Empty;

        if (_authenticationMode == AuthenticationMode.Forms)
        {
            // If user authenticated, recreate the authentication cookie with a new value
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                appName = ticket.UserData;
            }

            if (appName.Length == 0)
            {
                SignOutAndRedirect(httpApplication, true);
            }

            AppDto dto = Mediachase.Commerce.Core.AppContext.Current.GetApplicationDto(appName);

            // If application does not exists or is not active, prevent login
            if (dto == null || dto.Application.Count == 0 || !dto.Application[0].IsActive)
            {
                SignOutAndRedirect(httpApplication, true);
            }
            else
            {
                Membership.Provider.ApplicationName = appName;
                Roles.Provider.ApplicationName = appName;
                ProfileManager.ApplicationName = appName;
                Mediachase.Commerce.Core.AppContext.Current.ApplicationName = dto.Application[0].Name;
            }
        }

        // Check permissions
        if (Mediachase.Commerce.Security.SecurityContext.Current.IsPermissionCheckEnable)
        {
            if (!PrincipalInfo.Current.IsPermitted(x => x.Core.Login))
            {
                SignOutAndRedirect(httpApplication, false);
            }

            Mediachase.Commerce.Security.SecurityContext context = Mediachase.Commerce.Security.SecurityContext.Current;

            try
            {
                if (context != null && _authenticationMode == AuthenticationMode.Forms)
                {
                    Mediachase.Commerce.Customers.Profile.CustomerProfileWrapper profile = context.CurrentUserProfile as Mediachase.Commerce.Customers.Profile.CustomerProfileWrapper;

                    if (profile != null && profile.State != 2)
                    {
                        SignOutAndRedirect(httpApplication, false);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                SignOutAndRedirect(httpApplication, true);
            }
        }
        else if(PrincipalInfo.CurrentPrincipal.IsInRole(Mediachase.Commerce.Core.AppRoles.AdminRole) ||
                PrincipalInfo.CurrentPrincipal.IsInRole(Mediachase.Commerce.Core.AppRoles.ManagerUserRole))
        {
            SignOutAndRedirect(httpApplication, false);
        }
    }

    protected void Application_PostAcquireRequestState(object sender, EventArgs e)
    {
        try
        {
            SetCulture(Mediachase.Web.Console.ManagementContext.Current.ConsoleUICulture);
        }
        catch (Exception)
        {
        }
    }

    //Overwrite methods in GlobalBase because we don't need them.
    protected override void OnRoutesRegistrating(RouteCollection routes) { }
    protected override void OnRoutesRegistered(RouteCollection routes) { }
    protected override void RegisterRoutes(RouteCollection routes) { }

    public static void SetCulture(System.Globalization.CultureInfo culture)
    {
        // Set the CurrentCulture property to the requested culture.
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;

        // Initialize the CurrentUICulture property
        // with the CurrentCulture property.
        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
    }

    /// <summary>
    /// Sign out then redirect page.
    /// True if redirect to login, false will redirect to access denied page.
    /// </summary>
    private void SignOutAndRedirect(HttpApplication httpApplication, bool redirectToLogin)
    {
        FormsAuthentication.SignOut();
        if (redirectToLogin)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
        else
        {
            Response.Redirect("~/Apps/Shell/Pages/Unauthorized.html");
        }

        httpApplication.CompleteRequest();
    }
</script>
