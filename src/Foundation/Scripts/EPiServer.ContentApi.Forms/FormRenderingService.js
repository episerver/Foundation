// This class use ES6 syntax. In case you want to use it in a browser that does not support ES6 syntax (i.e Internet Explorer 11 and below),
// you can copy/paste the render() function to your own script file and replace 'let' keyword with 'var'.
export default {
    render: (formModel, attachNode) => {
        // Initialize form js. Those scripts are used for all forms and only need to execute once, so we check here to make sure
        // when call this method second time, these scripts not execute again.
        if (window.epi == null || window.epi.EPiServer == null || window.epi.EPiServer.Forms == null) {
            // Those eval statements are safe because js strings come from the formModel
            // and formModel comes directly from serverside (not any script can intercept)
            window.eval(formModel.assets.originalJquery);
            window.eval(formModel.assets.jquery);            
        }

        // In case form has custom scripts for certain types of element.
        // ex: DateTimeElementBlock inherits from IElementRequireClientResources and so has its own resources
        // Those resources are different for each form so we have to execute the prerequisite script once for each form.
        window.eval(formModel.assets.prerequisite);        

        // Inject form's css
        let style = document.createElement('style');
        style.type = 'text/css';
        style.innerHTML = formModel.assets.css;
        document.getElementsByTagName('head')[0].appendChild(style);

        // Attach form html template to the attachNode
        let element = typeof attachNode === 'object' ? attachNode : document.getElementById(attachNode);

        // We use jQuery.html() here to allow to execute script embedded inside template of Forms' elements (.ascx files). 
        // The form HTML template also contains script of `formModel.assets.formInitScript` so we dont need to explicitly eval formInitScript
        window.jQuery(element).html(formModel.template);

        // Execute form viewmode script. We must set window.epi.EPiServer.Forms.__Initialized = false here otherwise
        // in case page has two or more forms, the second form's viewmode script will not be called.
        window.epi.EPiServer.Forms.__Initialized = undefined;
        eval(formModel.assets.viewModeJs);
    }
};