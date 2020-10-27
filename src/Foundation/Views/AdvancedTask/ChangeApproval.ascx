<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AdvancedTask.Models.AdvancedTaskIndexViewData>" %>
<%@ Import Namespace="AdvancedTask.Business.AdvancedTask" %>
<%@ Import Namespace="AdvancedTask.Models" %>
<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="EPiServer.Editor" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>

<% Html.RenderPartial("Menu", "ChangeApproval"); %>
<style>
    .inner-table {
        border-collapse: collapse !important;
        /*border: none !important;*/
        empty-cells: show !important;
        width: 100% !important;
        height: 100% !important;
        border-bottom: 1pt solid #aeaeae;
        /*border-spacing: 0 !important;*/
    }

        .inner-table .short {
            width: 20% !important;
        }

        .inner-table td {
            padding: 6px !important;
        }

        .inner-table .header {
            border-bottom: 1pt solid #aeaeae !important;
        }

        .inner-table .header-right {
            border-right: 1pt solid #aeaeae !important;
        }

        .inner-table .epi-changeapproval-faded {
            color: #818181 !important;
        }

    .changetask-detail {
        background-color: #d9edf7 !important;
    }

    .change-anchor {
        text-decoration: underline !important;
        color: navy !important;
    }
</style>

<table class="epi-default">
    <thead>
        <tr>
            <th>
                <label>
                    <%= Html.ViewLink(
                                "Content Name",
                                "Content Name",  // title
                                "Index", // Action name
                                "", // css class
                                "",
                                new { pageNumber = Model.PageNumber, pageSize = Model.PageSize, sorting = Model.Sorting=="name_desc"?"name_aes":"name_desc",isChange = true})%>
                </label>
            </th>
            <th>
                <label>
                    <%= Html.ViewLink(
                                "Content Type",
                                "Content Type",  // title
                                "Index", // Action name
                                "", // css class
                                "",
                                new { pageNumber = Model.PageNumber, pageSize = Model.PageSize, sorting = Model.Sorting=="ctype_desc"?"ctype_aes":"ctype_desc",isChange = true})%>
                </label>
            </th>
            <th>
                <label>
                    <%= Html.ViewLink(
                                "Type",
                                "Type",  // title
                                "Index", // Action name
                                "", // css class
                                "",
                                new { pageNumber = Model.PageNumber, pageSize = Model.PageSize, sorting = Model.Sorting=="type_aes"?"type_aes":"type_desc",isChange = true})%>
                </label>
            </th>
            <th>
                <label>
                    <%= Html.ViewLink(
                                "Submitted Date/Time",
                                "Submitted Date/Time",  // title
                                "Index", // Action name
                                "", // css class
                                "",
                                new { pageNumber = Model.PageNumber, pageSize = Model.PageSize, sorting = Model.Sorting=="timestamp_desc"?"timestamp_aes":"timestamp_desc",isChange = true})%>
                </label>
            </th>
            <th>
                <label>
                    <%= Html.ViewLink(
                                "Started By",
                                "Started By",  // title
                                "Index", // Action name
                                "", // css class
                                "",
                                new { pageNumber = Model.PageNumber, pageSize = Model.PageSize, sorting = Model.Sorting=="user_desc"?"user_aes":"user_desc",isChange = true})%>
                </label>
            </th>
        </tr>
    </thead>

    <% if (Model.ContentTaskList.Count > 0)
        {
            foreach (ContentTask m in Model.ContentTaskList)
            {
    %>
    <tr <%= m.NotificationUnread ? "style=\"background-color: #FFF9C4;cursor: pointer;\"" : "cursor: pointer" %> class="parent" title="Click to expand/collapse" id="<%= m.ApprovalId %>">
        <td>
            <%= m.ContentName %>
        </td>
        <td>
            <%= m.ContentType %>
        </td>
        <td>
            <%= m.Type %>
        </td>
        <td>
            <%= m.DateTime %>
        </td>
        <td>
            <%= m.StartedBy %>
        </td>
    </tr>
    <% if (m.Details != null && m.Details.Any())
        { %>
    <tr class="child-<%= m.ApprovalId %>" style="display: none;">
        <td class="changetask-detail" colspan="6">
            <table class="inner-table">
                <thead>
                    <tr class="header">
                        <td class="short header-right"><strong>Name</strong></td>
                        <td class="header-right"><strong>Current Version</strong></td>
                        <td><strong>Suggested Version</strong></td>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var ct in m.Details.ToList())
                        {
                    %>
                    <tr>
                        <td class="short"><%= ct.Name %></td>
                        <td><%= ct.OldValue %></td>
                        <td><%= ct.NewValue %></td>
                    </tr>
                    <% }
                        } %>
                </tbody>
            </table>
            <p style="padding-top: 20px"><a class="change-anchor" href="<%=m.URL%>" target="_blank">Go to the Change Details</a></p>
        </td>
    </tr>
    <% }
        } %>
</table>
<br />
<script type="text/javascript">
    $(document).ready(function () {
        $('tr.parent')
            .css("cursor", "pointer")
            .attr("title", "Click to expand/collapse")
            .click(function () {
                $(this).siblings('.child-' + this.id).toggle();
            });
        //$('tr[@class^=child-]').hide().children('td');
    });    
</script>

