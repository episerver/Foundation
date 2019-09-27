tinyMCEPopup.requireLangPack();

var SearchReplaceDialog = {
    /* 
    Rewritten by EPiServer to make Search/Replace work with noneditable content (like Dynamic content) which we don't want to be found or replaced.
    */

    init: function (ed) {
        var f = document.forms[0], m = tinyMCEPopup.getWindowArg("mode");

        this.switchMode(m);

        f[m + '_panel_searchstring'].value = tinyMCEPopup.getWindowArg("search_string");

        // Focus input field
        f[m + '_panel_searchstring'].focus();
    },

    switchMode: function (m) {
    	// summary: call when user switch from Find tab to Replace tab or vice versa
	
        var f, lm = this.lastMode;

        if (lm != m) {
            f = document.forms[0];

            if (lm) {
                f[m + '_panel_searchstring'].value = f[lm + '_panel_searchstring'].value;
                f[m + '_panel_backwardsu'].checked = f[lm + '_panel_backwardsu'].checked;
                f[m + '_panel_backwardsd'].checked = f[lm + '_panel_backwardsd'].checked;
                f[m + '_panel_casesensitivebox'].checked = f[lm + '_panel_casesensitivebox'].checked;
            }

            mcTabs.displayTab(m + '_tab', m + '_panel');
	    // Get Replace and ReplaceAll buttons and notify their changing state
            var replaceBtn = document.getElementById("replaceBtn");
            var replaceAllBtn = document.getElementById("replaceAllBtn");

            replaceBtn.style.display = (m == "replace") ? "inline" : "none";
            replaceAllBtn.style.display = (m == "replace") ? "inline" : "none";

            replaceBtn.disabled = (m == "replace") ? "" : "disabled";
            replaceAllBtn.disabled = (m == "replace") ? "" : "disabled";

            if (typeof EPi != 'undefined') {
                EPi.GetDialog().ButtonChanged(replaceBtn);
                EPi.GetDialog().ButtonChanged(replaceAllBtn);
            }

            this.lastMode = m;

        }
    },

    searchNext: function (a) {
        var ed = tinyMCEPopup.editor, se = ed.selection, r = se.getRng(), f, m = this.lastMode, s, b, fl = 0, w = ed.getWin(), wm = ed.windowManager, fo = 0;

        // Get input
        f = document.forms[0];
        s = f[m + '_panel_searchstring'].value;
        b = f[m + '_panel_backwardsu'].checked;
        ca = f[m + '_panel_casesensitivebox'].checked;
        rs = f['replace_panel_replacestring'].value;

        if (s == '') {
            return;
        }

        function fix() {
            // Correct Firefox graphics glitches
            r = se.getRng().cloneRange();
            ed.getDoc().execCommand('SelectAll', false, null);
            se.setRng(r);
        }

        function replace() {
            if (tinymce.isIE) {
                ed.selection.getRng().duplicate().pasteHTML(rs); // Needs to be duplicated due to selection bug in IE
            } else {
                ed.getDoc().execCommand('InsertHTML', false, rs);
            }
        }

        function getNonEditableNode(node) {
            // Return parent node with contentEditable set to false or null
            if (node.getAttribute("contentEditable") == "false") {
                return node;
            } else {
                return ed.dom.getParent(node, function (n) { return n.getAttribute("contentEditable") == "false"; }, ed.getBody());
            }
        }

        function notFound(doReplace) {
            if (!doReplace) {
                tinyMCEPopup.alert(ed.getLang('searchreplace_dlg.notfound'));
            }
            return false;
        }

        function findNext(doReplace) {
            // If doReplace is true we not only want to find a word but replace it as well (used by replaceAll functionality)
            var nonEditableNode;

            if (tinymce.isIE) {
                var direction = b ? -1 : 1;
                if (r.findText(s, direction, fl)) {
                    nonEditableNode = getNonEditableNode(r.parentElement());
                    if (nonEditableNode !== null) {
                        r.moveToElementText(nonEditableNode);
                        r.collapse(b);
                        r.move("character", direction);
                        return findNext(doReplace);
                    } else {
                        r.scrollIntoView();
                        r.select();
                    }
                } else {
                    return notFound(doReplace);
                }
                tinyMCEPopup.storeSelection();
            } else {
                if (w.find(s, ca, b, false, false, false, false)) {
                    if (getNonEditableNode(ed.selection.getNode()) !== null) {
                        r = ed.selection.getRng();
                        r.collapse(b);
                        ed.selection.setRng(r);
                        return findNext(doReplace);
                    } else {
                        fix();
                    }
                } else {
                    return notFound(doReplace);
                }
            }
            return true;
        }

        // IE flags
        if (ca) {
            fl = fl | 4;
        }

        switch (a) {
            case 'all':
                // Move caret to beginning of text
                ed.execCommand('SelectAll');
                ed.selection.collapse(true);

                while (findNext(true)) {
                    replace();
                    fo = 1;
                }

                if (fo) {
                    tinyMCEPopup.alert(ed.getLang('searchreplace_dlg.allreplaced'));
                } else {
                    tinyMCEPopup.alert(ed.getLang('searchreplace_dlg.notfound'));
                }

                return;

            case 'current':
                if (!ed.selection.isCollapsed() && getNonEditableNode(ed.selection.getNode()) === null) {
                    replace();
                }
                break;
        }

        se.collapse(b);
        r = se.getRng();

        // Whats the point
        if (!s) {
            return;
        }

        findNext();
    }
};

tinyMCEPopup.onInit.add(SearchReplaceDialog.init, SearchReplaceDialog);
