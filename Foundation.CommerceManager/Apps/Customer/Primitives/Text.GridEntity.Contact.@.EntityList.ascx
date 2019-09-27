<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Text_GridEntity_Contact" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.Commerce.Manager" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retval = String.Empty;
		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
		{
			retval = GetLinkForTitleField(DataItem, FieldName, HttpUtility.HtmlEncode(DataItem[FieldName].ToString()));
		}
		return retval;
	}

	private string GetLinkForTitleField(EntityObject mo, string fieldName, string text)
	{
		string retval = text;
		MetaClass mc = MetaDataWrapper.GetMetaClassByName(mo.MetaClassName);
		
		if (mc.TitleFieldName == fieldName || (mc.CardOwner != null && mc.CardOwner.TitleFieldName == fieldName))
		{
			if (mc.CardOwner != null && mc.CardOwner.TitleFieldName == fieldName)
				mc = mc.CardOwner;

			string script = String.Format(System.Globalization.CultureInfo.InvariantCulture,
				"CSManagementClient.ChangeBafView('{0}', 'View', 'ObjectId={1}')",
				mc.Name, mo.PrimaryKeyId);
			
			retval = String.Format(CultureInfo.InvariantCulture, "<a href=\"javascript:{0}\">{1}</a>", script, text);
		}
		return retval;
	}
</script>
<%# GetValue(DataItem, FieldName) %>