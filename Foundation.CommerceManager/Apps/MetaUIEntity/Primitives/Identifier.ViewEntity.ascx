<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.BaseEntityType" ClassName="Mediachase.Ibn.Web.UI.MetaUI.EntityPrimitives.Identifier_ViewEntity" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
	  	<td>
		  <%# (DataItem == null || DataItem.Properties[FieldName] == null || DataItem[FieldName] == null) ? "" : DataItem[FieldName].ToString()%>
		</td>
	</tr>
</table>