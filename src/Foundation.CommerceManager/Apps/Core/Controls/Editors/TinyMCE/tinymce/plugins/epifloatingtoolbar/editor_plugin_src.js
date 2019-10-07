(function(tinymce, $) {
    tinymce.create('tinymce.plugins.epifloatingtoolbar', {
        /**
        * Initializes of epifloatingtool plugin.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function (ed, url) {
            ed.isFullscreen = false;
            ed.onInit.add(function (ed, cm) {
                if (!ed.isFullscreen) {
                    var d = tinymce.DOM, toolbar = d.get(ed.id + '_external').parentNode;
                    d.addClass(toolbar, "mceEditor epi-lightSkin epi-lightSkinOpe");
                    d.setAttrib(toolbar, "style", "");
                    d.add('epi-opeToolbar', toolbar);
                }
            });
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Floating Toolbar plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }
    });
    // Register plugin
    tinymce.PluginManager.add('epifloatingtoolbar', tinymce.plugins.epifloatingtoolbar);
} (tinymce, epiJQuery));