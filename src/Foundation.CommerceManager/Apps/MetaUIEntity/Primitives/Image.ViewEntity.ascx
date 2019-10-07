<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Image_ViewEntity" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem, string FieldName)
	{
		string retVal = String.Empty;

		if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null) 
		{
			FileInfo fi = (FileInfo)DataItem[FieldName];
			MetaField mf = MetaDataWrapper.GetMetaFieldByName(DataItem.MetaClassName, FieldName);

			string border = String.Empty;
			if (mf.Attributes.GetValue<bool>(McDataTypeAttribute.ImageShowBorder))
				border = " border=1";

			retVal = String.Format(CultureInfo.InvariantCulture,
				"<img src='{0}?FileUID={1}&mode=image'{2}/>",
				ResolveClientUrl("~/Apps/MetaDataBase/MetaUI/Pages/Public/DownloadFile.aspx"),
				fi.FileUID.ToString(), border);
		}
		return retVal;
	}
</script>
<%# GetValue(DataItem, FieldName) %>
