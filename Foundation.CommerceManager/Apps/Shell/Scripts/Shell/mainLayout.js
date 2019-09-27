var viewport;
var mainWidth;

var _northPanelHeight = 25;
var _northPanelHeightMinimized = 25;

var _topPlaceDivId = "TopPlaceDiv"; // div on the top of the page (in default.aspx)
var _rightFrameId = "right"; // id of the right frame
var _showPartTopFrameStateId = "showPartTopFrame"; // cookie name for storing whether to show top panel (TopPlaceDiv in default.aspx)
var _showLeftFrameStateId = "showLeftFrame"; // cookie name for storing whether to show left menu

// Create cards
var cards = new Ext.Panel({title: '',id:'menu_panel',layout:'card',region:'center',border: false,activeItem: 0,bodyStyle: 'padding:0px', defaults: {border:false}});

function initLayout()
{
    // Set constants
    Ext.BLANK_IMAGE_URL = 'images/s.gif';

	// Set cookie provider
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());
    
    var str = Ext.state.Manager.get(_showLeftFrameStateId);
    var hidden = false;
    if(str && str == "0")
		hidden = true;
            
	viewport = new Ext.Viewport({
		layout: 'border',
		autoShow: true,
		
		items: [{
			region: 'north',
			xtype: 'panel',
			contentEl: 'up_div',
			border: false,
			margins: '0 0 0 0',
			height: _northPanelHeight
		}, {
			region: 'west',
			collapsible: true,
			collapsed: hidden,
			collapseMode: 'mini',
			width: 200,
			minSize: 175,
			maxSize: 400,
			split: true,
			margins: '0 0 0 2',
			layout:'border',
			items:
		    [   
				cards,
		        {
                    region:'south',
                    border: false,
                    split:false,
                    contentEl: 'left_div'
                }
		    ],
			listeners: {
				beforecollapse: function(p) {
					HideLeft();
				},
				beforeexpand: function(p) {
					ShowLeft();
				}
			}
		}, 
		{
			region:'center',
			border: false,
			items:
				new Ext.Panel({
					region:'center',
					items:[
					{
						region:'center',
						border: false,
						contentEl:'center_div'
					}]
			}),
			listeners: {
					resize: function(p1,p2,p3,p4,p5) {
						ResizeIFrame();
					}
			}
		}]
	});
	
	var s = Ext.state.Manager.get(_showPartTopFrameStateId);
	if(s && s == "0")
		HideTopPart();
	else
		ShowTopPart();
		
	ResizeIFrame();
	
	setTimeout('leftTemplate_Initialize();', 500);
}

function ResizeIFrame()
{
	if(viewport && viewport.layout && viewport.layout.center && viewport.layout.center.panel)
	{
		var obj = document.getElementById(_rightFrameId);
		obj.height = (viewport.layout.center.panel.getInnerHeight() - 2).toString() + "px";
	}
}

function ExpandCollapse()
{
	var s = Ext.state.Manager.get(_showPartTopFrameStateId);
	if(s && s == "0")
		ShowTopPart();
	else
		HideTopPart();
}

function HideTopPart()
{
	var fset = viewport.layout.north;
	fset.panel.hide();
	fset.panel.setHeight(_northPanelHeightMinimized);
	fset.panel.show();
	viewport.doLayout();
	
	var TopPlaceDiv = document.getElementById(_topPlaceDivId);
	if (TopPlaceDiv)
		TopPlaceDiv.style.display = "none";
		
	Ext.state.Manager.set(_showPartTopFrameStateId, "0");
}

function ShowTopPart()
{
	var fset = viewport.layout.north;
	fset.panel.hide();
	fset.panel.setHeight(_northPanelHeight);
	fset.panel.show();
	viewport.doLayout();
	
	var TopPlaceDiv = document.getElementById(_topPlaceDivId);
	if (TopPlaceDiv)
		TopPlaceDiv.style.display = "";
		
	Ext.state.Manager.set(_showPartTopFrameStateId, "1");
}

function ShowLeft()
{
	Ext.state.Manager.set(_showLeftFrameStateId, "1");
}

function HideLeft()
{
	Ext.state.Manager.set(_showLeftFrameStateId, "0");
}