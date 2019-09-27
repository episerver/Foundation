<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerLinkTemplate.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.GridTemplates.CustomerLinkTemplate" %>
<%@ Import Namespace="Mediachase.Web.Console.Common" %>
<%@ Import Namespace="Mediachase.Web.Console.Controls" %>
<asp:HyperLink ID="HyperLink1" runat="server" 
    NavigateUrl='<%# String.Format("javascript:CSManagementClient.ChangeBafView(\"Contact\",\"View\",\"ObjectId={0}\");",DataBinder.Eval(DataItem,"PrimaryKeyId")) %>' 
    Text='<%# EcfListView.GetDataCellValue(DataBinder.Eval(DataItem, "FullName")) %>'>
</asp:HyperLink>
