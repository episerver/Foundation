<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AdvancedTask.Models.AdvancedTaskIndexViewData>" %>
<div class="task">
    <%
        if(Model.ChangeApproval)
            Html.RenderPartial("ChangeApproval", Model);
        else
            Html.RenderPartial("Tasks", Model);

        Html.RenderPartial("Pager", Model);
    %>
</div>
