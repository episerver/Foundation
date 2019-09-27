<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Tabs.ShippingMethodEditSettings" Codebehind="ShippingMethodEditSettings.ascx.cs" %>
<%@ Register TagPrefix="console" Namespace="Mediachase.Web.Console.Controls" Assembly="Mediachase.WebConsoleLib" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <th class="FormSectionHeader">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Markets %>"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="FormSectionCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Enabled_Markets %>"></asp:Label>
            </td>
        </tr>
		<tr>
			<td class="FormFieldCell">
				<console:DualList id="MarketList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
					LeftDataTextField="Value" LeftDataValueField="Key" RightDataTextField="Value" RightDataValueField="Key"
					ItemsName="Markets">
					<RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
					<ButtonStyle Width="100px"></ButtonStyle>
					<LeftListStyle Width="200px" Height="150px"></LeftListStyle>
				</console:DualList>
			</td>
		</tr>
    </table>
    <table class="DataForm"> 
        <tr>
            <th class="FormSectionHeader">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Restrictions %>"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="FormSectionCell">
                <asp:Label runat="server" ID="lblCountries" Text="<%$ Resources:SharedStrings, Countries %>"></asp:Label>
            </td>
        </tr>
        <tr>
			<td class="FormFieldCell">
				<console:DualList id="CountryList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
					LeftDataTextField="Name" LeftDataValueField="CountryId" RightDataTextField="Name" RightDataValueField="CountryId"
					ItemsName="Countries">
					<RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
					<ButtonStyle Width="100px"></ButtonStyle>
					<LeftListStyle Width="200px" Height="150px"></LeftListStyle>
				</console:DualList>
			</td>
		</tr>
		<tr>
            <td class="FormSectionCell">
                <asp:Label runat="server" ID="lblRegions" Text="<%$ Resources:SharedStrings, Regions %>"></asp:Label>
            </td>
        </tr>
		<tr>
			<td class="FormFieldCell">
				<console:DualList id="RegionList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
					LeftDataTextField="Name" LeftDataValueField="StateProvinceId" RightDataTextField="Name" RightDataValueField="StateProvinceId"
					ItemsName="Regions">
					<RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
					<ButtonStyle Width="100px"></ButtonStyle>
					<LeftListStyle Width="200px" Height="150px"></LeftListStyle>
				</console:DualList>
			</td>
		</tr>
		<tr>
            <td class="FormSectionCell">
                <asp:Label runat="server" ID="lblPayments" Text="<%$ Resources:SharedStrings, Restricted_Payments %>"></asp:Label>
            </td>
        </tr>
		<tr>
			<td class="FormFieldCell">
				<console:DualList id="PaymentsList" runat="server" ListRows="6" EnableMoveAll="True" CssClass="text"
					LeftDataTextField="Name" LeftDataValueField="PaymentMethodId" RightDataTextField="Name" RightDataValueField="PaymentMethodId"
					ItemsName="Payments">
					<RightListStyle Font-Bold="True" Width="200px" Height="150px"></RightListStyle>
					<ButtonStyle Width="100px"></ButtonStyle>
					<LeftListStyle Width="200px" Height="150px"></LeftListStyle>
				</console:DualList>
			</td>
		</tr>
    </table>
</div>