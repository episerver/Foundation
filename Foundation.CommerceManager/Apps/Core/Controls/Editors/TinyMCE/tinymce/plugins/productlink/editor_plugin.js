(function () {
    tinymce.create('tinymce.plugins.ProductLinkPlugin', {
        init: function (ed, url) {
            var t = this;
            t.editor = ed;
            var appRootPath = epi && epi.routes ? epi.routes.getActionPath({
                moduleArea: "CommerceUI",
                path: ""
            }) : "";
            appRootPath = appRootPath.substr(0, appRootPath.toLowerCase().indexOf("?"));
            ed.addCommand('openProductPicker', function () {
                var href = "",
                    s = ed.selection,
                    node = s.getNode(),
                    dom = ed.dom,
                    selectedLink = dom.getParent(node, "A"),
                    linkObject = {};

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

                var language = pageContext.epslanguage;

                linkObject.parentWindow = window;

                var callbackMethod = function (value) {
                    var html, link, isUpdated, language, text, elementArray;

                    if (value) {
                        //If we press delete in the dialog we get -1 as result and remove the link
                        if (value === "-1" && selectedLink) {
                            html = selectedLink.innerHTML;
                            dom.setOuterHTML(selectedLink, html);
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
                                ed.undoManager.add();
                            } else {
                                // To make sure we dont get nested links and have the same behavior as the default tiny 
                                // link dialog we unlink any links in the selection before we create the new link.
                                ed.getDoc().execCommand('unlink', false, null);
                                ed.execCommand("CreateLink", false, "#mce_temp_url#", { skip_undo: 1 });

                                elementArray = tinymce.grep(dom.select("a"), function (n) { return dom.getAttrib(n, 'href') == '#mce_temp_url#'; });
                                for (i = 0; i < elementArray.length; i++) {
                                    dom.setAttribs(elementArray[i], linkAttributes);
                                }
                                ed.undoManager.add();
                            }
                        }
                    }
                };

                //We have to send a new parameter that is handled in the dialog. We can not use the onlyUrl property sinc ethat disables too much.
                var hyperLinkUrl = appRootPath + '/Edit/ProductPicker/ProductLink/ProductLinkProperties.aspx?diablecontentedit=true&language=' + language;
                hyperLinkUrl += "&url=" + encodeURIComponent(href);
                EPi.CreateDialog(hyperLinkUrl, callbackMethod, null, linkObject, { width: 460, height: 330, scrollbars: 'no' });
            });
            ed.addButton('productlink', { title: 'productlink.desc', image: appRootPath + '/Edit/ProductPicker/ProductLink/Images/product_link.gif', cmd: 'openProductPicker' });

            ed.onNodeChange.add(function (ed, cm, n, co) {
                //Since we can have other inline elements nested within the link we want to try get the closest link parent.
                var a = ed.dom.getParent(n, 'a', ed.getBody());
                cm.setDisabled('productlink', co && (a === null));
                cm.setActive('productlink', (a !== null) && !a.name);
            });
        },
        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Product Link plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });
    tinymce.PluginManager.add('productlink', tinymce.plugins.ProductLinkPlugin);
})();
