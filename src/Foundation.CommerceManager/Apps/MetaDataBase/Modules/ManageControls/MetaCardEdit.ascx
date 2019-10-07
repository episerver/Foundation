<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.MetaCardEdit" Codebehind="MetaCardEdit.ascx.cs" %>
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
		<td style="padding:7px;">
			<table cellpadding="5" cellspacing="0" border="0" class="ibn-propertysheet" width="100%">
				<tr>
					<td class="ibn-label" width="170px">
						<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, ParentTable%>" />:
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlClass" Width="300"></asp:DropDownList>
					</td>
					<td width="16px"></td>
					<td width="16px"></td>
				</tr>
				<tr>
					<td class="ibn-label" width="170px">
						<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, SystemName%>" />:
					</td>
					<td>
						<asp:TextBox Runat="server" ID="txtClassName" Width="100%" MaxLength="50"></asp:TextBox>
					</td>
					<td></td>
					<td>
						<asp:RequiredFieldValidator id="vldClassName_Required" runat="server" ErrorMessage="*" ControlToValidate="txtClassName" Display="Dynamic"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator ID="vldClassName_RegEx" ControlToValidate="txtClassName" Runat="server" Display="Dynamic" ErrorMessage="*" ValidationExpression="^[A-Za-z0-9][\w]*$"></asp:RegularExpressionValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label" width="170px">
						<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:
					</td>
					<td>
						<asp:TextBox Runat="server" ID="txtClassFriendlyName" Width="100%" MaxLength="100"></asp:TextBox>
					</td>
					<td style="width:16px" align="left">
						<img src='<%=ResolveClientUrl("~/Apps/MetaDataBase/Images/resource.gif")%>' title='<%=GetGlobalResourceObject("GlobalMetaInfo", "ResourceTooltip").ToString()%>' style="width:16px; height:16px" alt="" />
					</td>
					<td width="16px">
						<asp:RequiredFieldValidator id="vldClassFriendlyName_Required" runat="server" ErrorMessage="*" ControlToValidate="txtClassFriendlyName" Display="Dynamic"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td class="ibn-label" width="170px">
						<asp:Literal ID="Literal4" runat="server" Text="<%$Resources : GlobalMetaInfo, PluralName%>" />:
					</td>
					<td>
						<asp:TextBox Runat="server" ID="txtClassPluralName" Width="100%" MaxLength="100"></asp:TextBox>
					</td>
					<td style="width:16px" align="left">
						<img src='<%=ResolveClientUrl("~/Apps/MetaDataBase/Images/resource.gif")%>' title='<%=GetGlobalResourceObject("GlobalMetaInfo", "ResourceTooltip").ToString()%>' style="width:16px; height:16px" alt="" />
					</td>
					<td width="16px">
						<asp:RequiredFieldValidator id="vldClassPluralName_Required" runat="server" ErrorMessage="*" ControlToValidate="txtClassPluralName" Display="Dynamic"></asp:RequiredFieldValidator>
					</td>
				</tr>
			</table>
			<br />
			<asp:Label runat="server" ID="lbError" ForeColor="Red" Visible="false" CssClass="text"></asp:Label>		
		</td>
	</tr>
</table>
<div style="padding: 10 10 0 0; text-align:right;">
	<btn:IMButton runat="server" ID="imbtnSave" Text="<%$Resources : GlobalMetaInfo, Save%>" class='text' style="width:110px"></btn:IMButton>&nbsp;&nbsp;
	<btn:IMButton runat="server" ID="imbtnCancel" Text="<%$Resources : GlobalMetaInfo, Cancel%>" CausesValidation="false" class='text' style="width:110px"></btn:IMButton>
</div>