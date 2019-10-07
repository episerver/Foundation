<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.UI.Web.Apps.MetaUI.EntityPrimitives.Duration_ViewEntity" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
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
			int hours, minutes;
			int value = 0;
			if (!String.IsNullOrEmpty(DataItem[FieldName].ToString()))
				value = Convert.ToInt32(DataItem[FieldName].ToString());
			
			hours = Convert.ToInt32(value / 60);
			minutes = value % 60;
			retVal = String.Format("{0:D2}:{1:D2}", hours, minutes);
						 
        }

		return String.Format("{0}", retVal);
    }
</script>
<div style="float: right;"><%# GetValue(DataItem, FieldName) %></div>