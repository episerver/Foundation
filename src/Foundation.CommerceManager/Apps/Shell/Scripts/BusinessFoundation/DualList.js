function MetaBuilders_DualList_Init() {
	for( var i = 0; i < MetaBuilders_DualLists.length; i++ ) {
		var parentName = MetaBuilders_DualLists[i];
		
		var leftBox = DynamicListBox_FindControl(parentName + "LeftBox");
		var rightBox = DynamicListBox_FindControl(parentName + "RightBox");
		
		if ( leftBox == null || rightBox == null ) {
			return;
		}
		
		var moveRight = DynamicListBox_FindControl(parentName + "MoveRight");
		var moveAllRight = DynamicListBox_FindControl(parentName + "MoveAllRight");
		var moveLeft = DynamicListBox_FindControl(parentName + "MoveLeft");
		var moveAllLeft = DynamicListBox_FindControl(parentName + "MoveAllLeft");
		var moveUp = DynamicListBox_FindControl(parentName + "MoveUp");
		var moveDown = DynamicListBox_FindControl(parentName + "MoveDown");
		
		var dualListParent = new Object();
		
		dualListParent.LeftBox = leftBox;
		dualListParent.RightBox = rightBox;
		dualListParent.MoveRight = moveRight;
		dualListParent.MoveAllRight = moveAllRight;
		dualListParent.MoveLeft = moveLeft;
		dualListParent.MoveAllLeft = moveAllLeft;
		dualListParent.MoveUp = moveUp;
		dualListParent.MoveDown = moveDown;
		dualListParent.SetEnabled = MetaBuilders_DualList_SetEnabled;
		
		leftBox.Parent = dualListParent;
		leftBox.MoveSelection = MetaBuilders_DualList_MoveRight;
		leftBox.ondblclick = leftBox.MoveSelection;

		rightBox.Parent = dualListParent;
		rightBox.MoveSelection = MetaBuilders_DualList_MoveLeft;
		rightBox.ondblclick = rightBox.MoveSelection;
		if ( moveUp != null && moveDown != null ) {
			rightBox.SetUpDownEnabled = MetaBuilders_DualList_SetUpDownEnabled;
			rightBox.onchange = rightBox.SetUpDownEnabled;
		} else {
			rightBox.SetUpDownEnabled = function() {};
		}

		moveRight.Parent = dualListParent;
		moveRight.DoCommand = MetaBuilders_DualList_MoveRight;
		moveRight.onclick = moveRight.DoCommand;

		if ( moveAllRight != null ) {
			moveAllRight.Parent = dualListParent;
			moveAllRight.DoCommand = MetaBuilders_DualList_MoveAllRight;
			moveAllRight.onclick = moveAllRight.DoCommand;
		}

		moveLeft.Parent = dualListParent;
		moveLeft.DoCommand = MetaBuilders_DualList_MoveLeft;
		moveLeft.onclick = moveLeft.DoCommand;

		if ( moveAllLeft != null ) {
			moveAllLeft.Parent = dualListParent;
			moveAllLeft.DoCommand = MetaBuilders_DualList_MoveAllLeft;
			moveAllLeft.onclick = moveAllLeft.DoCommand;
		}

		if ( moveUp != null ) {
			moveUp.Parent = dualListParent;
			moveUp.DoCommand = MetaBuilders_DualList_MoveUp;
			moveUp.onclick = moveUp.DoCommand;
		}

		if ( moveDown != null ) {
			moveDown.Parent = dualListParent;
			moveDown.DoCommand = MetaBuilders_DualList_MoveDown;
			moveDown.onclick = moveDown.DoCommand;
		}

		if ( !moveRight.disabled ) {
			dualListParent.SetEnabled();
			if ( moveUp != null ) {
				rightBox.SetUpDownEnabled();
			}
		}
	}
}
function MetaBuilders_DualList_SetEnabled() {
	var leftItemsEmpty = ( this.LeftBox.options.length == 0 );
	var rightItemsEmpty = ( this.RightBox.options.length == 0 );
	this.MoveRight.disabled = leftItemsEmpty;
	if ( this.MoveAllRight != null ) {
		this.MoveAllRight.disabled = leftItemsEmpty;
	}
	this.MoveLeft.disabled = rightItemsEmpty;
	if ( this.MoveAllLeft != null ) {
		this.MoveAllLeft.disabled = rightItemsEmpty;
	}
	this.RightBox.SetUpDownEnabled();
}
function MetaBuilders_DualList_SetUpDownEnabled() {
	var selectedIndex = this.options.selectedIndex;
	this.Parent.MoveUp.disabled = ( selectedIndex <= 0 );
	this.Parent.MoveDown.disabled = ( selectedIndex == -1 || selectedIndex == this.options.length - 1 );
}
function MetaBuilders_DualList_MoveRight() {
	MetaBuilders_DualList_MoveSelectedItems(this.Parent.LeftBox,this.Parent.RightBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveAllRight() {
	MetaBuilders_DualList_MoveAllItems(this.Parent.LeftBox,this.Parent.RightBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveLeft() {
	MetaBuilders_DualList_MoveSelectedItems(this.Parent.RightBox,this.Parent.LeftBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveAllLeft() {
	MetaBuilders_DualList_MoveAllItems(this.Parent.RightBox,this.Parent.LeftBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveUp() {
	this.Parent.RightBox.MoveUp();
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveDown() {
	this.Parent.RightBox.MoveDown();
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveSelectedItems(source,target) {
	if ( source.options.length == 0 ) {
		return;
	}
	var originalIndex = source.options.selectedIndex;
	while ( source.options.selectedIndex >= 0 ) {
		MetaBuilders_DualList_MoveItem( source.options.selectedIndex, source, target );
	}
	if ( originalIndex < source.options.length ) {
		source.options.selectedIndex = originalIndex;
	} else {
		source.options.selectedIndex = source.options.length - 1;
	}
	target.options.selectedIndex = target.options.length - 1;
}
function MetaBuilders_DualList_MoveAllItems(source,target) {
	while ( source.options.length > 0 ) {
		MetaBuilders_DualList_MoveItem( 0, source, target );
	}
}
function MetaBuilders_DualList_MoveItem(itemIndex,source,target) {
	var itemValue = source.options[itemIndex].value;
	var itemText = source.options[itemIndex].text;
	source.Remove( itemIndex );
	target.Add(itemValue, itemText);
}