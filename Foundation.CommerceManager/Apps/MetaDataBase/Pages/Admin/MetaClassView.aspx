<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaClassView" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaClassView.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="classView" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaClassView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<table cellspacing="0" cellpadding="0" width="100%" border="0" class="ibn-propertysheet">
	<tr>
		<td><mc:classView ID="ucClassView" runat="server" /></td>
	</tr>
</table>
</asp:Content>

