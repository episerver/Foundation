<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Text_GridEntity_Principal" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retval = String.Empty;
		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			retval = DataItem[FieldName].ToString();
		}
		return retval;
	}
</script>
<%# GetValue(DataItem, FieldName) %>