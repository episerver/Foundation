(function(tinymce, $) {
    var Event = tinymce.dom.Event;

    tinymce.create('tinymce.plugins.epieditordisable', {
        /**
        * Initializes of epieditordisablePlugin. Adds command, button and nodeChange event handler.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {

            var disabled = ed.settings.readonly;
            var t = this;

            t.editor = ed;

            if (disabled) {
                ed.onInit.add(function(ed, cm) {
                    //We put the onNodeChange inside onInit to try make sure that our onNodeChange is run last.
                    ed.onNodeChange.add(function(ed, cm, n, co) {
                        t._setAllDisabled(true);
                    });
                    $(".mceStatusbar", ed.getContainer()).addClass("disabled");
                });
            }
        },

        /**
        * Enables or disables controls of the editor instance if ueser selects or deselects a dynamic content node.
        *
        * @param {bool} disable true if we want to disable controls.
        * @param {string} enabledControls Comma separated string of id's indicating controls which should not be disabled.
        */
        _setAllDisabled: function(disable) {
            var t = this, ed = t.editor;

            //Since the buttons have the editor id in its id we add that to the ids specified in enabledControls.
            var i;

            tinymce.each(ed.controlManager.controls, function(control) {
                control.setDisabled(disable);
            });

        },


        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'EPiServer Editor Disable',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }
    });
    // Register plugin
    tinymce.PluginManager.add('epieditordisable', tinymce.plugins.epieditordisable);
}(tinymce, epiJQuery));