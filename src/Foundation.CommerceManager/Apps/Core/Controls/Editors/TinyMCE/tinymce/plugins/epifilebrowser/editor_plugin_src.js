(function(tinymce, $) {

    tinymce.create('tinymce.plugins.epifilebrowser', {
        /**
        * Initializes of the file browser plug-in. Sets the file_browser_callback setting which enables the browse button in tinyMCE dialogs.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(editor, url) {
            // Early exit if we don't have access to EPi...
            if (typeof EPi === "undefined" || typeof EPi.ResolveUrlFromUI != "function") {
                return;
            }

            var fileBrowserComplete = function(returnValue, onCompleteArguments) {
                // The File manager browser returns a an object with a collection of selected items
                if (returnValue && returnValue.items && returnValue.items.length === 1) {
                    var doc = onCompleteArguments.window.document;
                    var formInput = doc.getElementById(onCompleteArguments.formInputId);

                    var firstFile = returnValue.items[0];
                    
                    if (formInput.value === firstFile.path) {
                        return;
                    }

                    formInput.value = firstFile.path;

                    // The tinyMCE image dialogs has a function call specified in the onchange attribute.
                    // The advanced image dialog expects this.value as its first argument. For now we just mock that behaviour.
                    if (formInput.onchange) {
                        formInput.onchange(formInput.value);
                    }

                    // If we're setting the image src field try finding an alt field and use the image description as alt text automatically.
                    if (onCompleteArguments.formInputId === "src") {
                        var altInput = doc.getElementById("alt");
                        if (altInput != null && typeof (firstFile.description) === "string" && !altInput.value) {
                            altInput.value = firstFile.description;
                        }
                    }
                }

                editor.windowManager.onClose.dispatch();
            };

            var fileBrowserCallback = function(formInputId, value, type, win) {

                var pageContext = editor.settings.epi_page_context;

                // Create a new parameter object by extending the page context with file manager specific parameters
                var dialogParameters = $.extend({}, pageContext, {
                    hideclearbutton: true,
                    browserselectionmode: type,
                    selectedfile: value  // The url is encoded in the input field, but the unencoded version should be used in js variables.
                });

                var dialogUrl = EPi.ResolveUrlFromUI("edit/FileManagerBrowser.aspx") + "?" + $.param(dialogParameters);
                editor.windowManager.onOpen.dispatch();
                EPi.CreateDialog(dialogUrl, fileBrowserComplete, { window: win, formInputId: formInputId }, dialogParameters, { width: 800, height: 650 }, win);
            };

            editor.settings.file_browser_callback = fileBrowserCallback;
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'File Browser Plug-In',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };
        }


    });

    // Register plugin
    tinymce.PluginManager.add('epifilebrowser', tinymce.plugins.epifilebrowser);
}(tinymce, epiJQuery));