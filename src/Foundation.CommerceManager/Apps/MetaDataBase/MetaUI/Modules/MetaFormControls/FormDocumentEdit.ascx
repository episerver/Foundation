<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.FormDocumentEdit" Codebehind="FormDocumentEdit.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
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
<div style="padding: 15px; border-bottom: 1px solid #adadad" class="ibn-light text">
	<div style="font-weight:bold;width:90px;float:left;"><asp:Literal ID="Literal1" runat="server" Text='<%$Resources : MetaForm, Table %>' />:</div>
	<asp:DropDownList ID="ddClasses" runat="server" Width="220px">
	</asp:DropDownList>
	<asp:Label ID="lblClass" runat="server"></asp:Label>
</div>
<asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<div style="padding: 15px; border-bottom: 1px solid #adadad" class="ibn-light text">
			<div style="font-weight:bold;width:90px;float:left;"><asp:Literal ID="Literal2" runat="server" Text='<%$Resources : MetaForm, FormName %>' />:</div>
			<asp:DropDownList ID="ddFormType" runat="server" Width="220px" AutoPostBack="true" OnSelectedIndexChanged="ddFormType_SelectedIndexChanged">
			</asp:DropDownList>
			<asp:Label ID="lblForm" runat="server"></asp:Label>
			<div><asp:Label ID="lblError" ForeColor="red" runat="server" Text='<%$Resources : MetaForm, DuplicateTitleException %>' /></div>
		</div>
		<div id="divTitle" runat="server" style="padding: 15px; border-bottom: 1px solid #adadad" class="ibn-light text">
			<div style="font-weight:bold;width:90px;float:left;"><asp:Literal ID="Literal3" runat="server" Text='<%$Resources : MetaForm, Title %>' />:</div>
			<asp:TextBox ID="txtTitle" runat="server" Width="220px"></asp:TextBox>
			<asp:RequiredFieldValidator ID="rfVal" runat="server" ErrorMessage="*" Display="Dynamic"
				ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>
<div id="divLayout" runat="server" style="padding:15px;border-bottom:1px solid #adadad" class="ibn-light text">
  <b><asp:Literal ID="Literal4" runat="server" Text='<%$Resources : MetaForm, ViewType %>' />:</b>&nbsp;&nbsp;<br />
  <table cellspacing="7" cellpadding="0" class="text">
	 <tr>
	  <td><label for="rb11"><input type="radio" name="rbList" id="rb11" runat="server" checked="true" value="11" />&nbsp;<asp:Literal ID="Literal5" runat="server" Text='<%$Resources : MetaForm, TwoCols11 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Images/LayoutTypes/tab_1_1.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for="rb12"><input type="radio" name="rbList" id="rb12" runat="server" value="12" />&nbsp;<asp:Literal ID="Literal6" runat="server" Text='<%$Resources : MetaForm, TwoCols12 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Images/LayoutTypes/tab_1_2.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for="rb21"><input type="radio" name="rbList" id="rb21" runat="server" value="21" />&nbsp;<asp:Literal ID="Literal7" runat="server" Text='<%$Resources : MetaForm, TwoCols21 %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Images/LayoutTypes/tab_2_1.JPG") %>' /></td>
	</tr>
	<tr>
	  <td><label for="rb111"><input type="radio" name="rbList" id="rb111" runat="server" value="111" />&nbsp;<asp:Literal ID="Literal8" runat="server" Text='<%$Resources : MetaForm, ThreeCols %>' /></label></td>
	  <td><img style="border:0;" alt="" src='<%=CommerceHelper.GetAbsolutePath("/Apps/MetaDataBase/Images/LayoutTypes/tab_1_1_1.JPG") %>' /></td>
	</tr>
  </table>
</div>
<div style="padding: 15px;padding-top:7px; border-bottom: 1px solid #adadad" class="ibn-light text">
	<div style="font-weight:bold;width:90px;float:left;"><asp:Literal ID="Literal9" runat="server" Text='<%$Resources : MetaForm, CellPadding %>' />:</div>
	<asp:TextBox ID="txtCellPadding" runat="server" Width="40px"></asp:TextBox>
	<asp:CompareValidator ID="cv2" runat="server" Display="Static" ErrorMessage="*" ControlToValidate="txtCellPadding" ValueToCompare="0" Operator="GreaterThanEqual" Type="Integer"></asp:CompareValidator>
</div>
<div style="text-align: right; padding: 7px;">
	<button id="btnSave" runat="server" class="text" style="width: 80px" onserverclick="btnSave_ServerClick"></button>
	<button id="btnCancel" runat="server" onclick="window.close();" class="text" style="width: 80px"></button>
</div>