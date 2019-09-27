<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.UI.Web.Apps.MetaUI.EntityPrimitives.FloatPercent_GridEntity" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
    {
        string retVal = "";

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			double value = 0;
			value = Convert.ToDouble(DataItem[FieldName].ToString());
			if (DataItem.Properties[McDataTypeAttribute.DecimalPrecision] != null && DataItem.Properties[McDataTypeAttribute.DecimalScale] != null)
			{
				int totalCount = int.Parse(DataItem.Properties[McDataTypeAttribute.DecimalScale].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture) + int.Parse(DataItem.Properties[McDataTypeAttribute.DecimalPrecision].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
				retVal = value.ToString(String.Format("F0{0}", totalCount), System.Globalization.CultureInfo.CurrentCulture);
				retVal = String.Format("{0}%", retVal);
			}
			else
			{
				retVal = value.ToString("F02", System.Globalization.CultureInfo.CurrentCulture);
				retVal = String.Format("{0}%", retVal);
			}
		}
        
        return retVal;
    }
</script>
<div style="float: right;"><%# GetValue(DataItem, FieldName) %></div>