<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Admin.MetaFieldEditControl"
    CodeBehind="MetaFieldEditControl.ascx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<div id="FormMultiPage">
    <table class="FormMultiPage">
        <tr>
            <td class="FormLabelCell">
                <table>
                    <tr id="trError" runat="server">
                        <td class="FormLabelCell" colspan="2">
                            <asp:Label runat="server" ID="lblErrorMessage" CssClass="text" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                            <%=RM.GetString("ATTRIBUTEEDIT_NAME") %>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:TextBox runat="server" ID="tbName" CssClass="text" Width="200px" MaxLength="256"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Name_Required %>"
                                Font-Size="9pt" Font-Name="verdana" Display="Dynamic" ControlToValidate="tbName" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbName" Display="Dynamic"
                                ValidationExpression="^[a-z,A-Z,_,\d]*$" ID="RegularExpressionValidator1" ErrorMessage="<%$ Resources:SharedStrings, Latin_Symbols_Only %>" />
                            <asp:CustomValidator runat="server" ID="NameUniqueCustomValidator" ControlToValidate="tbName" OnServerValidate="NameCheck" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, MetaField_With_Name_AlreadyExists %>" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                            <%=RM.GetString("ATTRIBUTEEDIT_FRIENDLY_NAME") %>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:TextBox runat="server" ID="tbFriendlyName" CssClass="text" Width="200px" MaxLength="256"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:SharedStrings, Friendly_Name_Required %>"
                                Font-Size="9pt" Font-Name="verdana" Display="Dynamic" ControlToValidate="tbFriendlyName" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                            <%=RM.GetString("ATTRIBUTEEDIT_DESCRIPTION") %>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:TextBox runat="server" ID="tbDescription" Width="350px" Rows="4" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="FormSpacerCell">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabelCell" style="vertical-align: middle; width: 120px">
                            <%=RM.GetString("ATTRIBUTEEDIT_TYPE") %>:
                        </td>
                        <td class="FormFieldCell">
                            <asp:DropDownList runat="server" ID="ddlType" Width="200" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkMultiLanguage"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkMultiline"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkEditable"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkUseInComparing"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="FormFieldCell">
	                        <asp:CheckBox Runat="server" ID="chkAllowNulls"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="FormFieldCell">
                            <asp:CheckBox runat="server" ID="chkIsEncrypted"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr runat="server" id="trSearchProperties">
                        <td class="FormLabelCell" valign="top"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, MetaData_Search_Properties %>"/>:</td>
                        <td class="FormFieldCell">
                            <table>
                                <tr id="trAllowSearch" runat="server">
                                    <td class="FormFieldCell" colspan="2"><asp:CheckBox runat="server" ID="chkAllowSearch"></asp:CheckBox></td>
                                </tr>
                                <tr id="trIndexSortable" runat="server">
                                    <td class="FormFieldCell" colspan="2"><asp:CheckBox runat="server" ID="chkIndexSortable"></asp:CheckBox></td>
                                </tr>
                                <tr id="trIndexStored" runat="server">
                                    <td class="FormFieldCell" colspan="2"><asp:CheckBox runat="server" ID="chkIndexStored"></asp:CheckBox></td>
                                </tr>
                                <tr id="trIndexTokenized" runat="server">
                                    <td class="FormFieldCell" colspan="2"><asp:CheckBox runat="server" ID="chkIndexTokenized"></asp:CheckBox></td>
                                </tr>
                                <tr id="trIndexIncludeInDefaultSearch" runat="server">
                                    <td class="FormFieldCell" colspan="2"><asp:CheckBox runat="server" ID="chkIndexIncludeInDefault"></asp:CheckBox></td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                    <tr runat="server" id="DecimalProperties" visible="false">
                        <td class="FormLabelCell" valign="top">
                            <%=RM.GetString("ATTRIBUTEEDIT_DECIMAL_PROPERTIES") %>
                            :
                        </td>
                        <td class="FormLabelCell">
                            <table>
                                <tr>
                                    <td>
                                        <%=RM.GetString("ATTRIBUTEEDIT_PRECISION") %>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="30" Text="18" ID="tbPrecision"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPrecision" runat="server" ControlToValidate="tbPrecision"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidatorPrecision" runat="server" ControlToValidate="tbPrecision"
                                            Display="Dynamic" MinimumValue="1" MaximumValue="38" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                                        <asp:CustomValidator runat="server" ID="CustomValidatiorPrecision" ErrorMessage="<%$ Resources:CoreStrings, MetaData_Precision_LessThanOrEqual_38 %>"
                                            ControlToValidate="tbPrecision" OnServerValidate="PrecisionValidate" ValidationGroup="Precision"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%=RM.GetString("ATTRIBUTEEDIT_SCALE") %>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="30" Text="0" ID="tbScale"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorScale" runat="server" ControlToValidate="tbScale"
                                            ErrorMessage="*" Display="Dynamic" ValidationGroup="Scale"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidatorScale" runat="server" ControlToValidate="tbScale"
                                            Display="Dynamic" MinimumValue="0" MaximumValue="38" Type="Integer" ErrorMessage="*"
                                            ValidationGroup="Scale"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="ImageProperties" visible="false">
                        <td class="FormLabelCell" valign="top">
                            <%=RM.GetString("ATTRIBUTEEDIT_IMAGE_PROPERTIES") %>
                            :
                        </td>
                        <td class="FormFieldCell">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkAutoResizeImage" AutoPostBack="true" OnCheckedChanged="AutoResizeImage_Changed"
                                            Text="<%$ Resources:CoreStrings, MetaData_Auto_Resize_Image %>"></asp:CheckBox>
                                        <div style="padding: 10px;" id="AutoResizeImageProperties" runat="server">
                                            <%=RM.GetString("ATTRIBUTEEDIT_NEW_SIZE") %>
                                            :
                                            <asp:TextBox runat="server" Width="30" Text="220" ID="ImageWidth"></asp:TextBox><asp:RequiredFieldValidator
                                                runat="server" ControlToValidate="ImageWidth" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator
                                                    runat="server" ControlToValidate="ImageWidth" Display="Dynamic" MinimumValue="0"
                                                    MaximumValue="6000" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                                            x
                                            <asp:TextBox runat="server" Width="30" Text="300" ID="ImageHeight"></asp:TextBox>
                                            <%= RM.GetString("ATTRIBUTEEDIT_WIDTH_HEIGHT")%>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ImageHeight" ErrorMessage="*"
                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator runat="server" ControlToValidate="ImageHeight" Display="Dynamic"
                                                MinimumValue="0" MaximumValue="6000" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                                            <asp:CheckBox runat="server" ID="chkStretchImage" Text="<%$ Resources:CoreStrings, MetaData_Stretch_Image %>"></asp:CheckBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkAutoGenerateThumbnail" AutoPostBack="true" OnCheckedChanged="AutoResizeImage_Changed"
                                            Text="<%$ Resources:CoreStrings, MetaData_Auto_Create_Thumbnail_Image %>"></asp:CheckBox>
                                        <div style="padding: 10px;" id="AutoGenerateThumbnailProperties" runat="server">
                                            <%=RM.GetString("ATTRIBUTEEDIT_NEW_SIZE") %>
                                            :
                                            <asp:TextBox runat="server" Width="30" Text="50" ID="ThumbnailWidth"></asp:TextBox><asp:RequiredFieldValidator
                                                runat="server" ControlToValidate="ThumbnailWidth" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator
                                                    runat="server" ControlToValidate="ThumbnailWidth" Display="Dynamic" MinimumValue="0"
                                                    MaximumValue="6000" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                                            x
                                            <asp:TextBox runat="server" Width="30" Text="50" ID="ThumbnailHeight"></asp:TextBox>
                                            <%= RM.GetString("ATTRIBUTEEDIT_WIDTH_HEIGHT")%>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ThumbnailHeight" ErrorMessage="*"
                                                Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator runat="server"
                                                    ControlToValidate="ThumbnailHeight" Display="Dynamic" MinimumValue="0" MaximumValue="6000"
                                                    Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                                            <nobr><asp:CheckBox Runat="server" ID="chkStretchThumbnail" Text="<%$ Resources:CoreStrings, MetaData_Stretch_Thumbnail_Image %>"></asp:CheckBox></nobr>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!--
	<tr>
		<td></td>
		<td class="FormLabelCell">
			<asp:CheckBox Runat="server" ID="chkClientOption"></asp:CheckBox>
		</td>
	</tr>
	-->
                    <tr runat="server" id="DictionaryValues" visible="false">
                        <td class="FormLabelCell" colspan="2">
                            <fieldset>
                                <legend align="top" id="Legend1" class="text">
                                        <%=RM.GetString("ATTRIBUTEEDIT_DICTIONARY_VALUES")%>
                                </legend>
                                <asp:UpdatePanel UpdateMode="Conditional" ID="DictionaryItemsUpdatePanel" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <table>
                                            <tr id="AddValueRow" runat="server">
                                                <td class="FormLabelCell" style="vertical-align: middle">
                                                    <asp:Label ID="MetaLabelCtrl" runat="server"><%=RM.GetString("ATTRIBUTEEDIT_NEW_VALUE")%></asp:Label>
                                                </td>
                                                <td class="FormLabelCell">
                                                    <asp:TextBox ID="MetaValueCtrl" runat="server"></asp:TextBox>
                                                    <asp:Button Text="<%$ Resources:CoreStrings, MetaData_Add_Value %>" runat="server" ID="AddValue" CausesValidation="False"
                                                        OnClick="AddValue_Click"></asp:Button>
                                                    <br>
                                                </td>
                                            </tr>
                                            <tr id="AllValuesRow" runat="server">
                                                <td class="FormLabelCell" style="vertical-align: middle">
                                                    <asp:Label ID="AllValuesLabel" runat="server"><%=RM.GetString("ATTRIBUTEEDIT_ALL_VALUES")%></asp:Label>
                                                </td>
                                                <td class="FormLabelCell">
                                                    <asp:DropDownList ID="DicSingleValueCtrl" runat="server" Width="250px" AutoPostBack="false"></asp:DropDownList>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButtonRemove" runat="server" CausesValidation="False" OnClick="LinkButtonRemove_Click" Text="<%$ Resources:SharedStrings, Remove %>"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="uProgress" runat="server" AssociatedUpdatePanelID="DictionaryItemsUpdatePanel" DisplayAfter="500">
				                    <ProgressTemplate>
					                    <div class="upProgressMain">
						                    <div class="upProgressCenter">
							                    <img align='absmiddle' border='0' src='<%# this.ResolveUrl("~/Apps/Shell/Styles/images/Shell/loading_rss.gif") %>' />&nbsp;Loading...
						                    </div>
					                    </div>
				                    </ProgressTemplate>
			                    </asp:UpdateProgress>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<ecf:SaveControl ID="EditSaveControl" ShowDeleteButton="false" SavedClientScript="CSManagementClient.OpenTran('List', 'fieldnamespace='+CSManagementClient.QueryString('fieldnamespace'));"
    DeleteClientScript="CSManagementClient.OpenTran('List', 'fieldnamespace='+CSManagementClient.QueryString('fieldnamespace'));"
    CancelClientScript="CSManagementClient.OpenTran('List', 'fieldnamespace='+CSManagementClient.QueryString('fieldnamespace'));"
    runat="server"></ecf:SaveControl>