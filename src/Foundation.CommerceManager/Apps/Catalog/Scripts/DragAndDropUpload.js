//The file array for dropped files.  Needs to be accessible across the scope of the script functions.
var selectedFiles = new Array();

//generate new XmlHttpRequest for upload.
var request = new XMLHttpRequest();

/*
Function:  DragAndDropUploadInit
Input:  the Div ID of the drop box on the calling page.
Purpose:  Initializes the drop box functionality.  Called on document.ready from the calling page.
*/
function DragAndDropUploadInit(divId, url) {
    var uploadBox = divId;

    uploadBox.addEventListener('dragenter', OnDragEnter, false);
    uploadBox.addEventListener('dragover', OnDragOver, false);
    uploadBox.addEventListener('drop', OnDrop, false);

    uploadBox.innerHTML = "Drag files here for uploading!";

    function stopEvents(e) {
        e.stopPropagation();
        e.preventDefault();
    }

    function OnDragEnter(e) {
        stopEvents(e);
    }

    function OnDragOver(e) {
        stopEvents(e);
    }

    function OnDrop(e) {
        stopEvents(e);

        $.each(e.dataTransfer.files, function (index, file) {
            selectedFiles[index] = file;
        });

        uploadBox.innerHTML = "";

        for (var i = 0; i < selectedFiles.length; i++) {
            var div = document.createElement("div");
            div.style.width = "100px";
            div.style.height = "100px";
            div.style.color = "black";
            div.innerHTML = selectedFiles[i].name;

            //Uncomment the following lines (as well as the uploadCancel function below) to apply a close button to each file div.
            //var closeButton = document.createElement("div");
            //closeButton.style.width = "20px";
            //closeButton.style.height = "20px";
            //closeButton.style.color = "black";
            //closeButton.innerHTML = "X";
            //closeButton.onclick = uploadCancel;
            //div.appendChild(closeButton);

            uploadBox.appendChild(div);
        }

        //perform automatic upload.
        var uploadedFile;

        for (var i = 0; i < selectedFiles.length; i++) {
            var data = new FormData();
            data.filename = "data";

            data.append(selectedFiles[i].name, selectedFiles[i]);
            uploadedFile = selectedFiles[i].name;

            request.open("POST", url, false);
            request.send(data);
            request.onload = uploadSuccess(uploadedFile, uploadBox);
            request.onerror = uploadFailed(uploadedFile, uploadBox);
        }
        if (request.status == 200) {
            location.reload();
        }
    }
};

/*
Function:  uploadFailed
Input:  the uploaded file.
        the file drop zone.
Purpose:  Displays failed XmlHttpRequest status
*/
function uploadFailed(uploadedFile, uploadBox) {
    var div = document.createElement("div");
    div.style.width = "100px";
    div.style.height = "100px";
    div.style.color = "black";
    div.innerHTML = uploadedFile + " failed to upload:  HTTP" + request.status + "\n" + request.responseText;
    uploadBox.appendChild(div);
}

/*
Function:  uploadSuccess
Input:  the uploaded file.
        the file drop zone.
Purpose:  Displays a 'success' message.
*/
function uploadSuccess(uploadedFile, uploadBox) {
    var div = document.createElement("div");
    div.style.width = "100px";
    div.style.height = "100px";
    div.style.color = "black";

    if (request.readyState == 4) {
        if (request.status == 200) {
            div.innerHTML = uploadedFile + " sucessfully uploaded.";
            uploadBox.appendChild(div);
        }
    }
}


//Uncomment the following to add a close button to each file in the drop zone.
//function uploadCancel() {
//    var parent = $(this).parent();

//    for (var c = 0; c < selectedFiles.length; c++) {
//        if (selectedFiles[c].name == parent.get(0).firstChild.textContent) {
//            selectedFiles.splice(c, 1);
//            break;
//        }
//    }
//    $(this).parent().fadeTo(300, 0, function () {
//        $(this).remove();
//    });
//}


//Uncomment to add an uploadProgress display.
//function uploadProgress(evt) {
//    if (evt.lengthComputable) {
//        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
//        //$('#uploadBox').append(percentComplete.toString() + '%');
//        var div = document.createElement("div");
//        div.style.width = "100px";
//        div.style.height = "100px";
//        div.style.color = "black";
//        div.innerHTML = percentComplete.toString() + '%';
//        uploadBox.appendChild(div);
//    }
//    else {
//        uploadBox.innerHTML = 'unable to compute';
//    }
//}

