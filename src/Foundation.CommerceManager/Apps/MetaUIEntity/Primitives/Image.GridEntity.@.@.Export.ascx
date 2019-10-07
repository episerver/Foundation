<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Image_GridEntity_All_All_Export"%>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null) 
		{
			FileInfo fi = (FileInfo)DataItem[FieldName];

			retVal = fi.Name;
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>