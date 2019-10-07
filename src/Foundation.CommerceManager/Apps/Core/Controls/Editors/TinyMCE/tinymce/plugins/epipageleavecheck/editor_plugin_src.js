(function(tinymce, $) {
    var Event = tinymce.dom.Event;

    tinymce.create('tinymce.plugins.epipageleavecheck', {
        /**
        * Initializes of epipageleavecheck. Used to indicate that changes have been made in the editor when leaving page.
        *
        * @param {tinymce.Editor} ed Editor instance that the plugin is initialized in.
        * @param {string} url Absolute URL to where the plugin is located.
        */
        init: function(ed, url) {
            //We need to use our own isDirty since tiny version is reset on onSubmit event that sometimes is run before our check.
            var isDirty;
            var checkChanges;
            var disabled; // use to avoid showing the leave confirm twice
            var startContent; // since ed.startContent returns raw content, we use our own
            var startContentAssigned; //flag indicating that the start content has been assigned
            var DOM = tinymce.DOM, Event = tinymce.dom.Event;
            
            ed.onChange.add(function(ed) {
                //Changes are only applied after this event handler return. 
                //Therefore, just turn on the "checkChanges" flag. 
                //Actual changes will be checked when leaving the page
                if (!disabled) {
                    checkChanges = true;
                }
            });

            //Workaround a tinyMCE bug on IE7 emulated mode. See: http://tinymce.moxiecode.com/punbb/viewtopic.php?id=18217
            ed.onPostRender.add(function(ed) {
                var mouseUpHandler1, mouseUpHandler2;
                
                var endResize = function(e) {
                    //remove enresize handlers
                    if (mouseUpHandler1) {
					    Event.remove(DOM.doc, 'mouseup', mouseUpHandler1);
					}
					if (mouseUpHandler2) {
					    Event.remove(ed.getDoc(), 'mouseup', mouseUpHandler2);
					}
					
					// as soon as resizing finish, enable pageleavecheck again
					// since IE7 raises onbeforeunload after this point, we have to queue "enabling code" to run after that, 1ms is fine.
					window.setTimeout(function() {
                        EPi.PageLeaveCheck.enabled = true;
					}, 1);                     
                };
                
                Event.add(ed.id + '_resize', 'mousedown', function(e) {
                    //register endresize eventhandlers
				    mouseUpHandler1 = Event.add(DOM.doc, 'mouseup', endResize);
				    mouseUpHandler2 = Event.add(ed.getDoc(), 'mouseup', endResize);                

                    //disable page leave check on start resizing
                    EPi.PageLeaveCheck.enabled = false;
                });
            });
            
            // when tinymce set content for the first time
            ed.onSetContent.add(function(ed, o) {
                if (!startContentAssigned) {
                    if (ed.id === 'mce_fullscreen') {                
                        if (!o.initial) {
                            // in fullscreen mode, initial content is empty since tinymce create fullscreen editor from a dummy textarea. Catch the second call to setContent
                            startContent = tinymce.trim(ed.getContent({ format: 'html', no_events: 1 })); //do not need to trigger getContent event, because the editor is just initialized
                            startContentAssigned = true;
                        }
                    }
                    else {
                        startContent = tinymce.trim(ed.getContent({ format: 'html', no_events: 1 }));
                        startContentAssigned = true;
                    }
                }
            });
                
            if (typeof (EPi) !== 'undefined' && typeof (EPi.PageLeaveCheck) !== 'undefined') {
                EPi.PageLeaveCheck.AddToChecklist(function(setDirty) {
                    if (typeof (setDirty) !== 'undefined') {
                        isDirty = setDirty;
                        // when isDirty is set manually (ex: call to SetPageChanged), we don't need to check the actual changes
                        // so checkChanges is temporarily turned off until user makes new changes.
                        checkChanges = false;
                        disabled = true;
                    }
                    if (checkChanges) {
                        // we do not trust either isDirty() or isNotDirty, so we check ourselves.
                        if (ed.getDoc() != null) {
                            // set no_events to false if startContent is empty or else it will not set isDirty correctly.
                            // if we set no_events to true and startContent is empty we will get <p>&nbsp;</p> from getContent
                            var no_events = startContent != '';
                            var currentContent = tinymce.trim(ed.getContent({ format: 'html', no_events: no_events }));
                            isDirty = (startContent != currentContent);
                        }
                        else {
                            // the fullscreen editor instance return null doc when it is toggled off, just return "no change" in this case
                            isDirty = false;
                        }
                    }
                    ed.isNotDirty = !isDirty;
                    return isDirty;
                });
            }
        },

        /**
        * Returns information about the plugin as a name/value array.
        *
        * @return {Object} Name/value array containing information about the plugin.
        */
        getInfo: function() {
            return {
                longname: 'Page Leave Check plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
            };  
        }
    });
    // Register plugin
    tinymce.PluginManager.add('epipageleavecheck', tinymce.plugins.epipageleavecheck);
}(tinymce, epiJQuery));
