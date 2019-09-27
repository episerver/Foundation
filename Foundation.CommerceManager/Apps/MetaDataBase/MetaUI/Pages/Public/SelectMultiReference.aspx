<%@ Page Language="C#" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Public.SelectMultiReference" Theme="" Codebehind="SelectMultiReference.aspx.cs" %>
<%@ Register TagPrefix="ibn" TagName="ctrlMultiSelect" Src="~/Apps/MetaDataBase/MetaUI/Modules/SelectControls/SelectMultiReference.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMain" Runat="Server">
	<ibn:ctrlMultiSelect ID="ctrlMulti" runat="server" />
</asp:Content>