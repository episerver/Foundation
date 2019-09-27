<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.BusinessFoundation.CustomColumnBaseEntityType" ClassName="Mediachase.UI.Web.Apps.MetaUI.EntityPrimitives.ColumnsActions_GridEntity" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Hosting" %>
<%@ Import Namespace="System.Xml.XPath" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Core" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Business" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.Data.Meta.Management" %>
<%@ Import Namespace="Mediachase.Ibn.Web.UI" %>
<%@ Import Namespace="Mediachase.BusinessFoundation" %>
<%@ Import Namespace="Mediachase.BusinessFoundation.XmlObjectModel" %>

<script language="c#" runat="server">
	protected string GetValue(EntityObject DataItem)
	{
		string retVal = "";
		if (DataItem != null && DataItem.PrimaryKeyId.HasValue)
			retVal = BindAction(DataItem);
		else if (DataItem != null)
			RegisterCommands(DataItem);
		return retVal;
	}

	void RegisterCommands(EntityObject DataItem)
	{
		string ownClassName = DataItem.MetaClassName;
		MetaClass ownClass = MetaDataWrapper.GetMetaClassByName(DataItem.MetaClassName);
		if (ownClass.CardOwner != null)
			ownClassName = ownClass.CardOwner.Name;

		IXPathNavigable navigable = Mediachase.BusinessFoundation.XmlBuilder.GetXml(StructureType.ListViewUI, new Selector(ownClassName, this.ViewName, this.Place));
		XPathNavigator actions = navigable.CreateNavigator().SelectSingleNode("ListViewUI/Grid/CustomColumns");

		BafGrid grid = XmlModelHelper.GetBafGrid(Place, ownClassName);
		if (grid != null && grid.CustomColumns != null && grid.CustomColumns.Length > 0)
		{
			foreach (Column column in grid.CustomColumns)
			{
				string type = String.IsNullOrEmpty(column.type) ? string.Empty : column.type;
				string id = String.IsNullOrEmpty(column.id) ? string.Empty : column.id;

				foreach (object actionItem in column.Items)
				{
					Item item = actionItem as Item;
					if (item != null)
					{
						string commandName = item.commandName;
						CommandManager.GetCurrent(this.Page).AddCommand(ownClassName, this.ViewName, this.Place, commandName);
					}
				}
			}
		} else {
			foreach (XPathNavigator gridItem in actions.SelectChildren("Column", string.Empty))
			{
				string type = gridItem.GetAttribute("type", string.Empty);
				string id = gridItem.GetAttribute("id", string.Empty);

				if (type == "ColumnsActions" && id == this.ColumnId)
				{
					foreach (XPathNavigator actionItem in gridItem.SelectChildren("Item", string.Empty))
					{
						string commandName = actionItem.GetAttribute("commandName", string.Empty);
						CommandManager.GetCurrent(this.Page).AddCommand(ownClassName, this.ViewName, this.Place, commandName);
					}
				}
			}
		}

	}
	
	#region BindAction
	/// <summary>
	/// Binds the action.
	/// </summary>
	string BindAction(EntityObject DataItem)
	{	
		string retVal = string.Empty;

		string ownClassName = DataItem.MetaClassName;
		MetaClass ownClass = MetaDataWrapper.GetMetaClassByName(DataItem.MetaClassName);
		if (ownClass.CardOwner != null)
			ownClassName = ownClass.CardOwner.Name;

		IXPathNavigable navigable = Mediachase.BusinessFoundation.XmlBuilder.GetXml(StructureType.ListViewUI, new Selector(ownClassName, this.ViewName, this.Place));
		XPathNavigator actions = navigable.CreateNavigator().SelectSingleNode("ListViewUI/Grid/CustomColumns");

		BafGrid grid = XmlModelHelper.GetBafGrid(Place, ownClassName);
		if (grid != null && grid.CustomColumns != null && grid.CustomColumns.Length > 0)
		{
			foreach (Column column in grid.CustomColumns)
			{
				string type = String.IsNullOrEmpty(column.type) ? string.Empty : column.type;
				string id = String.IsNullOrEmpty(column.id) ? string.Empty : column.id;

				if (type == "ColumnsActions" && id == this.ColumnId)
				{
					foreach (object actionItem in column.Items)
					{
						Item item = actionItem as Item;
						if (item != null)
						{
							string imageUrl = String.IsNullOrEmpty(item.imageUrl) ? string.Empty : item.imageUrl;
							string commandName = String.IsNullOrEmpty(item.commandName) ? string.Empty : item.commandName;
							string paddingLeft = String.IsNullOrEmpty(item.paddingLeft) ? string.Empty : item.paddingLeft;
							string paddingRight = String.IsNullOrEmpty(item.paddingRight) ? string.Empty : item.paddingRight;
							string tooltip = String.IsNullOrEmpty(item.toolTip) ? string.Empty : item.toolTip;

							Dictionary<string, string> dic = new Dictionary<string, string>();
							if (this.DataItem == null)
								dic.Add("primaryKeyId", "");
							else if (!DataItem.PrimaryKeyId.HasValue)
								dic.Add("primaryKeyId", "");
							else
								dic.Add("primaryKeyId", DataItem.PrimaryKeyId.ToString());
							dic.Add("className", DataItem.MetaClassName);
							CommandParameters cp = new CommandParameters(commandName, dic);
							bool isEnable = true;
							string clientScript = CommandManager.GetCurrent(this.Page).AddCommand(ownClassName, this.ViewName, this.Place, cp, out isEnable);
							imageUrl = this.ResolveClientUrl(imageUrl);

							//create ImageButton control			
							ImageButton img = new ImageButton();
							img.ImageUrl = imageUrl;

							if (tooltip != string.Empty)
							{
								tooltip = CHelper.GetResFileString(tooltip);
								img.ToolTip = tooltip;
							}

							img.OnClientClick = string.Format("{0}; return false;", clientScript);
							if (paddingRight != string.Empty)
								img.Style.Add(HtmlTextWriterStyle.PaddingRight, paddingRight + "px");

							if (paddingLeft != string.Empty)
								img.Style.Add(HtmlTextWriterStyle.PaddingLeft, paddingLeft + "px");

							//ImageButton control -> toString()
							StringBuilder sb = new StringBuilder();
							StringWriter tw = new StringWriter(sb);
							HtmlTextWriter hw = new HtmlTextWriter(tw);
							img.RenderControl(hw);
							//add image button
							if (isEnable)
								retVal += sb.ToString();
						}
					}
				}
			}
		}
		else
		{
			foreach (XPathNavigator gridItem in actions.SelectChildren("Column", string.Empty))
			{
				string type = gridItem.GetAttribute("type", string.Empty);
				string id = gridItem.GetAttribute("id", string.Empty);

				if (type == "ColumnsActions" && id == this.ColumnId)
				{
					foreach (XPathNavigator actionItem in gridItem.SelectChildren("Item", string.Empty))
					{
						string imageUrl = actionItem.GetAttribute("imageUrl", string.Empty);
						string commandName = actionItem.GetAttribute("commandName", string.Empty);
						string paddingLeft = actionItem.GetAttribute("paddingLeft", string.Empty);
						string paddingRight = actionItem.GetAttribute("paddingRight", string.Empty);
						string tooltip = actionItem.GetAttribute("tooltip", string.Empty);

						Dictionary<string, string> dic = new Dictionary<string, string>();

						if (this.DataItem == null)
							dic.Add("primaryKeyId", "");
						else if (!DataItem.PrimaryKeyId.HasValue)
							dic.Add("primaryKeyId", "");
						else
							dic.Add("primaryKeyId", DataItem.PrimaryKeyId.ToString());
						dic.Add("className", DataItem.MetaClassName);
						CommandParameters cp = new CommandParameters(commandName, dic);
						bool isEnable = true;
						string clientScript = CommandManager.GetCurrent(this.Page).AddCommand(ownClassName, this.ViewName, this.Place, cp, out isEnable);
						imageUrl = this.ResolveClientUrl(imageUrl);

						//create ImageButton control			
						ImageButton img = new ImageButton();
						img.ImageUrl = imageUrl;

						if (tooltip != string.Empty)
						{
							tooltip = CHelper.GetResFileString(tooltip);
							img.ToolTip = tooltip;
						}

						img.OnClientClick = string.Format("{0}; return false;", clientScript);

						if (paddingRight != string.Empty)
							img.Style.Add(HtmlTextWriterStyle.PaddingRight, paddingRight + "px");

						if (paddingLeft != string.Empty)
							img.Style.Add(HtmlTextWriterStyle.PaddingLeft, paddingLeft + "px");

						//ImageButton control -> toString()
						StringBuilder sb = new StringBuilder();
						StringWriter tw = new StringWriter(sb);
						HtmlTextWriter hw = new HtmlTextWriter(tw);
						img.RenderControl(hw);
						//add image button
						if (isEnable)
							retVal += sb.ToString();
					}
				}
			}
		}

		return retVal;
	}
	#endregion
</script>
<%# GetValue(DataItem) %>