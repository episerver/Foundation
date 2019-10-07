<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.EnumMultiValue_Edit" Codebehind="EnumMultiValue.Edit.ascx.cs" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td valign="bottom">
			<table cellpadding="0" cellspacing="0" width="99%">
				<tr>
					<td width="100%">
						<div style="border: 1px solid #7F9DB9; padding:5px; height:20px; overflow-y: auto;" runat="server" id="divBlock">
						<asp:DataGrid id="grdMain" runat="server" allowsorting="False" allowpaging="False" width="100%" autogeneratecolumns="False" borderwidth="0" gridlines="None" cellpadding="0" CellSpacing="0" ShowFooter="False" ShowHeader="False">
							<columns>
								<asp:boundcolumn visible="false" datafield="Id"></asp:boundcolumn>
								<asp:templatecolumn itemstyle-cssclass="text">
									<itemtemplate>
										<asp:checkbox runat="server" id="chkItem" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:checkbox>
									</itemtemplate>
								</asp:templatecolumn>
							</columns>
						</asp:DataGrid>
						</div>
					</td>
					<td width="22px" runat="server" id="tdEdit" valign="top" style="padding-left:2px;">
						<button id="btnEditItems" runat="server" style="border:0px;padding:0px;height:20px;width:22px;background-color:transparent" type="button" title='<%$ Resources:GlobalMetaInfo,EditDictionary %>' tabindex="-1"><IMG 
						height="20" src='<%=ResolveClientUrl("~/Apps/MetaDataBase/images/dictionary_edit.gif")%>' width="22" border="0"></button>
					</td>
				</tr>
			</table>
		</td>
		<td width="20px" valign="top">
			<asp:CustomValidator runat="server" ID="vldCustom" ErrorMessage="*" Display="dynamic" OnServerValidate="vldCustom_ServerValidate"></asp:CustomValidator>
		</td>
	</tr>
</table>
<asp:Button id="btnRefresh" runat="server" CausesValidation="False" style="display:none;" OnClick="btnRefresh_Click"></asp:Button>

