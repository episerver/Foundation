<%@ Page Language="C#" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Pages.Public.SelectItem" Theme="" Codebehind="SelectItem.aspx.cs" %>
<%@ Register TagPrefix="ibn" TagName="ctrlSelectItem" Src="~/Apps/MetaDataBase/MetaUI/Modules/SelectControls/SelectItem.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMain" Runat="Server">
	<ibn:ctrlSelectItem ID="ctrlSelect" runat="server" />
</asp:Content>