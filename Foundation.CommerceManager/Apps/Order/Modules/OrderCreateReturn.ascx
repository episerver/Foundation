<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderCreateReturn.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderCreateReturn" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Customer/Modules/EcfListViewControlWithoutDockTop.ascx" TagName="EcfListViewControl" TagPrefix="cm" %>
<%@ Register TagPrefix="mc3" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<style type="text/css">
	#<%=DialogContentPanel.ClientID%>, #contentPanel
	{
		height: 100%;
	}
</style>
<div class="popup-outer" style="height: 100%;">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<asp:UpdatePanel UpdateMode="Conditional" ID="DialogContentPanel" runat="server" RenderMode="Block">
		<ContentTemplate>
			<div style="position: relative; height: 100%; overflow: hidden;">
				<div style="position: absolute; top: 0px; left: 0px; right: 0px; bottom: 45px; overflow-y: auto;">
				<table width="100%">
					<tr>
						<td style="padding: 3px; width: 250px;" valign="top">
							<table style="border-collapse:collapse;">
								<tr>
									<td class="centertext">
										<cm:EcfListViewControl id="MyListView" runat="server" ShowTopToolbar="true" LayoutResizeEnable="false" AutoFullHeight="false" AutocountHeaderBottom="true" ViewId="OrderCreateReturnForm" AppId="Order"></cm:EcfListViewControl>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<table width="400px" style="float: left;">
					<tr>
						<td style="padding: 3px; width: 250px;" valign="top" class="episerveroverwriteimage">
                            <!-- Title for block is set in code behind -->
							<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLight1" runat="server" Title="<%$ Resources:SharedStrings, Total_Information %>"></mc2:BlockHeaderLight>
							<!-- BEGIN: totals -->
							<table class="orderform-blockheaderlight-datatable">
								<tr>
									<td class="orderform-label orderform-label-normal">
										<asp:Label runat="server" Text="<%$ Resources:SharedStrings, Line_Items %>"></asp:Label>:
									</td>
									<td class="orderform-field">
										<asp:Label ID="ItemsTotal" runat="server" Text="0"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="orderform-label label-divider orderform-label-normal">
										<asp:Label ID="Label13" runat="server" Text="<%$ Resources:SharedStrings, Invalidated_Discounts %>"></asp:Label>:
									</td>
									<td class="orderform-field label-divider">
										<asp:Label ID="InvalidDiscountsTotal" runat="server" Text="0"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="orderform-label">
										<asp:Label ID="Label12" runat="server" Text="<%$ Resources:OrderStrings, Return_Total %>"></asp:Label>:
									</td>
									<td class="orderform-field">
										<asp:Label ID="OrderTotal" runat="server" Text="0"></asp:Label>
									</td>
								</tr>
							<!-- END: totals -->
							</table>
						</td>
					</tr>
				    
				    
				</table>
				<table style="float: right;">
					<tr>
					<!-- additional info -->
					<td style="padding: 3px; width: 400px;" valign="top" class="episerveroverwriteimage">
                    <!-- Title for block is set in code behind -->
					<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server" Title="<%$ Resources:SharedStrings, Additional_Information %>"></mc2:BlockHeaderLight>
					<table class="orderform-blockheaderlight-datatable" style="table-layout: fixed; width: 400px;">
						<tr>
							<td class="orderform-label orderform-label-normal" style="width: 100px;">
								<asp:Label ID="Label7" runat="server" Text="<%$ Resources:SharedStrings, Comments %>"></asp:Label>:
							</td>
							<td>
								<asp:TextBox runat="server" ID="tbComments" Width="250" MaxLength="1024" TextMode="MultiLine"></asp:TextBox>
							</td>
						</tr>
					</table>
					</td>
				<!-- end additional info -->
					</tr>
				</table>
				<div style="clear: both;"></div>
                <table width="400px">
                    <tr>
					    <td style="padding: 3px;" valign="top" class="episerveroverwriteimage">
				        <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="InvalidDiscountsBlockHeader" runat="server" Title="<%$ Resources:SharedStrings, Invalidated_Discounts %>"></mc2:BlockHeaderLight>
                        <table class="orderform-blockheaderlight-datatable">
                            <asp:PlaceHolder runat="server" ID="NoInvalidatedDiscounts">
                                <tr>
                                    <td class="orderform-field centertext">
                                        <asp:Label runat="server" Text="None"/>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:Repeater runat="server" ID="rptInvalidDiscounts">
                                <ItemTemplate>
                                    <tr>
                                        <td class="orderform-label orderform-label-normal">
                                            <asp:Label runat="server" Text='<%# Eval("Name") + ":" %>'/>
                                        </td>
                                        <td class="orderform-field">
                                            <asp:Label runat="server" Text='<%# ((decimal)Eval("SavedAmount")).ToString("#0.00") %>'></asp:Label>
                                        </td>
                                    </tr>    
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>        
                        </td>
                    </tr>
                </table>                    
				</div>
			
				<div style="position: absolute; left: 0px; right: 0px; bottom: 0px; height: 45px;">
				<table width="100%">
					<tr>
						<td class="popup-buttons" colspan="2">
							<mc2:IMButton ID="btnSave" runat="server" class="btn-save" OnServerClick="btnSave_ServerClick" Text="<%$ Resources:Common, btnOK %>">
							</mc2:IMButton>
							&nbsp;
							<mc2:IMButton ID="btnCancel" runat="server" class="btn-save" CausesValidation="false" OnServerClick="btnCancel_ServerClick"  Text="<%$ Resources:Common, btnCancel %>">
							</mc2:IMButton>
						</td>
					</tr>
				</table>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</div>
<script type="text/javascript" src="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/jquery.min.js") %>"></script>
<script type="text/javascript">
	$(document).ready(function(){
		$('td.episerveroverwriteimage > table td:has(img):nth-child(odd) img').each(function(){
			$(this).attr('src','../../Shell/EPi/Shell/Resources/leftCorner.GIF');
		});
		$('td.episerveroverwriteimage > table td:has(img):nth-child(even) img').each(function(){
			$(this).attr('src','../../Shell/EPi/Shell/Resources/rightCorner.GIF');
		});
		$('td.episerveroverwriteimage > table td[background]').attr('background','../../Shell/EPi/Shell/Resources/linehz.GIF');
	});
</script>