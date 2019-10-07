<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.FormDocumentView" Codebehind="FormDocumentView.ascx.cs" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.BusinessFoundation.MetaForm" Assembly="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design" Assembly="Mediachase.ConsoleManager" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<div runat="server" id="OuterDiv" style="overflow:auto;">
<mc:FormRenderer ID="fRenderer" runat="server" TableLayoutMode="View">
	<SectionHeaderTemplate>
		<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" id="bhl" runat="server" Title='<%# Eval("Title") %>'></mc2:BlockHeaderLight>
	</SectionHeaderTemplate>
</mc:FormRenderer>
</div>
<script type="text/javascript" src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Scripts/main.js") %>'></script>