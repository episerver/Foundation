<%@ Page Language="C#" AutoEventWireup="true" 
	Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Admin.FormItemEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Codebehind="FormItemEdit.aspx.cs" %>
<%@ Reference VirtualPath="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormItemEdit.ascx" %>
<%@ Register TagPrefix="mc" TagName="fItemEdit" Src="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormItemEdit.ascx" %>
<asp:Content ID="cPlace" ContentPlaceHolderID="phMain" runat="server">
	<mc:fItemEdit ID="ucItemEdit" runat="server" />
</asp:Content>