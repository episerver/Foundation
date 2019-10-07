<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Guid_GridEntity_Organization" %>
<%@ Import Namespace="Mediachase.Web.Console.Common" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<script language="c#" runat="server">
    protected string GetValue(EntityObject DataItem, string FieldName)
    {
        string retVal = "";

        if (DataItem != null && DataItem.Properties[FieldName] != null && DataItem[FieldName] != null)
        {
            retVal = DataItem[FieldName].ToString();

            Guid contactId;
            if (Guid.TryParse(retVal, out contactId))
            {
                retVal = ManagementHelper.GetUserName(contactId);
            }            
        }

        return retVal;
    }
</script>
<%# GetValue(DataItem, FieldName) %>