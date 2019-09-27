<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Decimal_GridEntity" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
			retVal = ((System.Decimal)DataItem[FieldName]).ToString();
		return retVal;
	}
</script>
<div style="float: right;"><%# GetValue(DataItem, FieldName) %></div>