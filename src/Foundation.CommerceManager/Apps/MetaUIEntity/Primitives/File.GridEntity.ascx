<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.Primitives.File"%>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null) 
		{
			FileInfo fi = (FileInfo)DataItem[FieldName];

			retVal = String.Format(CultureInfo.InvariantCulture,
				"<a href='{0}?FileUID={1}'>{2}</a>",
				ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/DownloadFile.aspx"),
				fi.FileUID.ToString(),
				fi.Name);
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>