<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaLinkEdit"  MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaLinkEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="linkEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaLinkEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:linkEdit id="ucLinkEdit" runat="server" />
</asp:Content>