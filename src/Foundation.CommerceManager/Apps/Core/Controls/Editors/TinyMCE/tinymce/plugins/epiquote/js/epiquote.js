tinyMCEPopup.requireLangPack();

var epiquote = {

    /**
    * Initialize of dialog. Set up proper form depending on editor selection when opening page.
    */
    init: function() {
        var i, e, radio,
        disabled = "disabled", checked = "checked",
        t = this,
        f = document.forms[0],
        ed = tinyMCEPopup.editor,
        s = ed.selection;

        e = ed.dom.getParent(s.getNode(), "q");

        tinyMCEPopup.resizeToInnerSize();

        if (e == null) {
            e = ed.dom.getParent(s.getNode(), "blockquote");
        }

        if (e != null) {
            t._bookmark = s.getBookmark();

            for (i = 0; i < f.quoteType.length; i++) {
                f.quoteType[i].setAttribute(disabled, disabled);
            }

            e.nodeName == "BLOCKQUOTE" ? radio = f.blockquote : radio = f.quote;

            radio.removeAttribute(disabled);
            radio.setAttribute(checked, checked);
            f.title.value = e.title;
            f.cite.value = e.cite;

            f.insert.value = ed.getLang("update");
            f.remove.style.display = "";

            document.title = document.getElementById("general_tab_title").innerHTML = ed.getLang("epiquote_dlg.titleupdate");
        }

        if (!epiquote._inlineQuoteEnabled()) {
            f.quote.setAttribute(disabled, disabled);
        }
    },

    /**
    * Surrounds current selection with a q or block quote with cite and title attribute depending on how user has filled in form.
    */
    insert: function() {
        var e, b, i, quoteType, code, p, contentLower, parentLower, outerSelection,
        f = document.forms[0],
        ed = tinyMCEPopup.editor,
        t = this,
        attrs = {},
        q = "q";

        tinyMCEPopup.restoreSelection();

        var s = ed.selection;
        // Restore selection to any previously selected quote (if the user has changed selection)
        if (t._bookmark) {
            s.moveToBookmark(t._bookmark);
        }

        // Check which quoteType is chosen
        for (i = 0; i < f.quoteType.length; i++) {
            if (f.quoteType[i].checked) {
                quoteType = f.quoteType[i].value;
            }
        }

        epiquote._trimSelection(s); // Trims whitespace in start and end of selection. Works only for IE at the moment.

        if (f.title.value.length) {
            attrs.title = f.title.value;
        }
        if (f.cite.value.length) {
            attrs.cite = f.cite.value;
        }

        if (quoteType === q) {
            var parentNode = s.getNode();
            e = ed.dom.getParent(parentNode, quoteType);
            p = ed.dom.getParent(parentNode, ed.dom.isBlock, ed.getBody());
            
            //Creates 2 trimmed versions of parent outerHTML and selected content so we can compare.
            contentLower = s.getContent().toLowerCase().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
            parentLower = ed.dom.getOuterHTML(p).toLowerCase().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
            outerSelection = contentLower === parentLower;
        }

        if (e == null) {
            // Create new quote/blockquote
            if (quoteType === q && !epiquote._inlineQuoteEnabled()) {
                // Should only happen if user changes selection to invalid quote selection after opening quote dialog with a valid selection.
                alert(ed.getLang("epiquote_dlg.invalidselectionforq"));
                return false;
            } 
            else {
                if (outerSelection) {
                    code = p.innerHTML;
                } else {
                    code = s.getContent();
                }
            }
            code = ed.dom.createHTML(quoteType, attrs, code);
            if (outerSelection) {
                //In this case we have removed the <p> tags so we need to get them back outside the q tags.
                code = ed.dom.createHTML("p", {}, code);
            }
            // It's better to use the insertHTML method on Gecko since it will combine/split/close block elements correctly when inserting the content.
            if (tinymce.isGecko) {
                ed.execCommand("insertHTML", false, code);
            } else {
                if (tinymce.isIE && ed.selection.getSel().type === "Control") {
                    ed.dom.setOuterHTML(ed.selection.getSel().createRange().item(0), code);
                } else {
                    ed.execCommand("mceInsertContent", false, code);
                }
            }
        } else {
            ed.dom.setAttribs(e, attrs);
        }

        //Wrap all changes as one undo level
        ed.undoManager.add();
        tinyMCEPopup.close();
    },

    /**
    * Removes the first parent quote found on current selection
    */
    remove: function() {
        var i, quoteType, e,
            t = this,
            f = document.forms[0],
            ed = tinyMCEPopup.editor;

        tinyMCEPopup.restoreSelection();
        s = ed.selection;

        // Restore selection to any previously selected quote (if the user has changed selection)
        if (t._bookmark) {
            s.moveToBookmark(t._bookmark);
        }

        for (i = 0; i < f.quoteType.length; i++) {
            if (f.quoteType[i].checked) {
                quoteType = f.quoteType[i].value;
            }
        }

        e = ed.dom.getParent(s.getNode(), quoteType);

        tinyMCEPopup.execCommand("mceRemoveNode", ed, e);
        tinyMCEPopup.close();
    },

    /**
    * Returns true if the HTML in current editor selection allows an inline quote (q element).
    *
    * @return {bool} true/false depending on current selection allows q element.
    */
    _inlineQuoteEnabled: function() {
        var ed = tinyMCEPopup.editor,
        s = ed.selection;
        e = s.getNode();

        if (ed.dom.getOuterHTML(e) === s.getContent() && ed.dom.isBlock(e)) {
            return false;
        }

        switch (e.nodeName) {
            case "BODY":
            case "TD":
            case "TH":
            case "UL":
            case "OL":
            case "BLOCKQUOTE":
                return false;
        }

        // TODO: More checks needed?
        return true;
    },

    /**
    * Trims whitespaces in the start and end of a selection.
    *
    * @param {tinymce.dom.Selection} s Editor selection to trim.
    */
    _trimSelection: function(s) {
        var text, initialLength, startSpaces, endSpaces, character;
        var r = s.getRng();
        var ed = tinyMCEPopup.editor;

        if (r.toString) {
            text = r.toString();
        } else if (r.text) {
            text = r.text;
        }

        // Check for start or end spaces
        if (!text || !text.length || text.length == tinymce.trim(text).length) {
            return;
        }

        initialLength = text.length;
        startSpaces = initialLength - text.replace(/^\s+/, '').length;
        endSpaces = initialLength - text.search(/\s+$/);

        if (r.toString) {
            // Gecko

            // TODO: Implement in Gecko browsers. Below code is experimental and can't cope with all nested elements.

            //            var r2, acSelected, eo2, counter;
            //            
            //            var sc = r.startContainer;
            //            var ec = r.endContainer;
            //            var so = r.startOffset;
            //            var eo = r.endOffset;
            //            var ac = r.commonAncestorContainer;
            //            
            //            var nodes = ac.childNodes;
            //            var endSpaceDiff = eo - endSpaces;
            //            
            //            if (endSpaceDiff < 0) {
            //                r2 = ed.getDoc().createRange();
            //                alert(endSpaceDiff);
            //                for (var i = 0; i < nodes.length; i++) {
            //                    if (nodes[i] === ec) {
            //                        counter = i;
            //                        while (endSpaceDiff < 0) {
            //                            counter = counter - 1;
            //                            r2.selectNodeContents(nodes[counter]);
            //                            ac2 = r2.commonAncestorContainer;

            //                            while (ac2.nodeType == 1) {
            //                                alert("|" + r2.toString() + "|" + ac2.childNodes.length);
            //                                r2.selectNodeContents(ac2.childNodes[r2.endOffset - 1]);
            //                                alert("content |" + r2.toString() + "|");
            //                                ac2 = r2.commonAncestorContainer;

            //                            }
            //                            
            //                            eo2 = r2.endOffset;
            //                            ec = r2.endContainer;
            //                            alert(endSpaceDiff + ":" + eo2);
            //                            endSpaceDiff += eo2;
            //                        }
            //                    }
            //                }
            //            }

            //            r.setStart(sc, so + startSpaces);
            //            r.setEnd(ec, endSpaceDiff);

        } else if (r.text) {
            // IE
            r.moveStart("character", startSpaces);
            r.moveEnd("character", -endSpaces);
        }

        s.setRng(r);
    }
};

tinyMCEPopup.onInit.add(epiquote.init, epiquote);
