<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.UI.Web.Apps.MetaUI.EntityPrimitives.IntegerPercent_GridEntity" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
    {
		string retVal = "";

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			int value = 0;
			if (DataItem[FieldName].ToString() != string.Empty)
				value = Convert.ToInt32(DataItem[FieldName].ToString());
			retVal = String.Format("{0}%", value);
		}

		return String.Format("{0}", retVal);
    }
</script>
<div style="float: right;"><%# GetValue(DataItem, FieldName) %></div>