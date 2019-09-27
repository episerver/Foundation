<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Float_ViewEntity" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
			retVal = ((double)DataItem[FieldName]).ToString("f");
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>