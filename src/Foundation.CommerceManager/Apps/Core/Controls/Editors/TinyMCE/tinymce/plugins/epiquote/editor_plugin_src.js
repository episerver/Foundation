(function(tinymce, $) {
    // Load plugin specific language pack
    tinymce.PluginManager.requireLangPack('epiquote');

    tinymce.create('tinymce.plugins.epiquote', {
        /**
        * Initializes the plugin.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            var t = this;

            // Register the command so that it can be invoked by using tinyMCE.activeEditor.execCommand('mceEPiQuote');
            ed.addCommand('mceEPiQuote', function() {
                ed.windowManager.open({
                    file: url + '/epiquote.htm',
                    width: 350, // + parseInt(ed.getLang('something.delta_width', 0)),
                    height: 240, // + parseInt(ed.getLang('something.delta_height', 0)),
                    inline: 1
                }, {
                    plugin_url: url // Plugin absolute URL
                });
            });

            // Register quote button
            // which makes use of default blockquote css classes.
            ed.addButton('epiquote', {
                title: 'epiquote.desc',
                cmd: 'mceEPiQuote',
                "class": "mce_blockquote"
            });

            ed.addShortcut("ctrl+shift+q", "epiquote.desc", "mceEPiQuote");

            // Add a node change handler, to set proper selected and disabled state.
            ed.onNodeChange.add(function(ed, cm, n, co) {
                var insideQuote = ed.dom.getParent(n, "q") != null || ed.dom.getParent(n, "blockquote") != null;
                cm.setDisabled('epiquote', co && !insideQuote);
                cm.setActive('epiquote', insideQuote);
            });

            //Add keydown event for the editor to catch double enter so that it is possible to leave a blockquote if it is last.
            ed.onKeyUp.addToTop(function(ed, e) {
                if (e.keyCode == 13) {
                    if (e.shiftKey || e.altKey || e.ctrlKey) {
                        return;
                    }
                    t._quoteEscape(ed, e);
                }
            });
        },

        /**
        * Used to escape a blockquote if the user hits enter key twice in the end of a blockquote.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {Event} ev DOM Event object.
        */
        _quoteEscape: function(ed, ev) {
            function isConsideredEmpty(node) {
                return (node.innerHTML == "" || node.innerHTML == " " || node.innerHTML == "<br>");
            }
            var pElem,
            s = ed.selection,
            current = s.getNode(),
            parent = ed.dom.getParent(current, "blockquote");

            if (parent != null) {
                var children = parent.childNodes,
                len = children.length;

                if (isConsideredEmpty(current) && children[len - 1] === current && isConsideredEmpty(children[len - 2])) {
                    ed.dom.remove(children[len - 2]);
                    ed.dom.remove(current);
                    pElem = ed.dom.create("p", null, " ");
                    ed.dom.insertAfter(pElem, parent);

                    s.select(pElem, true);
                    s.collapse();
                    tinymce.dom.Event.cancel(ev);
                    //Wrap all changes as one undo level
                    ed.undoManager.add();
                }
            }
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'Quote plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epiquote', tinymce.plugins.epiquote);
}(tinymce, epiJQuery));