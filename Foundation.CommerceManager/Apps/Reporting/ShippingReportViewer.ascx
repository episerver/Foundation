<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingReportViewer.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Reporting.ShippingReportViewer" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
	Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<div  style="width:100%;height:650px">
	<rsweb:ReportViewer ZoomMode="Percent" SizeToReportContent="True" ID="MyReportViewer"
		runat="server" ShowDocumentMapButton="False" ShowExportControls="True" ShowFindControls="False"
		ShowPageNavigationControls="True" ShowPrintButton="True" ShowPromptAreaButton="False"
		ShowRefreshButton="false" Font-Names="Verdana" Font-Size="8pt" HyperlinkTarget="_blank" Width="100%" Height="100%">
	</rsweb:ReportViewer>
</div>
