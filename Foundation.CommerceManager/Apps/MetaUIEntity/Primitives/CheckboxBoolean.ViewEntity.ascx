<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.CheckboxBoolean_ViewEntity" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<script runat="server" language="c#">
	public string GetValue()
	{
		string retval = string.Empty;
		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			if ((bool)DataItem[FieldName])
			{
				MetaField mf = MetaDataWrapper.GetMetaFieldByName(DataItem.MetaClassName, FieldName);
				if (mf.IsReferencedField)
				{
					string refClassName = mf.Attributes.GetValue<string>(McDataTypeAttribute.ReferenceMetaClassName);
					string refFieldName = mf.Attributes.GetValue<string>(McDataTypeAttribute.ReferencedFieldMetaFieldName);
					mf = MetaDataWrapper.GetMetaFieldByName(refClassName, refFieldName);
				}
				retval = CHelper.GetResFileString(mf.Attributes.GetValue<string>(McDataTypeAttribute.BooleanLabel));
			}
		}
		return retval;
	}
</script>
<%# GetValue() %>
