<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.FormItemEdit" Codebehind="FormItemEdit.ascx.cs" %>
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
<script type="text/javascript">
	function ModifyTxt(val)
	{
		var obj = document.getElementById('divTxt');
		if(obj && val==0)
			divTxt.style.display = "none";
		if(obj && val==1)
			divTxt.style.display = "";
	}
</script>

<div class="rightCustomHeader text" style="padding: 10px;">
	<asp:Label ID="lblTitle" runat="server"></asp:Label><br />
	<span style="font-weight: normal"><asp:Label ID="lblComments" runat="server"></asp:Label></span>
</div>

<div id="divSelector" runat="server" style="padding: 15px; border-bottom: 1px solid #adadad" class="ibn-light text">
	<b><asp:Literal ID="Literal7" runat="server" Text='<%$Resources : MetaForm, Control %>' />:</b>&nbsp;&nbsp;
	<asp:DropDownList ID="ddControl" runat="server" Width="240px" AutoPostBack="true"
		OnSelectedIndexChanged="ddControl_SelectedIndexChanged">
	</asp:DropDownList>
	<asp:Label ID="lblControl" runat="server"></asp:Label>
</div>
<div id="divProperties" runat="server">
	<asp:PlaceHolder ID="phProperties" runat="server"></asp:PlaceHolder>
</div>

<div style="padding: 15px; border-bottom: 1px solid #adadad" class="ibn-light text">
	<b><%= GetGlobalResourceObject("MetaForm", "ShowName").ToString() %></b>
	<br />
	<asp:RadioButton onclick="ModifyTxt(0)" GroupName="Label" runat="server" ID="rbNone" /><br />
	<asp:RadioButton onclick="ModifyTxt(0)" GroupName="Label" runat="server" ID="rbDefault" /><br />
	<asp:RadioButton onclick="ModifyTxt(1)" GroupName="Label" runat="server" ID="rbCustom" />
	<div id="divTxt">
	<br />
	<asp:TextBox ID="txtTitle" runat="server" Width="250px" CssClass="text"></asp:TextBox>
	</div><br /><br />
	<table cellpadding="0" cellspacing="5">
		<tr>
			<td style="font-weight: bold; width: 140px;">
				<asp:Literal ID="Literal2" runat="server" Text='<%$Resources : MetaForm, LabelWidth %>' />:
			</td>
			<td>
				<asp:TextBox ID="txtLabelWidth" runat="server" Width="40px"></asp:TextBox>&nbsp;px
			</td>
			<td>
				<asp:CompareValidator ID="cv2" runat="server" Display="Static" ErrorMessage="*" ControlToValidate="txtLabelWidth" ValueToCompare="0" Operator="GreaterThanEqual" Type="Integer"></asp:CompareValidator>
			</td>
		</tr>
		<tr>
			<td style="font-weight:bold;">
				<asp:Literal ID="Literal1" runat="server" Text='<%$Resources : MetaForm, TabIndex %>' />:
			</td>
			<td>
				<asp:TextBox ID="TabIndexText" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
			</td>
			<td>
				<asp:RangeValidator runat="server" ID="TabIndexRangeValidator" ControlToValidate="TabIndexText" ErrorMessage="*" MinimumValue="0" MaximumValue="999" Display="Dynamic" Type="Integer"></asp:RangeValidator>
			</td>
		</tr>
	</table>
</div>
<div id="divLayout" runat="server" style="padding:15px;border-bottom:1px solid #adadad" class="ibn-light text">
  <b><asp:Literal ID="Literal3" runat="server" Text='<%$Resources : MetaForm, ViewType %>' />:</b>&nbsp;&nbsp;<br />
  <table cellspacing="7" cellpadding="0" class="text">
	 <tr>
	  <td><label for='<%=rb1.ClientID %>'><input type="radio" name="rbList" id="rb1" runat="server" value="1" checked="true" />&nbsp;<asp:Literal ID="Literal4" runat="server" Text='<%$Resources : MetaForm, ColSpan1 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%= ResolveClientUrl("~/Apps/MetaDataBase/Images/LayoutTypes/colspan1.gif") %>' /></td>
	</tr>
	<tr>
	  <td><label for='<%=rb2.ClientID %>'><input type="radio" name="rbList" id="rb2" runat="server" value="2" />&nbsp;<asp:Literal ID="Literal5" runat="server" Text='<%$Resources : MetaForm, ColSpan2 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%= ResolveClientUrl("~/Apps/MetaDataBase/Images/LayoutTypes/colspan2.gif") %>' /></td>
	</tr>
	<tr>
	  <td><b><asp:Literal ID="Literal6" runat="server" Text='<%$Resources : MetaForm, Rows %>' />:</b></td>
	  <td><asp:DropDownList ID="ddRows" runat="server" Width="100%"></asp:DropDownList></td>
	</tr>
  </table>
</div>

<div style="text-align: right; padding: 7px;">
	<button id="btnSave" runat="server" class="text" style="width: 80px" onserverclick="btnSave_ServerClick"></button>
	<button id="btnCancel" runat="server" onclick="window.close();" class="text" style="width: 80px"></button>
</div>