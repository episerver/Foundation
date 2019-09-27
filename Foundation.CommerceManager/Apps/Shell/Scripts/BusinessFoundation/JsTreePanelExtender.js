Type.registerNamespace("Mediachase");

// ----------------------
// ------ TreePanel -------
// ----------------------

// TreePanel Constructor
Mediachase.JsTreePanel = function(element) 
{
    Mediachase.JsTreePanel.initializeBase(this, [element]);
    
    //internal variables
    var extTreePanel = null;
    
    //properties
    this.blankImageUrl = null;
    this.treeSourceUrl = null;
    this.folderId = null;
    this.rootId = null;
    this.iconUrl = null;
    this.rootNodeText = null;
    this.rootHrefCommand = null;
    this.rootHrefTarget = null;
    this.nodeTextCss = null;
    this.iconCss = null;
    this.rootVisible = null;
    this.autoScroll = null;
    this.autoHeight = null;
    this.showBorder = null;
    this.bodyStyle = null;
    this.afterRenderScript = null;
}

Mediachase.JsTreePanel.prototype = 
{
	// -========= Properties =========-		
	get_blankImageUrl: function () {
		return this.blankImageUrl;
	},
	set_blankImageUrl: function (value) {
		this.blankImageUrl = value;
	},		
	get_treeSourceUrl: function () {
		return this.treeSourceUrl;
	},
	set_treeSourceUrl: function (value) {
		this.treeSourceUrl = value;
	},		
	get_folderId: function () {
		return this.folderId;
	},
	set_folderId: function (value) {
		this.folderId = value;
	},		
	get_rootId: function () {
		return this.rootId;
	},
	set_rootId: function (value) {
		this.rootId = value;
	},		
	get_iconUrl: function () {
		return this.iconUrl;
	},
	set_iconUrl: function (value) {
		this.iconUrl = value;
	},		
	get_rootNodeText: function () {
		return this.rootNodeText;
	},
	set_rootNodeText: function (value) {
		this.rootNodeText = value;
	},	
	get_rootHrefCommand: function () {
		return this.rootHrefCommand;
	},
	set_rootHrefCommand: function (value) {
		this.rootHrefCommand = value;
	},		
	get_rootHrefTarget: function () {
		return this.rootHrefTarget;
	},
	set_rootHrefTarget: function (value) {
		this.rootHrefTarget = value;
	},
	get_nodeTextCss: function () {
		return this.nodeTextCss;
	},
	set_nodeTextCss: function (value) {
		this.nodeTextCss = value;
	},		
	get_iconCss: function () {
		return this.iconCss;
	},
	set_iconCss: function (value) {
		this.iconCss = value;
	},		
	
	get_rootVisible: function () {
		return this.rootVisible;
	},
	set_rootVisible: function (value) {
		this.rootVisible = value;
	},
	get_autoScroll: function () {
		return this.autoScroll;
	},
	set_autoScroll: function (value) {
		this.autoScroll = value;
	},
	get_autoHeight: function () {
		return this.autoHeight;
	},
	set_autoHeight: function (value) {
		this.autoHeight = value;
	},
	get_showBorder: function () {
		return this.showBorder;
	},
	set_showBorder: function (value) {
		this.showBorder = value;
	},
	get_bodyStyle: function () {
		return this.bodyStyle;
	},
	set_bodyStyle: function (value) {
		this.bodyStyle = value;
	},	
	get_afterRenderScript: function () {
		return this.afterRenderScript;
	},
	set_afterRenderScript: function (value) {
		this.afterRenderScript = value;
	},		
	
	
	// -========= Methods =========-
	// ctor()
	initialize : function() {
		Mediachase.JsTreePanel.callBaseMethod(this, 'initialize');
        
        //ExtJs s.gif problem fix
        if (this.blankImageUrl != '' && typeof(Ext) != 'undefined')
        {
			Ext.BLANK_IMAGE_URL = this.blankImageUrl;
        }
        
        this.generateTreePanel();
        this.AfterRender();
    },	
    
    dispose: function() {
			
		this.extTreePanel = null;	
		this.treeSourceUrl = null;
		this.blankImageUrl = null;
		this.folderId = null;
		this.rootId = null;
		this.iconUrl = null;
		this.rootNodeText = null;
		this.rootHrefCommand = null;
		this.rootHrefTarget = null;
		this.nodeTextCss = null;
		this.iconCss = null;
		Mediachase.JsTreePanel.callBaseMethod(this, 'dispose');
    },
    
    generateTreePanel: function()
    {
		var Tree = Ext.tree;
		this.extTreePanel = new Tree.TreePanel({
			id:'treeLeftStorage',
			el:this._element.id,
			autoScroll:this.autoScroll,
			autoHeight:this.autoHeight,
			animate:true,
			rootVisible:this.rootVisible,
			enableDD:false,
			containerScroll: true, 
			border: this.showBorder,
			bodyBorder: this.showBorder,
			bodyStyle: this.bodyStyle,
			loader: new Tree.TreeLoader({
				dataUrl: this.treeSourceUrl
			})
		});
		
		this.extTreePanel.loader.on("beforeload", function(treeLoader, node) {
			treeLoader.baseParams = node.attributes;
			}, this);
			
		this.extTreePanel.loader.on("load", function(treeLoader, node) {
			if(node.itemid == this.folderId)
				node.select();
			else
				this.SelectMe(node);
			}, this);
			
		// set the root node
		var root = new Tree.AsyncTreeNode({
			text: this.rootNodeText,
			cls: this.nodeTextCss,
			icon: this.iconUrl,
			iconCls: this.iconCss,
			draggable:false,
			expanded: true,
			href: this.rootHrefCommand,
			hrefTarget: this.rootHrefTarget,
			id: 'FL_root_id',
			type: 'root'
		});
		root.itemid = this.rootId;
		this.extTreePanel.setRootNode(root);

		// render the tree
		this.extTreePanel.render();
		root.expand();
    },
    
    AfterRender: function()
    {
		if (this.afterRenderScript && this.afterRenderScript != '')
		{
			eval(this.afterRenderScript);
		}
    },
    
    SelectMe: function(node)
    {
		for(var i=0; i<node.childNodes.length; i++)
		{
			var child = node.childNodes[i];
			if(child.attributes.itemid == this.folderId)
				child.select();
			this.SelectMe(child);
		}
    }
}

Mediachase.JsTreePanel.registerClass("Mediachase.JsTreePanel", Sys.UI.Control);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
