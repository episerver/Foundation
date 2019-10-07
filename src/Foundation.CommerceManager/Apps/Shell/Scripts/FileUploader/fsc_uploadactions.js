//*****MCFUA_Control**********************************
function MCFUA_Control(id, MCFUId, aShow, aHide, aUpload, aCancel)
{
  this.id = id;
  this.MCFUId = MCFUId;
  this.mainDiv = document.getElementById(id);
  this.aShow = document.getElementById(aShow);
  this.aHide = document.getElementById(aHide);
  this.aUpload = document.getElementById(aUpload);
  this.aCancel = document.getElementById(aCancel);
}
//***************************************************

//*****MISC Functions********************************
function MCFUA_StateChangedHandler(id)
{
  if(!MCFU_Array || !MCFU_Array[id] || !MCFU_Array[MCFU_Array[id].MCFUId])
  {
    setTimeout('MCFUA_StateChangedHandler("'+id+'")', 50);
    return;
  }
  var MCFUA = MCFU_Array[id];
  var MCFU = MCFU_Array[MCFUA.MCFUId];
  if(MCFU.mode=="embedded")
    return;
  if(MCFU.State == MCFU_States[0]) //hidden
  {
    MCFUA.mainDiv.style.display = "block";
    if(MCFUA.aShow)
      MCFUA.aShow.style.display = "inline";
    if(MCFUA.aHide)
      MCFUA.aHide.style.display = "none";
    if(MCFUA.aUpload)
      MCFUA.aUpload.style.display = "none";
    if(MCFUA.aCancel)
      MCFUA.aCancel.style.display = "none";
  }
  if(MCFU.State == MCFU_States[1]) //showed
  {
    MCFUA.mainDiv.style.display = "block";
    if(MCFU.mode=="iframe")
    {
      if(MCFUA.aShow)
        MCFUA.aShow.style.display = "none";
      if(MCFUA.aHide)
        MCFUA.aHide.style.display = "inline";
      if(MCFUA.aUpload)
        MCFUA.aUpload.style.display = "inline";
      if(MCFUA.aCancel)
        MCFUA.aCancel.style.display = "none";
    }
  }
  if(MCFU.State == MCFU_States[2] || MCFU.State == MCFU_States[3]) //preupload,upload
  {
    MCFUA.mainDiv.style.display = "block";
    if(MCFUA.aShow)
      MCFUA.aShow.style.display = "none";
    if(MCFUA.aHide)
      MCFUA.aHide.style.display = "none";
    if(MCFUA.aUpload)
      MCFUA.aUpload.style.display = "none";
    if(MCFUA.aCancel)
      MCFUA.aCancel.style.display = "inline";
  }
  if(MCFU.State == MCFU_States[4] || MCFU.State == MCFU_States[5]) //failed,success
  {
    MCFUA.mainDiv.style.display = "none";
  }
}
//***************************************************