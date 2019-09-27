<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Shipping.Plugins.WeightJurisdiction.ConfigureShippingMethod" Codebehind="ConfigureShippingMethod.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" RenderMode="Inline">
    <ContentTemplate>
<table width="600" id="GenericTable" runat="server">
	<tr>
		<td class="FormFieldCell" colspan="2"><b><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:OrderStrings, Jurisdiction_Group_Configure_Price_Per_Weight %>"/></b></td>
	</tr>
	<tr>
	    <td class="FormLabelCell" style="vertical-align: middle; width: 100px">
            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Jurisdiction_Group %>"></asp:Label>:
        </td>
        <td class="FormFieldCell" align="left">
            <%--<asp:UpdatePanel UpdateMode="Conditional" ID="upJurisdictionGroupList" runat="server" RenderMode="Inline">
                <ContentTemplate>--%>
                    <asp:DropDownList runat="server" id="JurisdictionGroupList" DataValueField="JurisdictionGroupId" DataTextField="DisplayName" AutoPostBack="true" Width="100px" ValidationGroup="AddWeightValidationGroup">
                        <asp:ListItem Value="" Text="<%$ Resources:OrderStrings, select_jurisdiction_group %>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="JurisdictionGroupRequired" ControlToValidate="JurisdictionGroupList" ErrorMessage="*" Display="Dynamic" ValidationGroup="AddWeightValidationGroup"></asp:RequiredFieldValidator>
               <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </td>
    </tr>
    <tr>
        <td class="FormLabelCell" style="vertical-align: middle; width: 100px">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Weight %>"></asp:Label>:
        </td>
        <td class="FormLabelCell" align="left">
	        <asp:TextBox runat="server" Width="100px" ID="Weight"></asp:TextBox>&nbsp;<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, or_more %>"/>&nbsp;&nbsp;&nbsp;
            <asp:RequiredFieldValidator runat="server" ID="WeightRequired" ControlToValidate="Weight" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Weight_Required %>" ValidationGroup="AddWeightValidationGroup" />
            <asp:RangeValidator runat="server" ID="WeightRange" ControlToValidate="Weight" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Enter_Valid_Value %>" ValidationGroup="AddWeightValidationGroup"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td class="FormLabelCell" style="vertical-align: middle; width: 100px">
            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Price %>"></asp:Label>:
        </td>
        <td class="FormLabelCell" align="left">
	        <asp:TextBox Runat="server" ID="Price" Width="100px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
	        <asp:requiredfieldvalidator id="PriceRequired" runat="server" ErrorMessage="*" Font-Size="9pt" Font-Name="verdana"
		        Display="dynamic" ControlToValidate="Price" EnableClientScript="False" ValidationGroup="AddWeightValidationGroup"></asp:requiredfieldvalidator>
	        <asp:RangeValidator ErrorMessage="*" Runat="server" ControlToValidate="Price" Type="Currency" MinimumValue="0"
		        MaximumValue="100000000" id="PriceRange" EnableClientScript="False" ValidationGroup="AddWeightValidationGroup"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td class="FormLabelCell" style="vertical-align: middle; width: 100px">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:SharedStrings, Start_Date %>"></asp:Label>:
        </td> 
        <td class="FormFieldCell" align="left">
            <ecf:CalendarDatePicker runat="server" ID="StartDate" ValidationGroup="AddWeightValidationGroup" />
        </td> 
    </tr>
    <tr>
        <td class="FormLabelCell" style="vertical-align: middle; width: 100px">
            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, End_Date %>"></asp:Label>:
        </td> 
        <td class="FormFieldCell" align="left">
            <ecf:CalendarDatePicker runat="server" ID="EndDate" ValidationGroup="AddWeightValidationGroup" />
        </td> 
    </tr>
    <tr>
        <td class="FormLabelCell" colspan="2" align="left">
            <asp:Button Runat="server" Text="<%$ Resources:SharedStrings, Add %>" ID="AddWeightButton" onclick="AddWeightButton_Click" ValidationGroup="AddWeightValidationGroup"></asp:Button>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td class="FormLabelCell" colspan="2">
	        <asp:datagrid id="ItemsGrid" runat="server" CssClass="Grid" AllowPaging="False" Width="800px" 
		        ShowFooter="True" DataKeyField="ShippingMethodCaseId" AutoGenerateColumns="False">
		        <ItemStyle CssClass="GridItem"></ItemStyle>
		        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
		        <%--<FooterStyle CssClass="GridFooter"></FooterStyle>--%>
		        <Columns>
		            <asp:templatecolumn headertext="<%$ Resources:SharedStrings, Group_Name %>" HeaderStyle-Width="250px">
				        <itemtemplate>
					        <%# JurisdictionGroupList.SelectedItem.Text %>
				        </itemtemplate>
				        <EditItemTemplate>
				            <%# JurisdictionGroupList.SelectedItem.Text %>
				        </EditItemTemplate>
			        </asp:templatecolumn>
			        <asp:templatecolumn headertext="<%$ Resources:SharedStrings, Weight %>" HeaderStyle-Width="200px">
				        <itemtemplate>
					        <%# DataBinder.Eval(Container.DataItem, "Total")%>&nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, or_more %>"/>
				        </itemtemplate>
				        <EditItemTemplate>
				            <asp:TextBox runat="server" ID="RowTotal" Text='<%# DataBinder.Eval(Container.DataItem, "Total")%>' Width="100px"></asp:TextBox>&nbsp;<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:SharedStrings, or_more %>"/>
				        </EditItemTemplate>
			        </asp:templatecolumn>
			        <asp:templatecolumn headertext="<%$ Resources:SharedStrings, Price %>" HeaderStyle-Width="100px">
				        <itemtemplate>
					        <%# DataBinder.Eval(Container.DataItem, "Charge")%>
				        </itemtemplate>
				        <EditItemTemplate>
				            <asp:TextBox runat="server" ID="RowPrice" Text='<%# DataBinder.Eval(Container.DataItem, "Charge")%>'>
					        </asp:TextBox>
				        </EditItemTemplate>
			        </asp:templatecolumn>
			        <asp:templatecolumn headertext="<%$ Resources:SharedStrings, Start_Date %>" HeaderStyle-Width="200px">
				        <itemtemplate>
				            <%# DataBinder.Eval(Container.DataItem, "StartDate") %>
				        </itemtemplate>
				        <EditItemTemplate>
				            <ecf:CalendarDatePicker runat="server" ID="StartDate" Value='<%# (DateTime)DataBinder.Eval(Container.DataItem, "StartDate")%>' Width="180px" />
				        </EditItemTemplate>
			        </asp:templatecolumn>
			        <asp:templatecolumn headertext="<%$ Resources:SharedStrings, End_Date %>" HeaderStyle-Width="200px">
				        <itemtemplate>
					        <%# DataBinder.Eval(Container.DataItem, "EndDate")%>
				        </itemtemplate>
				        <EditItemTemplate>
				            <ecf:CalendarDatePicker runat="server" ID="EndDate" Value='<%# (DateTime)DataBinder.Eval(Container.DataItem, "EndDate")%>' />
				        </EditItemTemplate>
			        </asp:templatecolumn>
			        <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="<%$ Resources:SharedStrings, Update %>" CancelText="<%$ Resources:SharedStrings, Cancel %>" EditText="<%$ Resources:SharedStrings, Modify %>"
				        ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:SharedStrings, Edit %>" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center"></asp:EditCommandColumn>
			        <asp:ButtonColumn Text="<%$ Resources:SharedStrings, Delete %>" CommandName="Delete" HeaderStyle-Width="50"></asp:ButtonColumn>
		        </Columns>
	        </asp:datagrid>
        </td>
    </tr>
</table>
    </ContentTemplate>
</asp:UpdatePanel>