<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaDataViewControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Core.MetaData.Admin.MetaDataViewControl" %>
<asp:ListView runat="server" ID="FieldList" DataKeyNames="MetaFieldName">
    <EmptyDataTemplate>
        <p>
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, No_Items %>"/>
        </p>
    </EmptyDataTemplate>
    <EmptyItemTemplate>
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, No_Items %>"/>
    </EmptyItemTemplate>
    <LayoutTemplate>
        <table runat="server" id="mainTable">
            <tr id="itemPlaceholder" runat="server" />
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
	        <td class='<%# LabelCssClass %>' width="20%">
	            <%# String.Format("{0} ({1})", Eval("MetaFieldFriendlyName"), Eval("LanguageCode"))%>:
            </td>
            <td class='<%# ValueCssClass %>'>
                <asp:PlaceHolder runat="server" ID="phValue"></asp:PlaceHolder>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>