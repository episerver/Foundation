<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomPageDesign.aspx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customization.Pages.CustomPageDesign" MasterPageFile="~/Apps/MetaDataBase/MasterPages/ExtGrid.master"%>
<%@ Register TagPrefix="mc" TagName="Module" Src="~/Apps/Customization/Modules/CustomPageDesign.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
	<mc:Module ID="McModule" runat="server" />
</asp:Content>
