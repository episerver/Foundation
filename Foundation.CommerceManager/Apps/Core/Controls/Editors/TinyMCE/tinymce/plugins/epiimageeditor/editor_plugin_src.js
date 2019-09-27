(function(tinymce, $) {
    //The image that cause Image Editor button enabled.
    var imageNode;

    tinymce.create('tinymce.plugins.epiimageeditor', {
        /**
        * Initializes the plugin, this will be executed after the plugin has been created.
        * This call is done before the editor instance has finished it's initialization so use the onInit event
        * of the editor instance to intercept that event.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            ed.addCommand('mceEPiImageEditor', function() {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1)
                    return;

                // Resolve the path to the dynamic content dialog and append thew page context parameters to the query string.
                var dialogURL = EPi.ResolveUrlFromUI('Edit/ImageEditor/ImageEditor.aspx?') + $.param(ed.settings.epi_page_context);
                var selectedNode = imageNode; //NOTE: IE9, image selection may be lost before clicking on buttons
                var sizeIsSpecified;

                var onDialogComplete = function(returnData, onCompleteArgs) {

                    if (!returnData) {
                        return;
                    }

                    // s.setContet() seems to have a bug that makes it loose its selection in IE8 if the main window don't have focus (and the selection is empty or not selectable).
                    // To solve this we need to make a focus call on editor window first.
                    ed.getWin().focus();
                    if (sizeIsSpecified) {
                        ed.dom.setAttrib(selectedNode, 'width', returnData.actualWidth);
                        ed.dom.setAttrib(selectedNode, 'height', returnData.actualHeight);
                    }
                    var currentwidth = ed.dom.getAttrib(selectedNode, "src");

                    //Add a timestamp value to query string to force browser to reload
                    var timeStamp = (new Date()).getTime();
                    var delimeter = (returnData.src.indexOf('?') < 0) ? '?' : '&';
                    ed.dom.setAttrib(selectedNode, 'src', returnData.src + delimeter + 'epi_image_timestamp=' + timeStamp);

                    //Wrap all changes as one undo level
                    ed.undoManager.add();

                    ed.windowManager.onClose.dispatch();
                };

                var dialogFeatures = { width: ed.getParam("epiImageEditor_dialogWidth"), height: ed.getParam("epiImageEditor_dialogHeight") };
                var dialogArguments = {};
                //remove timestamp from image source
                var imgSrc = ed.dom.getAttrib(selectedNode, "src").replace(/(\?|&)epi_image_timestamp=.*/g, '');
                dialogArguments.src = imgSrc;
                sizeIsSpecified = ed.dom.getAttrib(selectedNode, "width");

                ed.windowManager.onOpen.dispatch();
                EPi.CreateDialog(dialogURL, onDialogComplete, null, dialogArguments, dialogFeatures);
            });

            // Register button
            ed.addButton('epiimageeditor', {
                title: 'epiimageeditor.epiimageeditor_desc',
                cmd: 'mceEPiImageEditor',
                "class": "mce_epiimageeditor"
            });

            // Add a node change handler, selects the button in the UI when a image is selected
            ed.onNodeChange.add(function(ed, cm, n) {
                //Prevent tool from being activated as objects represented as images.
                var isStandardImage = n.nodeName === 'IMG' && ed.dom.getAttrib(n, 'class').indexOf('mceItem') === -1;
                cm.setActive('epiimageeditor', isStandardImage);
                cm.setDisabled('epiimageeditor', !isStandardImage);

                //Set or reset imageNode to the node cause Image Editor command enabled
                imageNode = isStandardImage ? n : null;
            });

            ed.onPostProcess.add(function(ed, o) {
                //remove timestamp from image sources
                o.content = o.content.replace(/(\?|&)epi_image_timestamp=.*"/g, '"');
            });
        },

        createControl: function(n, cm) {
            return null;
        },

        /**
        * Returns information about the plugin as a name/value array.
        * The current keys are longname, author, authorurl, infourl and version.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'EPiServer CMS Image Editor Plug-in',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: "1.0"
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('epiimageeditor', tinymce.plugins.epiimageeditor);
} (tinymce, epiJQuery));