<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerNumOrdersReport.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Reporting.CustomerNumOrdersReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker"
    TagPrefix="ecf" %>
<div class="report-view">
    <div class="report-filter">
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Start_Date %>"/>:
        <ecf:CalendarDatePicker runat="server" ID="StartDate" />
        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, End_Date %>"/>:
        <ecf:CalendarDatePicker runat="server" ID="EndDate" />
        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, Group_By %>"/>:
        <asp:DropDownList runat="server" ID="GroupBy">
            <asp:ListItem Value="day" Text="<%$ Resources:SharedStrings, Day %>"></asp:ListItem>
            <asp:ListItem Value="month" Text="<%$ Resources:SharedStrings, Month %>"></asp:ListItem>
            <asp:ListItem Value="year" Text="<%$ Resources:SharedStrings, Year %>"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Width="100" Text="<%$ Resources:ReportingStrings, Apply_Filter %>" 
            onclick="btnSearch_Click" />
    </div>
    <div class="report-content">
        <rsweb:ReportViewer OnBookmarkNavigation="MyReportViewer_BookmarkNavigation" ZoomMode="Percent"
            SizeToReportContent="true" AsyncRendering="false" ID="MyReportViewer" runat="server" ShowDocumentMapButton="False"
            ShowExportControls="True" ShowFindControls="False" ShowPageNavigationControls="True"
            ShowPrintButton="True" ShowPromptAreaButton="False" ShowRefreshButton="True"
            ShowZoomControl="True" Font-Names="Verdana" Font-Size="8pt" Width="100%" Height="90%"
            HyperlinkTarget="_blank">
            <LocalReport EnableHyperlinks="True">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
</div>
