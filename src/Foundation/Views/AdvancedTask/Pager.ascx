<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AdvancedTask.Models.AdvancedTaskIndexViewData>" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<div class="pagerwrapper">


    <div class="pager">
        <%= Html.TranslateFormat("/gadget/tasks/taskcount", Model.MinIndexOfItem, Model.MaxIndexOfItem, Model.TotalItemsCount)%>
    </div>
    <div class="pagercenter">

        <% if (Model.PageNumber > 1)
            { %>
        <%= Html.ViewLink(
                        "<img src='/App_Themes/Default/Images/Tools/ArrowLeft.gif' alt='Previous' /></a> ",  // html helper
                        "Previous",  // title
                        "Index", // Action name
                        "arrow", // css class
                        "Previous",
                         new { pageNumber = Model.PageNumber - 1,pageSize = Model.PageSize, sorting = Model.Sorting})%>
        <% }
            foreach (int page in Model.Pages)
            {
                if (page == 0)
                { %>
        <span>
            <%= Html.Translate("/gadget/tasks/split")%>
        </span>
        <% continue;
            }

            if (page == Model.PageNumber)
            {
                if (Model.TotalPagesCount > 1)
                { %>
        <span><b>
            <%= Model.PageNumber%>
        </b></span>
        <% }
                continue;
            } %>
        <%= Html.ViewLink(
                        page.ToString(),  // html helper
                        page.ToString(),  // title
                        "Index", // Action name
                        "", // css class
                        page.ToString(),
                        new { pageNumber = page, pageSize = Model.PageSize, sorting = Model.Sorting })%>
        <% }
            if (Model.PageNumber < Model.TotalPagesCount)
            { %>
        <%= Html.ViewLink(
                        "<img src='/App_Themes/Default/Images/Tools/ArrowRight.gif' alt='Next' /></a> ",  // html helper
                        "Next",  // title
                        "Index", // Action name
                        "arrow", // css class
                        "Next",
                        new { pageNumber = Model.PageNumber + 1, pageSize = Model.PageSize, sorting = Model.Sorting})%>
        <% } %>
    </div>

    <div class="pagerright">
        <% Html.BeginGadgetForm("Index"); %>
        <input type="submit" class="refresh" value="" />
        <input type="text" name="pageSize" class="refreshinput" value="<%=Model.PageSize %>" />
        <label class="refreshlabel">
            Page size:
        </label>
        <% Html.EndForm(); %>
    </div>
</div>
