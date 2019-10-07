define([
    "dojo/_base/declare",
    "dojo/_base/config",
    "dojo/_base/lang",

    "dojo/ready",
    "dojo/aspect",
    "dojo/dom-class",
    "dojo/dom-construct",
    "dojo/dom-style",
    "dojo/Deferred",

    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/layout/_LayoutWidget",

    "epi/epi",
    "epi/shell/widget/_ValueRequiredMixin",

    "/EPiServer/AceEditor/ClientResources/Scripts/AceEditor/lib/ace/ace.js",
    // templates
    "dojo/text!./aceEditor.html",
    // theme
    "xstyle/css!./lib/ace/razor_chrome.css"
],

    function (
        declare,
        config,
        lang,
        ready,
        aspect,
        domClass,
        domConstruct,
        domStyle,
        Deferred,

        _Widget,
        _TemplatedMixin,
        _LayoutWidget,

        epi,
        _ValueRequiredMixin,
        aceEditor,
        template
    ) {

        var _loadedModes = {};//global cache for loaded modes
        var _loadedThemes = {};//global cache for loaded themes

        return declare("aceeditor/aceEditor", [_LayoutWidget, _TemplatedMixin, _ValueRequiredMixin],
            {
                // baseClass: [public] String
                //    The widget's base CSS class.
                baseClass: "epi-ace-editor",

                // width: [public] Number
                //    The editor width.
                width: null,

                // height: [public] Number
                //    The editor height.
                height: null,

                // value: [public] String
                //    The editor content.
                value: null,

                // intermediateChanges: Boolean
                //    Fires onChange for each value change or only on demand
                intermediateChanges: true,

                // templateString: [protected] String
                //    Template for the widget.
                templateString: template,

                // settings: [public] object
                //    The editor settings.
                settings: null,

                // dirtyCheckInterval: [public] Integer
                //    How often should the widget check if it is dirty and raise onChange event, in milliseconds. The value is by default set to 2000 ms
                dirtyCheckInterval: 2000,

                // autoResizable: [public] Boolean
                //    States if the editor can be resized while text is added.
                autoResizable: false,

                // isResized: [public] Boolean
                //    Indicates if the widget has been already resized on its initialization.
                isResized: false,

                aceEditorRendered: null,

                dropTarget: null,

                // readOnly: [public] Boolean
                //    Denotes that the editor is read only.
                readOnly: false,

                // _dirtyCheckTimeout: [private] timeout
                //    Used for storing the dirty check timeout reference when dirtyCheckInterval is set.
                _dirtyCheckTimeout: null,

                // _editorValue: [private] String
                //    The value set to the editor
                _editorValue: null,

                editor: null,
                editorSession: null,

                loadScript: function (url, attributes, readyCB) {
                    // DOM: Create the script element
                    var jsElm = document.createElement("script");
                    // set the type attribute
                    jsElm.type = "application/javascript";
                    // make the script element load file
                    jsElm.src = url;
                    jsElm.onload = function () {
                        if (readyCB) {
                            readyCB();
                        }
                    };
                    // finally insert the element to the body element in order to load the script
                    document.body.appendChild(jsElm);
                },

                postMixInProperties: function () {

                    this.inherited(arguments);

                    if (this.autoResizable) {
                        this.height = 0;
                    }

                    this.aceEditorRendered = new Deferred();
                },

                postCreate: function () {
                    this.inherited(arguments);
                },

                startup: function () {
                    // summary:
                    //      Overridden to reset input field.
                    if (this._started) {
                        return;
                    }

                    this.inherited(arguments);

                    this._initAceEditor();
                },

                destroy: function () {
                    // summary:
                    //    Destroy widget.
                    //
                    // tags:
                    //    protected

                    if (this._destroyed) {
                        return;
                    }

                    this._cancelDirtyCheckInterval();

                    var ed = this.getEditor();

                    ed && ed.destroy();

                    this.inherited(arguments);
                },

                getEditor: function () {
                    // summary:
                    //    Return an editor instance.
                    //
                    // tags:
                    //    public
                    //
                    // returns:
                    //    A instance of the current editor.

                    if (typeof aceEditor === "undefined" || aceEditor === null) {
                        return null;
                    }

                    return this.editor;
                },

                _setValueAttr: function (newValue) {
                    //summary:
                    //    Value's setter
                    //
                    // tags:
                    //    protected

                    var ed = this.getEditor(),
                        editableValue = newValue || "";

                    this._set("value", newValue);

                    // If the editor has started, set the content to it
                    // otherwise it will be set from the textarea when tiny inits
                    if (ed) {
                        this.editorSession.setValue(editableValue);
                    } else {
                        $(this.editorFrame).text(editableValue);
                    }

                    this.inherited(arguments);
                },

                _initAceEditor: function () {
                    
                    this.editor = ace.edit(this.editorFrame.id);
                    this.editorSession = this.editor.getSession();

                    this.editor.setReadOnly(false);
                    this.editorSession.setTabSize(4);
                    this.editorSession.setUseSoftTabs(true);
                    this.editorSession.setUseWrapMode(true);
                    this.editor.setHighlightActiveLine(true);
                    this.editor.renderer.setShowGutter(true);

                    var theme = "chrome",
                        mode = this.aceEditorMode;

                    if (!_loadedThemes[theme]) {
                        this.loadScript("/EPiServer/AceEditor/ClientResources/Scripts/AceEditor/lib/ace/theme-" + theme + ".js", null, lang.hitch(this, function () {
                            _loadedThemes[theme] = true;
                            this.editor.setTheme('ace/theme/' + theme);
                        }));
                    } else {
                        this.editor.setTheme('ace/theme/' + theme);
                    }

                    if (!_loadedModes[mode]) {
                        this.loadScript("/EPiServer/AceEditor/ClientResources/Scripts/AceEditor/lib/ace/mode-" + mode + ".js", null, lang.hitch(this, function () {
                            _loadedModes[mode] = true;
                            this.editorSession.setMode("ace/mode/" + mode);
                        }));
                    } else {
                        this.editorSession.setMode("ace/mode/" + mode);
                    }

                    this._setupEditorEventHandling();

                },

                _setupEditorEventHandling: function () {
                    this.editor.on("blur", lang.hitch(this, function () {
                        if (!epi.areEqual(this.get('value'), this.editor.getValue())) {
                            this.set('value', this.editor.getValue());
                            this.onChange();
                        }
                    }));

                    this._startDirtyCheckInterval();
                },

                _cancelDirtyCheckInterval: function () {
                    // summary:
                    //      Stop any running dirty checks
                    // tags:
                    //      private

                    if (this._dirtyCheckTimeout) {
                        clearTimeout(this._dirtyCheckTimeout);
                        this._dirtyCheckTimeout = null;
                    }
                },

                _startDirtyCheckInterval: function () {
                    // summary:
                    //      Calls _dirtyCheck and schedules a new one according to the dirtyCheckInterval flag
                    // tags:
                    //      private

                    if (this._destroyed || !this.intermediateChanges) {
                        return;
                    }

                    this._cancelDirtyCheckInterval();

                    this._dirtyCheck();

                    this._dirtyCheckTimeout = setTimeout(lang.hitch(this, this._startDirtyCheckInterval), this.dirtyCheckInterval);
                },

                _dirtyCheck: function () {
                    // summary:
                    //    Check if the editor is dirty and raise onChange event.
                    //
                    // tags:
                    //    private
                }
            });
    });