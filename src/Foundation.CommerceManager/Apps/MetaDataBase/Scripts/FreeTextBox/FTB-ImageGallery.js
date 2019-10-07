FTB_LastImageDiv = null;
currentImage = null;
currentFolder = null;
function FTB_FolderClick(theDiv, folderName) {
	FTB_HightlightDiv(theDiv);
	currentFolder = folderName;
	document.getElementById('command_DeleteImageButton').style.display = 'none';
	document.getElementById('command_DeleteFolderButton').style.display = 'block';
	
	document.getElementById('img_feedback_title').innerHTML = "Selected Folder";
	document.getElementById('img_feedback_message').value = folderName;
};
function FTB_PreviewImage(theDiv,filepath,filename,width,height,size) {
	FTB_HightlightDiv(theDiv);
	currentImage = filename;
	document.getElementById('command_DeleteImageButton').style.display = 'block';
	document.getElementById('command_DeleteFolderButton').style.display = 'none';	
	document.getElementById('img_feedback_title').innerHTML = "Selected Image";
	document.getElementById('img_feedback_message').value = filename + " (" + size + ")";	
		
	//document.getElementById('img_url').innerHTML = filepath + "/" + filename;
	//document.getElementById('img_size').innerHTML = size;
	
	document.getElementById('img_alt').value = filename;
	document.getElementById('img_width').value = width;
	document.getElementById('img_height').value = height;	
	document.getElementById('img_border').value = "0";		
	
	document.getElementById('img_dim_percentage').checked = false;
	document.getElementById('img_dim_custom').checked = false;
	document.getElementById('img_dim_original').checked = true;
	
	image = document.getElementById('img_preview');
	image.src = filepath + "/" + filename;
	image.width = width;
	image.height = height;	
};
function FTB_HightlightDiv(theDiv) {
	if (FTB_LastImageDiv) {
		FTB_LastImageDiv.style.border = "1px solid #CCCCCC";
		FTB_LastImageDiv.style.padding = "1px";
	}
	FTB_LastImageDiv = theDiv;
	theDiv.style.border = "2px solid #316AC5";
	theDiv.style.padding = "0";
};
function FTB_InsertImage() {
	
	image = document.getElementById('img_preview');
	src = document.getElementById('img_preview').src;
	if (src == '' || src == null) return;
	
	alt = document.getElementById('img_alt').value;
	title = document.getElementById('img_title').value;
	width = image.width; //document.getElementById('img_width').value;
	height = image.height; //document.getElementById('img_height').value;
	align = document.getElementById('img_align').options[document.getElementById('img_align').selectedIndex].value;
	
	hspace = document.getElementById('img_hspace').value;
	vspace = document.getElementById('img_vspace').value;
	border = document.getElementById('img_border').value;
	
	ftb = document.getElementById('TargetFreeTextBox').value;

	img = '<img src="' + src + '"' + ' temp_src="' + src + '"' + 
		( (alt != '') ? ' alt="' + alt + '"' : '' ) + 
		( (title != '') ? ' title="' + title + '"' : '' ) + 
		( (width != '') ? ' width="' + width + '"' : '' ) + 
		( (height != '') ? ' height="' + height + '"' : '' ) + 
		( (height != '') ? ' height="' + height + '"' : '' ) + 
		( (align != '') ? ' align="' + align + '"' : '' ) + 
		( (hspace != '') ? ' hspace="' + hspace + '"' : '' ) + 
		( (vspace != '') ? ' vspace="' + vspace + '"' : '' ) + 
		( (border != '') ? ' border="' + border + '"' : '' ) + 
		' />';
		
	window.opener.FTB_API[ftb].InsertHtml(img);
};
function FTB_DeleteImage(galleryID) {
	__doPostBack(galleryID,"DeleteImage:" + currentImage);
};
function FTB_DeleteFolder(galleryID) {
	__doPostBack(galleryID,"DeleteFolder:" + currentFolder);
};
function FTB_GoToFolder(galleryID, rootfolder,newfolder) {
	__doPostBack(galleryID,"GoToFolder:" + newfolder);
};
function FTB_CreateFolder(galleryID) {
	folder = document.getElementById('command_NewFolderName');
	folderButton = document.getElementById('command_NewFolderButton');

	if (folder.value == '') {
		alert("You must enter a folder name to create");
		return false;
	}
	folder.disabled = true;
	folderButton.disabled = true;	
	
	__doPostBack(galleryID,"CreateFolder:" + folder.value);
};
function FTB_UploadFile(galleryID) {
	
	file = document.getElementById('command_UploadFile');
	uploadButton = document.getElementById('command_UploadButton');

	if (file.value == '') {
		alert("You must select a file to upload");
		return false;
	}
	// file.disabled = true;
	uploadButton.disabled = true;
	// need to check if valid type!
	__doPostBack(galleryID,"UploadImage");
};
function FTB_ResizeGalleryArea() {
	gallery = document.getElementById('Gallery');
	galleryTop = document.getElementById('GalleryTop');
	galleryBottom = document.getElementById('GalleryBottom');
	sideBar = document.getElementById('GallerySideBar');
	
	if (FTB_Browser.isIE) {
		// check window height
		if (document.body.offsetHeight < (galleryTop.offsetHeight + galleryBottom.offsetHeight + sideBar.offsetHeight) ) {
			window.resizeTo(750, galleryTop.offsetHeight + galleryBottom.offsetHeight + sideBar.offsetHeight+100);
		}
				
		gallery.style.height = document.body.offsetHeight - (galleryTop.offsetHeight + galleryBottom.offsetHeight);
		gallery.style.width = document.body.offsetWidth - sideBar.offsetWidth;
	
	} else {
		if (window.innerHeight < (galleryTop.offsetHeight + galleryBottom.offsetHeight + sideBar.offsetHeight) ) {
			window.resizeTo(750, galleryTop.offsetHeight + galleryBottom.offsetHeight + sideBar.offsetHeight+100);
		}
		gallery.style.height =  window.innerHeight - (galleryTop.offsetHeight + galleryBottom.offsetHeight);
		gallery.style.width = window.innerWidth - sideBar.offsetWidth;				
	}	
};
function FTB_DimensionChange(sender) {
	switch (sender.id) {
		default:
		case "img_dim_original": 
			document.getElementById('img_width_custom').value = '';
			document.getElementById('img_height_custom').value = '';
			document.getElementById('img_percentage').value = '';
			
			document.getElementById('img_width_custom').disabled = true;
			document.getElementById('img_height_custom').disabled = true;
			document.getElementById('img_percentage').disabled = true;
			
			FTB_ResetImage();
			break;
		case "img_dim_custom":
			document.getElementById('img_width_custom').value = document.getElementById('img_width').value;
			document.getElementById('img_height_custom').value = document.getElementById('img_height').value;
			document.getElementById('img_percentage').value = '';
			
			document.getElementById('img_width_custom').disabled = false;
			document.getElementById('img_height_custom').disabled = false;
			document.getElementById('img_percentage').disabled = true;			
			break;	
		case "img_dim_percentage":
			document.getElementById('img_width_custom').value = '';
			document.getElementById('img_height_custom').value = '';
			document.getElementById('img_percentage').value = '100';
			
			document.getElementById('img_width_custom').disabled = true;
			document.getElementById('img_height_custom').disabled = true;
			document.getElementById('img_percentage').disabled = false;	
			FTB_SetImageByPercentage();		
			break;	
	}
};
function FTB_SetImageByPercentage() {
	previewImage = document.getElementById('img_preview');
	width = document.getElementById('img_width').value;
	height = document.getElementById('img_height').value;
	percentage = document.getElementById('img_percentage').value;

	previewImage.width = width * percentage / 100;
	previewImage.height = height * percentage / 100;	
};
function FTB_ResetImage() {
	previewImage = document.getElementById('img_preview');
	width = document.getElementById('img_width').value;
	height = document.getElementById('img_height').value;

	previewImage.width = width;
	previewImage.height = height;
};
function FTB_UpdatePreview(sender) {
	src = document.getElementById('img_feedback_message').value;
	if (src == null || src == '') return;
	
	previewImage = document.getElementById('img_preview');
	width = document.getElementById('img_width').value;
	height = document.getElementById('img_height').value;
	customWidth = document.getElementById('img_width_custom').value;
	customHeight = document.getElementById('img_height_custom').value;
	lockRatio = document.getElementById('img_lockRatio').checked;
	
	if (sender.id == 'img_percentage') {
		FTB_SetImageByPercentage();
	} else {
	
		if (lockRatio) {
			if (sender.id == 'img_width_custom') {			
				previewImage.width = customWidth;
				previewImage.height = height * ( customWidth / width);
				document.getElementById('img_height_custom').value = height * ( customWidth / width);
			} else if (sender.id == 'img_height_custom') {
				previewImage.width = width * ( customHeight / height);		
				previewImage.height = customHeight;
				
				document.getElementById('img_width_custom').value = width * ( customHeight / height);
			}
		} else {
			previewImage.width = customWidth;
			previewImage.height = customHeight;
		}
		
	}
};
function FTB_RestorePreview() {
	previewImage = document.getElementById('img_preview');
	previewImage.width = document.getElementById('img_previewWidth').value;
	previewImage.height = document.getElementById('img_previewHeight').value;
	document.getElementById('img_width').value = document.getElementById('img_previewWidth').value;
	document.getElementById('img_height').value = document.getElementById('img_previewHeight').value;	
};