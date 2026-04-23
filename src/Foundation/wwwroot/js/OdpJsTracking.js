document.addEventListener("DOMContentLoaded", function (event) {
	if (typeof $$epiforms !== 'undefined') {
		$$epiforms(document).ready(function myfunction() {
			$$epiforms(".EPiServerForms").on("formsNavigationNextStep formsNavigationPrevStep formsSetupCompleted formsReset formsStartSubmitting formsSubmitted formsSubmittedError formsNavigateToStep formsStepValidating",
				function (event, param1, param2) {
					var eventType = event.type;
					var formName = event.workingFormInfo.Name;
					if (eventType == 'formsSetupCompleted') {
						console.log('ODP: web_form impression: ' + formName);
						//zaius.event('web_form', { action: 'impression', form_name: formName });
						zaius.event('web_form', { action: 'impression', form_name: formName, campaign: formName });
					} else if (eventType == 'formsStepValidating') {
						if (!event.isValid) {
							console.log('ODP: web_form validation failed: ' + formName);
							zaius.event('web_form', { action: 'submission_validation_failed', form_name: formName, campaign: formName });
						}
					} else if (eventType == 'formsSubmitted') {
						console.log('ODP: web_form submission: ' + formName);
						zaius.event('web_form', { action: 'submission', form_name: formName, campaign: formName });
					} else {
						// handle other form events here 
					}
				});
		});
	}
});