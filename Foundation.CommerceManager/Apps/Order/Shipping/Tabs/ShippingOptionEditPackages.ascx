<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingOptionEditPackages.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Tabs.ShippingOptionEditPackages" %>
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="lbAdd" Text="<%$ Resources:SharedStrings, Add_Item %>" OnClick="lbAdd_Click"></asp:LinkButton><br />
                </td>
            </tr>
            <tr>
                <td class="wh100">
                    <asp:ListView runat="server" ID="lvMain" DataKeyNames="ShippingPackageId">
                        <LayoutTemplate>
                            <table class="ecf-Grid">
                                <tr>
                                    <th style="width:250px;" class="HeadingCell">
                                        <asp:Label runat="server" ID="headerPackage" Text="<%$ Resources:SharedStrings, Package %>" CssClass="HeadingCellText"></asp:Label>
                                    </th>
                                    <th style="width:350px;" class="HeadingCell">
                                        <asp:Label runat="server" ID="headerPackageName" Text="<%$ Resources:SharedStrings, Package_Name %>" CssClass="HeadingCellText"></asp:Label>
                                    </th>
                                    <th style="width:150px;" class="HeadingCell">
                                        <asp:Label runat="server" ID="headerActions" Text="&nbsp;" CssClass="HeadingCellText"></asp:Label>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="Row">
                                <td class="DataCell">
                                    <asp:Label runat="server" ID="PackageNameLabel" CssClass="CellText">
                                        <%# GetPackageNameById((int)DataBinder.Eval(Container.DataItem, "PackageId"))%>
                                    </asp:Label>
                                </td>
                                <td class="DataCell">
                                    <asp:Label runat="server" ID="ShipppingPackageNameLabel" CssClass="CellText">
                                        <%# Eval("PackageName") %>
                                    </asp:Label>
                                </td>
                                <td style="text-align:center;" class="DataCell">
                                    <asp:LinkButton ID="EditButton" CommandName="Edit" runat="server" Text="<%$ Resources:SharedStrings, Edit %>"></asp:LinkButton> 
                                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CausesValidation="false" Text="<%$ Resources:SharedStrings, Delete %>"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr class="Row">
                                <td class="DataCell">
                                    <asp:DropDownList runat="server" ID="PackagesList" DataValueField="PackageId" DataTextField="Name" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="DataCell">
                                    <asp:TextBox ID="tbPackageName" runat="server" Text='<%# Bind("PackageName") %>' Width="350px" MaxLength="100" />
                                    <asp:RequiredFieldValidator runat="server" ID="PackageNameValidator" Text="*" Display="Dynamic" ControlToValidate="tbPackageName"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align:center;" class="DataCell">
                                    <asp:LinkButton ID="UpdateButton" CommandName="Update" runat="server" Text="<%$ Resources:SharedStrings, Update %>"></asp:LinkButton>
                                    <asp:LinkButton ID="CancelButton" CommandName="Cancel" runat="server" Text="<%$ Resources:SharedStrings, Cancel %>" CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <tr class="Row">
                                <td class="DataCell">
                                    <asp:DropDownList runat="server" ID="PackagesList" DataValueField="PackageId" DataTextField="Name" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="DataCell">
                                    <asp:TextBox ID="tbPackageName" runat="server" Width="340px" MaxLength="100"/>
                                    <asp:RequiredFieldValidator runat="server" ID="PackageNameValidator" Text="*" Display="Dynamic" ControlToValidate="tbPackageName"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align:center;" class="DataCell">
                                    <asp:LinkButton ID="SaveButton" CommandName="Save" runat="server" Text="<%$ Resources:SharedStrings, Save %>"></asp:LinkButton>
                                    <asp:LinkButton ID="CancelButton" CommandName="Cancel" runat="server" Text="<%$ Resources:SharedStrings, Cancel %>" CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                        </InsertItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>