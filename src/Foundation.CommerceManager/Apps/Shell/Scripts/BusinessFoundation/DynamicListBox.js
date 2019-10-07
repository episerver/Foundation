function DynamicListBox_Init() {
	for( var i = 0; i < DynamicListBoxes.length; i++ ) {
		var dlb = DynamicListBox_FindControl(DynamicListBoxes[i]);
		if ( dlb != null ) {
			DynamicListBox_Load( dlb );
		}
	}
}
function DynamicListBox_FindControl(controlName) {
	for ( var i = 0; i < document.forms.length; i++ ) {
		var theForm = document.forms[i];
		var theControl = theForm[controlName];
		if ( theControl != null ) {
			return theControl;
		}
	}
	return null;
}
function DynamicListBox_Load( list ) {
	list.Add = DynamicListBox_Add;
	list.InsertOption = DynamicListBox_InsertOption;
	list.Remove = DynamicListBox_Remove;
	list.MoveUp = DynamicListBox_MoveUp;
	list.MoveDown = DynamicListBox_MoveDown;
	list.Tracker = DynamicListBox_FindControl( list.name + "$itemTracker" );
	list.Tracker.text = "";
}
function DynamicListBox_Add( value, text, index ) {
	var newOption = new Option();
	newOption.text = text;
	newOption.value = value;
	var insertIndex = parseInt( index );
	if ( !isNaN( insertIndex ) ) {
		this.Tracker.value += "+" + value + "\x03" + text + "\x03" + index + "\x1F";
		this.InsertOption(newOption,insertIndex);
	} else {
		this.Tracker.value += "+" + value + "\x03" + text + "\x1F";
		this.InsertOption(newOption);
	}
}
function DynamicListBox_Remove( index ) {
	if ( typeof( this.options.remove ) == "undefined" ) {
		this.options.remove = DynamicListBox_DownlevelRemove;
	}
	this.Tracker.value += "-" + index + "\x1F";
	this.options.remove(index);
}
function DynamicListBox_DownlevelRemove( index ) {
	this[index] = null;
}
function DynamicListBox_InsertOption( option, index ) {
	if ( typeof( index ) == "undefined" || index >= this.options.length ) {
		this.options[this.options.length] = option;
	} else {
		for( var i = this.options.length; i > index; i-- ) {
			var optionCopy = new Option();
			optionCopy.text = this.options[i-1].text;
			optionCopy.value = this.options[i-1].value;
			this.options[i] = optionCopy;
		}
		this.options[index] = option;
	}
}
function DynamicListBox_MoveUp() {
	var index = this.options.selectedIndex;
	if ( index == -1 || index == 0 ) {
		return;
	}
	var theItem = this.options[index];
	var oldText = theItem.text;
	var oldValue = theItem.value;
	this.Remove( index );
	this.Add( oldValue, oldText, index - 1 );
	this.options.selectedIndex = index - 1;
}
function DynamicListBox_MoveDown() {
	var index = this.options.selectedIndex;
	if ( index == -1 || index == this.options.length - 1 ) {
		return;
	}
	var theItem = this.options[index];
	var oldText = theItem.text;
	var oldValue = theItem.value;
	this.Remove( index );
	this.Add( oldValue, oldText, index + 1 );
	this.options.selectedIndex = index + 1;
}