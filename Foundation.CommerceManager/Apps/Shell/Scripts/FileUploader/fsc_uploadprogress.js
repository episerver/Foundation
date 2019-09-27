//*****MCFUP_Control**********************************
function MCFUP_Control(id, MCFUId, percValueId, divWaitId, divProgressId, 
  progressBarId, divInfo, _uploadInfoUrl)
{
  this.id = id;
  this.MCFUId = MCFUId;
  this.mainDiv = document.getElementById(id);
  this.percValue = document.getElementById(percValueId);
  this.divWait = document.getElementById(divWaitId);
  this.divProgress = document.getElementById(divProgressId);
  this.progressBar = document.getElementById(progressBarId);
  this.url = _uploadInfoUrl;
  this.inPopUp = (_uploadInfoUrl!="");
  this.divInfo = document.getElementById(divInfo);
  this.divInfoFileName = document.getElementById(divInfo + "_UploadFileName");
  this.divInfoBytesReceived = document.getElementById(divInfo + "_UploadBytesReceived");
  this.divInfoBytesTotal = document.getElementById(divInfo + "_UploadBytesTotal");
  this.divInfoEstimatedTime = document.getElementById(divInfo + "_UploadEstimatedTime");
  this.divInfoTimeRemaining = document.getElementById(divInfo + "_UploadTimeRemaining");
  //
  this.divInfoResult = document.getElementById(divInfo + "_UploadResult");
  this.divInfoStartUpload = document.getElementById(divInfo + "_UploadStartUpload");
  this.divInfoEndUpload = document.getElementById(divInfo + "_UploadEndUpload");
  this.divInfoLastModified = document.getElementById(divInfo + "_UploadLastModified");
  this.divInfoErrorMessage = document.getElementById(divInfo + "_UploadErrorMessage");
  this.divInfoProgress = document.getElementById(divInfo + "_UploadProgress");
  this.divInfoTransferRate = document.getElementById(divInfo + "_UploadTransferRate");
  //
  if(this.inPopUp)
  {
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
    this.State = MCFU_States[2];
    this.RefreshUploading();
  }
}

MCFUP_Control.prototype.RefreshUploading = function()
{
  if(this.State == MCFU_States[2])//preupload
  {
    this.mainDiv.style.display = "block";
    
    this.progressBar.style.width = '0%';
    window.document.title = '0%';
  	
	  this.divWait.style.display = "block";
	  this.divInfo.style.display = "none";
  	
	  this.percValue.innerHTML = "0%";
	  try{
	    refreshFP(this.id, this.url);
	  }
	  catch(e){window.close();}
  }
  if(this.State == MCFU_States[3])//uploading
  {
    this.divWait.style.display = "none";
    this.divInfo.style.display = "block";
    
    this.divProgress.style.display = "block";
    
    this.progressBar.style.width = this.UploadPercent + '%';
    window.document.title = this.UploadPercent + '%';
    
    this.percValue.innerHTML = this.UploadPercent + '%&nbsp;';
    
    if(this.divInfoFileName)
      this.divInfoFileName.innerHTML = this.UploadFileName;
    if(this.divInfoBytesReceived)
      this.divInfoBytesReceived.innerHTML = this.UploadBytesReceived;
    if(this.divInfoBytesTotal)
      this.divInfoBytesTotal.innerHTML = this.UploadBytesTotal;
    if(this.divInfoEstimatedTime)
      this.divInfoEstimatedTime.innerHTML = this.UploadEstimatedTime;
    if(this.divInfoTimeRemaining)
      this.divInfoTimeRemaining.innerHTML = this.UploadTimeRemaining;
    //
    if(this.divInfoResult)
      this.divInfoResult.innerHTML = this.UploadResult;
    if(this.divInfoStartUpload)
      this.divInfoStartUpload.innerHTML = this.UploadStartUpload;
    if(this.divInfoEndUpload)
      this.divInfoEndUpload.innerHTML = this.UploadEndUpload;
    if(this.divInfoLastModified)
      this.divInfoLastModified.innerHTML = this.UploadLastModified;
    if(this.divInfoErrorMessage)
      this.divInfoErrorMessage.innerHTML = this.UploadErrorMessage;
	if(this.divInfoProgress)
      this.divInfoProgress.innerHTML = this.UploadProgress;
    if(this.divInfoTransferRate)
      this.divInfoTransferRate.innerHTML = this.UploadTransferRate;  
    //
    try{
      refreshFP(this.id, this.url);
    }
    catch(e){window.close();}
  }
  if(this.State == MCFU_States[4])//uploadcancelled
  {
    alert('Upload failed. Try again.');
	window.close();
  }
  if(this.State == MCFU_States[5])//uploadsuccessful
    window.close();
}
//***************************************************

//*****MISC Functions********************************
function MCFUP_StateChangedHandler(id)
{
  if(!MCFU_Array || !MCFU_Array[id] || !MCFU_Array[MCFU_Array[id].MCFUId])
  {
    setTimeout('MCFUP_StateChangedHandler("'+id+'")', 50);
    return;
  }
  var MCFUP = MCFU_Array[id];
  var MCFU = MCFU_Array[MCFUP.MCFUId];
  
  if(MCFU.State == MCFU_States[0])//hidden
  {
    MCFUP.mainDiv.style.display = "none";
  }
  if(MCFU.State == MCFU_States[1])//showed
  {
    MCFUP.mainDiv.style.display = "none";
  }
  if(MCFU.State == MCFU_States[2])//preupload
  {
    MCFUP.mainDiv.style.display = "block";
    
    MCFUP.progressBar.style.width = '0%';
  	
	  MCFUP.divWait.style.display = "block";
	  MCFUP.divInfo.style.display = "none";
  	
	  MCFUP.percValue.innerHTML = "0%";
  }
  if(MCFU.State == MCFU_States[3])//uploading
  {
    
    MCFUP.divWait.style.display = "none";
    MCFUP.divInfo.style.display = "block";
    
    MCFUP.divProgress.style.display = "block";
    
    MCFUP.progressBar.style.width = MCFU.UploadPercent + '%';
    
    MCFUP.percValue.innerHTML = MCFU.UploadPercent + '%&nbsp;';
    
    if(MCFUP.divInfoFileName)
      MCFUP.divInfoFileName.innerHTML = MCFU.UploadFileName;
    if(MCFUP.divInfoBytesReceived)
      MCFUP.divInfoBytesReceived.innerHTML = MCFU.UploadBytesReceived;
    if(MCFUP.divInfoBytesTotal)
      MCFUP.divInfoBytesTotal.innerHTML = MCFU.UploadBytesTotal;
    if(MCFUP.divInfoEstimatedTime)
      MCFUP.divInfoEstimatedTime.innerHTML = MCFU.UploadEstimatedTime;
    if(MCFUP.divInfoTimeRemaining)
      MCFUP.divInfoTimeRemaining.innerHTML = MCFU.UploadTimeRemaining;
    //
    if(MCFUP.divInfoResult)
      MCFUP.divInfoResult.innerHTML = MCFU.UploadResult;
    if(MCFUP.divInfoStartUpload)
      MCFUP.divInfoStartUpload.innerHTML = MCFU.UploadStartUpload;
    if(MCFUP.divInfoEndUpload)
      MCFUP.divInfoEndUpload.innerHTML = MCFU.UploadEndUpload;
    if(MCFUP.divInfoLastModified)
      MCFUP.divInfoLastModified.innerHTML = MCFU.UploadLastModified;
    if(MCFUP.divInfoErrorMesage)
      MCFUP.divInfoErrorMessage.innerHTML = MCFU.UploadErrorMessage;
    if(MCFUP.divInfoProgress)
      MCFUP.divInfoProgress.innerHTML = MCFU.UploadProgress;
    if(MCFUP.divInfoTransferRate)
      MCFUP.divInfoTransferRate.innerHTML = MCFU.UploadTransferRate;  
    //  
  }
  if(MCFU.State == MCFU_States[4])//uploadcancelled
  {
    MCFUP.divWait.style.display = "none";
    MCFUP.divInfo.style.display = "none";
    MCFUP.divProgress.style.display = "none";
    MCFUP.percValue.innerHTML = "";
    
    MCFUP.mainDiv.style.display = "none";
  }
  if(MCFU.State == MCFU_States[5])//uploadsuccessful
  {
    MCFUP.divWait.style.display = "none";
    MCFUP.divInfo.style.display = "none";
    MCFUP.divProgress.style.display = "none";
    MCFUP.percValue.innerHTML = "";
    
    MCFUP.mainDiv.style.display = "none";
  }
}

function refreshFP(obj_id, url)
{
  if(!MCFU_Array || !MCFU_Array[obj_id])
  {
    setTimeout('refreshFP("'+obj_id+'", "'+url+'")', 50);
    return;
  }
  var obj = MCFU_Array[obj_id];
  var ajaxRequest = GenerateAjaxObject();

  ajaxRequest.onreadystatechange = function()
  {
    if (ajaxRequest.readyState != 4 ) return ;
    if(ajaxRequest.readyState == 4)
    {
      var json = ajaxRequest.responseText;
      try
      {
		eval("var mas=" + json + ";");
		if (mas.UploadResult=="-2")
	      obj.State = MCFU_States[4];
	    else if (mas.UploadResult=="-3")
	      obj.State = MCFU_States[5];
		else if (mas.UploadResult=="-1")
		  obj.State = MCFU_States[2];
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
			obj.State = MCFU_States[3];
	    }
	    obj.RefreshUploading();
      }
      catch(e)
      {obj.RefreshUploading();}
      /*if(!ajaxRequest.responseXML.documentElement || 
        ajaxRequest.responseXML.documentElement.childNodes.length<0)
        return;
      var z = ajaxRequest.responseXML.documentElement.childNodes;
      if (z[0].childNodes[0].nodeValue=="-2")
	      obj.State = MCFU_States[4];
	    else if (z[0].childNodes[0].nodeValue=="-3")
	      obj.State = MCFU_States[5];
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
	      obj.State = MCFU_States[3];
	    }
	    obj.RefreshUploading();
	    */
    }
	}
	ajaxRequest.open("GET", url, true);
	ajaxRequest.send(null);
}
//***************************************************