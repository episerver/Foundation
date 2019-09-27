<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderMetaData.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.OrderMetaData" %>
<%@ Register Src="~/Apps/Core/MetaData/Admin/MetaDataViewControl.ascx" TagName="MetaDataView" TagPrefix="ecf" %>
<div>
    <ecf:MetaDataView runat="server" ID="MetaDataTab" TableCssClass="orderform-innertable" LabelCssClass="orderform-label" ValueCssClass="orderform-field" />
    <div style="padding-left:200px;">
    <br />
    <br />
    <button id="btnEditMetaData" runat="server" style="width:150px;">
        <asp:Literal ID="litEditText" runat="server"  />
    </button>
    </div>
</div>
