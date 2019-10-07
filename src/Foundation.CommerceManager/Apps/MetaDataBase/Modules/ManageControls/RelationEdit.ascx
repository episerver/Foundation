<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationEdit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.RelationEdit" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Reference Control="~/Apps/Common/Design/BlockHeader2.ascx" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/Common/Design/BlockHeader2.ascx" %>
<%@ Reference Control="~/Apps/MetaDataBase/Modules/ManageControls/MetaFormSelector.ascx" %>
<%@ Register TagPrefix="ibn" TagName="MetaFormSelector" Src="~/Apps/MetaDataBase/Modules/ManageControls/MetaFormSelector.ascx" %>
<%@ Register TagPrefix="ibn" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation"%>

<asp:UpdatePanel runat="server" ID="UpdatePanel1">
	<ContentTemplate>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
		<td><ibn:BlockHeader id="MainBlockHeader" runat="server" /></td>
	</tr>
	<tr>
		<td style="padding:5px;" class="episerveroverwriteimage">
			<ibn:BlockHeaderLight2 runat="server" id="InfoBlockHeader" Title="<%$Resources : GlobalMetaInfo, GeneralInfo%>" HeaderCssClass="ibn-toolbar-light"></ibn:BlockHeaderLight2>
			<div class="ibn-stylebox-light" style="padding:10px; padding-top:5px;">
				<table cellpadding="5" cellspacing="0" border="0" class="ibn-propertysheet" style="table-layout:fixed;">
					<tr>
						<td class="ibn-label" style="width:170px;">
							<asp:Literal ID="Literal4" runat="server" Text="<%$Resources : GlobalMetaInfo, PrimaryObject%>" />:
						</td>
						<td style="width:410px;">
							<asp:HyperLink runat="server" ID="PrimaryObjectLink"></asp:HyperLink>
							<ibn:indenteddropdownlist runat="server" ID="PrimaryObjectList" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="PrimaryObjectList_SelectedIndexChanged"></ibn:indenteddropdownlist>
						</td>
						<td style="width:20px"></td>
					</tr>
					<tr>
						<td class="ibn-label">
							<asp:Literal ID="Literal5" runat="server" Text="<%$Resources : GlobalMetaInfo, RelatedObject%>" />:
						</td>
						<td>
							<asp:HyperLink runat="server" ID="RelatedObjectLink"></asp:HyperLink>
							<ibn:indenteddropdownlist runat="server" ID="RelatedObjectList" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="RelatedObjectList_SelectedIndexChanged"></ibn:indenteddropdownlist>
						</td>
						<td style="width:20px"></td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td style="padding:5px; padding-top:0px;" class="episerveroverwriteimage">
			<ibn:BlockHeaderLight2 runat="server" id="FieldBlockHeader" HeaderCssClass="ibn-toolbar-light"></ibn:BlockHeaderLight2>
			<div class="ibn-stylebox-light" style="padding:10px; padding-top:5px;">
				<table cellpadding="5" cellspacing="0" border="0" class="ibn-propertysheet" style="table-layout:fixed;">
					<tr>
						<td class="ibn-label" style="width:170px;" valign="top">
							<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, FieldName%>" />:
						</td>
						<td style="width:410px;">
							<asp:TextBox Runat="server" ID="NameTextBox" Width="400" MaxLength="50"></asp:TextBox>
							<asp:Label Runat="server" ID="ErrorMessage" CssClass="ibn-error" Font-Bold="true"></asp:Label>
						</td>
						<td style="width:20px">
							<asp:RegularExpressionValidator ID="NameRegExValidator" ControlToValidate="NameTextBox" Runat="server" Display="Dynamic" ErrorMessage="<%$Resources : GlobalMetaInfo, LatinOnlyError%>" ValidationExpression="^[A-Za-z0-9][\w]*$"></asp:RegularExpressionValidator>
							<asp:RequiredFieldValidator id="NameRequiredValidator" runat="server" ErrorMessage="*" ControlToValidate="NameTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td class="ibn-label">
							<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:
						</td>
						<td>
							<asp:TextBox Runat="server" ID="FriendlyNameTextBox" Width="400" MaxLength="100"></asp:TextBox>
						</td>
						<td>
							<asp:RequiredFieldValidator id="FriendlyNameRequiredValidator" runat="server" ErrorMessage="*" ControlToValidate="FriendlyNameTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td></td>
						<td colspan="2">
							<asp:CheckBox Runat="server" ID="AllowNullsCheckBox" Checked="True" Text="<%$Resources : GlobalMetaInfo, AllowNulls%>"></asp:CheckBox>
						</td>
					</tr>
					<tr id="FormsRow" runat="server">
						<td class="ibn-label" valign="top">
							<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, AddToForm%>" />:
						</td>
						<td valign="top">
							<ibn:MetaFormSelector ID="mfs" runat="server" />
						</td>
						<td></td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr runat="server" id="DisplayRow">
		<td style="padding:5px; padding-top:0px;" class="episerveroverwriteimage">
			<ibn:BlockHeaderLight2 runat="server" id="DisplayBlockHeader" HeaderCssClass="ibn-toolbar-light"></ibn:BlockHeaderLight2>
			<div class="ibn-stylebox-light" style="padding:10px; padding-top:5px;">
				<table cellpadding="5" cellspacing="0" border="0" class="ibn-propertysheet" style="table-layout:fixed;">
					<tr>
						<td class="ibn-label" style="width:170px;">
							<asp:Literal ID="Literal6" runat="server" Text="<%$Resources : GlobalMetaInfo, DisplayRegion%>" />:
						</td>
						<td style="width:410px;">
							<asp:DropDownList runat="server" ID="DisplayRegion" Width="400" AutoPostBack="true" OnSelectedIndexChanged="DisplayRegion_SelectedIndexChanged"></asp:DropDownList>
						</td>
						<td style="width:20px;"></td>
					</tr>
					<tr runat="server" id="DisplayTextRow">
						<td class="ibn-label">
							<asp:Literal ID="Literal7" runat="server" Text="<%$Resources : GlobalMetaInfo, DisplayText%>" />:
						</td>
						<td>
							<asp:TextBox Runat="server" ID="DisplayText" Width="400" MaxLength="100"></asp:TextBox>
						</td>
						<td>
							<asp:RequiredFieldValidator id="DisplayTextRequiredValidator" runat="server" ErrorMessage="*" ControlToValidate="DisplayText" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr runat="server" id="DisplayOrderRow">
						<td class="ibn-label">
							<asp:Literal ID="Literal8" runat="server" Text="<%$Resources : GlobalMetaInfo, DisplayOrder%>" />:
						</td>
						<td>
							<asp:TextBox Runat="server" ID="DisplayOrderText" Width="400" MaxLength="6" Text="10000"></asp:TextBox>
							<asp:RangeValidator runat="server" ID="DisplayOrderRangeValidator" ControlToValidate="DisplayOrderText" CultureInvariantValues="true" Type="Integer" MinimumValue="0" MaximumValue="999999" ErrorMessage="*" Display="Dynamic"></asp:RangeValidator>
						</td>
						<td>
							<asp:RequiredFieldValidator id="DisplayOrderTextRequiredValidator" runat="server" ErrorMessage="*" ControlToValidate="DisplayOrderText" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td style="padding-left:170px; padding-top:5px; padding-bottom:10px;">	
			<ibn:IMButton runat="server" class="text" style="width:110px" ID="SaveButton" Text="<%$Resources : GlobalMetaInfo, Save%>" OnServerClick="SaveButton_ServerClick"></ibn:IMButton>&nbsp;&nbsp;
			<ibn:IMButton runat="server" class="text" style="width:110px" ID="CancelButton" Text="<%$Resources : GlobalMetaInfo, Cancel%>" CausesValidation="false" IsDecline="True"></ibn:IMButton>
		</td>
	</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/jquery.min.js") %>"></script>
<script type="text/javascript">
	var changeStyle = function () {
		$('td.episerveroverwriteimage > table td:has(img):nth-child(odd) img').each(function () {
			$(this).attr('src', '../../../Shell/EPi/Shell/Resources/leftCorner.GIF');
		});
		$('td.episerveroverwriteimage > table td:has(img):nth-child(even) img').each(function () {
			$(this).attr('src', '../../../Shell/EPi/Shell/Resources/rightCorner.GIF');
		});
		$('td.episerveroverwriteimage > table td[background]').attr('background', '../../../Shell/EPi/Shell/Resources/linehz.GIF');
	};
	$(document).ready(changeStyle);
	Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(function PageLoadingHandler(sender, args) {
		setTimeout(changeStyle, 50);
	});
   
</script>