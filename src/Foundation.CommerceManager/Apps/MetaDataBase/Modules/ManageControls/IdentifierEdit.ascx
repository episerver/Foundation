<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.IdentifierEdit" Codebehind="IdentifierEdit.ascx.cs" %>
<%@ register TagPrefix="btn" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation"%>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<script type="text/javascript">
	function SetName(sourceid, targetid, validatorid)
	{
		var input = document.getElementById(sourceid);
		if(input!=null && input.value.length>0)
		{
			var input1 = document.getElementById(targetid);
			if(input1.value.length==0)
				input1.value = input.value;
			if(validatorid!=null)
			{	
				input1 = document.getElementById(validatorid);	
				if(input1!=null)
					input1.style.display = "none";
			}
		}
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
			<table cellpadding="0" cellspacing="15" border="0" width="100%">
				<tr>
					<td valign="top" width="50%">
						<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet" style="table-layout:fixed">
							<tr><td width="120px"></td><td></td><td width="20px"></td><td width="16px"></td></tr>
							<tr>
								<td class="ibn-label"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, IdentifierName%>" />:</td>
								<td>
									<asp:TextBox Runat="server" ID="txtName" Width="100%" MaxLength="50"></asp:TextBox>
								</td>
								<td></td>
								<td>
									<asp:RequiredFieldValidator id="rfvName" runat="server" ErrorMessage="*" ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="reName" ControlToValidate="txtName" Runat="server" Display="Dynamic" ErrorMessage="*" ValidationExpression="^[A-Za-z0-9][\w]*$"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td class="ibn-label"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:</td>
								<td>
									<asp:TextBox Runat="server" ID="txtFriendlyName" Width="100%" MaxLength="100"></asp:TextBox>
								</td>
								<td>
									<img src='<%=Page.ResolveClientUrl("../../images/resource.gif")%>' title='<%=GetGlobalResourceObject("GlobalMetaInfo", "ResourceTooltip").ToString()%>' />
								</td>
								<td>
									<asp:RequiredFieldValidator id="rfvFriendlyName" runat="server" ErrorMessage="*" ControlToValidate="txtFriendlyName" Display="Dynamic"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td></td>
								<td colspan="3" style="padding-top:20px">
								</td>
							</tr>
						</table>
					</td>
					<td valign="top" width="50%">
						<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
							<tr>
								<td class="ibn-label" width="120px">
									<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, IdentifierScope%>" />:
								</td>
								<td>
									<asp:DropDownList runat="server" ID="ddlScope" Width="100%"></asp:DropDownList>
								</td>
								<td width="20px"></td>
							</tr>
							<tr>
								<td class="ibn-label" width="120px">
									<asp:Literal ID="Literal4" runat="server" Text="<%$Resources : GlobalMetaInfo, CounterReset%>" />:
								</td>
								<td>
									<asp:DropDownList runat="server" ID="ddlCounterReset" Width="100%"></asp:DropDownList>
								</td>
								<td width="20px"></td>
							</tr>
							<tr>
								<td class="ibn-label" width="120px">
									<asp:Literal ID="Literal5" runat="server" Text="<%$Resources : GlobalMetaInfo, IdentifierMask%>" />:
								</td>
								<td>
									<asp:TextBox Runat="server" ID="txtMask" Width="100%" MaxLength="50" Text="#" ToolTip="<%$Resources : GlobalMetaInfo, MaskTooltip%>"></asp:TextBox>
								</td>
								<td width="20px">
									<asp:RequiredFieldValidator id="rfvMask" runat="server" ErrorMessage="*" ControlToValidate="txtMask" Display="Dynamic"></asp:RequiredFieldValidator>
									<asp:RegularExpressionValidator ID="revMask" runat="server" ErrorMessage="*" ControlToValidate="txtMask" Display="dynamic" ValidationExpression="[^#]*[#][^#]*"></asp:RegularExpressionValidator>
								</td>
							</tr>
							<tr>
								<td class="ibn-label" width="120px">
									<asp:Literal ID="Literal6" runat="server" Text="<%$Resources : GlobalMetaInfo, CounterLength%>" />:
								</td>
								<td>
									<asp:TextBox Runat="server" ID="txtCounterLen" Width="100px" MaxLength="2" Text="5"></asp:TextBox>
									<asp:RequiredFieldValidator id="rfvCounterLen" runat="server" ErrorMessage="*" ControlToValidate="txtCounterLen" Display="Dynamic"></asp:RequiredFieldValidator>
									<asp:RangeValidator ID="rvCounterLen" runat="server" ErrorMessage="*" ControlToValidate="txtCounterLen" Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="30"></asp:RangeValidator>
								</td>
								<td></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

<table border="0" cellpadding="0" cellspacing="0" width="95%">
	<tr>
		<td align="right" style="padding-top:5px">
			<btn:IMButton runat="server" class="text" style="width:80px" ID="imbtnSave" Text="<%$Resources : GlobalMetaInfo, Save%>"></btn:IMButton>&nbsp;&nbsp;
			<btn:IMButton runat="server" class="text" style="width:80px" ID="imbtnCancel" Text="<%$Resources : GlobalMetaInfo, Cancel%>" CausesValidation="false"></btn:IMButton>
		</td>
	</tr>
</table>