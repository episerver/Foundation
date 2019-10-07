<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewAccountsReport.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Reporting.NewAccountsReport" %>
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
        <rsweb:ReportViewer OnBookmarkNavigation="MyReportViewer_BookmarkNavigation"
            SizeToReportContent="True" AsyncRendering="false" ID="MyReportViewer" runat="server" 
            ShowDocumentMapButton="False" ShowFindControls="False" 
            ShowPromptAreaButton="False" Font-Names="Verdana" Font-Size="8pt" 
            Width="100%" Height="90%"
            HyperlinkTarget="_blank" style="margin-top: 0px">
            <LocalReport EnableHyperlinks="True">
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" TypeName="ConsoleManager.CustomersDataSetTableAdapters.">
        </asp:ObjectDataSource>
    </div>
</div>
