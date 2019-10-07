leftTemplate_Initialize = function()
{
	if (typeof Ext == 'undefined') 
        return;       
       
    var menuView = Ext.getCmp('menu_panel');
    
    // Save state info
    var selectedIndex = Ext.state.Manager.get("menu-view-selected", 0);
    
    try{
		var buttons = Ext.get('left_div');
		var divItems = buttons.select("div");
		if(selectedIndex >= divItems.getCount())
		{
			Ext.state.Manager.set("menu-view-selected", 0);
			selectedIndex = 0;
		}
		divItems.item(selectedIndex).replaceClass('NavigationNavBarItem', 'NavigationNavBarItemActive');
    }
    catch(ex) {}
    
    menuView.getLayout().setActiveItem(selectedIndex);
}

function leftTemplate_RegisterMenuView(view)
{
    if (typeof Ext == 'undefined') 
        return;       
       
    var menuView = Ext.getCmp('menu_panel');
    if(!menuView)
		return;
	if(!menuView.items)
		menuView.add(view);
    else if(!menuView.items.containsKey(view.id));
        menuView.add(view);
}

leftTemplate_onMouseOver = function(button)
{
    if(leftTemplate_trim(button.className) == 'NavigationNavBarItem')
    {
        button.className='NavigationNavBarItemHover';
    }
    else
    {
    }
};

leftTemplate_onMouseOut = function(button)
{
    if(leftTemplate_trim(button.className) == 'NavigationNavBarItemHover')
    {
        button.className='NavigationNavBarItem';
    }
    else
    {
    }
};   

leftTemplate_onNavMenuSelect = function(button, index)
{
    var buttons = Ext.get('left_div');
    buttons.select("div").each(function(member){member.replaceClass('NavigationNavBarItemActive','NavigationNavBarItem');});

    button.className = 'NavigationNavBarItemActive';
    leftTemplate_ChangeActiveTab(index);
};

function leftTemplate_ChangeActiveTab(index)
{
   if (typeof Ext == 'undefined') 
        return;       
 
    var menuView = Ext.getCmp('menu_panel');
    
    menuView.getLayout().setActiveItem(index);
    
    // Save state info
    Ext.state.Manager.set("menu-view-selected", index);
}  

leftTemplate_trim = function(val)
{
    return val.replace(/^\s+|\s+$/g, '');
};

//function leftTemplate_AddMenuTab(panelId, panelTitle, dataUrlString)
//{
//	Ext.onReady(function(){
//    var tree = new Ext.tree.TreePanel({
//        id: panelId,
//        autoScroll:true,
//        animate:true,
//        rootVisible:false,
//        enableDD:false,
//        title: panelTitle,
//        containerScroll: true, 
//        loader: new Ext.tree.TreeLoader({
//            dataUrl: dataUrlString
//        })
//    });
//    
//    leftTemplate_RegisterMenuView(tree);
//    
//    // set the root node
//    var root = new Ext.tree.AsyncTreeNode({
//		id: 'leftTemplate_tree_rootId',
//        draggable: false,
//        expanded: true
//    });
//    tree.setRootNode(root);
//});
//}

// leftTemplate_AddMenuTab adapted for ECF
function leftTemplate_ECFAddMenuTab(panelId, panelTitle, dataUrlString)
{
    if (typeof Ext == 'undefined') 
	{
		setTimeout('leftTemplate_ECFAddMenuTab(' + panelId +', '+panelTitle+', '+dataUrlString+')', 300);
        return;
	}
	//Ext.onReady(function(){
    var tree = new Ext.tree.TreePanel({
        id: panelId,
        autoScroll:true,
        animate:true,
        rootVisible:false,
        enableDD:false,
        title: panelTitle,
        containerScroll: true, 
        loader: new Ext.tree.TreeLoader({
            dataUrl: dataUrlString
        }),
        tools:[{
                id:'refresh',
                qtip: 'Refresh inner panel contents',
                on:{
                    click: function(){
                        var tree = Ext.getCmp(panelId);
                        tree.body.mask('Loading', 'x-mask-loading');
                        tree.root.reload();
                        tree.root.collapse(true, false);
                        setTimeout(function(){ // mimic a server call
                            tree.body.unmask();
                            tree.root.expand(false, true);
                        }, 1000);
                    }
                }
            }]
    });
    tree.defaultLoaderUrl = dataUrlString;
    
    leftTemplate_RegisterMenuView(tree);
    
    tree.on('click', function (node) {
        if (node.attributes.viewid != null)
            CSManagementClient.ChangeView(node.attributes.app, node.attributes.viewid, node.attributes.parameters);
    });
    
//    tree.on('contextmenu', function (node, e) {
//        if(node.attributes.displayContextMenu)
//        {
//            if(!node.menu)  // create context menu on first right click
//            {
//                node.menu = new Ext.menu.Menu({
//                    id: node.id,
//                    items: [
//                        {
//                            id: "menu-"+node.id,
//                            text: 'Refresh',
//                            icon: '../../../Apps/Shell/Styles/images/Refresh.png',
//                            treeNode: node,
//                            handler: onMenuItemClick
//                       }]
//                });
//            }
//            node.menu.showAt(e.getXY());
//        }
//    });
//    
//    function onMenuItemClick(item, e){
//        var node = item.treeNode;
//        if (node != null)
//        {
//            if(confirm('Refresh node '+node.text+'?'))
//            {
//                // reload TreeNode
//                node.loaded = false;
//                node.collapse();
//                node.expand();
//            }
//        }
//    }
    
    tree.loader.on("beforeload", function(treeLoader, node) {
        treeLoader.baseParams = node.attributes;
        if(node.attributes.treeLoader)
        {
			treeLoader.dataUrl = node.attributes.treeLoader;
		}
		else
		{
			treeLoader.dataUrl = node.ownerTree.defaultLoaderUrl;
		}
    }, this);
     
    // set the root node
    var root = new Ext.tree.AsyncTreeNode({
		id: 'leftTemplate_tree_rootId',
        draggable: false,
        expanded: true,
        type: 'root'
    });
    tree.setRootNode(root);
//});
}