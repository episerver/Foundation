<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.SmartTableLayoutProperties" Codebehind="SmartTableLayoutProperties.ascx.cs" %>
<div style="padding:15px;padding-top:7px;border-bottom:1px solid #adadad" class="ibn-light text">
  <b><asp:Literal ID="Literal1" runat="server" Text='<%$Resources : MetaForm, ViewType %>' />:</b>&nbsp;&nbsp;<br />
  <table cellspacing="7" cellpadding="0" class="text">
	 <tr>
	  <td><label for='<%=rb1.ClientID %>'><input type="radio" name="rbList" id="rb1" runat="server" checked="true" value="1" />&nbsp;<asp:Literal ID="Literal5" runat="server" Text='<%$Resources : MetaForm, OneCol %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=ResolveClientUrl("~/Apps/MetaDatabase/Images/LayoutTypes/1.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for='<%=rb11.ClientID %>'><input type="radio" name="rbList" id="rb11" runat="server" value="11" />&nbsp;<asp:Literal ID="Literal2" runat="server" Text='<%$Resources : MetaForm, TwoCols11 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=ResolveClientUrl("~/Apps/MetaDatabase/Images/LayoutTypes/1_1.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for='<%=rb12.ClientID %>'><input type="radio" name="rbList" id="rb12" runat="server" value="12" />&nbsp;<asp:Literal ID="Literal3" runat="server" Text='<%$Resources : MetaForm, TwoCols12 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=ResolveClientUrl("~/Apps/MetaDatabase/Images/LayoutTypes/1_2.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for='<%=rb21.ClientID %>'><input type="radio" name="rbList" id="rb21" runat="server" value="21" />&nbsp;<asp:Literal ID="Literal4" runat="server" Text='<%$Resources : MetaForm, TwoCols21 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=ResolveClientUrl("~/Apps/MetaDatabase/Images/LayoutTypes/2_1.JPG") %>' /></td>
	</tr>
  </table>
</div>
<div style="padding: 15px;padding-top:7px; border-bottom: 1px solid #adadad" class="ibn-light text">
	<div style="font-weight:bold;width:90px;float:left;"><asp:Literal ID="Literal6" runat="server" Text='<%$Resources : MetaForm, CellPadding %>' />:</div>
	<asp:TextBox ID="txtCellPadding" runat="server" Width="40px"></asp:TextBox>
	<asp:CompareValidator ID="cv2" runat="server" Display="Static" ErrorMessage="*" ControlToValidate="txtCellPadding" ValueToCompare="0" Operator="GreaterThanEqual" Type="Integer"></asp:CompareValidator>
</div>