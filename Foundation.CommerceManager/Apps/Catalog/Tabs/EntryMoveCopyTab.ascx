<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryMoveCopyTab" Codebehind="EntryMoveCopyTab.ascx.cs" %>
<script type="text/javascript">
function ComboBox_onExpand(sender, eventArgs)
{
    InitTree('<%= TreeDataSource %>');
}

function Tree_onClick(treeNode)
{
    var ctrl = $get('<%=targetFolder.ClientID%>');
    var text = treeNode.text;

    if(treeNode.attributes.type == 'Catalogs')
        return false;
    else
        ctrl.value = treeNode.attributes.itemid;

    while(treeNode != null)
    {
        treeNode = treeNode.parentNode;

        if(treeNode==null || treeNode.attributes.type == 'root')
            break;
        
        text = treeNode.text + '/' + text;
    }

    ComboBox1.set_text(text);
    ComboBox1.collapse();
}

function InitTree(dataUrlString)
{
    var ext = null;
    
    if (typeof Ext != 'undefined')
        ext = Ext;
        
    // try to get Ext from parent window
//    if (window.parent != null &&  (typeof window.parent.Ext != 'undefined'))
//            ext = window.parent.Ext;
    
    if (ext == null)
    {
        alert('Ext is undefined!');
        return;
    }

    // container object for the tree
    var treeParentObj = /*$get('tree-div');*/
    $get('<%= ComboBox1.ClientID + "_DropDownContent" %>');

    if (treeParentObj == null)
        return;

    // clear dropdown contents
    if (treeParentObj != null)
        treeParentObj.innerHTML = '';
        
    // create the root node
    var rootNode = new ext.tree.AsyncTreeNode({
		id: 'movecopy_tree_rootId',
        draggable: false,
        expanded: true,
        type: 'root'
    });
	
	// create tree
    var tree = new ext.tree.TreePanel({
        renderTo: treeParentObj,
        id: 'tree1',
        autoScroll:true,
        animate:true,
        rootVisible:false,
        enableDD:false,
        border: false,
        title: '',
        containerScroll: true, 
        root: rootNode,
        loader: new ext.tree.TreeLoader({
            dataUrl: dataUrlString
        })
    });
    
    tree.defaultLoaderUrl = dataUrlString;
    
    tree.on('click', function (node) {
        if (node.attributes != null)
            Tree_onClick(node);
    });
    
    tree.loader.on("beforeload", function(treeLoader, node) {
        treeLoader.baseParams = node.attributes;
        if(node.attributes.treeLoader)
			treeLoader.dataUrl = node.attributes.treeLoader;
		else
			treeLoader.dataUrl = node.ownerTree.defaultLoaderUrl;
    }, this);
}
</script>
<asp:HiddenField ID="targetFolder" runat="server"/>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td valign="middle" style="padding: 1px; width: 180px;">
            <asp:RadioButtonList RepeatDirection="Horizontal" ID="MoveCopyOption" runat="server" Visible="true">
                <asp:ListItem Value="Move" Selected="True" Text="<%$ Resources:SharedStrings, move_or %>"/>
                <asp:ListItem Value="Copy" Text="<%$ Resources:SharedStrings, link_to_folder %>"/>
            </asp:RadioButtonList>
        </td>
        <td style="padding: 1px; height: 50px;" align="left" valign="middle">
            <!-- Content Area -->
                <ComponentArt:ComboBox id="ComboBox1" runat="server"
                      AutoFilter="false"
                      AutoHighlight="false"
                      AutoComplete="true"
                      CssClass="comboBox"
                      HoverCssClass="comboBoxHover"
                      FocusedCssClass="comboBoxHover"
                      TextBoxCssClass="comboTextBox"
                      TextBoxHoverCssClass="comboBoxHover"
                      DropDownCssClass="comboDropDown"
                      ItemCssClass="comboItem"
                      ItemHoverCssClass="comboItemHover"
                      SelectedItemCssClass="comboItemHover"
                      DropHoverImageUrl="~/Apps/Shell/styles/images/combobox/drop_hover.gif"
                      DropImageUrl="~/Apps/Shell/styles/images/combobox/drop.gif"
                      Width="230"
                      DropDownHeight="120"
                      DropDownWidth="227" RunningMode="Client" LoadingText="<%$ Resources:SharedStrings, Loading_Ellipsis %>">
                      <ClientEvents>
                        <Expand EventHandler="ComboBox_onExpand" />
                      </ClientEvents>
                      <DropdownContent>
                          <%--<div id="tree-div" style="overflow:auto; height:100% !important;width:227px;border:0px !important"></div>--%>
                      </DropdownContent>
                    </ComponentArt:ComboBox>
            <!-- /Content Area -->
        </td>
    </tr>
    <tr>
        <td colspan="2" style="background-image: url('<%= this.ResolveUrl("~/Apps/Shell/Styles/images/dialog/bottom_content.gif")%>');
            height: 41px; padding-right: 10px;" align="right">
            <asp:Button runat="server" ID="btnOK" Text="<%$ Resources:SharedStrings, OK %>" Width="80px" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnClose" Text="<%$ Resources:SharedStrings, Close %>" Width="80px" />
        </td>
    </tr>
</table>