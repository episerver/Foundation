<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.MetaControls.StringDictionaryControl" Codebehind="StringDictionaryControl.ascx.cs" %>
<tr>
	<td class="FormLabelCell"><strong><asp:Label id="MetaLabelCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>:</strong></td>
	<td class="FormFieldCell">
		<table width="100%">
	        <tr>
		        <td class="GridItem">
		            <table>
		                <tr>
		                    <td align="right">
		                        <%=RM.GetString("STRINGDICTIONARYCONTROL_KEY")%>:
		                    </td>
		                    <td align="left">
		                        <asp:TextBox Runat="server" ID="Key" Width="100px" ValidationGroup="AddStringDictionaryItemButtonValidationGroup"></asp:TextBox><asp:RequiredFieldValidator Runat="server" ControlToValidate="Key" ErrorMessage="*" id="rfValidatorKey" EnableClientScript="False" ValidationGroup="AddStringDictionaryItemButtonValidationGroup"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
		                    </td>
		                </tr>		
			            <tr>
		                    <td align="right">
    		                    <%=RM.GetString("STRINGDICTIONARYCONTROL_VALUE")%>:
		                    </td>
		                    <td align="left">
		                        <asp:TextBox Runat="server" ID="Value" Width="100px" ValidationGroup="AddStringDictionaryItemButtonValidationGroup"></asp:TextBox><asp:RequiredFieldValidator Runat="server" ControlToValidate="Value" ErrorMessage="*" id="rfValidatorValue" EnableClientScript="False" ValidationGroup="AddStringDictionaryItemButtonValidationGroup"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;
		                    </td>
		                </tr>
		                <tr>
		                    <td align="left" colspan="2">
    		                    <asp:Button Runat="server" Text="<%$ Resources:SharedStrings, Add_Lowercase %>" ID="AddStringDictionaryItemButton" onclick="AddStringDictionaryItemButton_Click" ValidationGroup="AddStringDictionaryItemButtonValidationGroup"></asp:Button>
		                    </td>
		                </tr>
                    </table>
                </td>
            </tr>
            <tr>
		        <td>
                    <asp:GridView ID="ItemsGrid" runat="server" CssClass="Grid" AllowPaging="False" Width="100%" ShowFooter="False" DataKeyNames="Key" AutoGenerateColumns="False">
	                    <RowStyle CssClass="GridItem"></RowStyle>
	                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
	                    <FooterStyle CssClass="GridFooter"></FooterStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:SharedStrings, Key %>" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Literal ID="txtKey" runat="server" Text='<%# Bind("Key") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:SharedStrings, Value %>" HeaderStyle-Width="70%">
                                <ItemTemplate>
                                    <asp:Literal ID="txtValue" runat="server" Text='<%# Bind("Value") %>'></asp:Literal>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="tbValue" Text='<%# Bind("Value") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbValue" ErrorMessage="*" ID="rfValidatorValue" EnableClientScript="False" ValidationGroup="EditStringDictionaryItemButtonValidationGroup"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

		                    <asp:templatefield HeaderText="<%$ Resources:SharedStrings, Options %>" HeaderStyle-Width="60" ItemStyle-HorizontalAlign="Center">
			                    <ItemTemplate>
				                    <asp:imagebutton id="ibEdit" runat="server" borderwidth="0" alternatetext='<%# RM.GetString("GENERAL_EDIT")%>' imageurl="~/Apps/MetaDataBase/images/edit.gif" commandname="Edit" causesvalidation="False" />
				                    &nbsp;&nbsp;
				                    <asp:imagebutton id="ibDelete" runat="server" borderwidth="0" alternatetext='<%# RM.GetString("GENERAL_DELETE")%>' imageurl="~/Apps/MetaDataBase/images/delete.gif" commandname="Delete" causesvalidation="False" />
			                    </ItemTemplate>
			                    <EditItemTemplate>
				                    <asp:imagebutton id="ibUpdate" runat="server" borderwidth="0" alternatetext='<%# RM.GetString("GENERAL_UPDATE")%>' imageurl="~/Apps/MetaDataBase/images/saveitem.gif" commandname="Update" causesvalidation="True" ValidationGroup="EditStringDictionaryItemButtonValidationGroup" />
				                    &nbsp;&nbsp;
				                    <asp:imagebutton id="ibCancel" runat="server" borderwidth="0" alternatetext='<%# RM.GetString("GENERAL_CANCEL")%>' imageurl="~/Apps/MetaDataBase/images/cancel.gif" commandname="Cancel" causesvalidation="False" ValidationGroup="EditStringDictionaryItemButtonValidationGroup" />
			                    </EditItemTemplate>
		                    </asp:templatefield>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br /><asp:Label CssClass="FormFieldDescription" id="MetaDescriptionCtrl" runat="server" Text="<%$ Resources:SharedStrings, Label %>"></asp:Label>
    </td>
</tr>
