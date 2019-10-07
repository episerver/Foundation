
	function DisableButtons(object)
	{
		var ParentId = "";
		
		if (typeof(object) != "undefined")
			ParentId = object.id.toLowerCase();
			
		var CanDesable = true;
		if ((typeof(Page_Validators) != "undefined") && (ParentId.indexOf("cancel") < 0) && (ParentId.indexOf("back") < 0))
		{
			if(Page_ValidationActive)
			{
				Page_ClientValidate();
				if(!Page_IsValid)
					CanDesable = false;
			}
		}
		if(CanDesable && !browseris.nav)
		{
			var btnCollection = document.getElementsByTagName("button");
			for(var i = 0;i < btnCollection.length;i++)
				btnCollection[i].disabled = true;
			btnCollection = document.getElementsByTagName("input");		
			for(var i = 0;i < btnCollection.length;i++)
			{
				if(btnCollection[i].type == "submit" || btnCollection[i].type == "reset" ||
					btnCollection[i].type == "button")
						btnCollection[i].disabled = true;		
			}
		}
	}