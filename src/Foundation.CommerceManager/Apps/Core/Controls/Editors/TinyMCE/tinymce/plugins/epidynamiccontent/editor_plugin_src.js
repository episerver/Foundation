(function (tinymce, $) {
    var Event = tinymce.dom.Event;
    tinymce.PluginManager.requireLangPack('epidynamiccontent');

    tinymce.create('tinymce.plugins.epidynamiccontent', {
        _onInitCalled: false, // A flag to signal if the editors onInit event has occured

        /**
        * Initializes of epidynamiccontentPlugin. Adds command, button and nodeChange event handler.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {
            var t = this;
            // Early exit if we don't have access to EPi...
            if (typeof EPi === "undefined" || typeof EPi.ResolveUrlFromUI != "function") {
                return;
            }

            if (typeof epiContentBlockUtilities == "undefined") {
                var scriptUrl = tinymce.baseURI.toAbsolute("plugins/epipersonalizedcontent/epicontentblockutilities.js");
                tinymce.ScriptLoader.load(scriptUrl, function () {
                    this._init(ed, url);
                }, this);

                tinymce.ScriptLoader.loadQueue();
            }
            else {
                this._init(ed, url);
            }

            ed.addButton('epidynamiccontent', {
                title: 'epidynamiccontent.desc',
                cmd: 'mceEPiDynamicContent'
            });

            // Flagging that onInit has been executed since _init is executed async and can be called after the editors onInit has been raised.
            ed.onInit.add(function () {
                t._onInitCalled = true;
            });
        },
        /**
        * The real initialization triggered by a callback. Adds command and event handlers.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        _init: function (ed, url) {
            //We extend this plug.in with methods that are common for both personalized content and dynamic content (defined in personalizedblock.js).
            $.extend(this, epiContentBlockUtilities);
            var t = this, dynamicContentClass;

            t._url = url;
            t.ed = ed;
            t._disabled = false; // Initially no controls are disabled.
            dynamicContentClass = t._dynamicContentClass = ed.getParam("epidynamiccontent_cssclass", "epi_dc");
            t._enabledControls = ed.getParam("epidynamiccontent_enabledcontrols", "");

            ed.addCommand('mceEPiDynamicContent', function () {
                // Resolve the path to the dynamic content dialog and append thew page context parameters to the query string.
                var ctx = $.extend({}, ed.settings.epi_page_context);
                $(document).trigger("epi.loading.page_context", ctx)
                var dialogURL = EPi.ResolveUrlFromUI('Editor/Dialogs/DynamicContent.aspx') + "?" + $.param(ctx);
                var selectedNode = null;

                var onDialogComplete = function (returnData, onComleteArgs) {
                    // Called to insert dynamic content when the dialog is closed.
                    var s = ed.selection;

                    if (!returnData) {
                        ed.windowManager.onClose.dispatch();
                        return;
                    }

                    if (selectedNode) {
                        s.select(selectedNode);
                    }

                    // s.setContet() seems to have a bug that makes it loose its selection in IE8 if the main window don't have focus (and the selection is empty or not selectable).
                    // To solve this we need to make a focus call on editor window first.
                    ed.getWin().focus();
                    var dc = t._getParentContentBlock(s.getNode());
                    //We need to remove the old dynamic content since setContent does not replace the outer tag in IE
                    if (dc) {
                        ed.dom.remove(dc);
                    }

                    if (returnData.match("^<div")) {
                        //Make sure that we do not insert the div in a p or other element that does not allow divs.
                        dc = t._insertContent(returnData);
                    }
                    else {
                        //spans are allowed everywhere so just insert it.
                        dc = t._insertInlineContent(returnData);
                    }

                    //Move the caret to the adjacent node
                    t._moveToAdjacentNode(dc, true);

                    t._onContentChanged();
                    ed.windowManager.onClose.dispatch();
                };

                var s = ed.selection; // Get the selection from the editor.
                selectedNode = t._getParentContentBlock(s.getStart());

                var dialogFeatures = { width: 600, height: 640 };

                var allContentGroups = t._getAllContentGroups(ed);

                if (selectedNode) {
                    // Inject a form into the opened dialog to be able to post the dynamic content data to the aspx.
                    var systemCSS = EPi.ResolveUrlFromUtil("../App_Themes/Default/Styles/system.css");

                    var groupAttribute = selectedNode.getAttribute("data-groups");
                    var groupValue = groupAttribute ? groupAttribute.replace(/"/g, '&quot;') : "";

                    var contentgroupAttribute = selectedNode.getAttribute("data-contentgroup");
                    var contentgroupValue = contentgroupAttribute ? contentgroupAttribute.replace(/"/g, '&quot;') : "";

                    var formString = '<html xmlns="http://www.w3.org/1999/xhtml"><head><link href="' + systemCSS + '" type="text/css" rel="stylesheet" /><title></title></head><body onload="document.forms[0].submit()">' +
                    '<form action="' + dialogURL + '" method="post">' +
                    '<input name="state" type="hidden" value="' + selectedNode.getAttribute("data-state").replace(/"/g, '&quot;') + '" />' +
                    '<input name="hash" type="hidden" value="' + selectedNode.getAttribute("data-hash").replace(/"/g, '&quot;') + '" />' +
                    '<input name="dynamicclass" type="hidden" value="' + selectedNode.getAttribute("data-dynamicclass").replace(/"/g, '&quot;') + '" />' +
                    '<input name="groups" type="hidden" value="' + groupValue + '" />' +
                    '<input name="contentgroup" type="hidden" value="' + contentgroupValue + '" />' +
                    '<input name="allcontentgroups" type="hidden" value="' + allContentGroups + '" />' +
                    '</form></body></html>';
                    
                    ed.windowManager.onOpen.dispatch();
                    var dialog = EPi.CreateDialog("", onDialogComplete, null, null, dialogFeatures);
                    dialog._dialog.document.write(formString);
                    dialog._dialog.document.close();
                } else {
                    dialogURL += '&allcontentgroups=' + allContentGroups;

                    ed.windowManager.onOpen.dispatch();
                    EPi.CreateDialog(dialogURL, onDialogComplete, null, null, dialogFeatures);
                }
            });

            // Removed keyboard shortcut due to conflict with Gecko browsers bookmark dialog.
            //ed.addShortcut("ctrl+shift+d", "epidynamiccontent.desc", "mceEPiDynamicContent");

            /**
            * Get run when the editor is initialized or changes state (full size editor for instance).
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            */
            var pluginInit = function (ed) {
                // Prevent drag & drop of content into dynamic content.
                // Note: This makes use of jQuery to overcome differences in native event object.
                var editorBody = $(ed.getBody());
                if (editorBody.length) {
                    editorBody.bind("drop", function (e) {
                        if ($(e.target).closest("." + dynamicContentClass).length) {
                            return false;
                        }
                    });
                }
                $(ed.dom.doc).click(function (e) { t._handleButtonsClick(e); });

                /**
                * Add a top node change handler to set overall enable/disable state of buttons.
                *
                * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
                * @param {Control manager} cm Control manager for the node change event.
                * @param {node} n The current node.
                * @param {bool} co If the selection is collapsed or not.
                * @param {options} options Additional options like if it's the node change that is run on initialization for the editor.
                */
                ed.onNodeChange.addToTop(function (ed, cm, n, co, options) {
                    // Listening to onNodeChange very early (addToTop) since we're fiddling with
                    // other plugin states and possibly changing the current selection.

                    var s = ed.selection;
                    var c = null;

                    if (s.isCollapsed()) {
                        // When the selection is collapsed it seems start and end of selection often differ, 
                        // but the start seems more reliable
                        c = t._getParentContentBlock(s.getNode());
                    } else {
                        // Check if start or end of current selection is inside a dynamic content block.
                        c = t._getParentContentBlock(s.getStart()) || t._getParentContentBlock(s.getEnd());
                    }

                    //hack fix for IE when caret is before block DC
                    //tinymce still reports n as the node before dc since it relies on IE's native Selection object
                    //W3C-like range object surprisingly works well in this situation
                    if (tinymce.isIE && !c && s.isCollapsed()) {
                        var rng = ed.selection.getRng(true);
                        var rngNode = rng.commonAncestorContainer;
                        if (rngNode && rngNode.nodeType === 1) { //element
                            if (ed.dom.hasClass(rngNode.childNodes[rng.startOffset], t._dynamicContentClass)) {
                                c = rngNode.childNodes[rng.startOffset];
                            }
                        }
                    }

                    if (c) {
                        // Inside a dynamic content block. 
                        // Expand selection
                        t._selectContentBlock(ed, c);

                        // Disable all other buttons
                        t._updateCommandState(false);

                        // Enable dynamic content button
                        t._showButton(ed, cm, true, true);

                        // Since no click event is fired inside a controlselection in IE, we need to check if a button was clicked
                        if (tinymce.isIE && co && (ed.dom.hasClass(n, "epi_dc_editBtn") || ed.dom.hasClass(n, "epi_dc_previewBtn"))) {
                            t._handleButtonsClick({ target: n });
                        }

                        // Always let other plug-ins set initial state on first event
                        var allowEvents = (options.initial === 1);

                        if (!allowEvents) {
                            // Update undo redo since we will block further events which would normally update them.
                            t._updateUndoRedoButtons(cm);
                        }

                        // Returning false prevents other node change handlers from executing.
                        return allowEvents;
                    } else {
                        // Outside dynamic content, enable all other buttons again
                        t._updateCommandState(true);

                        // co = Is the selection collapsed
                        // Enable dynamic content button if the current selection is collapsed.
                        t._showButton(ed, cm, co, false);
                    }
                });

                /**
                * Handles keyboard events with special logic for arrow and enter keys.
                *
                * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
                * @param {Event} e The event.
                */
                ed.onKeyDown.addToTop(function (ed, e) {
                    if ((e.keyCode < 37 || e.keyCode > 40) && e.keyCode != 13 && e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 32) {
                        return;
                    }

                    var s = ed.selection;
                    var dc = t._getParentContentBlock(s.getNode());

                    if (e.keyCode == 8 || e.keyCode == 46) { // Del or Bksp should be handled for all selections
                        // When having the cursor immediately after an inline dc span and pressing Backspace the getNode
                        // reports the surrounding (p?) element, but getEnd reports the dc span. 
                        // Check both when backspace or delete is pressed to prevent deleting parts of dc span, 
                        if (t._handleDeleteKeys(dc || t._getParentContentBlock(s.getEnd()), e.keyCode === 8)) {
                            return tinymce.dom.Event.cancel(e);
                        }
                        return;
                    }
                    if (!dc) {
                        return;
                    }

                    if (e.keyCode == 13) {
                        t._handleEnterKey();
                        return;
                    }

                    if (e.keyCode >= 37 && e.keyCode <= 40) {
                        //Arrow down or right = true, up and left = false
                        var moveAfter = e.keyCode == 39 || e.keyCode == 40;
                        t._moveToAdjacentNode(dc, moveAfter);
                        return tinymce.dom.Event.cancel(e);
                    }
                });
            };

            // If the editors onInit event has already been raised, just run the method. Otherwise attach.
            if (t._onInitCalled) {
                pluginInit(ed);
            } else {
                ed.onInit.add(pluginInit);
            }
        },

        /**
        * Set enabled and active state of the dynamic content button
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {Control manager} cm Control manager.
        * @param {bool} enabled If true, the button is enabled; otherwise disabled.
        * @param {bool} active If true, the button is set active; otherwise inactive.
        *
        **/
        _showButton: function (ed, cm, enabled, active) {
            var isHidden = ed.dom.isHidden(ed.container);
            cm.setDisabled("epidynamiccontent", isHidden || !enabled);
            cm.setActive("epidynamiccontent", !isHidden && active);
        },


        /**
        * Handle action buttons in dc header
        *
        * @param {Event} e The event object.
        */
        _handleButtonsClick: function (e) {
            //Left button only, IE never gets here so we can trust that 0 is left button
            if (this.ed.dom.hasClass(e.target, "epi_dc_editBtn") && (e.button === 0)) {
                this._hidePreview();
                var dc = this._getParentContentBlock(e.target);
                // We need to focus the editor since selection since the editor might not be activated yet and 
                // execCommand calls focus which causes our selection to be lost.
                this.ed.focus();
                this.ed.selection.select(dc);
                tinyMCE.execCommand('mceEPiDynamicContent');
                return tinymce.dom.Event.cancel(e);
            }
            else if ($(e.target).is(".epi_dc_previewBtn") && (e.button === 0)) {
                this._showPreview(e.target);
                return tinymce.dom.Event.cancel(e);
            }
            else {
                this._hidePreview();
            }
        },

        /**
        * Creates a new text node or p tag to be able to set the cursor next to dynamic content.
        *
        * @param {bool} nodeIsInline Set to true if the node is inline or false if it's not.
        */
        _createNewNode: function (nodeIsInline) {
            if (nodeIsInline) {
                return this.ed.getDoc().createTextNode('');
            }
            else {
                //The magic "_mce_bogus"-br is used to create a node last in the document that is removed if no content is entered.
                return this.ed.dom.create('p', null, '<br _mce_bogus="1" />');
            }
        },

        /**
        * Returns any dynamic content that is a parent to the given node.
        *
        * @param {Node} node The current node.
        */
        _getParentContentBlock: function (node) {
            return this._getParentNode(node, this._dynamicContentClass);
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Dynamic content plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.1
            };
        },

        /**
        * Moves the cursor before or after the given node and inserts a new node if necessary.
        *
        * @param {Node} node The current node.
        * @param {Boolean} moveAfter If the cursor should be placed after the node, set to true; otherwise false.
        */
        _moveToAdjacentNode: function (node, moveAfter) {
            nodeIsInline = !this.ed.dom.isBlock(node);
            var nodeToMoveTo;

            if (moveAfter) {
                if (nodeIsInline) {
                    nodeToMoveTo = node.nextSibling;
                }
                if (!nodeToMoveTo) {
                    nodeToMoveTo = $(node).next()[0];
                }
                if (!nodeToMoveTo || nodeToMoveTo.tagName == 'BR') {
                    nodeToMoveTo = this._createNewNode(nodeIsInline);
                    this.ed.dom.insertAfter(nodeToMoveTo, node);
                }

                this._moveSelection(nodeToMoveTo, true);
            }
            else {
                if (nodeIsInline) {
                    nodeToMoveTo = node.previousSibling;
                }
                if (!nodeToMoveTo) {
                    nodeToMoveTo = $(node).prev()[0];
                }
                if (!nodeToMoveTo) {
                    nodeToMoveTo = this._createNewNode(nodeIsInline);
                    node.parentNode.insertBefore(nodeToMoveTo, node);
                }
                this._moveSelection(nodeToMoveTo, false);
            }
        },

        /**
        * Event handler for key and paste events used to prevent almost all actions.
        * Doesn't block arrow keys, pg up/down, and F1-F12. Also allows del and backspace.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {Event object} e Event object.
        */
        _block: function (ed, e) {
            var k = e.keyCode;

            if ((k > 32 && k < 41) || (k > 111 && k < 124) || e.ctrlKey || e.metaKey) {
                return;
            }
            //FF still triggers keypress even we have cancelled the corresponding keydown. We must explicitly cancel keypress
            else if ((e.type == "keydown") && (k == 13 || k == 46 || k == 8)) {
                return;
            }

            return tinymce.dom.Event.cancel(e);
        },

        /**
        * Inserts a new paragraph before the dynamic content.
        */
        _handleEnterKey: function () {
            //TODO: should create a text block or do nothing if the selected content is an inline dynamic content.
            var pc = this._getParentContentBlock(this.ed.selection.getNode());
            if (!this._isNull(pc)) {
                this._ensureInitialUndoLevel();
                this._insertParagraphBefore(pc);
                this._onContentChanged();
            }
        },

        /**
        * Special handling of delete keys to handle deletion of dynamic content.
        *
        * @param {node} dc The currently selected dynamic content or null.
        */
        _handleDeleteKeys: function (dc, deleteBackward) {
            if (!this._isNull(dc)) {
                this._ensureInitialUndoLevel();
                this.ed.dom.remove(dc);
                //Trigger nodeChanged to reset button states and move outside of input block mode.
                this._onContentChanged();
                return true;
            }
            else {
                if (!this.ed.selection.isCollapsed()) {
                    return false;
                }
                var range = this.ed.selection.getRng(true);
                if (deleteBackward) {
                    var caretPos = this._getCaretPosition(range.startContainer);
                    var previousSibling = this._getPreviousCaretNode(range.startContainer);
                    if (caretPos === 0 && !this._isNull(previousSibling) && this.ed.dom.hasClass(previousSibling, this._dynamicContentClass)) {
                        //If cursor is next to dynamic content, prevent deletion of parts of it and select it instead.
                        this._selectContentBlock(this.ed, previousSibling);
                        return true;
                    }
                }
                else {
                    var caretPos = this._getCaretPosition(range.endContainer);
                    var nextSibling = this._getNextCaretNode(range.endContainer, caretPos);
                    if (this._caretIsAtTheEnd(range.endContainer, caretPos) && !this._isNull(nextSibling) && this.ed.dom.hasClass(nextSibling, this._dynamicContentClass)) {
                        if (caretPos == 0) { //selection node is empty, then delete it
                            this.ed.dom.remove(this.ed.selection.getNode());
                        }
                        //If cursor is next to dynamic content, prevent deletion of parts of it and select it instead.                        
                        this._selectContentBlock(this.ed, nextSibling);
                        return true;
                    }
                }
            }
            return false;
        },

        _url: null, // the url this plugin was initialized with
        _previewedDynamicContent: null, // used to synchronize display of the preview popup
        _previewVisible: false, // whether a preview frame has been created in the parent frame
        _lastSelection: null,

        /**
        * Creates a popup and shows the preview dynamic content frame.
        *
        * @param {Node} node The node or child node to show the preview for.
        */
        _showPreview: function (node) {
            if (this._hidePreview()) {
                $(window['epi-dcPreview'].document.documentElement).remove();
            }
            var dcHtml = $("<div/>", this.document).append($(node).closest(".epi_dc").clone()).html();
            this._previewedDynamicContent = dcHtml;

            var t = this;
            setTimeout(function () {
                if (dcHtml !== t._previewedDynamicContent) {
                    return;
                }

                t._previewVisible = true;
                t._lastSelection = node;

                // create a preview frame next to the dynamic content element in the parent frame
                var iframe = $("iframe", t.ed.container);
                var popupPosition = t._getPositionInParentFrame(node, t.ed.contentWindow, iframe, window);

                var dialogWidth = t.ed.settings.editorwidth ? t.ed.settings.editorwidth : t.ed.settings.width;

                var frame = t._createPopupIframe(popupPosition, "epi-dcPreview", dialogWidth,
                /*buttons:*/[{ onclick: function () { t._openEditDialog(t.ed); t._hidePreview(); }, text: t.ed.translate("epidynamiccontent.edit") },
                             { onclick: function () { t._hidePreview(); }, text: t.ed.translate("epidynamiccontent.close") }
                ]);

                var previewUrl = t._getPreviewUrl();
                var previewData = { "DynamicContent": dcHtml }; // a little hack to retrieve "outerHTML"
                t._postFrameTo(frame, previewUrl, previewData);

                $(window).bind("resize.dynamicContent",
                    function (e) {
                        var position = t._getPositionInParentFrame(node, t.ed.contentWindow, iframe, window);
                        var previewBorder = $("#epi-dcPreview");
                        previewBorder.css(position);
                    }
                );

                // add some text below the preview
                //TODO: Reimplement this if possible
                //$("#dcPreviewDescription").html($(dcSpan).html().replace(/[{}]/g, ""));

            }, 250);
        },

        /**
        * Gets an url to the preview page that depends on new page or existing page.
        */
        _getPreviewUrl: function () {
            var url = "Editor/Dialogs/DynamicContentPreview.aspx";
            var ctx = jQuery.extend({ "content_css": this.ed.settings.content_css }, this.ed.settings.epi_page_context);
            $(document).trigger("epi.loading.page_context", ctx);

            return EPi.ResolveUrlFromUI(url + "?" + jQuery.param(ctx));
        },

        /**
        * Opens the edit dialog for the dynamic content when a user has clicked on the inline edit button.
        */
        _openEditDialog: function () {
            this.ed.focus();
            var dc = this._getParentContentBlock(this._lastSelection);
            this.ed.selection.select(dc);
            tinyMCE.execCommand('mceEPiDynamicContent');
        },

        /**
        * Removes the dynamic content preview frame.
        */
        _hidePreview: function () {
            this._previewedDynamicContent = null;
            if (!this._previewVisible) {
                return false;
            }
            this._previewVisible = false;
            this._lastSelection = null;
            $("#epi-dcPreview").hide();

            $(window).unbind("resize.dynamicContent");

            return true;
        },

        /**
        * Gets the absolute position in the parent farme next to an element inside the editor frame
        */
        _getPositionInParentFrame: function (element, childWindow, childIframe, parentWindow) {
            // calculate popup position relative to element, frame position and scroll in both frames
            var framePosition = $(childIframe).offset();
            var dcPosition = $(element).position();
            var windowScroll = { y: $(parentWindow).scrollTop(), x: $(parentWindow).scrollLeft() };
            var frameScroll = { y: Math.max($(childWindow.document.body).scrollTop(), $(childWindow.document.documentElement).scrollTop()), x: $(childWindow.document.body).scrollLeft() };
            var position = {
                top: framePosition.top + dcPosition.top - windowScroll.y - frameScroll.y,
                left: framePosition.left + dcPosition.left + Math.min($(element).width() + 10, $(childIframe).width()) - windowScroll.x - frameScroll.x
            };

            // deal with browser quirks
            if (windowScroll.y > 0 && (tinymce.isGecko || tinymce.isWebKit)) {
                position.top += frameScroll.y;
            }
            if (tinymce.isIE) {
                position.top += windowScroll.y;
            }

            // avoid disappearing off-screen
            var $parentWindow = $(parentWindow);
            var lowerBound = position.top + 300;
            var windowBottomY = windowScroll.y + $parentWindow.height();
            if (lowerBound > windowBottomY) {
                position.top = Math.max(windowScroll.y, position.top - lowerBound + windowBottomY);
            }

            var dialogWidth = this.ed.settings.editorwidth ? this.ed.settings.editorwidth : this.ed.settings.width;
            lowerBound = position.left + parseInt(dialogWidth, 10) + 24; //24 = dialog padding
            var windowRightX = windowScroll.x + $parentWindow.width();
            if (lowerBound > windowRightX) {
                position.left = Math.max(windowScroll.x, position.left - lowerBound + windowRightX);
            }

            return position;
        },

        /**
        * Creates a popup iframe
        */
        _createPopupIframe: function (popupPosition, frameName, width, buttons) {
            var t = this;
            var previewBorder = $("#" + frameName).show();
            if (previewBorder.length === 0) {
                previewBorder = $("<div id='" + frameName + "' class='epi-dcPreviewBorder'><iframe frameborder='0' style='height:300px;' class='episystemiframe' name='" + frameName + "' src=''></iframe><div id='epi-dcPreviewButtons'></div><div id='epi-dcPreviewDescription'></div></div>")
                    .appendTo(window.document.body);
                $(window.document.body).click(function () { t._hidePreview(); });
            }
            previewBorder.css({ position: "absolute", top: popupPosition.top, left: popupPosition.left, width: width })
                .children("iframe").css({ height: "250px", width: "100%" });

            if (buttons) {
                $("#epi-dcPreviewButtons").html("");
                for (var i in buttons) {
                    $("<span class='epi-cmsButton'><input type='button' value='" + buttons[i].text + "'/></span>")
                        .appendTo("#epi-dcPreviewButtons")
                        .click(buttons[i].onclick);

                    //previewBorder.find("#dcPreviewButtons").unbind().click(buttonOptions.onclick).attr("value", buttonOptions.text);
                }
            }

            // post the dynamic content to be rendered by the server
            var frame = window[frameName];
            return frame;
        },

        /**
        * Writes some content to a frame and performs a http post
        */
        _postFrameTo: function (frame, url, data) {
            var formString = '<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;background:#fff url(' + EPi.ResolveUrlFromUtil("../App_Themes/Default/Images/General/AjaxLoader.gif") + ') no-repeat 50% 50%"><head><title></title></head><body>';
            formString += '<form action="' + url + '" method="post">';
            var key;
            for (key in data) {
                formString += '<input name="' + key + '" id="' + key + '" type="hidden" />';
            }
            formString += '</form></body></html>';

            frame.document.write(formString);

            for (key in data) {
                frame.document.getElementById(key).value = data[key];
            }
            frame.document.forms[0].submit();
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epidynamiccontent', tinymce.plugins.epidynamiccontent);
} (tinymce, epiJQuery));
