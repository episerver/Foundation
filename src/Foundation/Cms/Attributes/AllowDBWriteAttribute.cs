using EPiServer.Data;
using EPiServer.ServiceLocation;
using System.Web.Mvc;

namespace Foundation.Cms.Attributes
{
    public class AllowDBWriteAttribute : ActionMethodSelectorAttribute
    {
        protected Injected<IDatabaseMode> DBMode;

        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo) => DBMode.Service != null && DBMode.Service.DatabaseMode != DatabaseMode.ReadOnly;
    }
}
