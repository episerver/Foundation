<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MetaClassList" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MetaClassList.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="classList" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaClassList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:classList ID="ucClassList" runat="server" />
</asp:Content>

