<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>
<%@ Import Namespace="EPiServer.Cms.Shell" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>

<div class="epi-tabView">
    <ul>
        <li class="<%= Model == "Index" ? "selected" : "" %> ntab">
            <%= Html.ViewLink(
                    "Content Approval ", // html helper
                    "", // title
                    "Index", // Action name
                    "", // css class
                    "",
                    new {}) %>
        </li>
        
        <li class="<%= Model == "ChangeApproval" ? "selected" : "" %> ntab">
            <%= Html.ViewLink(
                    "Change Approval ", // html helper
                    "", // title
                    "Index", // Action name
                    "", // css class
                    "",
                    new {isChange = true}) %>
        </li>
    </ul>
</div>