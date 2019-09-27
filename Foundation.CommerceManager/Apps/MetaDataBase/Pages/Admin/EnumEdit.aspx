<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.EnumEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="EnumEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="enumEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/EnumEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:enumEdit ID="ucEnumEdit" runat="server" />
</asp:Content>