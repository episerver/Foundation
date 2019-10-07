<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.CatalogCSVMappingTab" Codebehind="CatalogCSVMappingTab.ascx.cs" %>
<%@ Register Src="../../Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl" TagPrefix="core" %>
<script type="text/javascript">
	function ddlOnChange(o, val, cid) {
		$get(cid).style.display = (o.value == val) ? 'block' : 'none';
	}
	
	function adjustDivs() {
		var df = document.getElementById('<%=UpdateProgress1.ClientID%>');
		dfs = df.style;
		dfs.position = 'absolute' ;

		if (window.innerWidth) {
			dfs.left = (window.innerWidth / 2 + document.body.scrollLeft - 27);
			dfs.top = (window.innerHeight / 2 + document.body.scrollTop - 27);
		}
		else {
			dfs.left = (document.documentElement.offsetWidth / 2 + document.body.scrollLeft - 27);
			dfs.top = (document.documentElement.offsetHeight / 2 + document.body.scrollTop - 27);
		}
	}

	window.onload = adjustDivs;
	window.onresize = adjustDivs;
	window.onscroll = adjustDivs;
</script>

<asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="0" runat="server">
	<ProgressTemplate>
		<img alt="" style="z-index: 100003; border: 0" src='<%= ResolveClientUrl("~/Apps/Shell/Styles/images/Shell/loading_rss.gif") %>' />
	</ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
<ContentTemplate>
	<div id="DataForm">
		<table>
		  <tr>
			<td colspan="2" class="FormSectionCell">
			   <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Edit_Existing_Mapping_File %>" />
			</td>
		  </tr> 
		  <tr>
			<td class="FormLabelCell">
			  <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Load_Mapping_File %>" />
			</td>
			<td class="FormFieldCell">  
				<asp:DropDownList runat="server" ID="ddlMappingFiles" Width="200">
				</asp:DropDownList>
			  <asp:Button runat="server" ID="btnLoadMappingFile" Text="<%$ Resources:SharedStrings, Load_Ellipsis %>" style="vertical-align:top;" OnClick="btnLoadMapFile_Click" />  
			</td>
		  </tr>
		</table>
		<br />
		<table width="100%">
		  <col width="30%" />
		  <col width="30%" />
		  <col width="30%" />
		  <tr>
			<td valign="top">
			  <table cellpadding="4" width="100%">
				<tr>
					<td colspan="3" class="FormSectionCell">
						<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_MetaClass_Language %>" />
					</td>
				</tr> 
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Mapping_Type %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
					  <asp:DropDownList runat="server" ID="ddlTypeData" AutoPostBack="true" EnableViewState="true" Width="200" OnSelectedIndexChanged="ddlMappingType_SelectedIndexChanged"/>
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:CatalogStrings, Meta_Class %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
					  <asp:DropDownList runat="server" ID="ddlMetaClass" AutoPostBack="true" EnableViewState="true" Width="200" DataTextField="FriendlyName" DataValueField="Id" OnSelectedIndexChanged="ddlMetaClassList_SelectedIndexChanged"/>
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SharedStrings, Language %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
						<asp:DropDownList runat="server" ID="ddlLanguage" Width="200" DataValueField="LanguageId" AutoPostBack="true" DataTextField="Name" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
						</asp:DropDownList>
					</td>
				</tr>
			  </table>
		  </td>
		  <td>
			<table cellpadding="4" width="100%">
				<tr>
					<td colspan="3" class="FormSectionCell">
						<asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Data_File_CSV_Adjustment %>" />
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Data_File %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
						<asp:UpdatePanel runat="server" ID="upCatalogCSVMappingDataFiles" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:DropDownList runat="server" ID="ddlDataFiles" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlFields_SelectedIndexChanged">
								</asp:DropDownList>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:SharedStrings, Delimiter %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
						<asp:DropDownList style="width:90px;" CssClass="text" Runat="server" ID="ddlDelimiter" AutoPostBack=True OnSelectedIndexChanged="ddlFields_SelectedIndexChanged">
							<asp:ListItem Value=";">;</asp:ListItem>
							<asp:ListItem Selected=True Value=",">,</asp:ListItem>
							<asp:ListItem Value=" " Text="<%$ Resources:SharedStrings, Space %>"></asp:ListItem>
							<asp:ListItem Value="	" Text="<%$ Resources:SharedStrings, Tab %>"></asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Text_Qualifier %>" />:
					</td>
					<td class="FormFieldCell" colspan="2">
						<asp:DropDownList style="width:90px;" CssClass="text" Runat="server" ID="ddlTextQualifier" AutoPostBack=True OnSelectedIndexChanged="ddlFields_SelectedIndexChanged">
										  <asp:ListItem Value="" Text="<%$ Resources:SharedStrings, None_Parenthesized %>" ></asp:ListItem>
										  <asp:ListItem Selected=True Value='"'>"</asp:ListItem>
										  <asp:ListItem Value="'">'</asp:ListItem>
									  </asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="FormLabelCell">
						<asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:SharedStrings, Encoding %>" />:
					</td>
					<td class="FormFieldCell">
						<asp:DropDownList style="width:90px;" CssClass="text" Runat="server" ID="ddlEncoding" AutoPostBack=True OnSelectedIndexChanged="ddlFields_SelectedIndexChanged">
						  <asp:ListItem Selected=True Value="Default" Text="<%$ Resources:SharedStrings, Default %>"></asp:ListItem>
						  <asp:ListItem Value="ASCII" Text="<%$ Resources:SharedStrings, ASCII %>"></asp:ListItem>
						  <asp:ListItem Value="UTF-8" Text="<%$ Resources:SharedStrings, UTF8 %>"></asp:ListItem>
						  <asp:ListItem Value="Unicode" Text="<%$ Resources:SharedStrings, Unicode %>"></asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			  </table>
			</td>
			<td>
			</td>
		  </tr>
		 </table>
		 <br />
		<asp:DataGrid id="grdFields" Runat="server" AutoGenerateColumns="False" GridLines="None" Width="650">
			<Columns>
				<asp:TemplateColumn HeaderText="" HeaderStyle-CssClass="FormSectionCell">
					<ItemStyle Width=200 CssClass="FormLabelCell"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"name") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="" HeaderStyle-CssClass="FormSectionCell">
					<ItemStyle Width=250 HorizontalAlign="Center" CssClass="FormFieldCell"></ItemStyle>
					<ItemTemplate>
						<asp:DropDownList style="width:240px;" ID="ddlFields" Runat="server"></asp:DropDownList>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderStyle-CssClass="FormSectionCell">
					<ItemStyle Width=200 CssClass="FormFieldCell"></ItemStyle>
					<ItemTemplate>
						<asp:TextBox CssClass="text" style="display:none; padding: 0px;" Width="190" ID="tbCustomValue" Runat="server"></asp:TextBox>
						<asp:DropDownList CssClass="text" style="display:none; padding: 0px;" Width="196" ID="ddlValues" Runat="server"></asp:DropDownList>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="key" Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="Type" Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="IsSystemDictionary" Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="AllowNulls" Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="IsConstant" Visible="False"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
		<br />
		<table>
		  <tr>
			<td colspan="2" class="FormSectionCell">
			   <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Save_Mapping_File %>" />
			</td>
		  </tr> 
		  <tr>
			<td class="FormLabelCell">
			  <asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Enter_File_Name %>" />
			</td>
			<td class="FormFieldCell">  
			  <span><asp:TextBox CssClass="Text" Runat="server" Width="270" ID="tbMappingFileName"/>.xml</span>
			  <asp:Button CssClass="text" ID="SaveMapping" runat="server" Text='Save' Width="100" OnClick="SaveMapping_Click"></asp:Button>
			</td>
		  </tr>
		</table>
	</div>
	</ContentTemplate>
</asp:UpdatePanel>