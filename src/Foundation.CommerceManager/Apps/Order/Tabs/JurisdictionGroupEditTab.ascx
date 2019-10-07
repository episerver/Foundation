<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JurisdictionGroupEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.JurisdictionGroupEditTab" %>
<%@ Register TagPrefix="console" Namespace="Mediachase.Web.Console.Controls" Assembly="Mediachase.WebConsoleLib" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label5" runat="server" text="<%$ Resources:SharedStrings, Display_Name %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="DisplayName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="DisplayName" Display="Dynamic" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label ID="Label1" runat="server" text="<%$ Resources:SharedStrings, Code %>"></asp:Label>:
            </td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="Code"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator0" ControlToValidate="Code" Display="Dynamic" ErrorMessage="*" />
                <asp:CustomValidator runat="server" ID="CodeCheckCustomValidator" ControlToValidate="Code" OnServerValidate="CodeCheck" Display="Dynamic" ErrorMessage="<%$ Resources:OrderStrings, JurisdictionGroup_With_Code_Exists %>" />
            </td>
        </tr>
        <tr>
            <td class="FormSectionCell" colspan="2">
                <asp:Label runat="server" ID="lblShippingMethods" text="<%$ Resources:OrderStrings, Jurisdictions_In_The_Group %>"></asp:Label>
            </td>
        </tr>
		<tr>
			<td class="FormFieldCell" colspan="2">
				<console:DualList id="JurisdictionsList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
					LeftDataTextField="DisplayName" LeftDataValueField="JurisdictionId" RightDataTextField="DisplayName" RightDataValueField="JurisdictionId"
					ItemsName="Jurisdictions">
					<RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
					<ButtonStyle Width="100px"></ButtonStyle>
					<LeftListStyle Width="200px" Height="150px"></LeftListStyle>
				</console:DualList>
			</td>
		</tr>
    </table>
</div>