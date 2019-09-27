<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleAssignmentEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.RoleAssignmentEdit" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="RoleFilter.ascx" TagName="RoleFilter" TagPrefix="uc1" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Register TagPrefix="mc" TagName="EntityDD" Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<script type="text/javascript" src='<%=CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/Scripts/main.js") %>' ></script>
<script type="text/javascript">
    function RunValidation(arrVGroups) {
        var validated = true;
        if (arrVGroups != null && (arrVGroups instanceof Array) && (typeof (Page_ClientValidate) == 'function')) {
            for (var i = 0; i < arrVGroups.length; i++) {
                validated = Page_ClientValidate(arrVGroups[i]);
                if (!validated)
                    break;
            }

            //remove the flag to block the submit if it was raised
            Page_BlockSubmit = false;
        }
        //return the results
        return validated;
    }
</script>
<table cellpadding="2" class="ibn-padding5" style="background-color: #F8F8F8; height: 100%; width:100%;">
	<tr>
		<td colspan="2">
			<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
		</td>
	</tr>
	<tr>
		<td class="FormLabelCell" style="width: 100px; vertical-align:middle">
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Customer, SecurityRole  %>" />:
		</td>
		<td class="FormLabelCell">
			<uc1:RoleFilter ID="uc1SecurityRoleFilter" IsFieldRequired="true" runat="server" />
		</td>
	</tr>
	<tr>
		<td  class="FormLabelCell" style="width: 100px; vertical-align:middle">
			<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources: Customer, RoleAssignment_OnlyForOwner  %>" />:
		</td>
		<td class="FormLabelCell">
			<asp:CheckBox runat="server" ID ="chkbOnlyForOwner" AutoPostBack="true"/>
		</td>
	</tr>
	<tr>
		<td class="FormLabelCell" style="width: 100px; vertical-align:middle">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: Customer, Organization  %>" />:
		</td>
		<td class="FormFieldCell">
			<mc:EntityDD ID="refObjects" ItemCount="6" runat="server" Width="320" CommandName="MC_MUI_EntityDD_Inner" />
			<asp:Button runat="server" ID="RefreshButton" onclick="RefreshButton_Click" Visible="false" />
		</td>
	</tr>
	<tr runat="server" id="CheckModeRow">
		<td class="FormLabelCell" style="width: 100px; vertical-align:middle">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Customer, RoleAssignment_CheckMode  %>" />:
		</td>
		<td class="FormFieldCell">
			<asp:DropDownList runat="server" ID="ddlCheckMode" Enabled="false" Width="320" DataValueField="ModeValue"
				DataTextField="ModeName">
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td colspan="2" style="text-align: right; padding:138px 10px 20px 10px" >
			<asp:Button runat="server" ID="btnSave" Text="<%$ Resources: SharedStrings, Save %>" Width="100" />
			&nbsp;
			<asp:Button runat="server" ID="CancelButton" Text="<%$ Resources: SharedStrings, Cancel %>" Width="100" OnClick="CancelButton_Click" />
		</td>
	</tr>
</table>
