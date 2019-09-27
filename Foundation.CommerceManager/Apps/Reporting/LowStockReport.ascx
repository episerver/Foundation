<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LowStockReport.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Reporting.LowStockReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<div class="report-view">
    <div class="report-content">
        <rsweb:ReportViewer OnBookmarkNavigation="MyReportViewer_BookmarkNavigation" ZoomMode="Percent" 
            SizeToReportContent="true" AsyncRendering="false" ID="MyReportViewer" runat="server" ShowDocumentMapButton="False"
            ShowExportControls="True" ShowFindControls="False" ShowPageNavigationControls="True"
            ShowPrintButton="True" ShowPromptAreaButton="False" ShowRefreshButton="True"
            ShowZoomControl="True" Font-Names="Verdana" Font-Size="8pt" Width="100%" Height="90%" HyperlinkTarget="_blank">
            <LocalReport EnableHyperlinks="True">
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
</div>
