<%@ Page Language="C#" AutoEventWireup="true" 
	Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Admin.FormDocumentEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Codebehind="FormDocumentEdit.aspx.cs" %>
<%@ Reference VirtualPath="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormDocumentEdit.ascx" %>
<%@ Register TagPrefix="mc" TagName="fDocEdit" Src="~/Apps/MetaDataBase/MetaUI/Modules/MetaFormControls/FormDocumentEdit.ascx" %>
<asp:Content ID="cPlace" ContentPlaceHolderID="phMain" runat="server">
	<mc:fDocEdit ID="ucDocEdit" runat="server" />
</asp:Content>
