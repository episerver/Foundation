<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AdvancedTask.Models.AdvancedTaskIndexViewData>" %>
<div class="task">
    <%
        Html.RenderPartial("Tasks", Model);
        Html.RenderPartial("Pager", Model);
    %>
</div>
