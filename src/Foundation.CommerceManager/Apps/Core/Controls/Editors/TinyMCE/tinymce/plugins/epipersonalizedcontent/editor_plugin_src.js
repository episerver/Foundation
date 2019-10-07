(function (tinymce, $) {
    tinymce.create('tinymce.plugins.epipersonalizedcontent', {
        _onInitCalled: false, // A flag to signal if the editors onInit event has occured

        /**
        * Initializes the plugin, this will be executed after the plugin has been created.
        * This call is done before the editor instance has finished it's initialization so use the onInit event
        * of the editor instance to intercept that event.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {
            var t = this;
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

            // Register button - must be done in standard init since it seems to be to late in callback
            ed.addButton('epipersonalizedcontent', {
                title: 'epipersonalizedcontent.epipersonalizedcontent_desc',
                cmd: 'epipersonalizedcontent',
                "class": "mce_epipersonalizedcontent"
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
            //We extend this plug.in with methods that are common for both personalized content and dynamic content (defined in epicontentblockutilities.js).
            $.extend(this, epiContentBlockUtilities);
            var t = this;
            t._pcIsSelecting = false;
            t._pcInClipboard = false;
            t._pcDragging = false;
            t._selectedPcHeader = null;
            t._activePcBlock = null;
            t._personalizedContentHeaderClass = ed.getParam("epipersonalizedcontent_headercssclass", "epi_pc_h");
            t._personalizedContentFooterClass = ed.getParam("epipersonalizedcontent_footercssclass", "epi_pc_f");
            t._personalizedContentClass = ed.getParam("epipersonalizedcontent_cssclass", "epi_pc");
            t._personalizedContentHolderClass = ed.getParam("epipersonalizedcontent_holdercssclass", "epi_pc_content");
            t.ed = ed;
            t._disabled = false; // Initially no controls are disabled.
            t._enabledControls = ed.getParam("epipersonalizedcontent_enabledcontrols", "");

            ed.addCommand('epipersonalizedcontent', function () {
                if (!t._validateAndCorrectSelection()) {
                    ed.windowManager.alert(ed.translate('epipersonalizedcontent.invalidselectionwarning'));
                    return;
                }
                var pc = t._getParentContentBlock(t._getCommonAncestor());

                var onDialogComplete = function (returnData, onCompleteArgs) {

                    if (!returnData) {
                        ed.windowManager.onClose.dispatch();
                        return;
                    }
                    var innerContent, html;
                    var contentNode;

                    if (returnData.removePersonalization) {
                        contentNode = ed.dom.select("." + t._personalizedContentHolderClass, pc)[0];
                        innerContent = contentNode.innerHTML;
                        //Remove container block an insert content instead
                        ed.dom.setOuterHTML(pc, innerContent);
                    }
                    else if (pc) {
                        contentNode = ed.dom.select("." + t._personalizedContentHolderClass, pc);
                        innerContent = $(contentNode).html();
                        html = returnData.replace("[personalizedContentPlaceHolder]", innerContent);
                        //Update existing personalized content block
                        ed.dom.setOuterHTML(pc, html);
                    }
                    else {
                        //Create new personalized content block
                        innerContent = ed.selection.getContent();
                        if (innerContent === "") {
                            innerContent = '<p><br mce_bogus="1" /></p>';
                        }
                        else {
                            //Since there is currently no way to get selected content in "raw" format we need to trigger
                            //the event handlers run when calling set content. This should be replaced when tinymce has a better
                            //support for inserting/replacing content (splitting p tag etc). This is connected to the ed.onSetContent.dispatch(ed); below.
                            var o = { content: innerContent };
                            ed.onBeforeSetContent.dispatch(this, o);
                            innerContent = o.content;
                        }
                        html = returnData.replace("[personalizedContentPlaceHolder]", innerContent);
                        html = html.replace(t._personalizedContentHolderClass, t._personalizedContentHolderClass + " mce_bogus");
                        t._insertContent(html);

                        contentNode = ed.dom.select("div.mce_bogus")[0];
                        t._normalizeContentNode(contentNode);
                        ed.dom.removeClass(contentNode, "mce_bogus");
                        var lastChild = $(contentNode).children(":last")[0];

                        //Convert the content back to editor form to handle media elements treated as images etc.
                        ed.onSetContent.dispatch(ed);

                        t._moveSelection(lastChild, false);
                    }
                    //Wrap all changes as one undo level
                    ed.undoManager.add();
                    //Trigger node change event to update button status etc.
                    ed.nodeChanged();

                    ed.windowManager.onClose.dispatch();
                };

                var dialogFeatures = { width: 600, height: 450 };
                var dialogURL = EPi.ResolveUrlFromUI('Editor/Dialogs/PersonalizedContent.aspx');

                var parameters = {
                    allContentGroups: t._getAllContentGroups()
                };

                if (!tinymce.isIE) {
                    var currentNode = ed.selection.getNode();
                    var shouldExpandSelection = currentNode.nodeName !== "BODY" && currentNode.nodeName !== "LI" && currentNode.nodeName !== "TD";
                    if (shouldExpandSelection && ed.selection.getStart() === ed.selection.getEnd()) {
                        //In Firefox, selecting for instance a h1 tag by mouse drag will only select the text content and not the element.
                        //Therefore we expand the selection to the parent node if the selection starts and ends in the same node.
                        //We don't want to expand up to the body node of if were inside a table cell or list item.
                        ed.selection.select(currentNode);
                    }
                }

                if (pc) {
                    parameters.groups = ed.dom.getAttrib(pc, "data-groups", "");
                    parameters.contentGroup = ed.dom.getAttrib(pc, "data-contentgroup", "");
                } else if (ed.selection.isCollapsed()) {
                    // We have no personalized content selected and no active selection. The editor 
                    // has likely been activated by a click on the PC button and the selection lost.
                    return;
                }

                ed.windowManager.onOpen.dispatch();
                EPi.CreateDialog(dialogURL + "?" + $.param(parameters), onDialogComplete, null, null, dialogFeatures);
            });

            /**
            * Adds event handlers.
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            */
            var pluginInit = function (ed) {
                // Prevent drag & drop of content into personalized content.
                // Note: This makes use of jQuery to overcome differences in native event object.
                var editorBody = $(ed.getBody());
                if (editorBody.length) {
                    $(editorBody).bind("drop", function (e) {
                        var dropTarget = e.srcElement || e.originalTarget;

                        //work around for IE8 or ealier versions
                        //dragover event seems never be fired on pc_content
                        if (dropTarget === editorBody[0] && dropTarget.ownerDocument.elementFromPoint) {
                            //IE8 or earlier version seem not to fire drop event on non conteneditable element (pc) but its body
                            dropTarget = dropTarget.ownerDocument.elementFromPoint(e.clientX, e.clientY);
                        }

                        //if it is still editor's body
                        if (dropTarget === editorBody[0]) {
                            //drop target can be the caret node
                            dropTarget = ed.selection.isCollapsed() ? ed.selection.getNode() : dropTarget;
                        }

                        //dropping inside PC block but not inside PCHolder should be prevented                        
                        if ($(dropTarget).closest("." + t._personalizedContentClass).length && (!$(dropTarget).closest("." + t._personalizedContentHolderClass).length)) {
                            return false;
                        }

                        //prevent nested pc
                        if (t._pcDragging && $(dropTarget).closest("." + t._personalizedContentClass).length) {
                            return false;
                        }
                    });

                    editorBody.bind('dragstart', function (e) {
                        if (t._selectionContainsPC()) {
                            t._pcDragging = true;
                        }
                        else {
                            t._pcDragging = false;
                        }
                    });

                    editorBody.bind('dragend', function (e) {
                        if (t._pcDragging) {
                            t._pcDragging = false;
                        }
                    });

                    editorBody.bind('cut', function (e) {
                        if (t._selectionContainsPC()) {
                            t._pcInClipboard = true;
                        }
                        else {
                            t._pcInClipboard = false;
                        }
                    });

                    editorBody.bind('copy', function (e) {
                        if (t._selectionContainsPC()) {
                            t._pcInClipboard = true;
                        }
                        else {
                            t._pcInClipboard = false;
                        }
                    });

                    editorBody.bind('paste', function (e) {
                        //prevent nested pc
                        //tinymce onPaste is not appropriate since the event object target on a dummy node called mcepaste
                        if (t._pcInClipboard && $(e.srcElement || e.originalTarget).closest('.' + t._personalizedContentClass).length) {
                            //flag to cancel tinymce's paste event as well
                            t._toCancelPasting = true;
                            return false;
                        }
                    });
                }
                $(ed.dom.doc).click(function (e) {
                    if (ed.dom.hasClass(e.target, "epi_pc_editBtn") && (e.button === 0)) { //Left button only, IE never gets here so we can trust that 0 is left button
                        t._handleButtonsClick(e);
                    }
                });
                if (tinymce.isIE) {
                    $(ed.getBody()).blur(function (e) {
                        //IE triggers blur on body element
                        t._onEditorDeactivated();
                    });
                    ed.dom.bind(ed.getDoc(), 'controlselect', function (e) {
                        if (t._isIE9StandardMode()) { //IE9 should behave W3C way
                            return true;
                        }

                        var n = e.target;
                        //Not allow control select on pc header's sub-elements or pc footer
                        if (!t._pcIsSelecting && (ed.dom.getParent(n, '.' + t._personalizedContentHeaderClass) || ed.dom.getParent(n, '.' + t._personalizedContentFooterClass))) {
                            //indicate that this personalized content is being selected, will be done when the timeout collapsed.
                            //to prevent duplicated processing in nodeChange event handler
                            t._pcIsSelecting = true;
                            setTimeout(function () {
                                var pc = t._getParentContentBlock(n);
                                //make the parent content block selected if the content header clicked
                                if (pc) {
                                    t._handlePCSelection(pc);

                                    //selection processing is done
                                    t._pcIsSelecting = false;
                                }
                            }, 1);
                            return false;
                        }
                    });
                }
                else {
                    $(ed.dom.doc).blur(function (e) {
                        //Firefox triggers blur on document element
                        t._onEditorDeactivated();
                    });
                }
            };

            // If the editors onInit event has already been raised, just run the method. Otherwise attach.
            if (t._onInitCalled) {
                pluginInit(ed);
            } else {
                ed.onInit.add(pluginInit);
            }


            /**
            * Gets executed before DOM to HTML string serialization, remove any marker classes and empty nodes in beginning and end of document.
            * Please note that this affects what is shown in the html plug-in dialog.
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            * @param {options} o Options for the event.
            */
            ed.onPreProcess.add(function (ed, o) {
                // State get is set when contents is extracted from editor
                if (o.get) {
                    ed.dom.removeClass(ed.dom.select('div.mceSelected'), 'mceSelected');
                    ed.dom.setAttrib(ed.dom.select('div.' + t._personalizedContentClass), 'contentEditable', null);
                }
            });

            /**
            * Add a node change handler, selects the button in the UI when the selection is not collapsed
            * or inside an existing personalized content block.
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            * @param {Control manager} cm Control manager for the node change event.
            * @param {node} n The current node.
            * @param {bool} co If the selection is collapsed or not.
            * @param {options} options Additional options like if it's the node change that is run on initialization for the editor.
            */
            ed.onNodeChange.addToTop(function (ed, cm, n, co, options) {
                if (options.initial === 1) {
                    // on the very first chance, just init the personalized content button, don't need to do any further thing
                    t._showButton(ed, cm, false, false);
                    return;
                }

                // Listening to onNodeChange very early (addToTop) since we're fiddling with
                // other plugin states and possibly changing the current selection.

                // Because click event is not fired inside a controlselection in IE, we need to check if a button was clicked
                if (tinymce.isIE && co && ed.dom.hasClass(n, "epi_pc_editBtn")) {
                    t._handleButtonsClick({ target: n });
                }

                if (!co) {
                    //Node is not always the common parent node depending on selection when not collapsed.
                    n = t._getCommonAncestor();
                }

                var s = ed.selection;
                var sc = t._getParentContentBlock(s.getStart());
                var ec = t._getParentContentBlock(s.getEnd());
                var c = sc || ec;

                var pc;
                var pcIsPartialSelected = false;
                var overlapsMarginOf2Blocks = (sc || ec) && (sc !== ec);
                // match start or end selection in heading to allow cursor passing through
                var overlapsHeadingMargin = t._overlapsHeadingMargin(s);
                if (overlapsMarginOf2Blocks || overlapsHeadingMargin) {
                    pc = sc || ec;
                    n = pc;
                    pcIsPartialSelected = true;
                } else {
                    pc = t._getParentContentBlock(n);
                }

                //set select for Firefox, when we do not have oncontrolselect event, or for the case oncontrolselect event is not fired (IE bug)
                //Note: When click on a selected div, FF reports event on the div while IE reports event on the inner text. Must check the difference here.
                //Also make the pc block selected if the cursor is somewhere among header, footer, and content but not inside any of them. This case = n is epi_pc and current selected is collapsed 
                if (pc && !t._pcIsSelecting && (
					(ed.dom.hasClass(n, t._personalizedContentClass) && (ed.selection.isCollapsed() || ed.selection.getSel().type == 'Control')) || // IE: after empty control selection, collapsed is still false
                    ed.dom.getParent(n, '.' + t._personalizedContentHeaderClass) ||
                    ed.dom.getParent(n, '.' + t._personalizedContentFooterClass) ||
                    pcIsPartialSelected)) {

                    t._pcIsSelecting = true;
                    setTimeout(function () {
                        //Fix: remove blue glitch on pc's header in IE (header's text selection remains after control selection taken place)
                        if (pcIsPartialSelected && tinymce.isIE) {
                            var tempSelection = sc ? s.getEnd() : (ec ? s.getStart() : null);
                            if (tempSelection) {
                                ed.selection.select(tempSelection);
                            }
                        }
                        t._handlePCSelection(pc);
                        t._pcIsSelecting = false;
                    }, 1);
                }

                if (ed.dom.hasClass(n, t._personalizedContentHolderClass)) {
                    //We have somehow gotten into the div and not it's content. Validate that there exists at least one element in content node.

                    // Adds a new p tag and wraps any existing text value inside it.
                    t._normalizeContentNode(n);
                    if (co) {
                        // Only move cursor when selection is collapsed since selecting the first paragraph in IE results
                        // in the content holder node being the selected node.
                        t._moveSelection($(n).children(":last")[0], false);
                    }
                }

                var pcHeader = t._getParentContentBlockHeader(n) || t._getPCContentBlockHeader(n);
                t._updateSelectedElements(n, pc, pcHeader);

                var allowEvents = (pcHeader === null);
                t._updateCommandState(pcHeader === null);

                var enablePCButton = !t._isNull(pc) || !co;
                t._showButton(ed, cm, enablePCButton, !t._isNull(pc));

                //Update undo redo since we will block further events which would normally update them.
                if (!allowEvents) {
                    t._updateUndoRedoButtons(cm);
                }

                //Always let other plug-in set initial state on first event
                return allowEvents;
            });

            /**
            * Paste event handler.
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            * @param {Event} e The event.
            */
            ed.onPaste.add(function (ed, e) {
                if (t._toCancelPasting) {
                    t._toCancelPasting = false;
                    return tinymce.dom.Event.cancel(e);
                }

                //Paste event doesn't cause onNodeChange
                //And clipboard data is inserted to ed after this event
                //So, we need to use a timeout to clean mceSelected up.
                setTimeout(function () {
                    var mceSelectedPCNodes = ed.dom.select('.mceSelected.' + t._personalizedContentClass);
                    //There cannot be any PC being selected after paste
                    for (var i = 0; i < mceSelectedPCNodes.length; i++) {
                        ed.dom.removeClass(mceSelectedPCNodes[i], 'mceSelected');
                    }
                }, 1);
            });

            /**
            * Key up event handlers. Handle special caret position in PC block here because it is too early on keydown and too late on node changed
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            * @param {Event} e The event.
            */
            ed.onKeyUp.addToTop(function (ed, e) {
                var k = e.keyCode;
                if (!(k >= 37 && k <= 40)) {
                    return;
                }

                var commonAncestor = t._getCommonAncestor();
                var pc = t._getParentContentBlock(commonAncestor);

                if (!pc) {
                    return;
                }

                var caretPos = (pc === commonAncestor) ? t._getCaretPosition(commonAncestor) : 0;
                var caretIsBeforeHeader = (pc === commonAncestor) && (caretPos == 0);
                var caretIsBeforeFooter = (pc === commonAncestor) && (caretPos > 0);
                var caretIsInsideHeader = ed.dom.getParent(commonAncestor, '.' + t._personalizedContentHeaderClass);
                var caretIsInsideFooter = ed.dom.getParent(commonAncestor, '.' + t._personalizedContentFooterClass);

                if (k == 37 || k == 38) {           //Up or Left
                    if (caretIsBeforeHeader) {
                        //Move up out of the pc block
                        t._moveUp(pc, false);
                    }
                    else if (caretIsBeforeFooter || caretIsInsideFooter) {
                        //Move up into content
                        t._moveUp(pc, true);
                    }
                }
                else if (k == 39 || k == 40) {      //Down or Right
                    if (caretIsBeforeFooter || caretIsInsideFooter) {
                        //Move down out of the pc block
                        t._moveDown(pc, false);
                    }
                }
            });

            /**
            * Key event handlers - need to this add to top to be able to prevent delete key in IE.
            *
            * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
            * @param {Event} e The event.
            */
            ed.onKeyDown.addToTop(function (ed, e) {
                var k = e.keyCode;
                if (!((k >= 37 && k <= 40) || k == 8 || k == 46 || k == 13)) {
                    //block typing if caret is somehow moved into restricted area
                    //pc is surely not selected because if it is selected, _block function already did blocking keydown event
                    if (ed.dom.getParent(ed.selection.getStart(), '.' + t._personalizedContentClass)
                        && !ed.dom.getParent(ed.selection.getStart(), '.' + t._personalizedContentHolderClass)
                        && !e.ctrlKey
                        && !e.metaKey) {
                        return tinymce.dom.Event.cancel(e);
                    }
                    return;
                }
                //Allow expanding selection with shift and arrow keys
                if (e.shiftKey && k >= 37 && k <= 40) {
                    return;
                }

                var pc, commonAncestor = t._getCommonAncestor();

                //If current selection is control selection
                if (ed.selection.getRng() && ed.selection.getRng().item) {
                    var selectedControl = ed.selection.getRng().item(0);
                    pc = ed.dom.hasClass(selectedControl, t._personalizedContentClass) ? selectedControl : null;
                    if (pc) {
                        pc.removeAttribute("contentEditable");
                        if (document.selection) {
                            document.selection.empty();
                        }
                    }
                }
                else {
                    pc = t._getParentContentBlock(commonAncestor);
                }

                if (!pc) {
                    if (k == 46 || k == 8) {
                        if (t._validateSelectionBeforeDeletion()) {
                            return tinymce.dom.Event.cancel(e);
                        }
                        else if (ed.selection.isCollapsed()) {
                            // check if user press Bksp when the caret is being at beginning of the element after a pc
                            // or if user press Del when the caret is being at the end of the element before a pc
                            var c = ed.selection.getNode();

                            var previousCaretNode = t._getPreviousCaretNode(c);
                            if (k == 8 && previousCaretNode && ed.dom.hasClass(previousCaretNode, t._personalizedContentClass)) {
                                if (t._getCaretPosition(c) == 0) {
                                    t._moveUp(previousCaretNode, true);
                                    return tinymce.dom.Event.cancel(e);
                                }
                            }

                            var nextCaretNode = t._getNextCaretNode(c);
                            if (k == 46 && nextCaretNode && ed.dom.hasClass(nextCaretNode, t._personalizedContentClass)) {
                                var caretPos = t._getCaretPosition(c);
                                if (t._caretIsAtTheEnd(c, caretPos)) {
                                    t._moveDown(nextCaretNode, true);
                                    if (caretPos == 0) { //c is empty, then delete it
                                        ed.dom.remove(c);
                                    }
                                    return tinymce.dom.Event.cancel(e);
                                }
                            }
                        }
                    }
                    return;
                }

                var actionTaken = false;

                //When the whole pc block is being selected, handle arrow keys. In these cases, it is too late to handle on key up
                if ((pc === commonAncestor) && (k == 39 || k == 40)) {
                    actionTaken = t._moveDown(pc, true);
                }
                else if (k == 37 || k == 38) {
                    if (pc === commonAncestor) {
                        actionTaken = t._moveUp(pc, false);
                    }
                    //IE allows caret before the first paragraph inside pc_content. By that, if user hits Enter, pc_content will be splited into 2 divs and pc's structure will be broken up
                    //Avoid this unwanted behavior by moving caret into the header, then the whole pc will consequently be selected
                    //Note: the function getPreviousCaretNode return pc_header (not the actual caret destination) like it should. We use the function combine with IE to detect the situation
                    else if (tinymce.isIE) {
                        var previousCaretNode = t._getPreviousCaretNode(commonAncestor);
                        //We are at first position in pc_content and about to move up
                        if (ed.dom.hasClass(previousCaretNode, t._personalizedContentHeaderClass) && t._getCaretPosition(commonAncestor) == 0) {
                            t._moveSelection(previousCaretNode, true);
                            actionTaken = true;
                        }
                    }
                }
                else if (k == 46 || k == 8) {
                    actionTaken = t._handleDeletion(pc, k == 8); //if deletion is caused by BkSp, we should check possibility of breaking pc's heading.
                }
                else if (k == 13) {
                    actionTaken = t._handleEnterKey(pc);
                }

                if (actionTaken) {
                    return tinymce.dom.Event.cancel(e);
                }
            });
        },

        /**
        * Set enabled and active state of the personalized content button
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {Control manager} cm Control manager.
        * @param {bool} enabled If true, the button is enabled; otherwise disabled.
        * @param {bool} active If true, the button is set active; otherwise inactive.
        *
        **/
        _showButton: function (ed, cm, enabled, active) {
            var isHidden = ed.dom.isHidden(ed.container);
            cm.setDisabled("epipersonalizedcontent", isHidden || !enabled);
            cm.setActive("epipersonalizedcontent", !isHidden && active);
        },

        /**
        * Handle action buttons in dc header
        *
        * @param {Event} e The event object.
        */
        _handleButtonsClick: function (e) {
            // We need to focus the editor since selection since the editor might not be activated yet and 
            // execCommand calls focus which causes our selection to be lost.
            this.ed.focus();
            if (!tinymce.isIE) this.ed.selection.select(e.target);
            tinyMCE.execCommand('epipersonalizedcontent');
            return tinymce.dom.Event.cancel(e);
        },

        /**
        * Handle selection of personalized content block
        *
        * @param {Element} pc The personalized content block
        */
        _handlePCSelection: function (pc) {
            var t = this;
            t._selectContentBlock(t.ed, pc);
            var deselect = function (e) {
                t.ed.dom.unbind(t.ed.dom.doc, 'click', deselect);

                //if pc has been moved into clipboard, do nothing
                if (!pc || !pc.parentNode) {
                    return;
                }
                t._deselectContentBlock(t.ed, pc);

                //reset selected header and remove key blocking
                t._updateSelectedElements(pc, pc, null);
                t._updateCommandState(true);

                //if mouse is on the pc block, move the caret to the content
                var eventSrc = e.originalTarget || e.srcElement;
                if (t.ed.dom.getParent(eventSrc, '.' + t._personalizedContentClass)) {
                    t._moveDown(pc, true);

                    return tinymce.dom.Event.cancel(e);
                }
            };
            t.ed.dom.unbind(t.ed.dom.doc, 'click', deselect);
            t.ed.dom.bind(t.ed.dom.doc, 'click', deselect);
        },

        /**
        * Event handler that is executed on enter key pressed. Makes sure that we do not get any stand alone text nodes
        * when pressing enter inside the pc and adds a paragraph before the pc when the header is selected.
        *
        * @param {Personalized content element} pc The personalized content that is currently active.
        */
        _handleEnterKey: function (pc) {
            var pcHolder = this._getParentContentBlockHolder(this._getCommonAncestor());
            if (!this._isNull(pcHolder)) {
                //Make sure that we wrap any text fragments directly in the content div in p tags before the enter action is performed.
                this._normalizeContentNode(pcHolder);
            }
            else {
                pcHeader = this._getParentContentBlockHeader(this._getCommonAncestor()) || this._getPCContentBlockHeader(this._getCommonAncestor());
                if (!this._isNull(pcHeader)) {
                    this._ensureInitialUndoLevel();
                    this._insertParagraphBefore(pc);
                    this._onContentChanged();
                    return true;
                }
            }
            return false;
        },

        /**
        * Reset state when leaving focus from editor.
        */
        _onEditorDeactivated: function () {
            if (!this._isNull(this._selectedPcHeader)) {
                this.ed.dom.removeClass(this._selectedPcHeader.parentNode, 'mceSelected');
                this._selectedPcHeader = null;
            }
        },

        /**
        * Run on node change to handle selection styles and normalize a content block when leaving it.
        *
        * @param {Current node} n The currently selected node.
        * @param {Personalized content element} pc The personalized content that is currently active.
        * @param {Personalized content header} pc The personalized content header that is currently active.
        */
        _updateSelectedElements: function (n, pc, pcHeader) {
            if (this._selectedPcHeader === pcHeader) {
                //We are still in same header or still outside of a header
                return;
            }

            if (!this._isNull(this._selectedPcHeader)) {
                this.ed.dom.removeClass(this._selectedPcHeader.parentNode, 'mceSelected');
                this._selectedPcHeader = null;
            }

            var pcContent = this._getParentContentBlockHolder(n);
            if (this._activePcBlock === pcContent) {
                //We are still in same content block or still outside one. Do nothing.
            }
            else {
                if (!this._isNull(this._activePcBlock)) {
                    //Leaving a content node. Repair to wrap any loose text nodes.
                    this._normalizeContentNode(this._activePcBlock);
                    this._activePcBlock = null;
                }
                if (!this._isNull(pcContent)) {
                    //New content node. Add enter key-handler.
                    this._activePcBlock = pcContent;
                }
            }

            if (pcHeader !== null) {
                this.ed.dom.addClass(pc, 'mceSelected');
                this._selectedPcHeader = pcHeader;
            }
        },

        /**
        * Validates that the current selection does not contain parts of a personalized content.
        */
        _validateAndCorrectSelection: function () {
            var parentNode = this._getCommonAncestor();
            var startNode = this.ed.selection.getStart();
            var endNode = this.ed.selection.getEnd();
            if (startNode !== endNode) {
                if (this._isInvalidPartialSelectionElement(startNode) || this._isInvalidPartialSelectionElement(endNode)) {
                    if (parentNode.nodeName == "TR" || parentNode.nodeName == "TH" || parentNode.nodeName == "TBODY" || parentNode.nodeName == "THEAD") {
                        parentNode = this.ed.dom.getParent(parentNode, 'table');
                        this.ed.selection.select(parentNode);
                        return true;
                    }
                    else if (parentNode.nodeName == "TABLE" || parentNode.nodeName == "UL" || parentNode.nodeName == "OL") {
                        this.ed.selection.select(parentNode);
                        return true;
                    }
                    //We have a selection where the start or end is inside a table or list but the selection goes outside of the list or table
                    return false;
                }
            }

            var parentNode = this._getCommonAncestor();
            if (!this.ed.dom.isBlock(parentNode)) {
                var t = this;
                parentNode = this.ed.dom.getParent(parentNode, function (node) {
                    return t.ed.dom.isBlock(node);
                });
            }

            if (!this._isNull(parentNode)) {
                //We have found a parent node that is a block element
                if (!this.ed.dom.hasClass(parentNode, this._personalizedContentClass)) {
                    var personalizedContents = tinyMCE.activeEditor.dom.select("div." + this._personalizedContentClass, parentNode);
                    if (personalizedContents.length > 0) {
                        return false;
                    }
                }
            }
            else {
                //We have found a parent node that is a block element. This is probably the body element.
                return this.ed.selection.getContent().match(this._personalizedContentClass) === null;
            }
            return true;
        },

        /**
        * Validates that the node is not invalid as start or end of an selection unless the selection starts and ends within the node.
        *
        * @param {Element} node The node that you want to validate.
        */
        _isInvalidPartialSelectionElement: function (node) {
            return node.nodeName == "TD" || node.nodeName == "TR" || node.nodeName == "TH" ||
            node.nodeName == "TBODY" || node.nodeName == "THEAD" || node.nodeName == "LI";
        },

        /**
        * Check if selection contains any pc block
        */
        _selectionContainsPC: function () {
            var pcMatchRule = new RegExp('<div.*' + this._personalizedContentClass
                + '.*>(\\n|.)*<div.*' + this._personalizedContentHeaderClass
                + '.*>(\\n|.)*<div.*' + this._personalizedContentHolderClass
                + '.*>(\\n|.)*<div.*' + this._personalizedContentFooterClass
                + '.*>');
            return this.ed.selection.getContent().match(pcMatchRule);
        },

        /**
        * Gets the common ancestor for the current selection.
        */
        _getCommonAncestor: function () {
            //sometime tinymce return incorrect common ancestor, we should return the correct value if we exactly know
            if (this.ed.selection.getStart() == this.ed.selection.getEnd()) {
                return this.ed.selection.getStart();
            }

            var commonAncestor = this.ed.selection.getRng(true).commonAncestorContainer;
            if (this._isNull(commonAncestor) || commonAncestor.tagName == "HTML") {
                return this.ed.getBody();
            }
            return commonAncestor;
        },

        /**
        * Gets the current content block or null if selection is not inside of one.
        *
        * @param {Current node} n The currently selected node.
        */
        _getParentContentBlock: function (node) {
            return this._getParentNode(node, this._personalizedContentClass);
        },

        /**
        * Tells whether the selection starts or ends in a personalized content heading.
        *
        * @param {Selection} s The tinyMCE selection object.
        */
        _overlapsHeadingMargin: function (s) {
            var start = $(s.getStart());
            var end = $(s.getEnd());
            var startsInHeading = start.closest(".epi_pc_h").length == 1;
            var endsInHeading = end.closest(".epi_pc_h").length == 1;
            if (tinymce.isIE) {
                var startIsBlock = start.is(".epi_pc");
                var endsInContent = end.closest(".epi_pc_content").length == 1;
                if (startIsBlock && endsInContent) {
                    // selection spans from start of block into content
                    return true;
                }
            }
            return (startsInHeading && !endsInHeading) || (!startsInHeading && endsInHeading);
        },

        /**
        * Gets the current content block holder or null if selection is not inside of one.
        *
        * @param {Current node} n The currently selected node.
        */
        _getParentContentBlockHolder: function (node) {
            return this._getParentNode(node, this._personalizedContentHolderClass);
        },

        /**
        * Gets the PC's content block holder or null if the given node is not a pc.
        *
        * @param {Current node} n The currently selected node.
        */
        _getPCContentBlockHolder: function (node) {
            if (this.ed.dom.hasClass(node, this._personalizedContentClass)) {
                return $(node).children('.' + this._personalizedContentHolderClass)[0];
            }
            else {
                return null;
            }
        },

        /**
        * Gets the current content block header or null if selection is not inside of one.
        *
        * @param {Current node} n The currently selected node.
        */
        _getParentContentBlockHeader: function (node) {
            return this._getParentNode(node, this._personalizedContentHeaderClass);
        },

        /**
        * Gets the PC's content block header or null if the given node is not a pc or pc footer.
        *
        * @param {Current node} n The currently selected node.
        */
        _getPCContentBlockHeader: function (node) {
            if (this.ed.dom.hasClass(node, this._personalizedContentClass)) {
                return $(node).children('.' + this._personalizedContentHeaderClass)[0];
            }
            else if (this.ed.dom.hasClass(node, this._personalizedContentFooterClass)) {
                return $(node.parentNode).children('.' + this._personalizedContentHeaderClass)[0];
            }
            else {
                return null;
            }
        },

        /**
        * Handler for delete and backspace keys.
        *
        * @param {Personalized content element} pc The personalized content that is currently active.
        * @param {bool} deleteBackward Indicate that the function should check if deletion can break pc's heading.
        */
        _handleDeletion: function (pc, deleteBackward) {
            if (this._deletePersonalizedContent(pc)) {
                return true;
            } else {
                //press del at the end of pc_content -> move down
                var c = this.ed.selection.getNode();

                if (!deleteBackward) {
                    var nextCaretNode = this._getNextCaretNode(c);
                    if (this.ed.dom.hasClass(nextCaretNode, this._personalizedContentFooterClass)) {
                        var caretPos = this._getCaretPosition(c);
                        if (this._caretIsAtTheEnd(c, caretPos)) { //costly call, should be shortcut
                            this._moveDown(pc, false);
                            if (caretPos == 0) { //c is empty, then remove it
                                if (!(this.ed.dom.hasClass(c.parentNode, this._personalizedContentHolderClass) && (c.parentNode.children.length == 1))) {
                                    //never delete the last element inside the pc_content holder
                                    this.ed.dom.remove(c);
                                }
                            }
                            return true;
                        }
                    }
                }

                //press bksp at the begining of pc_content -> move up
                if (deleteBackward) {
                    var previousCaretNode = this._getPreviousCaretNode(c);
                    if (this.ed.dom.hasClass(previousCaretNode, this._personalizedContentHeaderClass)) {
                        if (this._getCaretPosition(c) == 0) { //costly call, should be shortcut
                            this._moveUp(pc, false);
                            return true;
                        }
                    }
                }
            }
            return this._blockContentBlockDeletion();
        },

        /**
        * Deletes the personalized content block if the header for the block is selected.
        *
        * @param {Personalized content element} pc The personalized content that is currently active.
        */
        _deletePersonalizedContent: function (pc) {
            if (!this.ed.dom.hasClass(pc, "mceSelected")) {
                //We have just deleted the node under a pc element, the selection has changed but we have not gotten the 
                //selection changed event and marked the element as selected. Do not delete the pc element.
                return false;
            }
            //before remove pc, start a undo level
            this._ensureInitialUndoLevel();
            this.ed.dom.remove(pc);
            this._onContentChanged();

            return true;
        },

        /**
        * Makes sure that a pc holder is not deleted.
        */
        _blockContentBlockDeletion: function () {
            var currentNode = this._getCommonAncestor();
            var pcContent = this._getParentContentBlockHolder(currentNode);
            if (this._isNull(pcContent)) {
                return false;
            }
            var childNodes = this._getChildren(pcContent);
            if (childNodes.length === 0) {
                //Somehow the inner content has no elements. Add a new p tag, insert any existing text and block current delete action.
                _normalizeContentNode(pcContent);
                return true;
            }
            else if (childNodes.length > 1) {
                return false;
            }
            else if ($.trim($(pcContent).children().text()).length > 0) {
                return false;
            }
            else {
                var innerChildren = $(childNodes[0]).children();
                if (innerChildren.length === 0 || innerChildren[0].tagName == "BR") {
                    return true;
                }
                return false;
            }
        },

        /**
        * Deletes the personalized content block if the header for the block is selected.
        */
        _validateSelectionBeforeDeletion: function () {
            if (this.ed.selection.isCollapsed()) {
                return false;
            }
            var sc = this._getParentContentBlockHolder(this.ed.selection.getStart());
            var ec = this._getParentContentBlockHolder(this.ed.selection.getEnd());
            if (!(this._isNull(sc) && this._isNull(ec)) && sc != ec) {
                this.ed.windowManager.alert(this.ed.translate('epipersonalizedcontent.invalidselectionondeletewarning'));
                return true;
            }
            return false;
        },

        /**
        * Adds a new p tag and wraps any existing text value inside it.
        *
        * @param {Personalized content holder} contentNode The personalized content holder that is currently active.
        */
        _normalizeContentNode: function (contentNode) {
            var jContentNode = $(contentNode);
            if (jContentNode.children().length === 0) {
                var innerHtml = jContentNode.html();
                if (innerHtml.length === 0) {
                    innerHtml = '<br mce_bogus="1" />';
                }
                jContentNode.html("");
                this.ed.dom.add(contentNode, 'p', null, innerHtml);
            }
            else {
                for (var i = 0; i < contentNode.childNodes.length; i++) {
                    var child = contentNode.childNodes[i];
                    //Wrap non block element nodes that contain text (not whitespace only) in a p tag.
                    if (!this.ed.dom.isBlock(child) && !(child.nodeType == 3 && !child.data.match("."))) {
                        $(child).wrap('<p />')
                    }
                }
            }
        },

        /**
        * Event handler that enables the cursor to be set the after pc by making sure that there exists content after it (or by inserting a new p tag).
        *
        * @param {Personalized content element} pc The personalized content that is currently active.
        * @param {Boolean} moveIn Move the caret inside PC or not
        */
        _moveDown: function (pc, moveIn) {
            if (moveIn) {
                //First try to find first element and set selection there, otherwise set selection in first node (text fragment)
                var pcHolder = this._getPCContentBlockHolder(pc);
                if (!pcHolder) {
                    return false;
                }
                var firstChild = $(pcHolder).children()[0];
                if (this._isNull(firstChild)) {
                    firstChild = pcHolder[0].firstChild;
                }
                this._moveSelection(firstChild, true);

                return true;
            }
            else {
                // Skip empty text nodes form the end
                var last = this.ed.getBody().lastChild;
                while (last && last.nodeType == 3 && !last.nodeValue.length) {
                    last = last.previousSibling;
                }

                //If we are using keys to move down in the last personlized content block, create a p tag to be able to move outside
                //Could perhaps be improved so that we check that we are at the last character in the pc content section
                if (last && last === pc) {
                    last = this.ed.dom.add(this.ed.getBody(), 'p', null, '<br mce_bogus="1" />');
                }

                //Move to beginning of the next element
                var next = pc.nextSibling || $(pc).next().length ? $(pc).next()[0] : null; //next textnode or next element
                if (next) { //if pc is at the end of a table cell, let browser does its logic
                    this._moveSelection(next, true);
                }
            }
        },

        /**
        * Event handler that enables the cursor to be set the before pc by making sure that there exists content before it (or by inserting a new p tag).
        *
        * @param {Personalized content element} pc The personalized content that is currently active.
        * @param {Boolean} moveIn Move the caret inside PC or not
        */
        _moveUp: function (pc, moveIn) {
            if (moveIn) {
                var pcContent = this._getPCContentBlockHolder(pc);
                var pcContentChildren = $(pcContent).children();
                if (pcContentChildren.length > 0) {
                    this._moveSelection(pcContentChildren[pcContentChildren.length - 1], false);
                }
                else {
                    this._moveSelection(pcContent, false);
                }
            }
            else {
                // Skip empty text nodes in the beginning
                var first = $(pc).prev()[0];

                if (typeof first === "undefined") {
                    first = this.ed.dom.create('p', null, '<br _mce_bogus="1" />');
                    pc.parentNode.insertBefore(first, pc);
                }

                this._moveSelection(first, false);
                return true;
            }
        },

        /**
        * Event handler that blocks all input except certain characters when having a pc header selected.
        *
        * @param {tinymce editor} ed The active editor.
        * @param {Event} e The event that was triggered.
        */
        _block: function (ed, e) {
            //in FF, keyup, keypress events are still triggered event we have cancelled keydown event
            //block enter key events (keyup, keypress) if personalized content is being selected
            if (this._selectedPcHeader && (e.keyCode == 13) && (e.type == 'keydown')) {
                return;
            }

            if (this._isSpecialKey(e.keyCode) || e.ctrlKey || e.metaKey) {
                return;
            }

            return tinymce.dom.Event.cancel(e);
        },

        /**
        * Determines whether the key is allowed inside a pc header.
        *
        * @param {Key code} k The key code for the current event.
        */
        _isSpecialKey: function (k) {
            return (k > 32 && k < 41) || (k > 111 && k < 124) || k == 46 || k == 8;
        },

        /**
        * Returns information about the plugin as a name/value array.
        * The current keys are longname, author, authorurl, infourl and version.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'EPiServer CMS Personalized Content Plug-in',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epipersonalizedcontent', tinymce.plugins.epipersonalizedcontent);
} (tinymce, epiJQuery));
