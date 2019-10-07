/**
* Add trailing element while editing, to enable placing the cursor at the end of the body.
*/
(function(tinymce, $) {
    tinymce.create('tinymce.plugins.EPiTrailing', {
        /**
        * Initializes the plugin, this will be executed after the plugin has been created.
        * This call is done before the editor instance has finished it's initialization so use the onInit event
        * of the editor instance to intercept that event.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            var t = this;
            ed.onSetContent.add(function(ed, o) {
                t._insertTrailingElement(ed);
            });
            ed.onChange.add(function(ed, cm, e) {
                t._insertTrailingElement(ed);
            });
            ed.onPreProcess.add(function(ed, o) {
                t._trimEmptyElements(ed, o);
            });
        },

        /**
        * Gets information about the plug-in.
        */
        getInfo: function() {
            return {
                longname: 'Trailing element',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        },

        /**
        * Makes sure that there always exists a p tag last in the document to be able to set the cursor.
        *
        * @param {tinymce.Editor} ed Editor instance.
        */
        _insertTrailingElement: function(ed) {
            var body = ed.getBody();
            var last = body && body.lastChild;

            if (!body || !last) {
                return;
            }

            if (ed.dom.isBlock(last) && last.tagName !== "P") {
                body.appendChild(ed.dom.create('p', null, '<br _mce_bogus="1" />'));
            }
        },

        /**
        * Removes any p tags considered as empty when getting the content from the editor (or viewing content with the html plug-in).
        *
        * @param {tinymce.Editor} ed Editor instance.
        * @param {options} o Function options.
        */
        _trimEmptyElements: function(ed, o) {
            if (!o.get) {
                return;
            }
            if (o.node.childNodes.length === 1) {
                //Don't remove the last element in the document.
                return;
            }
            var last = o.node.lastChild;
            while (last && last.nodeName === "P" && last.childNodes.length == 1 && last.firstChild.nodeName == 'BR') {
                ed.dom.remove(last);
                last = o.node.lastChild;
            }

            //We remove any empty nodes in the beginning of the document as well.
            var first = o.node.firstChild;
            while (first && first.nodeName === "P" && first.childNodes.length == 1 && first.firstChild.nodeName == 'BR') {
                ed.dom.remove(first);
                first = o.node.firstChild;
            }
        }
    });

    tinymce.PluginManager.add('epitrailing', tinymce.plugins.EPiTrailing);
}(tinymce, epiJQuery));
