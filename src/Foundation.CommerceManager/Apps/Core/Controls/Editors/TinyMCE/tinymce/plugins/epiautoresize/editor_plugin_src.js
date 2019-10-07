(function(tinymce, $) {
    tinymce.create('tinymce.plugins.epiautoresizeplugin', {
        init: function(ed, url) {
            if (ed.getParam('fullscreen_is_enabled')) {
                return;
            }

            function resize() {
                var height = this.getBody().parentNode.scrollHeight;
                if (height > 0) {
                    this.getContentAreaContainer().firstChild.style.height = height + 'px';
                    $(this.getContainer()).resize();
                }
            }

            ed.onInit.add(function(ed, l) {
                //Remove vertical scrollbar                 
                (tinymce.isIE ? ed.getBody().parentNode : ed.getBody()).style.overflowY = 'hidden';

                //Make the surrounding table expand
                ed.getContainer().lastChild.style.height = 'auto';

                //Remove padding and margin to eliminate jumping when switching top edit mode
                tinymce.DOM.setStyles(ed.getBody(), { 'margin': '0', 'padding': '0', 'height': 'auto' });

                //Listen for changes in content
                ed.onChange.add(resize);
                ed.onSetContent.add(resize);
                ed.onPaste.add(resize);
                ed.onKeyUp.add(resize);
                ed.onPostRender.add(resize);
                ed.onMouseUp.add(resize);
            });
            ed.addCommand('mceepiAutoResize', resize);
        },
        
        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function () {
            return {
                longname: 'Auto resize plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }

    });
    tinymce.PluginManager.add('epiautoresize', tinymce.plugins.epiautoresizeplugin);
} (tinymce, epiJQuery));