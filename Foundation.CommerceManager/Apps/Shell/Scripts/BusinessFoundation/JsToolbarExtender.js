Type.registerNamespace("Mediachase");

// ----------------------
// ------ Toolbar -------
// ----------------------

// Toolbar Constructor
Mediachase.JsToolbar = function(element) {
    Mediachase.JsToolbar.initializeBase(this, [element]);

    //internal variables
    var extToolbar = null;

    //properties
    var toolbarItems = null;
    var cssClassGeneral = null;
    this.blankImageUrl = null;

    //handlers
    var _windowOnLoadHandler = null;
}

Mediachase.JsToolbar.prototype =
{
    // -========= Properties =========-		
    get_toolbarItems: function() {
        return this.toolbarItems;
    },
    set_toolbarItems: function(value) {
        this.toolbarItems = value;
    },
    get_cssClassGeneral: function() {
        return this.cssClassGeneral;
    },
    set_cssClassGeneral: function(value) {
        this.cssClassGeneral = value;
    },
    get_blankImageUrl: function() {
        return this.blankImageUrl;
    },
    set_blankImageUrl: function(value) {
        this.blankImageUrl = value;
    },


    // -========= Methods =========-
    // ctor()
    initialize: function() {
        //TODO: create and init 		
        //alert('start toolbar loader');
        Mediachase.JsToolbar.callBaseMethod(this, 'initialize');

        //ExtJs s.gif problem fix
        if (this.blankImageUrl != '' && typeof (Ext) != 'undefined') {
            Ext.BLANK_IMAGE_URL = this.blankImageUrl;
        }

        this._windowOnLoadHandler = Function.createDelegate(this, this.windowOnLoad);

        Sys.Application.add_load(this._windowOnLoadHandler);

        this.generateToolbar();
        //alert('end toolbar loader');
    },

    dispose: function() {
        // ie fix bug: El.chache is null (?)
        //		if (this.extToolbar)
        //			this.extToolbar.destroy();
        if (Sys && Sys.Application && this._windowOnLoadHandler)
            Sys.Application.remove_load(this._windowOnLoadHandler);

        this.extToolbar = null;
        this.cssClassGeneral = null;
        Mediachase.JsToolbar.callBaseMethod(this, 'dispose');
    },

    generateToolbar: function() {
        this.extToolbar = new Ext.Toolbar(Sys.Serialization.JavaScriptSerializer.deserialize(this.toolbarItems));

        if (typeof (this.cssClassGeneral) !== 'undefined')
            this.extToolbar.addClass(this.cssClassGeneral);

        //TO DO: delay before render			
        //this.extToolbar.render(this._element);	
    },

    renderToolbar: function() {
        if (this.extToolbar)
            this.extToolbar.render(this._element);

        if (navigator.appVersion.indexOf('MSIE 7.0') > -1) {
            this.extToolbar.el.dom.style.position = 'static';
        }

        //generate tooltips
        for (var i = 0; i < this.extToolbar.items.length; i++) {
            if (this.extToolbar.items.items[i].tooltip != null && this.extToolbar.items.items[i].tooltip != '') {
                this.extToolbar.items.items[i].el.dom.title = this.extToolbar.items.items[i].tooltip;
            }
        }
    },

    windowOnLoad: function(sender, args) {
        var _delegate = Function.createDelegate(this, this.renderToolbar);
        window.setTimeout(function() { _delegate(); }, 100);
    },

    hideAllMenus: function() {
        if (this.extToolbar) {
            for (var i = 0; i < this.extToolbar.items.length; i++) {
                if (typeof (this.extToolbar.items.items[i].menu) !== 'undefined') {
                    this.extToolbar.items.items[i].hideMenu();
                }
            }
        }
    }
}

Mediachase.JsToolbar.registerClass("Mediachase.JsToolbar", Sys.UI.Control);

//default action handlers
function defaultToolbarOnClick(item) {
	item.params = decodeURIComponent(item.params.replace(/\+/g, " "));
    var obj = Sys.Serialization.JavaScriptSerializer.deserialize(item.params);

    if (obj.CommandArguments.CommandManagerScript) {
        eval(obj.CommandArguments.CommandManagerScript);
    }
}

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
