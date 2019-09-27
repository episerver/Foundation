<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Url_ViewEntity" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			string txt = CHelper.ParseText(DataItem[FieldName].ToString().Trim());
			string url = txt;
			if (!(url.IndexOf("://") >= 0 || url.IndexOf("\\\\") >= 0))
			{
				url = "http://" + url;
			}

			MetaField mf = MetaDataWrapper.GetMetaFieldByName(DataItem.MetaClassName, FieldName);
			if (mf.IsReferencedField)
			{
				string refClassName = mf.Attributes.GetValue<string>(McDataTypeAttribute.ReferenceMetaClassName);
				string refFieldName = mf.Attributes.GetValue<string>(McDataTypeAttribute.ReferencedFieldMetaFieldName);
				mf = MetaDataWrapper.GetMetaFieldByName(refClassName, refFieldName);
			}

			retVal = String.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" target=\"{1}\">{2}</a>", url, mf.Attributes.GetValue<string>(McDataTypeAttribute.StringUrlTarget), txt);
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>

