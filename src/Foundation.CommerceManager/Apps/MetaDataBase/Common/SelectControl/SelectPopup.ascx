<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.SelectPopup" Codebehind="SelectPopup.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" TagPrefix="Ibn" %>
<div runat="server" id="backgroundContainer" style="position: absolute; left: 0px; top:0px; height: 100%; width: 100%; background-color: #666666; display: none; z-index: 10001; opacity: 0.4; filter: alpha(opacity=40);">
    &nbsp;
</div> 
<div id="divQuickAdd" runat="server" style=" background-color:#ffffff; position:absolute;width:400px;left:300px;top:100px;border:1px solid black;display:none;z-index:10002">

<div style="position:relative;">
<div id="divClose" runat="server" style="cursor:pointer;position: absolute; left: 382px; top:2px; height: 16px; width: 16px; z-index: 10002;">
	<img alt="" src='<%= CommerceHelper.GetAbsolutePath("/images/deny_black.gif") %>' />
</div>
<div style="position: absolute; left: 282px; top:2px; z-index: 10002;">Advanced View</div>
<table style="padding:5px" cellspacing="0" cellpadding="5" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox runat="server" ID="tbMain" Width="200px" CssClass="ffBugFix"></asp:TextBox>
			<Ibn:TextBoxExtender runat="server" ID="tbMainExtender" TargetControlID="tbMain"></Ibn:TextBoxExtender>
		</td>
	</tr>
	<tr>
		<td style="padding-top:10px;">
			<asp:UpdatePanel ID="upSelectPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">			
			<ContentTemplate>
            <asp:GridView runat="server" ID="GridMain" 
				AutoGenerateColumns="false" Width="100%" GridLines="None">     
            <Columns>
                <asp:TemplateField ItemStyle-Width="40px">
					<ItemStyle CssClass="ibn-rowTable" />
					<ItemTemplate>
                        <i><%# Eval("PrimaryKeyId")%></i>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
					<ItemStyle CssClass="ibn-rowTable" />
					<ItemTemplate>
                        <b><%# Eval(TitleFieldName)%></b>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="16px">
					<ItemStyle CssClass="ibn-rowTable" />
					<ItemTemplate>
						<asp:ImageButton ImageAlign="AbsMiddle" runat="server" ID="btnSave" ImageUrl="~/Images/select.gif" CommandName="Save" CommandArgument='<%# Eval("PrimaryKeyId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>	
            <div style="padding:10px;">
            <asp:Label ID="lblTotalCount" runat="server"></asp:Label>
            </div>
            </ContentTemplate>
			<Triggers>
			<asp:AsyncPostBackTrigger ControlID="tbMain" />
			<asp:AsyncPostBackTrigger ControlID="hfClassName" />
			</Triggers>
			</asp:UpdatePanel>
		</td>
	</tr>
	<asp:HiddenField ID="hfClassName" runat="server" />
</table>
</div>

</div>