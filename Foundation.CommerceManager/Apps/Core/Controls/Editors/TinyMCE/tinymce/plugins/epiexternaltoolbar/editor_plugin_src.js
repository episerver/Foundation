(function(tinymce, $) {
    var Event = tinymce.dom.Event;

    tinymce.create('tinymce.plugins.epiexternaltoolbar', {
        /**
        * Initializes of epidynamiccontentPlugin. Adds command, button and nodeChange event handler.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            ed.isFullscreen = false;
            ed.onInit.add(function(ed, cm) {

                if (!ed.isFullscreen) {
                    var row,
                    edTable = $("table.mceLayout", ed.getContainer()),
                    edToolbarRows = $("td.mceToolbar", edTable),
                    newTable = $("<table/>");
                    $.each(edToolbarRows, function(i) {
                        row = $(this).parent();
                        row.remove(); //Remove the tr from the current place in the dom                
                        newTable.append(row); //Add the tr to our new table
                    });
                    $("<div/>").insertBefore($(edTable)).append(newTable).addClass("epiExternalToolbar");
                    newTable.addClass("mceLayout");
                    edTable.css("height", "100%");
                }
            });
            //Since tyhe "external toolbar" have alot of visual issues in fullscreen we listen on ExecCommand event so we can
            //add functionality when we maximize and minimize the editor. (Like disable the toolbar init above).
            ed.onExecCommand.add(function(ed, cmd, ui, val) {
                if (cmd == "mceFullScreen") {
                    var fullEd = tinyMCE.activeEditor;
                    if (fullEd.id !== ed.id && fullEd.id === "mce_fullscreen") {
                        fullEd.isFullscreen = true;
                    } else {
                        fullEd.isFullscreen = false;
                    }
                }
            });
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'External Toolbar plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }
    });
    // Register plugin
    tinymce.PluginManager.add('epiexternaltoolbar', tinymce.plugins.epiexternaltoolbar);
}(tinymce, epiJQuery));