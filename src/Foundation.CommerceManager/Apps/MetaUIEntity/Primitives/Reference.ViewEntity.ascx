<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Reference_ViewEntity" %>
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
			MetaClass mcOwn = MetaDataWrapper.GetMetaClassByName(DataItem.MetaClassName);
			MetaField field = MetaDataWrapper.GetMetaFieldByName(mcOwn, FieldName);
			if (field == null)
				field = MetaDataWrapper.GetMetaFieldByName(mcOwn.CardOwner, FieldName);

			if (field == null)
				return retVal;
			
			string referencedClass = field.Attributes.GetValue<string>(McDataTypeAttribute.ReferenceMetaClassName);

			EntityObject obj = BusinessManager.Load(referencedClass, (PrimaryKeyId)DataItem[FieldName]);

			string script = CHelper.GetClientScriptEntityView(obj.MetaClassName, obj.PrimaryKeyId.ToString());

			MetaClass mc = MetaDataWrapper.GetMetaClassByName(referencedClass);
			retVal = String.Format(System.Globalization.CultureInfo.InvariantCulture,
				"<a href=\"javascript:{0}\">{1}</a>",
                script, HttpUtility.HtmlEncode(CHelper.GetResFileString(obj.Properties[mc.TitleFieldName].Value.ToString())));
		}
		return retVal;
	}
</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
	 	<td valign='top'>
		  <%# GetValue(DataItem, FieldName) %>
		</td>
	</tr>
</table>