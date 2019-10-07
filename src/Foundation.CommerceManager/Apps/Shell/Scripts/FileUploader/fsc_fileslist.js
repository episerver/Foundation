//*****MCFList_Control**********************************
function MCFList_Control(id, MCFUId, deleteTemplate, binderProvider, showTN, SSI, MaxTNDimension)
{
  this.id = id;
  this.MCFUId = MCFUId;
  this.deleteTemplate = deleteTemplate;
  this.binderProvider = binderProvider;
  this.mainDiv = document.getElementById(id);
  this.OnRefresh = raiseRefresh;
  if(showTN.toLowerCase()=="true")
	this.showTN = true;
  else	
	this.showTN = false;
  if(SSI.toLowerCase()=="true")
	this.SSI = true;
  else	
	this.SSI = false;
  this.MaxTNDimension = Number(MaxTNDimension);
  this.FilesCount = 0;
  this.OnRefresh();
}

MCFList_Control.prototype.RefreshList = function() {
  var MCFU = MCFU_Array[this.MCFUId];
  refreshFList(this.id, MCFU.sessionUid, MCFU.uploadInfoUrl);
};

MCFList_Control.prototype.DeleteStream = function(streamUid) {
  var MCFU = MCFU_Array[this.MCFUId];
  deleteStream(this.id, MCFU.uploadInfoUrl, streamUid);
};
//***************************************************

//*****MISC Functions********************************
function MCFList_StateChangedHandler(id)
{
  if(!MCFU_Array || !MCFU_Array[id] || !MCFU_Array[MCFU_Array[id].MCFUId])
  {
    setTimeout('MCFList_StateChangedHandler("'+id+'")', 50);
    return;
  }
  var MCFList = MCFU_Array[id];
  var MCFU = MCFU_Array[MCFList.MCFUId];
  
  if(MCFU.State == MCFU_States[0]) //hidden
  {
    MCFList.mainDiv.style.display = "block";
    MCFList.RefreshList();
  }
  if(MCFU.State == MCFU_States[1]) //showed
    MCFList.mainDiv.style.display = "block";
  if(MCFU.State == MCFU_States[2] || MCFU.State == MCFU_States[3] ||
     MCFU.State == MCFU_States[4] || MCFU.State == MCFU_States[5]) //preupload,upload,failed,success
    MCFList.mainDiv.style.display = "none";
}

function refreshFList(obj_id, sessionUid, url)
{
  if(!MCFU_Array || !MCFU_Array[obj_id])
  {
    setTimeout('refreshFList("'+obj_id+'", "'+sessionUid+'", "'+url+'")', 50);
    return;
  }
  
  var obj = MCFU_Array[obj_id];
  var ajaxRequest = GenerateAjaxObject();
  
  ajaxRequest.onreadystatechange = function()
  {
    if (ajaxRequest.readyState != 4 ) return ;
    if(ajaxRequest.readyState == 4)
    {
      if(obj.binderProvider=="xml")
      {
        if(!ajaxRequest.responseXML.documentElement || 
          ajaxRequest.responseXML.documentElement.childNodes.length<0)
          return;
        var z = ajaxRequest.responseXML.documentElement.childNodes;
        var innerStr = "";
        for(var i=0; i<z.length; i++)
        {
          if(obj.showTN == true)
          {
			innerStr += "<img src='"+url+"?key=thumbnail&streamUid="+z[i].childNodes[0].childNodes[0].nodeValue+"&SSI="+obj.SSI+(obj.MaxTNDimension>0?('&MaxTNDimension='+obj.MaxTNDimension.toString()):'')+"'/>&nbsp;"
          }
          innerStr += z[i].childNodes[1].childNodes[0].nodeValue;
          innerStr += "&nbsp;<a href=\"javascript:MCFU_Array['" + obj_id + "'].DeleteStream('"+z[i].childNodes[0].childNodes[0].nodeValue+"');\">" + obj.deleteTemplate + "</a><br/>";
        }
        obj.mainDiv.innerHTML = innerStr;
        obj.FilesCount = z.length;
        obj.OnRefresh();
//          var innerStr = ajaxRequest.responseText;
//          var re = /__deleteTemplate/g;
//          innerStr = innerStr.replace(re, "<span onclick=\"fileDeleteFromList('"+obj_id+"', this)\">" + obj.deleteTemplate + "</span>");
//          obj.mainDiv.innerHTML = innerStr;
      }
      else if(obj.binderProvider=="json")
      {
        var innerStr = "";
        var _count = 0;
        var json = ajaxRequest.responseText;
        try{
          eval("var mas=" + json + ";");
          for(var i=0; i<mas.files.file.length; i++)
          {
			if(obj.showTN==true)
            {
				innerStr += "<img src='"+url+"?key=thumbnail&streamUid="+mas.files.file[i].uId+"&SSI="+obj.SSI+(obj.MaxTNDimension>0?('&MaxTNDimension='+obj.MaxTNDimension.toString()):'')+"'/>&nbsp;"
            }
            innerStr += mas.files.file[i].fileName;
            innerStr += "&nbsp;<a href=\"javascript:MCFU_Array['" + obj_id + "'].DeleteStream('"+mas.files.file[i].uId+"');\">" + obj.deleteTemplate + "</a><br/>";
          }
          _count = mas.files.file.length;
        }
        catch(e){obj.RefreshList();}
        obj.FilesCount = _count;
        obj.OnRefresh();
        obj.mainDiv.innerHTML = innerStr;
      }
    }
  }
	ajaxRequest.open("GET", url + "?key=fileslist&provider="+obj.binderProvider+"&sessionUid="+sessionUid, true);
	ajaxRequest.send(null);
}

function fileDeleteFromList(obj_id, delobj)
{
  if(delobj && delobj.parentNode && delobj.parentNode.parentNode && delobj.parentNode.parentNode.childNodes.length>0)
  {
    var str = "";
    for(var i=0; i<delobj.parentNode.parentNode.childNodes.length; i++)
    {
      var _temp = delobj.parentNode.parentNode.childNodes[i];
      if(_temp.className=="uid")
      {
        str = delobj.parentNode.parentNode.childNodes[i].innerHTML;
        break;
      }
    }
    if(str!="")
      MCFU_Array[obj_id].DeleteStream(str);
  }
}

function deleteStream(obj_id, url, streamUid)
{
  var obj = MCFU_Array[obj_id];
  var ajaxRequest = GenerateAjaxObject();
  
  ajaxRequest.onreadystatechange = function()
  {
    if (ajaxRequest.readyState != 4 ) return ;
    if(ajaxRequest.readyState == 4)
    {
      obj.RefreshList();
    }
	}
	ajaxRequest.open("GET", url + "?key=deletefile&streamUid="+streamUid, true);
	ajaxRequest.send(null);
}

function raiseRefresh()
{
  var _coll = document.getElementsByTagName("*");
  for(var i=0; i<_coll.length; i++)
  {
    var _obj = _coll[i];
    if(_obj.getAttribute("onMCFListRefresh"))
    try{
      eval(_obj.getAttribute("onMCFListRefresh"));
    }
    catch(e){}
  }
}
//***************************************************