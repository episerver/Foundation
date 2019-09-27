function ChangeCurrentCulture(cult)
{
  var CurrentCulture = document.getElementById('CurrentCulture');
  CurrentCulture.value = cult;
}
function ChangeCurrentUICulture(cult)
{
  var CurrentUICulture = document.getElementById('CurrentUICulture');
  CurrentUICulture.value = cult;
}

function SendReq(objId, str)
{
  var obj = document.getElementById(objId);  
  var req = window.XMLHttpRequest? 
		new XMLHttpRequest() : 
		new ActiveXObject("Microsoft.XMLHTTP");
	req.onreadystatechange = function(){
	  if (req.readyState != 4 ) return ;
    if (req.readyState == 4)
    {
      if (req.status == 200)
	      obj.value = req.responseText.toString();
	    else
	      alert('Cant load XML data.');
	      //SendReq();
    }
	} 
	var dt = new Date();
	var uniqID = dt.getMinutes() + "_" + dt.getSeconds() + "_" + dt.getMilliseconds();
	req.open("GET", "../Util/GetXML.aspx?uniqID="+uniqID+"&"+str, true);
	req.send(null);
}

function OpenWindow(query,w,h,scroll,resize,status)
{
  var l = (screen.width - w) / 2;
	var t = (screen.height - h) / 2;
	
	winprops = 'height='+h+',width='+w+',top='+t+',left='+l;
	if (scroll) winprops+=',scrollbars=1';
	if (resize) 
		winprops+=',resizable=1';
	else
		winprops+=',resizable=0';
	if (status)	
		winprops+=',status=1';
	else
		winprops+=',status=0';
	var f = window.open(query, "_blank", winprops);
}

function GetTotalOffset(eSrc)
{
	this.Top = 0;
	this.Left = 0;
	while (eSrc)
	{
		this.Top += eSrc.offsetTop;
		this.Left += eSrc.offsetLeft;
		eSrc = eSrc.offsetParent;
	}
	return this;
}

function getElem(s)
{
	return document.getElementById(s)
}