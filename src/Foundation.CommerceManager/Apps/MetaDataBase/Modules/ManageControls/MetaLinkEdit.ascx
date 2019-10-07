<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.MetaLinkEdit" Codebehind="MetaLinkEdit.ascx.cs" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<script type="text/javascript">
  function SelectFunc()
  {
    var obj = document.getElementById('<%=lbLinkMetaClasses.ClientID%>');
    var fl = false;
    if(obj && obj.options.length>0)
    {
      for(var j=0; j<obj.options.length; j++)
        if(obj.options[j].selected)
        {
          fl = true;
          break;
        }
    }
    if(fl)
      return true;
    else
      return false;
  }
function SaveFunc()
{
	var selectColl = document.getElementsByTagName("select");
    var str="";
    for(j=0;j<selectColl.length;j++)
    {
		var sSelect = selectColl[j];
		if(sSelect.getAttribute("metaclass"))
			str += sSelect.getAttribute("metaclass") + ":" + sSelect.value + ";";
    }
    var _hid = document.getElementById('<%=hidField.ClientID %>');
    _hid.value = str;
    if(str!="")
		return true;
    else
      return false;
}
</script>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
    <td>
      <ibn:BlockHeader id="secHeader" runat="server" />
    </td>
  </tr>
	<tr>
		<td>
			<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table class="text" cellpadding="7" cellspacing="0" border="0" class="text">
						<tr id="trEdit1" runat="server">
							<td><b>Field Name:</b></td>
							<td><asp:Label ID="lblFieldName" runat="server"></asp:Label></td>
						</tr>
						<tr id="trEdit2" runat="server">
							<td><b>Friendly Name:</b></td>
							<td><asp:TextBox ID="txtFriendlyName" runat="server"></asp:TextBox></td>
						</tr>
						<tr>
						  <td><b>Meta Class:</b></td>
						  <td><asp:Label ID="lblOwnerMetaClass" runat="server" CssClass="text"></asp:Label></td>
						</tr>
						<tr>
						  <td valign="top"><b>Meta Fields:</b></td>
						  <td valign="top">
							<asp:DropDownList ID="ddOwnerFields" runat="server" width="300px"></asp:DropDownList>
						  </td>
						  <td valign="top"><b>Link Meta Classes:</b></td>
						  <td valign="top">
							<asp:ListBox ID="lbLinkMetaClasses" runat="server" Width="300px" Rows="4" SelectionMode="multiple"></asp:ListBox>
							<asp:Button ID="btnAdd" runat="server" CssClass="text" Text="Select" OnClick="btnAdd_Click" />
						  </td>
						</tr>
						<tr>
						  <td colspan="4">
							<asp:GridView style="border:1px solid #ddd;" runat="server" ID="grdMain" AutoGenerateColumns="false" CellPadding="4" 
								GridLines="None" AllowPaging="false" AllowSorting="false" 
								OnRowCommand="grdMain_RowCommand" OnRowDeleting="grdMain_RowDeleting" EnableViewState="false">
								  <Columns>
									<asp:BoundField DataField="Name" Visible="false" />
									<asp:TemplateField>
										<ItemStyle CssClass="ibn-vb" Width="26px" />
										  <HeaderStyle CssClass="ibn-vh" Width="26px" />
										  <ItemTemplate>
											  <asp:ImageButton ImageUrl="../../Images/delete.gif" Runat=server ToolTip="Delete" Width=16 Height=16 CommandName="Delete" CommandArgument='<%# Eval("Name") %>' ID="ibDelete"></asp:ImageButton> 
										  </ItemTemplate>
									  </asp:TemplateField>
									<asp:TemplateField HeaderText="">
									  <ItemStyle CssClass="ibn-vb" Width="20px" HorizontalAlign="Center" />
										  <ItemTemplate>
											  <asp:Image runat="server" ID="imFieldType" ImageUrl='<%#Eval("FieldTypeImageUrl") %>' Width="16px" Height="16px" />
										  </ItemTemplate>
									  </asp:TemplateField>
									<asp:TemplateField HeaderText="Owner Field">
										  <ItemStyle CssClass="ibn-vb" Width="120px"/>
										  <HeaderStyle CssClass="ibn-vh" />
										  <ItemTemplate>
											<%# Eval("FriendlyName")%>
										  </ItemTemplate>
									  </asp:TemplateField>
									</Columns>
								</asp:GridView>
						  </td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
  </tr>
</table>
<asp:HiddenField ID="hidField" runat="server" />
<div style="padding: 10 10 0 0; text-align:right">
	<mc:IMButton runat="server" class="text" style="width:110px" Text="<%$Resources : GlobalMetaInfo, Save%>" ID="imbtnSave"></mc:IMButton>
	<mc:IMButton runat="server" class="text" style="width:110px" ID="imbtnCancel" Text="<%$Resources : GlobalMetaInfo, Cancel%>" CausesValidation="false"></mc:IMButton>
</div>