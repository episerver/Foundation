function CheckExistence(objTo,str)
{
	for(var j=0;j<objTo.options.length;j++)
		if(objTo.options[j].text==str)
			 return true;
	return false;
}

function CheckExistenceValue(objTo,strValue)
{
	for(var j=0;j<objTo.options.length;j++)
		if(objTo.options[j].value==strValue)
			 return true;
	return false;
}

function AddOption(objTo,Option)
{
	var oOption = document.createElement("OPTION");
	oOption.text = Option.text;
	oOption.value = Option.value;
	objTo.options[objTo.options.length] = oOption;
}

function AddOption2(objTo,oText, oValue)
{
	var oOption = document.createElement("OPTION");
	oOption.text = oText;
	oOption.value = oValue;
	objTo.options[objTo.options.length] = oOption;
}

function DeleteAll(FromControl)
{
	if(FromControl!=null)
	{
		do
		{
			FromControl.options[0]=null;
		}
		while (FromControl.options.length>0)
	}
}

function MoveAll(FromControl,ToControl)
{
	if((FromControl!=null)&&(ToControl!=null))
	{
		for(var i=0;i<FromControl.options.length;i++)
			if(!CheckExistence(ToControl,FromControl.options[i].text))
					AddOption(ToControl,FromControl.options[i]);
		do
		{
			FromControl.options[0]=null;
		}
		while (FromControl.options.length>0)
	}
}

function MoveOne(FromControl,ToControl)
{
	if((FromControl!=null)&&(ToControl!=null))
	{
		for(var i=0;i<FromControl.options.length;i++)
			if((FromControl.options[i].selected) && (!CheckExistence(ToControl,FromControl.options[i].text)))
					AddOption(ToControl,FromControl.options[i]);

		for(var i=0;i<FromControl.options.length;i++)
			if(FromControl.options[i].selected)
			{
				FromControl.options[i]=null;		
				FromControl.selectedIndex = (i < FromControl.options.length) ? i : 0;
				return true;
			}
	}
		
}

function MoveUp(control)
{
	if(control!=null && control.selectedIndex > 0)
	{
		var pos = control.selectedIndex;
		var opt = control.options[pos];
		control.options[pos] = null;
//		control.options.remove(pos);
		control.options.add(opt, pos-1);
	}
}

function MoveDown(control)
{
	if(control!=null && control.selectedIndex >= 0 && control.selectedIndex < control.options.length - 1)
	{
		var pos = control.selectedIndex;
		var opt = control.options[pos];
		control.options[pos] = null;
//		control.options.remove(pos);
		control.options.add(opt, pos+1);
	}
}

//TODO: Make it work in IE
function ClearListSelection(obj)
{
	if (obj != null)
	{
		for (var i = 0; i < obj.options.length; i++)
		{
			if (obj.options[i].selected)
			{
				obj.options[i].selected = false;
			}
		}
	}
}

//DV 2004-04-06

function MoveOne2(FromControl, ToControl, FromHidden, ToHidden)
{
	if((FromControl!=null)&&(ToControl!=null))
	{
		for(var i=0;i<FromControl.options.length;i++)
			if((FromControl.options[i].selected) && (!CheckExistenceValue(ToControl,FromControl.options[i].value)))
			{
				AddOption(ToControl,FromControl.options[i]);
				var str = FromControl.options[i].value;
				//dobavit v hidden pole uid kolonki
				if (ToHidden.value == '')
				    ToHidden.value = str;
				else
				    ToHidden.value += ',' + str;
				    
				if (FromHidden.value.indexOf(str) > 0)
				    FromHidden.value = FromHidden.value.replace(','+str, '');
				else
				    FromHidden.value = FromHidden.value.replace(str+',', '');
		    }

		for(var i=0;i<FromControl.options.length;i++)
			if(FromControl.options[i].selected)
			{
				FromControl.options[i]=null;		
				FromControl.selectedIndex = (i < FromControl.options.length) ? i : 0;
				return true;
			}
	}
	
		
}


//AlexK - 2007-02-27

function AddToSel(fromCol, toCol, toHide) 
{
	var toLen = toCol.options.length;
	var toName = toCol.name;
	var selArr = new Array;
	var sel = false;
	var cnt = -1;
	for (i = 0; i < fromCol.options.length; i++) {
		if (fromCol.options[i].selected) {
			sel = true;
			toCol.options[toLen] = new Option(fromCol.options[i].text, fromCol.options[i].value);
			cnt++;
			selArr[toLen] = "sel";
			toLen++;
			if (toHide.value.length > 0)
				toHide.value = toHide.value + "," + fromCol.options[i].value;
			else
				toHide.value = fromCol.options[i].value;
		}
	}
	toCol.options.length = toLen;
	if (sel == false) {
		return;
	}
	for (i = 0; i < toCol.options.length; i++) {
		if (selArr[i] == "sel") {
			toCol.options[i].selected = true;
		} else {
			toCol.options[i].selected = false;
		}
	}
}

function DeleteFromSel(delCol, delHide) 
{
	var delArr = delHide.value.split(",");
	var cnt = 0;
	for (i = 0; i < delCol.options.length; i++) {
		if (delCol.options[i].selected != true) {
			delCol.options[cnt].text = delCol.options[i].text;
			delCol.options[cnt].value = delCol.options[i].value;
			delArr[cnt] = delArr[i];
			cnt++;
		} else {
			delCol.options[i].selected = false;
		}
	}
	for (i = cnt; i < delCol.options.length; i++) {
		delCol.options[i] = null;
		delArr[i] = null;
	}
	delCol.options.length = cnt;
	delArr.length = cnt;
	var joinStr = delArr.join(",");
	delHide.value = joinStr;
}

function MoveColSel(moveCol, dir, moveHide)
{
	if(moveCol.options.length < 1)
		return;
	if ((moveCol.options[0].selected && dir == -1) ||
		(moveCol.options[moveCol.options.length-1].selected && dir == 1) ) 
		return;	
	
	var moveVal;
	var moveArr = moveHide.value.split(",");
	var boarderSelect = false;
	var boarderVal;
	var boarderText;
	var begin;
	var end;
	var sel = false;
	if (dir == 1) {
		begin = moveCol.options.length - 1;
		end = -1;
	} else {
		begin = 0;
		end = moveCol.options.length;
	}
	var cnt = begin;
	while (cnt != end) {
		if (moveCol.options[cnt].selected) {
			sel = true;
			if (boarderSelect == false) {
				boarderSelect = true;
				boarderVal = moveCol.options[cnt + dir].value;
				boarderText = moveCol.options[cnt + dir].text;
				moveVal = moveArr[cnt + dir];
			}
			moveCol.options[cnt + dir].text = moveCol.options[cnt].text;
			moveCol.options[cnt + dir].value = moveCol.options[cnt].value;
			moveArr[cnt + dir] = moveArr[cnt];
			moveCol.options[cnt + dir].selected = true;
			moveCol.options[cnt].selected = false;
			if (cnt == end + dir) {
				moveCol.options[cnt].value = boarderVal;
				moveCol.options[cnt].text = boarderText;
				moveArr[cnt] = boarderVal;
			}
		} else {
			if (boarderSelect == true) {
				boarderSelect = false;
				moveCol.options[cnt + dir].value = boarderVal;
				moveCol.options[cnt + dir].text = boarderText;
				moveArr[cnt + dir] = boarderVal;
			}
		}
		cnt = cnt - dir;
	}
	if (sel == false)
		return;
	var joinStr = moveArr.join(",");
	moveHide.value = joinStr;
}