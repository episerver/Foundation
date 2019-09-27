Type.registerNamespace('Mediachase');

Mediachase.AutoResizer = function(element) {
    Mediachase.AutoResizer.initializeBase(this, [element]);
    this.innerArray = null;
}

Mediachase.AutoResizer.prototype =
{
	initialize : function() {
		Mediachase.AutoResizer.callBaseMethod(this, 'initialize');		
		$addHandler(window, "resize", Function.createDelegate(this, this.onWindowResizeProxy));
		this.innerArray = new Array();
		window.setTimeout(Function.createDelegate(this, this.onWindowResizeHandler), 500);
	},
	
	dispose : function() {
		this.innerArray = null;
		Mediachase.AutoResizer.callBaseMethod(this, 'dispose');
	},
	
	onWindowResizeProxy: function()
	{
		window.setTimeout(Function.createDelegate(this, this.onWindowResizeHandler), 500);
	},
	
	//window onResize handler
	onWindowResizeHandler: function()
	{
		if (this.innerArray.length <= 1)
			this.fillElements();
		
		//todo:
		for (var i = 0; i < this.innerArray.length; i++)
		{
			var parentNode = this.innerArray[i].parentNode;
			if (parentNode)
			{
				if (this.innerArray[i].firstChild.getAttribute("mc_autoresizer_refresh"))
				{
					this.innerArray[i].firstChild.style.display = 'none';
					if (parentNode.clientHeight - parentNode.offsetTop - 16 > 0)
						this.innerArray[i].firstChild.style.height = parentNode.clientHeight - parentNode.offsetTop + 'px';
						
					this.innerArray[i].firstChild.style.height = parentNode.clientHeight - parentNode.offsetTop + 'px';
					this.innerArray[i].firstChild.style.display = 'block';
				}
				else if (this.innerArray[i].getAttribute('mc_autoresizer_generateDiv'))
				{
					var innerDiv = document.createElement('DIV');
					innerDiv.style.overflow = 'auto';					
					innerDiv.setAttribute("mc_autoresizer_refresh", "1");
					innerDiv.style.display = 'none';
					
					var allNodes = this.innerArray[i].childNodes;
					innerDiv.innerHTML = this.innerArray[i].innerHTML;
					this.innerArray[i].innerHTML = '';
					
//					for (var j = 0; j < allNodes.length; j++)
//					{
//						this.innerArray[i].removeChild(allNodes[j]);
//					}
					
					this.innerArray[i].appendChild(innerDiv);
				}
				else				
				{
					this.innerArray[i].style.height = parentNode.offsetHeight + 'px';
				}
				
				if (innerDiv)
				{
					if (parentNode.clientHeight - parentNode.offsetTop > 0)
						innerDiv.style.height = parentNode.clientHeight - parentNode.offsetTop + 'px';
					innerDiv.style.display = 'block';
				}
			}
		}
	},
	
	// fill innerArray with DOM elements, which needs resize
	fillElements: function()
	{
		var _innerElems = document.body.getElementsByTagName("*");
		
		for (var i = 0; i < _innerElems.length; i++)
		{
			if (_innerElems[i].getAttribute('mc_autoresizer'))
			{
				this.innerArray.push(_innerElems[i]);
			}
		}
	}
}

Mediachase.AutoResizer.registerClass('Mediachase.AutoResizer', Sys.UI.Control);
