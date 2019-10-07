(function(tinymce, $) {
    tinymce.create('tinymce.plugins.epivisualaid', {
        /**
        * Initializes the Filemanager drag and drop plugin.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
             //Add event handler for visualaid change functionality.
            ed.onVisualAid.add(function(ed, e, s) {
                if (s || s === 1) {
                    ed.dom.addClass(ed.getBody(), "epi_visualaid");
                } else {
                    ed.dom.removeClass(ed.getBody(), "epi_visualaid");
                }
            });

            ed.onSubmit.addToTop(function(ed) {
                ed.dom.removeClass(ed.getBody(), "epi_visualaid");
            });
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'Enhanced Visual Aid',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epivisualaid', tinymce.plugins.epivisualaid);
}(tinymce, epiJQuery));