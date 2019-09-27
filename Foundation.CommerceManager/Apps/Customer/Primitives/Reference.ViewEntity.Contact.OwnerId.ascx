<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Reference_ViewEntity_Contact_OwnerId" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.Commerce.Manager" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
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

			MetaClass mc = MetaDataWrapper.GetMetaClassByName(referencedClass);

			string script = String.Format(System.Globalization.CultureInfo.InvariantCulture,
				"CSManagementClient.ChangeBafView('{0}', 'View', 'ObjectId={1}')",
				referencedClass, obj.PrimaryKeyId);

            retVal = String.Format("<a href=\"javascript:{0}\">{1}</a>", script, HttpUtility.HtmlEncode(CHelper.GetResFileString(obj.Properties[mc.TitleFieldName].Value.ToString())));
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>
