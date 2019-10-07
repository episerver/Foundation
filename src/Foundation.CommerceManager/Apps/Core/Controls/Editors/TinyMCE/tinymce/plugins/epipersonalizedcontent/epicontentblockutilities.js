(function(tinymce, $) {
    epiContentBlockUtilities = function() {
        //This is a shared property to ensure that a plug-in does not unlock controls after another plug-in has locked them.
        var controlsLockedBy = null;
        var pub =
    {
        _isNull: function(object) {
            return object === null || typeof object === "undefined";
        },

        /**
        * Gets the closest parent with the given css class.
        *
        * @param {Node} node The current node.
        * @param {string} cssClass The css class of the element that you want to find.
        */
        _getParentNode: function(node, cssClass) {
            if (this.ed.dom.hasClass(node, cssClass)) {
                return node;
            }
            return this.ed.dom.getParent(node, "." + cssClass);
        },

        /**
        * Enables or disables tinymce command handlers (plug-ins).
        *
        * @param {Bool} enable Set to true to enable all commands or to false to disable unsupported commands for the current selection.
        */
        _updateCommandState: function(enable) {
            // Block or unblock
            if (enable && controlsLockedBy !== null && controlsLockedBy != this) {
                //We don't want to unlock controls if another plug-in has locked them.
                this._disabled = false;
                return true;
            }
            if (enable) {
                if (this._disabled) {
                    //Only enable controls if we are in disable state
                    this._setDisabled(false, this._enabledControls);
                    controlsLockedBy = null;
                }
            } else {
                this._setDisabled(true, this._enabledControls);
                controlsLockedBy = this;
                return false; // Always return false to prevent other plugins listening to nodeChange event from enabling tools.
            }
            return true;
        },

        /**
        * Enables or disables controls of the editor instance if user selects or deselects a content block.
        *
        * @param {bool} disable Set to true if we want to disable controls.
        * @param {string} enabledControls Comma separated string of id's indicating controls which should not be disabled.
        */
        _setDisabled: function(disable, enabledControls) {
            var i,
            buttons = enabledControls.split(",");

            enabledControls = "";
            var prefix = "|" + this.ed.id + "_";
            var length = buttons.length;

            for (i = 0; i < length; i++) {
                enabledControls = enabledControls + prefix + tinymce.trim(buttons[i]);
            }
            enabledControls += "|";

            tinymce.each(this.ed.controlManager.controls, function(control) {
                if (enabledControls.indexOf("|" + control.id + "|") < 0) {
                    control.setDisabled(disable);
                    if (disable) {
                        // There's never a reson for a disabled button to be active for the current selection.
                        control.setActive(false);
                    }
                }
            });

            this._addRemoveInputBlockingEventHandlers(disable);
            this._disabled = disable;
        },

        /**
        * Adds or removes event handlers to ensure that all user input other than specific keys are blocked.
        *
        * @param {bool} add Set to true to add event handlers and false to remove existing handlers.
        */
        _addRemoveInputBlockingEventHandlers: function(add) {
            if (add === this._disabled) {
                return; // Make sure we don't do any unnecessary work if event handlers already have been activated.
            }
            if (add) {
                this.ed.onKeyDown.addToTop(this._block, this);
                this.ed.onKeyPress.addToTop(this._block, this);
                this.ed.onKeyUp.addToTop(this._block, this);
                this.ed.onPaste.addToTop(this._block, this);
            }
            else {
                this.ed.onKeyDown.remove(this._block, this);
                this.ed.onKeyPress.remove(this._block, this);
                this.ed.onKeyUp.remove(this._block, this);
                this.ed.onPaste.remove(this._block, this);
            }
        },

        /**
        * Moves the cursor to the given node. Will handle both block and inline nodes.
        *
        * @param {Node} node The node to move to.
        * @param {Bool} setCursorFirst Set to true to place the cursor first in the content and false to place it last in the content.
        */
        _moveSelection: function(node, setCursorFirst) {
            var range;
            var root = this.ed.dom.getRoot();

            if (root.createTextRange &&  !this._isIE9StandardMode()) { //IE9 should behave W3C way
                range = root.createTextRange();
                if (setCursorFirst && !node.canHaveHTML && node.nodeType != 3 && node.previousSibling) {
                    // When moving the cursor immediately after an inline dynamic content and the following element
                    // doesn't support content (e.g. images) we add a text node before the element.
                    var tNode = this.ed.dom.doc.createTextNode("");
                    this.ed.dom.insertAfter(tNode, node.previousSibling);
                    node = tNode;
                }

                if (node.nodeType == 3) {
                    //Text node (no wrapping element)
                    var nbsp = String.fromCharCode(160);
                    if (setCursorFirst) {
                        //Can't have selection just after to the block element due to an IE bug so we create a nbsp; if text does not already have it.
                        if (node.data.match("^" + nbsp) === null) {
                            node.data = nbsp + node.data;
                        }
                        //insert som dummy text just to find where we are since IE does not seem to have a method to select a text fragment
                        node.nodeValue = '~~~¤' + node.data;
                        range.findText('~~~¤');
                        range.collapse(false);
                        node.replaceData(0, 4, '');
                        range.moveStart('character', 1);
                    }
                    else {
                        //insert som dummy text just to find where we are since IE does not seem to have a method to select a text fragment
                        node.nodeValue = node.data + '~~~¤';
                        var nodeLength = node.data.length;
                        range.findText('~~~¤');
                        range.collapse(true);
                        node.replaceData(nodeLength - 4, 4, '');
                    }
                }
                else {
                    range.moveToElementText(node);
                    if (!setCursorFirst && this.ed.dom.isBlock(node)) {
                        //We seem to have a selection outside the element for block elements, move end character inside element.
                        range.moveEnd('character', -1);

                        var lastChild = $(":last", node)[0];
                        if (!this._isNull(lastChild) && lastChild.tagName == "BR") {
                            //If the last content in the node is a br, move before this since IE will get a wrong selection otherwise
                            range.moveEnd('character', -1);
                        }
                    }
                    range.collapse(setCursorFirst);
                }
            }
            else {
                //Logic for non IE browsers
                range = this.ed.dom.createRng();
                if (node.nodeType === 1 && 
                    (node.tagName === "IMG" || node.TagName === "HR")) {
                    // For images and hr (not having inner content) we always set the caret before the element.
                    // When moving the caret after dc/pc this works very much as expected, 
                    // but when moving the caret before it may feel a bit glitchy, since the 
                    // caret is moved before any image or other element. 
                    // The best solution would be if setStartAfter had behaved as one might expect, 
                    // but this causes the caret position to be lost when pressing left arrow again, 
                    // even though other characters are inserted correctly.
                    range.setStartBefore(node);
                } else {
					if (setCursorFirst && node.nodeType === 3 && node.textContent === "") {
                        // To be able to set selection, text nodes needs to have content.
                        node.textContent = String.fromCharCode(160); // nbsp;
                    }
                    range.selectNodeContents(node);
                    range.collapse(setCursorFirst);
                }
            }
            this.ed.selection.setRng(range);
        },

        /**
        * Inserts a new content block at the position of the current selection. Makes sure that we do not insert a div inside or an element that
        * does not support block elements. If that is the case then the element will be split into two.
        *
        * @param {string} html The html to insert.
        * @return {Node} Inserted content as html Node
        */
        _insertContent: function(html) {
            //This code is taken from the tiny mce table plug-in which is used to ensure that
            // a block element is not inserted inside an element which does not allow block elements as children.
            var patt = '';
            this.ed.focus();

            //Remove any br-bogus elements first in current selection.
            var currentNode = this.ed.selection.getNode();

            if (currentNode && currentNode.tagName === "P" && currentNode.firstChild && currentNode.firstChild.nodeName == 'BR') {
                this.ed.selection.select(currentNode);
            }

            this.ed.selection.setContent('<br class="_mce_marker" />');

            tinymce.each('h1,h2,h3,h4,h5,h6,p'.split(','), function(n) {
                if (patt) {
                    patt += ',';
                }

                patt += n + ' ._mce_marker';
            });

            var t = this;
            tinymce.each(this.ed.dom.select(patt), function(n) {
                t.ed.dom.split(t.ed.dom.getParent(n, 'h1,h2,h3,h4,h5,h6,p'), n);
            });

            var tempNode = this.ed.dom.create('div', null, html).firstChild;
            this.ed.dom.replace(tempNode, this.ed.dom.select('br._mce_marker')[0]);

            //return the inserted node
            return tempNode;
        },

        /**
        * Inserts a new inline block at the position of the current selection. 
        *
        * @param {string} html The html to insert.
        * @return {Node} Inserted content as html Node
        */
        _insertInlineContent: function(html) {
            this.ed.focus();
            var selectionNode = this.ed.selection.getNode();
            
            //create wrapper element if needed
            if (selectionNode === this.ed.getBody()) {
                var wrapper = this.ed.dom.create(this.ed.getParam('element'));
                this.ed.dom.add(selectionNode, wrapper);
                this.ed.selection.select(wrapper);
                this.ed.selection.collapse();
            }
            
            var tempNode = this.ed.dom.create('span', null, html).firstChild;
            this.ed.selection.setContent('<span class="_mce_marker">&nbsp;</span>');
            this.ed.dom.replace(tempNode, this.ed.dom.select('._mce_marker', this.ed.selection.getNode())[0]);
            return tempNode;
        },

        /**
        * Returns a comma separated string with all content groups for the current editor.
        */
        _getAllContentGroups: function() {
            var dynamiccontentNodes = this.ed.dom.select('[data-contentgroup!=]');
            var allContentGroups = '';
            for (var i = 0; i < dynamiccontentNodes.length; i++) {
                var attribute = dynamiccontentNodes[i].getAttribute("data-contentgroup");
                var value = attribute ? attribute.replace(/"/g, '&quot;') : "";
                if (value !== '') {
                    if ((allContentGroups == value) || (allContentGroups.indexOf(',' + value) >= 0) || (allContentGroups.indexOf(value + ',') >= 0)) {
                        continue;
                    }
                    if (allContentGroups !== '') {
                        allContentGroups += ',' + value;
                    }
                    else {
                        allContentGroups += value;
                    }
                }
            }
            return allContentGroups;
        },

        /**
        * Does the same as jquery children() method but due to a tiny/jquery bug new paragraphs will get the same id and they will be removed in the children call.
        *
        * @param {element} node The element to get the children for.
        */
        _getChildren: function(node) {
            var children = [];
            var i;
            if (node.childNodes) {
                for (i = 0; i < node.childNodes.length; i++) {
                    var child = node.childNodes[i];
                    if (child.nodeType == 1) {
                        children.push(child);
                    }
                }
            }
            return children;
        },

        /**
        * Inserts a new empty paragraph before the given node.
        *
        * @param {element} node The element to insert a paragraph before.
        */
        _insertParagraphBefore: function(node) {
            var newNode = this.ed.dom.create('p', null, '<br _mce_bogus="1" />');
            node.parentNode.insertBefore(newNode, node);
        },

        /**
        * Select a content block.
        *
        * @param {Editor} e The editor.
        * @param {Element} c The content block.
        */
        _selectContentBlock: function(ed, c) {
            var root = ed.dom.getRoot();

            if (root.createControlRange && !this._isIE9StandardMode()) { //IE9 should behave W3C way
                //create IE's control selection
                var range = root.createControlRange();
                c.setAttribute("contentEditable", false);
                range.add(c);
                ed.selection.setRng(range);

                var preventResizing = function(e) { e.returnValue = false; };
                ed.dom.unbind(c, "resizestart", preventResizing); //just to make sure we have no duplication here
                ed.dom.bind(c, "resizestart", preventResizing);
            }
            else {
                //Firefox
                if (ed.selection.isCollapsed()) {
                    ed.selection.select(c);
                } else {
                    // Hack to set selection again when an already selected
                    // textblock is clicked, since the selection will be unselected 
                    // by the click, but after the event cycle has finished.
                    setTimeout(function() { ed.selection.select(c); }, 0);
                }

            }
        },

        /**
        * Deselect a content block.
        *
        * @param {Editor} e The editor.
        * @param {Element} c The content block.
        */
        _deselectContentBlock: function(ed, c) {
            if (document.selection) {
                document.selection.empty();
                c.removeAttribute("contentEditable");
            }
        },

        /**
        * Start an undo level. (Just initialize undo manager if necessary)
        *
        */
        _ensureInitialUndoLevel: function() {
            //Sometime, our handler is executed before tinymce initialize the undoManager with the starting level
            if (!this.ed.undoManager.hasUndo()) {
                this.ed.undoManager.add();
            }
        },

        /**
        * Add an undo level and trigger nodeChanged event. 
        *
        */
        _onContentChanged: function() {
            this.ed.undoManager.add();
            this.ed.nodeChanged();
        },

        /**
        * Get current position of the caret inside a node
        * @param {Node} node The node.
        *
        */
        _getCaretPosition: function(node) {
            if (!this.ed.selection.isCollapsed()) {
                return;
            }
            else {
                var rng = this.ed.selection.getRng(true); //Let tinymce construct a W3C-like Range object
                if (rng.startOffset || rng.startOffset == 0) {
                    //If browser supports startOffset, just return it
                    return rng.startOffset;
                }
                else {
                    //Work around for old IE browser. Add a special character to current caret position, get its index, that is the offset. 
                    //These things should be handled in a duplicated range instead of the actual range.
                    var c = "\001",
                        rng = document.selection.createRange(),
                        newRng = rng.duplicate(),
                        len = 0;

                    newRng.moveToElementText(node);
                    rng.text = c;
                    var offset = newRng.text.indexOf(c);
                    rng.moveStart('character', -1);
                    rng.text = "";

                    return offset;
                }
            }
        },

        /**
        * Returns DOM node to which the caret will be moved if user press Left or Bksp
        *
        * @param {Node} n The given node.
        */
        _getPreviousCaretNode: function(n) {
            while (n && n != document.body) {
                var prevSibling = this._getPreviousElementNode(n);
                if (prevSibling) {
                    return prevSibling;
                }
                else {
                    n = n.parentNode;
                }
            }
            return null;
        },

        /**
        * Returns the closest previous sibling which is an element
        *
        * @param {Node} n The given node.
        */
        _getPreviousElementNode: function(n) {
            while (n && n.previousSibling && n.previousSibling.nodeType != 1) {
                n = n.previousSibling;
            }
            return n ? n.previousSibling : null;
        },

        /**
        * Returns DOM node to which the caret will be moved if user press Right or Del
        *
        * @param {Node} n The given node.
        */
        _getNextCaretNode: function(n) {
            while (n && n != document.body) {
                var nextSibling = this._getNextElementNode(n);
                if (nextSibling) {
                    return nextSibling;
                }
                else {
                    n = n.parentNode;
                }
            }
            return null;
        },

        /**
        * Returns the closest previous sibling which is an element
        *
        * @param {Node} n The given node.
        */
        _getNextElementNode: function(n) {
            while (n && n.nextSibling && n.nextSibling.nodeType != 1) {
                n = n.nextSibling;
            }
            return n ? n.nextSibling : null;
        },

        /**
        * Check if caret is at the end of the selection
        *
        * @param {Node} n The selection's container node.
        * @param {Integer} caretPos Caret position (optional).
        */
        _caretIsAtTheEnd: function(n, caretPos) {
            if (this.ed.selection.isCollapsed()) {
                if (caretPos != 0 && !caretPos) {
                    caretPos = this._getCaretPosition(n);
                }
                var rng = this.ed.selection.getRng(true);

                if (rng.commonAncestorContainer) {
                    //W3C Range object
                    var rngNode = rng.commonAncestorContainer;
                    if (rngNode && rngNode.nodeType == 3) { //Text node
                        var contentLength = rngNode.textContent ? rngNode.textContent.length : (rngNode.nodeValue ? rngNode.nodeValue.length : -1);
                        return (caretPos == contentLength);
                    }
                    else if (rngNode && rngNode.nodeType == 1) { //Element node
                        //skip empty textnodes and br
                        for (var i = caretPos; i < rngNode.childNodes.length; i++) {
                            if ((rngNode.childNodes[i].tagName == 'BR') || (rngNode.childNodes[i].nodeType == 3 && $.trim(rngNode.childNodes[i].textContent).length == 0)) {
                                continue;
                            }
                            return false;
                        }
                        return true;
                    }
                    else { //Document node, comment node, etc. Should never happen
                        return false;
                    }
                }
                else if (rng.parentElement) { //IE quirks
                    //can only detect the simplest case, when the selection is in a node that only contains text content
                    //if the node contains some inner element the result will always be false 
                    //(as a result it does not move the caret to the next element and fortunately it does not break the PC neither)
                    return caretPos == $(rng.parentElement()).text().length;
                }
                else {
                    //unknown Range object, should never happen
                    return false;
                }
            } else {
                // selection is not collapsed, caret position makes no sense
                return false;
            }
        },

        /**
        * Updates the state of the undo and redo buttons.
        * Logic copied from _nodeChanged method in advanced theme since updates are done in the onNodeChange event which we might block.
        *
        * @param {Control manager} cm Control manager for the node change event.
        */
        _updateUndoRedoButtons: function(cm) {
            cm.setDisabled('undo', !this.ed.undoManager.hasUndo() && !this.ed.typing);
            cm.setDisabled('redo', !this.ed.undoManager.hasRedo());
        },
        
        /**
        * Test if the editor is running in IE9 standard mode
        *
        */
        _isIE9StandardMode: function() {
            var root = this.ed.dom.getRoot();
            if (!this._ie9Std && (this._ie9Std !== false)) {
                //Note: "!root.createControlRange().commonParentElement" is a trick to know that we are really using IE9 (not any compatibility mode)
                //IE9's createControlRange returns a ControlRangeCollection object, which does not fully implement IHTMLControlRange interface
                //Lacking of the method "commonParentElement" causes an uncaughtable exception when trying add a control range to selection object
                this._ie9Std = root.createControlRange && !root.createControlRange().commonParentElement;
            }
            return this._ie9Std;
        }
    };
        return pub;
    } ();
} (tinymce, epiJQuery));
