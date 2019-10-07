(function(tinymce, $) {
	tinymce.create('tinymce.plugins.epistylematcher', {
		init : function(ed, url) {						
			ed.onInit.add(function(ed) {
			    //By default, when a node matches more than one style formats, tinyMCE take the first one to set as selected on the style formats drop down list.
			    //This is not correct when we have a style format which is a narrower case of another one.
			    //Therefore, we extend the formatter's matchAll method to sort the matched style formats.

			    var formatter = ed.formatter;
			    var oldMatchAll = formatter.matchAll;
			    var oldRemove = formatter.remove;
    			
    			//Replace the default method
    			formatter.matchAll = function() {
    			    //Call the default method
    			    var list = oldMatchAll.apply(this, arguments);    			    
    			    
    			    //Sort the result list to have the better matched style formats on the top
    			    list = list.sort(function(item1, item2) {
			            var format1 = formatter.get(item1)[0];
			            var format2 = formatter.get(item2)[0];
			        
			            //Only applicable to the formats which change element type
			            if ((format1.block == format2.block) && (format1.inline == format2.inline)) {
			                //If a format contains no class, it should be put after
			                if (!format1.classes) { return 1; }
			                if (!format2.classes) { return -1; }
			                
			                //Find out the format contains more classes
			                var classes1 = format1.classes.length > format2.classes.length ? format1.classes : format2.classes;
			                var classes2 = format1.classes.length > format2.classes.length ? format2.classes : format1.classes;
			                
			                //Check if it contains the other one
			                var contains = true;
			                for (var i in classes2) {
			                    var found = false;
			                    for (var j in classes1) {
			                        if (classes1[j] == classes2[i]) {
			                            found = true;
			                            break;
			                        }
			                    }
			                    if (!found) {
			                        contains = false;
			                        break;
			                    }
			                }
			                
			                //If one format contains the other, the container is the better match, put it before
			                if (contains) {
			                    return format1.classes.length > format2.classes.length ? -1 : 1
			                }
			                else {
			                    return 0;
			                }
			            }
			            else {
			                return 0;
			            }
    			    });
    			    
    			    //Return the list
    			    return list;
    			};
    			formatter.remove = function(name, vars, node) {
    			    if (!name) {
    			        var sel = ed.selection;
    			        var currentNode;
    			        if ((currentNode = sel.getStart()) == sel.getEnd()) {
    			            var currentNodeFormat = ed.dom.isBlock(currentNode) ? {
    			                block: currentNode.tagName.toLowerCase()
                            } : {
                                inline: currentNode.tagName.toLowerCase()
                            }
    			            this.register('__current__', currentNodeFormat);
    			            oldRemove.apply(this, ['__current__', vars, node]);
    			        }
    			    }
    			    else {
    			        oldRemove.apply(this, arguments);
    		        }
    		    };
			});
		},

		getInfo : function() {
			return {
                longname: 'styling improvement plugin',
                author: 'EPiServer AB',
                authorurl: 'http://www.episerver.com',
                infourl: 'http://www.episerver.com',
                version: 1.0
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('epistylematcher', tinymce.plugins.epistylematcher);
} (tinymce, epiJQuery));