//********Main Upload Class**************************
function MCFU_Class(id, sessionUid, url, uploadInfoUrl, mode)
{
  this.id = id;
  this.sessionUid = sessionUid;
  this.url = url;
  this.uploadInfoUrl = uploadInfoUrl;
  this.mode = mode;
  this.UploadPercent = "0";
  this.UploadBytesReceived = "";
  this.UploadBytesTotal = "";
  this.UploadEstimatedTime = "";
  this.UploadTimeRemaining = "";
  this.UploadFileName = "";
  //
  this.UploadResult = "";
  this.UploadStartUpload = "";
  this.UploadEndUpload = "";
  this.UploadLastModified = "";
  this.UploadErrorMessage = "";
  this.UploadProgress = "";
  this.UploadTransferRate = "";
  //
  this.State = MCFU_States[6];
  this.uploadDiv = document.getElementById(id);
  this.ChangeState = raiseStateChanged;
  
  this.UploadToServer = true;
}

MCFU_Class.prototype.Show = function() {
	this.ChangeState(this, MCFU_States[1]);
};

MCFU_Class.prototype.Hide = function() {
	this.ChangeState(this, MCFU_States[0]);
};

MCFU_Class.prototype.Upload = function() {
};

MCFU_Class.prototype.Cancel = function() {
};
//***************************************************

//*******IFrame Mode*********************************
function MCFU_IFrame(id, sessionUid, url, uploadInfoUrl, uploadFrame)
{
  MCFU_Class.call(this, id, sessionUid, url, uploadInfoUrl, "iframe");
  
  this.uploadFrameId = uploadFrame;
  
  this.Hide();
}

MCFU_IFrame.prototype = new MCFU_Class();
MCFU_IFrame.prototype.constructor = MCFU_IFrame;

MCFU_IFrame.prototype.Show = function() {
//  if(this.uploadDiv)
//    this.uploadDiv.style.display = "block";
  MCFU_Class.prototype.Show.call(this);
};

MCFU_IFrame.prototype.Hide = function() {
//  if(this.uploadDiv)
//    this.uploadDiv.style.display = "none";
  MCFU_Class.prototype.Hide.call(this);
};
  
MCFU_IFrame.prototype.Cancel = function() {
  var uploadDocument = window.frames[this.uploadFrameId].document;
  var uploadForm = uploadDocument.getElementById("uploadForm");
  
  uploadDocument.location = uploadDocument.location;
  
  this.UploadPercent = "0";
  this.UploadBytesReceived = "";
  this.UploadBytesTotal = "";
  this.UploadEstimatedTime = "";
  this.UploadTimeRemaining = "";
  this.UploadFileName = "";
  //
  this.UploadResult = "";
  this.UploadStartUpload = "";
  this.UploadEndUpload = "";
  this.UploadLastModified = "";
  this.UploadErrorMessage = "";
  this.UploadProgress = "";
  this.UploadTransferRate = "";
  //
  
  this.Hide();
}

MCFU_IFrame.prototype.Upload = function() {
  var uploadDocument = window.frames[this.uploadFrameId].document;
  var uploadForm = uploadDocument.getElementById("uploadForm");
  
	var fl = false;
	var colls = uploadDocument.getElementsByName("McFileUp");
	for(var i=0; i<colls.length; i++)
	{
	  var col = colls[i];
	  if(col.value!="")
	  {
	    fl = true;
	    break;
	  }
	}
	
	if(!fl)
	  return;
	
	this.ChangeState(this, MCFU_States[2]);
  
	uploadForm.submit();
	
//	if(this.uploadDiv)
//    this.uploadDiv.style.display = "none";
    
  this.RefreshUploading();
};

MCFU_IFrame.prototype.FilesNotUploaded = function() {
	var uploadDocument = window.frames[this.uploadFrameId].document;
	var uploadForm = uploadDocument.getElementById("uploadForm");
  	var fl = false;
	var colls = uploadDocument.getElementsByName("McFileUp");
	for(var i=0; i<colls.length; i++)
	{
	  var col = colls[i];
	  if(col.value!="")
	  {
	    fl = true;
	    break;
	  }
	}
	return fl;
};

MCFU_IFrame.prototype.RefreshUploading = function() {
  try
  {
    if(this.UploadToServer)
    {  
      var uploadDocument = window.frames[this.uploadFrameId].document;
      var progressUid = uploadDocument.forms[0].__MEDIACHASE_FORM_UNIQUEID.value;
      refreshFU(this.id, this.uploadInfoUrl, progressUid);
    }
    else
    {
      var progressUid2 = document.forms[0].__MEDIACHASE_FORM_UNIQUEID2;
      if((typeof(progressUid2) != "undefined") && (progressUid2 != null) && (progressUid2.value.length > 0))
          refreshFU(this.id, this.uploadInfoUrl, progressUid2.value);
    }
  }
  catch(e){
  	this.UploadPercent = "0";
    this.UploadBytesReceived = "";
    this.UploadBytesTotal = "";
    this.UploadEstimatedTime = "";
    this.UploadTimeRemaining = "";
    this.UploadFileName = "";
    //
    this.UploadResult = "";
    this.UploadStartUpload = "";
    this.UploadEndUpload = "";
    this.UploadLastModified = "";
    this.UploadErrorMessage = "";
    this.UploadProgress = "";
    this.UploadTransferRate = "";
    //
    
    var uploadDocument = window.frames[this.uploadFrameId].document;
    uploadDocument.location = uploadDocument.location;
    
    this.Hide();
	}
};
//***************************************************

//*****MISC Functions********************************
function raiseStateChanged(obj, _state)
{
  obj.State = _state;
  
  var _coll = document.getElementsByTagName("*");
  for(var i=0; i<_coll.length; i++)
  {
    var _obj = _coll[i];
    if(_obj.getAttribute("onMCFUstatechanged"))
    try{
      eval(_obj.getAttribute("onMCFUstatechanged"));
    }
    catch(e){}
  }
}

function refreshFU(obj_id, url, progressUid)
{
  var obj = MCFU_Array[obj_id];
  var ajaxRequest = GenerateAjaxObject();
  ajaxRequest.onreadystatechange = function()
  {
    if (ajaxRequest.readyState != 4 ) return ;
    if(obj.State == MCFU_States[0])
      return;
    if(ajaxRequest.readyState == 4)
    {
      var json = ajaxRequest.responseText;
      try
      {
		eval("var mas=" + json + ";");
		if (mas.UploadResult=="-2")
		{
			obj.ChangeState(obj, MCFU_States[4]);
			obj.UploadPercent = "0";
			obj.UploadBytesReceived = "";
			obj.UploadBytesTotal = "";
			obj.UploadEstimatedTime = "";
			obj.UploadTimeRemaining = "";
			obj.UploadFileName = "";
			//
			obj.UploadResult = "";
			obj.UploadStartUpload = "";
			obj.UploadEndUpload = "";
			obj.UploadLastModified = "";
		  obj.UploadErrorMessage = "";
			obj.UploadProgress = "";
			obj.UploadTransferRate = "";
			//
			alert('Files have not been uploaded! Try again!');
			if(obj.uploadFrameId)
		    {
				var uploadDocument = window.frames[obj.uploadFrameId].document;
				uploadDocument.location = uploadDocument.location;
			}
			if(obj.popupwindow)
				obj.popupwindow.document.location = obj.popupwindow.document.location;
			obj.Show();
		}
		else if (mas.UploadResult=="-3")
		{
		    if(obj.UploadToServer)
		    {
		        obj.UploadToServer = false;
		        obj.ChangeState(obj, MCFU_States[7]);
		    }
				
		}
		else if (mas.UploadResult=="-1")
		{
		    obj.ChangeState(obj, MCFU_States[2]);
				obj.RefreshUploading();
		}
		else
		{
						obj.UploadPercent = mas.UploadProgress;
						obj.UploadBytesReceived = mas.UploadBytesReceived;
						obj.UploadBytesTotal = mas.UploadBytesTotal;
						obj.UploadResult = mas.UploadResult;
						obj.UploadStartUpload = mas.UploadStartUpload;
						obj.UploadEndUpload = mas.UploadEndUpload;
						obj.UploadLastModified = mas.UploadLastModified;
						obj.UploadFileName = mas.UploadCurrentFileName;
						obj.UploadErrorMessage = mas.UploadErrorMessage;
						obj.UploadProgress = mas.UploadProgress;
						obj.UploadTransferRate = mas.UploadTransferRate;
						obj.UploadEstimatedTime = mas.UploadEstimatedTime;
						obj.UploadTimeRemaining = mas.UploadTimeRemaining;
						obj.ChangeState(obj, MCFU_States[3]);
						obj.RefreshUploading();
					}	    
      }//try
      catch(e)
      {obj.RefreshUploading();}
      /*
      if(!ajaxRequest.responseXML.documentElement || 
        ajaxRequest.responseXML.documentElement.childNodes.length<0)
        return;
      var z = ajaxRequest.responseXML.documentElement.childNodes;
      if (z[0].childNodes[0].nodeValue=="-2")
	    {
	      obj.ChangeState(obj, MCFU_States[4]);
	      
	      obj.UploadPercent = "0";
        obj.UploadBytesReceived = "";
        obj.UploadBytesTotal = "";
        obj.UploadEstimatedTime = "";
        obj.UploadTimeRemaining = "";
        obj.UploadFileName = "";
	      
	      alert('Files have not been uploaded! Try again!');
	      if(obj.uploadFrameId)
	      {
	        var uploadDocument = window.frames[obj.uploadFrameId].document;
	        uploadDocument.location = uploadDocument.location;
	      }
	      if(obj.popupwindow)
	        obj.popupwindow.document.location = obj.popupwindow.document.location;
	      obj.Show();
	    }
	    else if (z[0].childNodes[0].nodeValue=="-3")
	    {
	      obj.ChangeState(obj, MCFU_States[5]);
	      
	      obj.UploadPercent = "0";
        obj.UploadBytesReceived = "";
        obj.UploadBytesTotal = "";
        obj.UploadEstimatedTime = "";
        obj.UploadTimeRemaining = "";
        obj.UploadFileName = "";
        
        if(obj.uploadFrameId)
        {
	        var uploadDocument = window.frames[obj.uploadFrameId].document;
	        uploadDocument.location = uploadDocument.location;
	      }
	      if(obj.popupwindow)
	      {
	        obj.popupwindow.document.location = obj.popupwindow.document.location;
	        obj.popupwindow.close();
	        obj.popupwindow = null;
	      }
	      obj.Hide();
	    }
	    else if (z[0].childNodes[0].nodeValue=="-1")
	    {
	      obj.ChangeState(obj, MCFU_States[2]);
	      obj.RefreshUploading();
	    }
	    else
	    {
	      if(z.length>0 && z[0].childNodes.length>0)
	        obj.UploadPercent = z[0].childNodes[0].nodeValue;
	      if(z.length>1 && z[1].childNodes.length>0)
	        obj.UploadBytesReceived = z[1].childNodes[0].nodeValue;
	      if(z.length>2 && z[2].childNodes.length>0)
          obj.UploadBytesTotal = z[2].childNodes[0].nodeValue;
        if(z.length>3 && z[3].childNodes.length>0)
          obj.UploadEstimatedTime = z[3].childNodes[0].nodeValue;
        if(z.length>4 && z[4].childNodes.length>0)
          obj.UploadTimeRemaining = z[4].childNodes[0].nodeValue;
        if(z.length>5 && z[5].childNodes.length>0)
          obj.UploadFileName = z[5].childNodes[0].nodeValue;
	      obj.ChangeState(obj, MCFU_States[3]);
		    obj.RefreshUploading();
	    }
	 */   
    }
	}
	ajaxRequest.open("GET", url + "?key=progress&progressUid="+progressUid, true);
	ajaxRequest.send(null);
}

function NextFile()
{
  var coll = document.getElementsByName("McFileUp");
  var LatestFile = coll[coll.length - 1];
  var objBr = document.createElement("BR");
  
  LatestFile.parentNode.appendChild(objBr);

  objBr = document.createElement("BR");

  LatestFile.parentNode.appendChild(objBr);

  var objFile = document.createElement("input");
  objFile.type="file";
  objFile.name="McFileUp";
  objFile.style.width="60%";
  objFile.setAttribute("size","60");
  objFile.onchange=NextFile;
  LatestFile.parentNode.appendChild(objFile);
}
//***************************************************