<%@ Control Language="C#" AutoEventWireup="true" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Text_GridEntity" Inherits="Mediachase.BusinessFoundation.BaseEntityType" %>
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
		{
			retval = GetLinkForTitleField(DataItem, FieldName, DataItem[FieldName].ToString());
		}
		return retval;
	}

	private string GetLinkForTitleField(EntityObject mo, string fieldName, string text)
	{
		string retval = text;
		MetaClass mc = MetaDataWrapper.GetMetaClassByName(mo.MetaClassName);
		if (mc.TitleFieldName == fieldName || (mc.CardOwner != null && mc.CardOwner.TitleFieldName == fieldName))
		{
			string script = CHelper.GetClientScriptEntityView(mc.Name, mo.PrimaryKeyId.ToString());

			retval = String.Format(CultureInfo.InvariantCulture,
				"<a href=\"javascript:{0}\">{1}</a>",
                script, HttpUtility.HtmlEncode(text));
		}
		return retval;
	}
</script>
<%# GetValue(DataItem, FieldName) %>