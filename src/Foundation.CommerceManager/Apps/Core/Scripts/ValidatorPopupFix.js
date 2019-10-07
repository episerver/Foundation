if (typeof(jQuery) != 'undefined')
{
	$(document).ready(function() 
	{
		if (AjaxControlToolkit && AjaxControlToolkit.ValidatorCalloutBehavior)
		{
			AjaxControlToolkit.ValidatorCalloutBehavior.prototype.AddOption = function(objTo,Option) 
				{
					var oOption = document.createElement("OPTION");
					oOption.text = Option.text;
					oOption.value = Option.value;
					if(objTo!=null)
						objTo.options[objTo.options.length] = oOption;
					updateListBoxHeight();
				};
		}
	});
}
