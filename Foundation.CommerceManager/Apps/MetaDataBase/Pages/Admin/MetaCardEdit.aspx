<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaCardEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaCardEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="cardEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaCardEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:cardEdit ID="ucCardEdit" runat="server" />
</asp:Content>