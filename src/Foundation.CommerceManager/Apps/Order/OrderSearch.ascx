<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.OrderSearchList" CodeBehind="OrderSearch.ascx.cs" %>
<%@ Register Src="../Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<%@ Register Src="../Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<%@ Register TagPrefix="IbnWebControls" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<%@ Register TagPrefix="orders" Namespace="Mediachase.Commerce.Orders.DataSources" Assembly="Mediachase.Commerce" %>
<IbnWebControls:McDock ID="DockTop" runat="server" Anchor="Top" EnableSplitter="False" DefaultSize="75">
    <DockItems>
        <asp:Panel runat="server" ID="pnlMain" DefaultButton="btnSearch" Height="75px" BackColor="#F8F8F8" BorderColor="Gray" BorderWidth="0">

            <div id="DataForm">
                <table cellpadding="0" style="background-color: #F8F8F8;" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Class_Type %>" />:</td>
                        <td>
                            <asp:DropDownList ID="ClassType" Width="140" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="PurchaseOrder" Text="<%$ Resources:OrderStrings, Order_Purchase_Order %>" />
                                <asp:ListItem Value="ShoppingCart" Text="<%$ Resources:OrderStrings, Order_Shopping_Cart %>" />
                                <asp:ListItem Value="PaymentPlan" Text="<%$ Resources:OrderStrings, Order_Payment_Plan %>" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, Date_Range %>" />:
                        </td>
                        <td>
                            <asp:DropDownList ID="DateRange" Width="140" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="lbRmaDescr" runat="server" Text="<%$ Resources:OrderStrings, Return_Number%>" />:
                        </td>
                        <td>
                            <asp:TextBox ID="tbRmaNUmber" Width="140" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:SharedStrings, Customer %>" />:
                        </td>
                        <td>
                            <asp:TextBox ID="CustomerKeyword" Width="140" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Status %>" />:
                        </td>
                        <td>
                            <asp:DropDownList ID="OrderStatusList" Width="140" runat="server"></asp:DropDownList>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <span id="customDateFromLabel">From:</span>
                        </td>
                        <td>
                            <span id="customDateFromInput">
                                <ecf:CalendarDatePicker runat="server" ID="StartDate" TimeDisplay="false" ValidationEnabled="False" />
                            </span>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, ID %>" />:</td>
                        <td>
                            <asp:TextBox ID="OrderNumber" Width="140" runat="server"></asp:TextBox></td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SharedStrings, Email %>" />:</td>
                        <td>
                            <asp:TextBox ID="CustomerEmail" Width="140" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                              <asp:Button ID="btnReset" runat="server" Width="100" Text="<%$ Resources:SharedStrings, Reset %>" OnClick="btnReset_OnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:SharedStrings, Market %>" />:</td>
                        <td>
                            <asp:DropDownList ID="MarketsList" Width="140" runat="server" DataTextField="MarketName" DataValueField="MarketId" />
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <span id="customDateUntilLabel">Until:</span>
                        </td>
                        <td>
                            <span id="customDateUntilInput">
                                <ecf:CalendarDatePicker runat="server" ID="EndDate" TimeDisplay="false" ValidationEnabled="False" />
                            </span>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:SharedStrings, Phone_Number %>" />:</td>
                        <td>
                            <asp:TextBox ID="CustomerPhone" Width="140" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td>
                            <asp:UpdatePanel ID="upSearchButton" ChildrenAsTriggers="true" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnSearch" runat="server" Width="100" Text="<%$ Resources:SharedStrings, Search %>" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </DockItems>
</IbnWebControls:McDock>
<orders:OrderDataSource runat="server" ID="OrderListDataSource"></orders:OrderDataSource>
<core:EcfListViewControl ID="MyListView" runat="server" AppId="Order" ViewId="OrderSearch-List" ShowTopToolbar="true"></core:EcfListViewControl>