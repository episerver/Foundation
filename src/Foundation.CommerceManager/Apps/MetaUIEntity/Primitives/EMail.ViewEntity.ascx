<%@ Control Language="C#" AutoEventWireup="true"  Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.EMail_ViewEntity" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && 
			DataItem[FieldName] != null)
		{
			string str = CHelper.ParseText(DataItem[FieldName].ToString());
			retVal = String.Format("<a href='mailto:{0}'>{0}</a>", str);
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>