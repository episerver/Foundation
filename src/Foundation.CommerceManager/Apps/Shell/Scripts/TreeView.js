function OnTreeClick(evt) {
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        CheckUncheckParents(src, src.checked, false);
    }
}

function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}

function CheckUncheckParents(srcChild, check, anyChildChecked) {
    var parentDiv = GetParentByTagName("div", srcChild);
    var parentNodeTable = parentDiv.previousSibling;

    if (parentNodeTable) {
        var checkUncheckSwitch; // 1 - checked, 2 - unchecked, 0 - some are checked
        if (check) //checkbox checked
        {
            var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
            if (isAllSiblingsChecked) {
                checkUncheckSwitch = 1;
            }
            else if (AreAnySiblingsChecked(srcChild)) {
                checkUncheckSwitch = 0;
            }
            else {
                return;    //do not need to check parent if any child is not checked
            }
        }
        else //checkbox unchecked
        {
            if (AreAnySiblingsChecked(srcChild))
                checkUncheckSwitch = 0;
            else       
                checkUncheckSwitch = 2;
        }

        var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
        if (inpElemsInParentTable.length > 0) {
            var parentNodeChkBox = inpElemsInParentTable[0];
            parentNodeChkBox.nextSibling.style.fontWeight = 'normal';
            if (checkUncheckSwitch == 1 || checkUncheckSwitch == 0) {
                if (!anyChildChecked)
                    anyChildChecked = true;
                    
                parentNodeChkBox.checked = true;
                parentNodeChkBox.nextSibling.style.fontWeight = 'bold';
            }
            else {
                parentNodeChkBox.checked = false;
                parentNodeChkBox.nextSibling.style.fontWeight = 'normal';
                if (checkUncheckSwitch == 0 || anyChildChecked) {
                    if (!anyChildChecked)
                        anyChildChecked = true;
                

                }
            }
            
            //do the same recursively
            CheckUncheckParents(parentNodeChkBox, parentNodeChkBox.checked, anyChildChecked);
        }
    }
}

function AreAllSiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (!prevChkBox.checked) {
                    return false;
                }
            }
        }
    }
    return true;
}

function AreAnySiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are checked, return true
                if (prevChkBox.checked) {
                    return true;
                }
            }
        }
    }
    return false;
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}