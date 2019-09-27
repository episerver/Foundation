<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.MultiReferenceTypeList" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master" Codebehind="MultiReferenceTypeList.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="typeList" Src="~/Apps/MetaDataBase/Modules/ManageControls/MultiReferenceTypeList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:typeList id="ucTypeList" runat="server" />
</asp:Content>