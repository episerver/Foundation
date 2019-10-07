<%@ Control Language="C#" AutoEventWireup="true" 
	Inherits="Mediachase.Ibn.Web.UI.MetaUI.FormSectionEdit" Codebehind="FormSectionEdit.ascx.cs" %>
<style type="text/css">
  html, body {
   margin: 0;
   height: 100%;
  }
  .rightCustomHeader
    {
        background:#3364B9;
        color:White;
        font-weight:bold;
        padding:5px;
    }
</style>
<div class="rightCustomHeader text" style="padding: 10px;">
	<asp:Label ID="lblTitle" runat="server"></asp:Label><br />
	<span style="font-weight: normal"><asp:Label ID="lblComments" runat="server"></asp:Label></span>
</div>
<div style="padding:15px;border-bottom:1px solid #adadad" class="ibn-light text">
  <b><asp:Literal ID="Literal1" runat="server" Text='<%$Resources : MetaForm, Title %>' />:</b>&nbsp;&nbsp;
  <asp:TextBox ID="txtTitle" runat="server" Width="250px"></asp:TextBox>
</div>
<div style="padding:15px;border-bottom:1px solid #adadad" class="ibn-light text">
  <asp:CheckBox ID="cbShowBorder" runat="server" Checked="true" /><br /><br />
  <asp:CheckBox ID="cbShowTitle" runat="server" Checked="true" />
</div>
<div id="divControl" runat="server" style="padding:15px;border-bottom:1px solid #adadad" class="ibn-light text">
  <b><asp:Literal ID="Literal2" runat="server" Text='<%$Resources : MetaForm, Control %>' />:</b>&nbsp;&nbsp;
  <asp:DropDownList ID="ddControl" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddControl_SelectedIndexChanged"></asp:DropDownList>
  <asp:Label ID="lblControl" runat="server"></asp:Label>
</div>
<div id="divProperties" runat="server">
	<asp:PlaceHolder ID="phProperties" runat="server"></asp:PlaceHolder>
</div>
<div style="text-align: right; padding: 7px;">
	<button id="btnSave" runat="server" class="text" style="width: 80px" onserverclick="btnSave_ServerClick"></button>
	<button id="btnCancel" runat="server" onclick="window.close();" class="text" style="width: 80px"></button>
</div>