using EPiServer.Shell.Modules;
using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.Web.Mvc;

namespace Foundation.Infrastructure.Cms.Settings
{
    public class SettingsController : Controller
    {
        private readonly IBootstrapper _bootstrapper;
        private readonly IViewManager _viewManager;

        public SettingsController()
            : this(ServiceLocator.Current.GetInstance<IBootstrapper>(), ServiceLocator.Current.GetInstance<IViewManager>())
        {
        }

        public SettingsController(IBootstrapper bootstrapper, IViewManager viewManager)
        {
            _bootstrapper = bootstrapper;
            _viewManager = viewManager;
        }

        public ActionResult Index(ShellModule module, string controller)
        {
            EPiServer.Data.Validator.ValidateArgNotNull("module", module);
            EPiServer.Data.Validator.ValidateArgNotNull("controller", controller);

            var view = _viewManager.GetView(module, controller);
            var viewModel = _bootstrapper.CreateViewModel(view.Name, ControllerContext, module.Name);

            return View(_bootstrapper.BootstrapperViewName, viewModel);
        }
    }
}