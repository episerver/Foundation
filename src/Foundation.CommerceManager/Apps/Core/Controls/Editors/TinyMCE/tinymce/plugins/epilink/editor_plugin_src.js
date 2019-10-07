(function(tinymce, $) {

    tinymce.create('tinymce.plugins.epilink', {
        /**
        * Initializes the plugin.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            // Early exit if we don't have access to EPi
            if (typeof EPi === "undefined" || typeof EPi.ResolveUrlFromUI !== "function") {
                return;
            }

            // Register the command so that it can be invoked by using tinyMCE.activeEditor.execCommand('mceEPiLink');  
            ed.addCommand('mceEPiLink', function() {
                var href = "",
                    s = ed.selection,
                    node = s.getNode(),
                    dom = ed.dom,
                    selectedLink = dom.getParent(node, "A"),
                    imageObj = {},
                    linkObject = {},
                    r = s.getRng();

                // No selection and not in link
                if (s.isCollapsed() && !selectedLink) {
                    return;
                }

                if (selectedLink) {
                    href = dom.getAttrib(selectedLink, "href");
                }

                if (href.length) {
                    linkObject.href = href;
                    linkObject.target = dom.getAttrib(selectedLink, "target");
                    linkObject.title = dom.getAttrib(selectedLink, "title");
                }

                linkObject.text = "none";
                var pageContext = ed.settings.epi_page_context;

                var dialogParameters = $.extend({}, pageContext, {
                    hideclearbutton: true
                });

                var dialogUrl = EPi.ResolveUrlFromUI("edit/FileManagerBrowser.aspx") + "?" + $.param(dialogParameters);

                linkObject.fileManagerBrowserUrl = dialogUrl;
                linkObject.parentWindow = window;

                //Find all Anchors in the document and add them to the Anchor list
                var allAnchors = new Array();
                var allLinks = $("a", ed.getDoc());
                allLinks.each(function(i) {
                    //Check if we have an anchor and if so add it to the array.
                    if (((typeof $(this).attr("name") !== "undefined") && $(this).attr("name").length > 0) && ((typeof $(this).attr("href") === "undefined") || ($(this).attr("href").length === 0))) {
                        allAnchors.push($(this).attr("name"));
                    }
                });

                linkObject.anchors = allAnchors.sort();
                linkObject.imageObject = imageObj;

                var callbackMethod = function(value) {
                    var html, link, isUpdated, language, text, imageObject, elementArray;
                    if (value) {
                        //If we press delete in the dialog we get "-1" (as a string) in result and remove the link
                        if (value == "-1" && selectedLink) {
                            html = selectedLink.innerHTML;
                            dom.setOuterHTML(selectedLink, html);

                            //Wrap all changes as one undo level
                            ed.undoManager.add();
                        }
                        //if we close the dialog with cancel we get 0. So if we dont get 0 or -1 we get an object as value that we can use to create the link.
                        else if (value !== 0) {
                            isUpdated = value.isUpdated;

                            var linkAttributes = {
                                href: value.href,
                                title: value.title,
                                target: value.target ? value.target : null
                            };

                            if (selectedLink) {
                                s.select(selectedLink);
                                dom.setAttribs(selectedLink, linkAttributes);

                                //Wrap all changes as one undo level
                                ed.undoManager.add();
                            } else {
                                // When opening the link properties dialog in OPE mode an inline iframe is used rather than a popup window.
                                // When using IE clicking in this iframe causes the selection to collapse in the tinymce iframe which
                                // breaks the link creation immediately below. The workaround is to store the selection range before
                                // opening, and restoring it before creating the link.
                                s.setRng(r);
                                //To make sure we dont get nested links and have the same behavior as the default tiny 
                                // link dialog we unlink any links in the selection before we create the new link.
                                ed.getDoc().execCommand('unlink', false, null);
                                ed.execCommand("CreateLink", false, "#mce_temp_url#", { skip_undo: 1 });

                                elementArray = tinymce.grep(dom.select("a"), function(n) { return dom.getAttrib(n, 'href') == '#mce_temp_url#'; });
                                for (i = 0; i < elementArray.length; i++) {
                                    dom.setAttribs(elementArray[i], linkAttributes);
                                }

                                //move selection into the link content to be able to recorgnize it when looking at selection
                                if (elementArray.length > 0) {
                                    var range = ed.dom.createRng();
                                    range.selectNodeContents(elementArray[0]);
                                    ed.selection.setRng(range);
                                }

                                //Wrap all changes as one undo level
                                ed.undoManager.add();
                            }
                        }
                    }
                };

                //We have to send a new parameter that is handled in the dialog. We can not use the onlyUrl property sinc ethat disables too much.
                var hyperLinkUrl = EPi.ResolveUrlFromUI("editor/dialogs/HyperlinkProperties.aspx?diablecontentedit=true");
                hyperLinkUrl += "&url=" + encodeURIComponent(href);
                EPi.CreateDialog(hyperLinkUrl, callbackMethod, null, linkObject, { width: 800, height: 650, scrollbars: 'no' });
            });


            // Register buttons
            ed.addButton('epilink', {
                title: 'epilink.desc',
                cmd: 'mceEPiLink',
                "class": "mce_epilink"
            });

            ed.addShortcut("ctrl+k", "epilink.desc", "mceEPiLink");

            ed.onNodeChange.add(function(ed, cm, n, co) {
                //Since we can have other inline elements nested within the link we want to try get the closest link parent.
                var a = ed.dom.getParent(n, 'a', ed.getBody()) || (n.tagName === 'A' ? n : null);

                //select the a tag if all the inner text selected
                if (a && (a.innerHTML === ed.selection.getContent())) {
                    ed.selection.select(a);
                }

                cm.setDisabled('epilink', co && (a === null));
                cm.setActive('epilink', (a !== null) && !a.name);
            });
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'Link plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epilink', tinymce.plugins.epilink);

} (tinymce, epiJQuery));