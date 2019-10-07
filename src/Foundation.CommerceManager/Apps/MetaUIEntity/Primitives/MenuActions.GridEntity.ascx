<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.CustomColumnBaseEntityType" ClassName="Mediachase.UI.Web.Apps.MetaUI.EntityPrimitives.MenuActions_GridEntity" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Hosting" %>
<%@ Import Namespace="System.Xml.XPath" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Services" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<%@ Import Namespace="Mediachase.BusinessFoundation" %>
<%@ Import Namespace="Mediachase.BusinessFoundation" %>
<%@ Import Namespace="Mediachase.BusinessFoundation" %>
<%@ Import Namespace="Mediachase.IbnNext.TimeTracking" %>
<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem)
	{
		if (DataItem != null)
			BindAction(DataItem);

		return string.Empty;
	}
	
	#region BindAction
	/// <summary>
	/// Binds the action.
	/// </summary>
	void BindAction(EntityObject DataItem)
	{
		string retVal = "var menu = new Ext.menu.Menu({ id: 'mainMenu', items: "; //[{text: 'main', menu: { items:
		string jsonVal = string.Empty;
		
		IXPathNavigable navigable = Mediachase.BusinessFoundation.XmlBuilder.GetXml(StructureType.ListViewUI, new Selector(DataItem.MetaClassName, this.ViewName, this.Place));
		XPathNavigator actions = navigable.CreateNavigator().SelectSingleNode("ListViewUI/Grid/CustomColumns");

		foreach (XPathNavigator gridItem in actions.SelectChildren("Column", string.Empty))
		{
			string type = gridItem.GetAttribute("type", string.Empty);
			string id = gridItem.GetAttribute("id", string.Empty);

			if (type == "MenuActions" && id == this.ColumnId)
			{
				foreach (XPathNavigator actionItem in gridItem.SelectChildren("Item", string.Empty))
				{
					string imageUrl = actionItem.GetAttribute("imageUrl", string.Empty);
					string commandName = actionItem.GetAttribute("commandName", string.Empty);
					string text = actionItem.GetAttribute("text", string.Empty);
					string tooltip = actionItem.GetAttribute("tooltip", string.Empty);

					text = CHelper.GetResFileString(text);
					Dictionary<string, string> dic = new Dictionary<string, string>();

					if (this.DataItem == null)
						dic.Add("primaryKeyId", "");
					else if (!DataItem.PrimaryKeyId.HasValue)
						dic.Add("primaryKeyId", "");
					else
						dic.Add("primaryKeyId", DataItem.PrimaryKeyId.ToString());

					CommandParameters cp = new CommandParameters(commandName, dic);
					bool isEnable = true;
					
					string clientScript = CommandManager.GetCurrent(this.Page).AddCommand(DataItem.MetaClassName, this.ViewName, this.Place, cp, out isEnable);
					clientScript = clientScript.Replace("'", "\"");
					clientScript = clientScript.Remove(clientScript.Length - 1);
					
					imageUrl = this.ResolveClientUrl(imageUrl);					
					
					if (isEnable)
						jsonVal += String.Format("{{ text: '{0}', icon: '{1}', handler: menuItems_OnClickHandler,  tooltip: '{3}', disabledClass: '{2}||{4}' }},", text, imageUrl, commandName, CHelper.GetResFileString(tooltip), cp.ToString());
					
					
				}
			}
		}

		if (jsonVal.Length > 0)
			jsonVal = jsonVal.Remove(jsonVal.Length - 1);

		jsonVal = String.Format("[{0}]", jsonVal);
		retVal += jsonVal + "});"; //}}]
		HtmlGenericControl divContainer = (HtmlGenericControl)this.FindControl("menuContainer");
		if (divContainer != null)
		{
			retVal += string.Format(" menu.show($get('{0}'));", divContainer.ClientID);
			divContainer.Attributes.Add("onclick", retVal);
		}
		//return retVal;
	}
	#endregion	
</script>
<div runat="server" id="menuContainer" class="x-btn-menu-arrow-wrap">test</div>
<%# GetValue(DataItem) %>