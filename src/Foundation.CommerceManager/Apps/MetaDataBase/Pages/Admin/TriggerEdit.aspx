<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Pages.Admin.TriggerEdit" MasterPageFile="~/Apps/MetaDataBase/MasterPages/DialogMasterPage.master" Codebehind="TriggerEdit.aspx.cs" %>
<%@ Register TagPrefix="mc" TagName="triggerEdit" Src="~/Apps/MetaDataBase/Modules/ManageControls/TriggerEdit.ascx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="phMain">
	<mc:triggerEdit id="ucTriggerEdit" runat="server" />
</asp:Content>