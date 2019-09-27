var isSubmit = false;

// Check to see if a form has been changed after the page loads
function isFormChanged() {

    if(CSManagementClient.IsPageDirty)
        return true;
        
  var f = document.forms;
  
  // loop through all forms
  for (var i=0; i < f.length; i++) {
	 	 
    var el = f[i].elements;
	 // loop through each element in each form
	 for (var j=0; j < el.length; j++) {
		if (el[j].type && isElementChanged(el[j])) {
		    //alert(el[j].type);
		  return true;
		} 
	 }
  }
  
  return false;
}

// Check to see if a form element has been changed after the page loads
function isElementChanged(el) {
  // correct case of element type
  switch (el.type.toLowerCase()) {
    case 'text':
	 case 'textarea':
	 case 'password':
	   if (el.value != el.defaultValue) {
		  return true;
		}
		break;
		/*
    case 'radio':
	 case 'checkbox':
	   if (el.checked != el.defaultChecked) {
		  return true;
		}
		break;
		*/
		/*
    case 'select-one':
	 case 'select-multiple':
	   for (var k=0; k < el.options.length; k++) {
		  if (el.options[k].selected != el.options[k].defaultSelected) {
		    return true;
		  }
		}
		break;
		*/
		
    return false;
  }
}


// Check form for any unsaved changes before leaving page
function formCheck(e_) {
  var message = 'You have unsaved changes.'; 
  
  if (!e_ && window.event) {
    e_ = window.event;
  }
  
  var isChanged = isFormChanged();
  
  // don't run if submit button was clicked or form hasn't changed or there is no event
  if (!isSubmit && isChanged && e_) {
    e_.returnValue = message;
    return message;
  }
}

//window.onbeforeunload = formCheck;