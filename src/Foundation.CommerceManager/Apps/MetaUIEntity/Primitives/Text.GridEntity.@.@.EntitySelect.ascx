<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUIEntity.Primitives.Text_GridEntity_Any_Any_EntitySelect" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retval = String.Empty;
		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
			retval = HttpUtility.HtmlEncode(DataItem[FieldName].ToString());
		return String.Format("<div>{0}</div>", retval);
	}
</script>
<%# GetValue(DataItem, FieldName) %>