<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Reference_GridEntity" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.BusinessFoundation" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = "";

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			MetaField field = Mediachase.BusinessFoundation.MetaForm.FormController.GetMetaField(DataItem.MetaClassName, FieldName);
			string sReferencedClass = field.Attributes.GetValue<string>(McDataTypeAttribute.ReferenceMetaClassName);

			EntityObject obj = BusinessManager.Load(sReferencedClass, (PrimaryKeyId)DataItem[FieldName]);

			string script = CHelper.GetClientScriptEntityView(obj.MetaClassName, obj.PrimaryKeyId.Value.ToString());
			
			MetaClass mc = MetaDataWrapper.GetMetaClassByName(sReferencedClass);
			retVal = String.Format(System.Globalization.CultureInfo.InvariantCulture,
				"<a href=\"javascript:{0}\">{1}</a>", 
				script, 
				CHelper.GetResFileString(obj.Properties[mc.TitleFieldName].Value.ToString()));
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>