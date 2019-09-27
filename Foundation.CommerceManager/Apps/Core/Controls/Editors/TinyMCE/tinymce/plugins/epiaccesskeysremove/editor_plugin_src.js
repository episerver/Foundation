(function(tinymce, $) {
    var Event = tinymce.dom.Event;

    tinymce.create('tinymce.plugins.epiaccesskeysremove', {
        /**
        * Initializes of epiaccesskeysremovePlugin. Adds command, button and nodeChange event handler.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            ed.onInit.add(function(ed, cm) {
                //the name sent to removeattr is case sensitive. Since Tiny adds with capital K that should be used. 
                $("[accesskey]", ed.getContainer()).removeAttr("accessKey");
            });
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'EPiServer Access Keys Remove',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }
    });
    // Register plugin
    tinymce.PluginManager.add('epiaccesskeysremove', tinymce.plugins.epiaccesskeysremove);
} (tinymce, epiJQuery));