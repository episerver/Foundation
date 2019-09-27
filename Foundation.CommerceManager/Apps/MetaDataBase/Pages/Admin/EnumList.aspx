<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.EnumList" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="EnumList.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="enumList" Src="~/Apps/MetaDataBase/Modules/ManageControls/EnumList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:enumList ID="ucEnumList" runat="server" />
</asp:Content>


