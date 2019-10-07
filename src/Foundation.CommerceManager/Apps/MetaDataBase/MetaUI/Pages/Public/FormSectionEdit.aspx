<%@ Page Language="C#" AutoEventWireup="true" 
	Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Admin.FormSectionEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Codebehind="FormSectionEdit.aspx.cs" %>
<%@ Reference VirtualPath="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormSectionEdit.ascx" %>
<%@ Register TagPrefix="mc" TagName="fSecEdit" Src="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormSectionEdit.ascx" %>
<asp:Content ID="cPlace" ContentPlaceHolderID="phMain" runat="server">
	<mc:fSecEdit ID="ucSecEdit" runat="server" />
</asp:Content>