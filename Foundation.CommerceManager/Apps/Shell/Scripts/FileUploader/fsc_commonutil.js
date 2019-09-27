//****Mediachase.FileUploader Control****************
var MCFU_Array = new Object();
var MCFU_States = ['hidden', 'showed', 'preupload', 'uploading', 'uploadcancelled', 'uploadsuccessful', 'notinitialized'];
//***************************************************

//*****MISC Functions********************************
function GenerateAjaxObject()
{
  var ajaxRequest;
    
  try
  {
    ajaxRequest = new XMLHttpRequest();
  } 
  catch (e)
  {
	  try
	  {
		  ajaxRequest = new ActiveXObject("Msxml2.XMLHTTP");
	  } 
	  catch (e) 
	  {
		  try
		  {
			  ajaxRequest = new ActiveXObject("Microsoft.XMLHTTP");
		  } 
		  catch (e)
		  {
			  alert("Your browser broke!");
			  return null;
		  }
	  }
  }
  return ajaxRequest;
}
//***************************************************