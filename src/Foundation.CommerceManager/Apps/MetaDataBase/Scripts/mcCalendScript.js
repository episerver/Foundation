var mcCalend_pEvcanbub=false;

var g_mcCalend_ElsTable =  new Array();//elements Table

var g_mcCalend_ElsBgColor="#ddecfe";
var g_mcCalend_ElsHLColor="#ffffff";
var g_mcCalend_ElsSelColor="#b8d7f5";

function mcCalend_FSubmitIt()
{
	alert("js ok!");
	return true;
}

function mcCalend_ClearSelection(pClId)
{
	var txtDtInt_El = mcCalend_FGetElById(pClId,'txtDtInt');
	if(txtDtInt_El)
	{
		mcCalend_FOnChangeErrorFree(txtDtInt_El);
		txtDtInt_El.value = "";
	}
	var selDtHours_El = mcCalend_FGetElById(pClId,'selDtHours');
	if(selDtHours_El) selDtHours_El.selectedIndex = 0;

	var selDtMinutes_El = mcCalend_FGetElById(pClId,'selDtMinutes');
	if(selDtMinutes_El) selDtMinutes_El.selectedIndex = 0;

	var selDtSeconds_El = mcCalend_FGetElById(pClId,'selDtSeconds');
	if(selDtSeconds_El) selDtSeconds_El.selectedIndex = 0;

	var selDtHours_El = mcCalend_FGetElById(pClId,'txtSpinHours');
	if(selDtHours_El)
	{
		mcCalend_FOnChangeErrorFree(selDtHours_El);
		selDtHours_El.value = "";
	}

	var selDtMinutes_El = mcCalend_FGetElById(pClId,'txtSpinMinutes');
	if(selDtMinutes_El)
	{
		mcCalend_FOnChangeErrorFree(selDtMinutes_El);
		selDtMinutes_El.value = "";
	}

	var selDtSeconds_El = mcCalend_FGetElById(pClId,'txtSpinSeconds');
	if(selDtSeconds_El)
	{
		mcCalend_FOnChangeErrorFree(selDtSeconds_El);
		selDtSeconds_El.value = "";
	}
}

var g_mcCalend_SpinStop=false;
var g_mcCalend_SpinCnrtl="";
var g_mcCalend_SpinElem="";
var g_mcCalend_SpinIndex=-1;
var g_mcCalend_SpinElText=null;

function mcCalend_FspinInActive()
{
	g_mcCalend_SpinCnrtl="";
	g_mcCalend_SpinElem="";
	g_mcCalend_SpinIndex=-1;
	g_mcCalend_SpinElText=null;
	g_mcCalend_SpinStop = false;
}

function mcCalend_FfocusElement(elemName)
{
    var elem = mcCalend_FGetById(elemName);
    if(!elem)
		return;
    elem.focus();
    elem.select();
}

function mcCalend_FspinOnSelect(elemName)
{
    var elem = mcCalend_FGetById(elemName);
    g_mcCalend_SpinElText=elem.value;
}

function mcCalend_FspinOnClick()
{
	g_mcCalend_SpinStop = true;
}

function mcCalend_FspinOnKeyDown(pClId,El1Id,el,El2Id,pE)
{
	var vKC = mcCalend_FGetKeyCode(pE);
	var vKCStr = String.fromCharCode(vKC);
	if ( (vKC == 9) || (vKC == 37) || (vKC == 38) || (vKC == 39) || (vKC == 40) )
	{
		switch (vKC) 
		{
			case 37:
			//alert("left");
				if(El1Id && mcCalend_FGetById(El1Id))
					setTimeout("mcCalend_FfocusElement('" + El1Id + "')", 0);
				else
					setTimeout("mcCalend_FfocusElement('" + el.id + "')", 0);
				break;
			case 39:
			//alert("right");
				if(El2Id && mcCalend_FGetById(El2Id))
					setTimeout("mcCalend_FfocusElement('" + El2Id + "')", 0);
				else
					setTimeout("mcCalend_FfocusElement('" + el.id + "')", 0);
				break;
			case 38:
			//alert("up");
				mcCalend_FimgSpin(pClId,0);
				setTimeout("mcCalend_FfocusElement('" + el.id + "')", 0);
				break;
			case 40:
			//alert("down");
				mcCalend_FimgSpin(pClId,1);
				setTimeout("mcCalend_FfocusElement('" + el.id + "')", 0);
				break;
			case 9:
			//alert("tab");
				mcCalend_FspinInActive();
				break;
			default:
			//alert(vKC);
			break;
		}
	}
	else
	{
		var i=g_mcCalend_SpinIndex;
		var sphl = mcCalend_FspinGetHourLimit();
		var lmax = 1;
		if(i==0 && sphl>0)
			lmax=(sphl-1).toString().length-1;
//alert(document.getElementById('"+el.id+"').value);
		if(el.value.length==lmax)
		{
			if(El2Id)
				setTimeout("mcCalend_FfocusElement('" + El2Id + "')", 0);
			else
				setTimeout("mcCalend_FfocusElement('" + el.id + "')", 0);
		}
	}
}

function mcCalend_FspinOnFocus(pClId,pElId,index)
{
	g_mcCalend_SpinCnrtl=pClId;
	g_mcCalend_SpinElem=pElId;
	g_mcCalend_SpinIndex=index;
	setTimeout("mcCalend_FfocusElement('" + pElId + "')", 0);
	mcCalend_FAttachDocOnClick('mcCalend_Fdoconclick();');
}

function mcCalend_FspinGetHourLimit()
{
	var default_limit = 24;
	var calControl = mcCalend_FGetById(g_mcCalend_SpinCnrtl);
	if (calControl == null)
		return default_limit;
	var sphl_attr = null;
	sphl_attr = calControl.mcCalend_ASpinHourLimit;
	if(sphl_attr == null)
	{
		if( calControl.getAttribute("mcCalend_ASpinHourLimit") )
			sphl_attr = calControl.getAttribute("mcCalend_ASpinHourLimit");
	}
	if (sphl_attr == null)
		return default_limit;
	sphl_attr = parseInt(sphl_attr,10);
	return sphl_attr;
}

function mcCalend_FspinElValid(ElVal,Ind)
{
	var IsErr = true;
	if( !isNaN(ElVal) )
	{
		var cur_el_value = parseInt( ElVal,10 );

		if(Ind==0)
		{
			var sphl = mcCalend_FspinGetHourLimit();
			if( (cur_el_value>=sphl && sphl>0) || cur_el_value<0)
				IsErr = false;
		}
		if(Ind>0)
		{
			if(cur_el_value>=60 || cur_el_value<0)
				IsErr = false;
		}
	}
	else IsErr = false;
	return IsErr;
}

function mcCalend_FspinOnBlur(pClId,pElId)
{
	var cur_el = mcCalend_FGetById(pElId);
	var IsValid = mcCalend_FspinElValid(cur_el.value,g_mcCalend_SpinIndex);
	var cur_el_value = parseInt( cur_el.value,10 );
	
	var i=g_mcCalend_SpinIndex;

	if(!IsValid)
	{
		mcCalend_FOnChangeError(cur_el);
		mcCalend_FspinInActive();
		return false;
	}
	
	//
	var sphl = mcCalend_FspinGetHourLimit();

	var str_t=""+cur_el_value;
	if(i==0 && sphl>0)
	{
		var lcur=cur_el_value.toString().length;
		var lmax=(sphl-1).toString().length;
		if(lcur<lmax)
		{
			for(var j=lcur;j<lmax;j++) str_t="0"+str_t;
			cur_el.value=str_t;
		}
	}
	else
	{
		if(cur_el_value<10)
		{
			str_t="0"+cur_el_value;
			cur_el.value=str_t;
		}
	}
	/*
	var str_t=""+cur_el_value;
	if(cur_el_value<10)
	{
		str_t="0"+cur_el_value;
		cur_el.value=str_t;
	}
	*/
	
	if(window.opera)
	{
		temp_value=cur_el.value;
		cur_el.value="";
		cur_el.value=temp_value;
	}
	mcCalend_FOnChangeErrorFree(cur_el);
	mcCalend_FspinOnSelect(pElId);
	return true;
}

function mcCalend_FimgSpin(pClId,Direct)
{
	var cur_el = null;
	var i=0;
	if(g_mcCalend_SpinCnrtl=="")
		return;
	else
	{
		if(g_mcCalend_SpinCnrtl==pClId)
		{
			cur_el=mcCalend_FGetById(g_mcCalend_SpinElem);
			i=g_mcCalend_SpinIndex;
		}
		else return;
	}

	var IsValid = mcCalend_FspinElValid(cur_el.value,i);
	if(!IsValid)
	{
		mcCalend_FOnChangeError(cur_el);
		return;
	}
	else mcCalend_FOnChangeErrorFree(cur_el);

	if(g_mcCalend_SpinElText)
	{
		if(g_mcCalend_SpinElText!=cur_el.value)
			return;
	}
	else return;

	cur_el_value=parseInt(cur_el.value,10);
	var sphl = mcCalend_FspinGetHourLimit();
	if(Direct==0)
	{
		cur_el_value++;
		if(i==0 && (cur_el_value>=sphl && sphl>0))
			cur_el_value=0;
		if(i>0 && cur_el_value>=60)
			cur_el_value=0;
	}
	else
	{
		cur_el_value--;
		if(i==0 && cur_el_value<0)
		{
			if(sphl>0)
				cur_el_value=sphl-1;
			else cur_el_value=0;
		}
		if(i>0 && cur_el_value<0)
			cur_el_value=59;
	}

	var str_t=""+cur_el_value;
	if(i==0 && sphl>0)
	{
		var lcur=cur_el_value.toString().length;
		var lmax=(sphl-1).toString().length;
		if(lcur<lmax)
		{
			for(var j=lcur;j<lmax;j++) str_t="0"+str_t;
		}
	}
	else
	{
		if(cur_el_value<10) str_t="0"+cur_el_value;
	}
	cur_el.value=str_t;

	g_mcCalend_SpinStop = true;
	setTimeout("mcCalend_FfocusElement('" + cur_el.id + "')", 0);
}

function mcCalend_FbtnImgMain(pClId, pSrcId, pDestId)
{
	var pDestElement = mcCalend_FGetById(pDestId);//dvMain
    mcCalend_FHideCalendars(pDestElement);
    
    var pSrcElement = mcCalend_FGetById(pSrcId);//btnMain
     
    var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');

    if(document.body != pDestElement.parentNode)
    {
		var oCloneNode = pDestElement.cloneNode(true);
		document.body.appendChild(oCloneNode);
		pDestElement.parentNode.removeChild(pDestElement);
		pDestElement = oCloneNode;
	}
	
	var tbMain_El = mcCalend_FGetElById(pClId,'tbMain');
	if(tbMain_El==null)
	{
		//Safari tupit
		if(pDestElement.childNodes)
		{
			for(i=0;i<pDestElement.childNodes.length;i++)
			{
				if(pDestElement.childNodes[i].tagName=="TABLE")
				{
					tbMain_El = pDestElement.childNodes[i];
					break;
				}
			}
		}
		if(tbMain_El==null)
		{
			alert("Can't open calendar!\r\nPlease try again.");
			return;
		}
	}
	
	var oTotalOffset = mcCalend_FGetTotalOffset(pSrcElement);
	//pDestElement.style.left = oTotalOffset.Left;
	//pDestElement.style.top = oTotalOffset.Top + pSrcElement.offsetHeight;

	//alert("button left:" + oTotalOffset.Left);
	//alert("button offsetHeight:" + pSrcElement.offsetHeight);

	//alert("(x,y): " + oTotalOffset.Left + "," + oTotalOffset.Top + "," + pDestElement.style.top);

	pDestElement_attr = null;
	pDestElement_attr = pDestElement.mcCalend_AElAlign;
	if(pDestElement_attr == null)
	{
		if( pDestElement.getAttribute("mcCalend_AElAlign") ) pDestElement_attr = pDestElement.getAttribute("mcCalend_AElAlign");
	}
	if( pDestElement_attr != null ) pDestElement_attr = parseInt( pDestElement_attr,10 );
	
	//alert("0 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("0 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

	pDestElement.style.visibility = "hidden";
	pDestElement.style.display = "block";
	
	var calWidth = tbMain_El.offsetWidth;

	//alert("1 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("1 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

	//pDestElement.style.visibility = "";
	//alert("visible");
	mcCalend_Fpaint(pClId, txtShowYear_El.value, txtShowMonth_El.value);

	//alert("2 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("2 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);
	if(tbMain_El.offsetWidth)
	{
		if(tbMain_El.offsetWidth > calWidth) calWidth = tbMain_El.offsetWidth;
	}

	//pDestElement.style.left = oTotalOffset.Left;
	//pDestElement.style.top = oTotalOffset.Top + pSrcElement.offsetHeight;
	pDestElement.style.left = -500-calWidth + "px";
	pDestElement.style.top = "0px";

	//alert("3 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("3 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);
	if(tbMain_El.offsetWidth)
	{
		if(tbMain_El.offsetWidth > calWidth) calWidth = tbMain_El.offsetWidth;
	}

	pDestElement.style.visibility = "visible";

	//alert("4 dvMain Visible (" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("4 tbMain Visible (" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

	//
	var dvPosLeft = oTotalOffset.Left;
	if( pDestElement_attr != null )
	{
		if(tbMain_El.offsetWidth)
		{
			if(tbMain_El.offsetWidth > calWidth) calWidth = tbMain_El.offsetWidth;
		}
		if(pDestElement_attr == 0)
			dvPosLeft = oTotalOffset.Left + pSrcElement.offsetWidth - calWidth;
		if(pDestElement_attr == 1)
			dvPosLeft = oTotalOffset.Left + parseInt((pSrcElement.offsetWidth - calWidth)/2);
		if(dvPosLeft<0) dvPosLeft=0;
		if(dvPosLeft>oTotalOffset.Left) dvPosLeft=oTotalOffset.Left;
	}
	//alert("4_1 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	pDestElement.style.visibility = "hidden";
	//tbMain_El.style.width = tbMain_El.offsetWidth;
	tbMain_El.style.width = calWidth + "px";
	pDestElement.style.left = dvPosLeft + "px";
	//alert("4_2 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	pDestElement.style.top = oTotalOffset.Top + pSrcElement.offsetHeight + "px";
	//

	//alert("5 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("5 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

	mcCalend_FHideSelects(pClId);//hide selects after paint

	//alert("6 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("6 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

	pDestElement.style.visibility = "visible";

	mcCalend_pEvcanbub=true;

	//document.onclick = function () { mcCalend_Fdoconclick(); };
	//mcCalend_FAttachDocOnClick('mcCalend_Fdoconclick();');
	//ibn 4.1
	mcCalend_FAttachDocOnClick('mcCalend_Fdoconclick(); try {HideFrames();} catch(e) {}');

	pDestElement.onclick = function () { mcCalend_FtablCalonclick(); };
	//alert("7 dvMain(" + pDestElement + ") offsetWidth:" + pDestElement.offsetWidth);
	//alert("7 tbMain(" + tbMain_El + ") offsetWidth:" + tbMain_El.offsetWidth);

}

function mcCalend_FAttachDocOnClick(funcBody)
{
	document.onclick = new Function( funcBody );
}

function mcCalend_FStaticPaint(pClId)
{
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
	//mcCalend_Fpaint(pClId, txtShowYear_El.value, txtShowMonth_El.value);
	//window.onload = function () { mcCalend_Fpaint(pClId, txtShowYear_El.value, txtShowMonth_El.value); };
	window.onload = new Function( "mcCalend_Fpaint( '"+pClId+"',"+txtShowYear_El.value+","+txtShowMonth_El.value+" );" );
}

function mcCalend_FGetTotalOffset(eSrc)
{
	this.Top = 0;
	this.Left = 0;
	while (eSrc)
	{
		this.Top += eSrc.offsetTop;
		this.Left += eSrc.offsetLeft;
		eSrc = eSrc.offsetParent;
	}
	return this;
}

function mcCalend_FHideSelects(pClId)
{
	if(window.opera) return;
	//getElement('divCalendar').style.display = "";
	//top-left
	var dvMain_El = mcCalend_FGetElById(pClId,'dvMain');
	var dvoTotalOffset = mcCalend_FGetTotalOffset(dvMain_El);
	var x1cal = 0, x2cal = 0, y1cal, y2cal, cal_cs=0, xcal_delta=0, ycal_delta=0;
	var x2cal_max = 99999;
	var y2cal_max = 99999;
	if(this.screen)
	{
		if(this.screen.width) x2cal_max = this.screen.width;
		if(this.screen.height) y2cal_max = this.screen.height;
	}
	x1cal = dvoTotalOffset.Left;
	y1cal = dvoTotalOffset.Top;
	//alert("x1,y1: (" + x1cal + "," + y1cal + ")");// - (" + x2dv + "," + y2dv + ")");
	//bottom-right
	var tbMain_El = mcCalend_FGetElById(pClId,'tbMain');
	if(tbMain_El == null) return;
	var tbMain_El_SBBW, tbMain_El_SBRW;
	x2cal = x2cal_max;
	y2cal = y2cal_max;
	var el = tbMain_El.rows[tbMain_El.rows.length-1].cells[tbMain_El.rows[tbMain_El.rows.length-1].cells.length-1];
	if(el != null)
	{
		//el.style.backgroundColor = "#000000";
		var eloTotalOffset = mcCalend_FGetTotalOffset(el);
		x2cal = eloTotalOffset.Left;
		y2cal = eloTotalOffset.Top;

		if(tbMain_El.cellSpacing != null)
		{
			if( isFinite(tbMain_El.cellSpacing) ) cal_cs = parseInt( tbMain_El.cellSpacing,10 );
		}
		if(tbMain_El.border != null)
		{
			if( isFinite(tbMain_El.border) ) cal_cs = cal_cs + parseInt( tbMain_El.border,10 );
		}
		xcal_delta = xcal_delta + cal_cs;
		ycal_delta = ycal_delta + cal_cs;
		if(tbMain_El.style)
		{
			if(tbMain_El.style.borderRightWidth)
			{
				tbMain_El_SBRW = tbMain_El.style.borderRightWidth.replace("px","");
				if( isFinite(tbMain_El_SBRW) ) xcal_delta = xcal_delta + parseInt( tbMain_El_SBRW,10 );
			}
			if(tbMain_El.style.borderBottomWidth)
			{
				tbMain_El_SBBW = tbMain_El.style.borderBottomWidth.replace("px","");
				if( isFinite(tbMain_El_SBBW) ) ycal_delta = ycal_delta + parseInt( tbMain_El_SBBW,10 );
			}
		}
		//alert(xcal_delta + "-" + ycal_delta);

		if(el.offsetWidth) x2cal = x2cal + el.offsetWidth + xcal_delta;
		else x2cal = x2cal_max;
		if(el.offsetHeight) y2cal = y2cal + el.offsetHeight + ycal_delta;
		else y2cal = y2cal_max;
		//el.style.backgroundColor = "";
	}
	
	var selectColl = document.getElementsByTagName("select");
	var oTotalOffset, x1, x2, y1, y2;
	for(j=0;j<selectColl.length;j++)
	{
		obj_temp = selectColl[j];
		if(obj_temp.document == null) continue;
		if(obj_temp.mc_Calend_Hide)
		{
			if(obj_temp.mc_Calend_Hide > 0) continue;//already hide
		}
		oTotalOffset = mcCalend_FGetTotalOffset(obj_temp);
		x1 = oTotalOffset.Left;
		x2 = x1 + obj_temp.offsetWidth;
		y1 = oTotalOffset.Top;
		y2 = y1 + obj_temp.offsetHeight;
		if( !( (x1 >= x2cal) || (x2 <= x1cal) || (y1 >= y2cal) || (y2 <= y1cal) ) )
		{
			if(obj_temp.style.visibility) obj_temp.mc_Calend_Hide_Last = obj_temp.style.visibility;
			obj_temp.style.visibility = "hidden";
			obj_temp.mc_Calend_Hide = 1;
		}
	}
}

function mcCalend_FShowSelects()
{
	if(window.opera) return;
	//getElement('divCalendar').style.display = "none";
	var selectColl = document.getElementsByTagName("select");
	var obj_temp;
	for(j=0;j<selectColl.length;j++)
	{
		obj_temp = selectColl[j];
		if(obj_temp.style.visibility == "hidden")
		{
			if(obj_temp.mc_Calend_Hide)
			{
				if(obj_temp.mc_Calend_Hide_Last)
				{
					if(obj_temp.mc_Calend_Hide > 0) obj_temp.style.visibility = obj_temp.mc_Calend_Hide_Last;
				}
				else obj_temp.style.visibility = "";
				obj_temp.mc_Calend_Hide = 0;
			}
		}
	}
}

function mcCalend_FHideCalendars(pCurrentElement)
{
	var el, els_hidden=0;
	if(typeof(g_mcCalend_dvMainClIds) == "undefined")
		return;
	for (var i = 0; i < g_mcCalend_dvMainClIds.length; i++)
	{
		el = mcCalend_FGetById(g_mcCalend_dvMainClIds[i]);
		if(el)
		{
			if(pCurrentElement)
			{
				if(el == pCurrentElement) continue;
			}
			if (el.style.display != "none" )
			{
				if(el.style.position=="absolute") el.style.display="none";
				els_hidden++;
			}
		}
	}
	if(els_hidden>0)mcCalend_FShowSelects();
}

function mcCalend_FGetById(pId)
{
	if (document.getElementById)
		return document.getElementById(pId);
	else if (document.all)
		return document.all[pId]
	else
		return null;
} 

function mcCalend_FGetElById(pClId,pIdEltoSearch)
{
	var pClIdElement = mcCalend_FGetClIdById(pClId,pIdEltoSearch);
	if (pClIdElement)
		return mcCalend_FGetById(pClIdElement);
	else
		return null;
}

function mcCalend_FGetClIdById(pClId,pIdEltoSearch)
{
	if(g_mcCalend_ElsTable[pClId] == null)
	{
		g_mcCalend_ElsTable[pClId] = new Array();
		if( !mcCalend_FGenerateArrays(pClId) ) return null;
	}
	if(g_mcCalend_ElsTable[pClId][pIdEltoSearch] != null)
		return g_mcCalend_ElsTable[pClId][pIdEltoSearch];
	else return null;
}

function mcCalend_FGenerateArrays(pClId)
{
	//alert("mcCalend_FGenerateArrays:" + pClId);
	var calControl = mcCalend_FGetById(pClId);
	if(calControl == null) return false;
	var coll;
	if(calControl.all)
		coll = calControl.all;
	else
	{
		coll = new Array();
		mcCalend_FProcessChildren(calControl, coll);		
	}
	if(coll == null) return false;

	var obj_temp, obj_temp_attr;
	for (var vI=0; vI<coll.length; vI++)
	{
		if (coll[vI])
		{
			obj_temp = coll[vI];
			if(obj_temp.id)
			{
				if(obj_temp.id == "") continue;
			}
			obj_temp_attr = null;
			obj_temp_attr = obj_temp.mcCalend_AElId;
			if(obj_temp_attr == null)
			{
				if( obj_temp.getAttribute("mcCalend_AElId") ) obj_temp_attr = obj_temp.getAttribute("mcCalend_AElId");
			}
			if (obj_temp_attr)
			{
				g_mcCalend_ElsTable[pClId][obj_temp_attr] = obj_temp.id;
			}
		}
	}
	return true;
}

function mcCalend_FProcessChildren(obj, allChildren)
{
	var collChildren = obj.childNodes;
	for (var i=0; i<collChildren.length; i++)
	{
		if (typeof(collChildren[i]) == "object" && collChildren[i].getAttribute)
		{
			if(collChildren[i].id)
			{
				if(collChildren[i].id != "") allChildren[allChildren.length] = collChildren[i];
			}
			mcCalend_FProcessChildren(collChildren[i], allChildren);
		}
	}
}

function mcCalend_Fpaint(pClId, y, m)
{
	var el = mcCalend_FGetElById(pClId,'tbMain');
	
	var txtSelDayWeek_El = mcCalend_FGetElById(pClId,'txtSelDayWeek');
	var SelDateWeek = parseInt(txtSelDayWeek_El.value);

	//debug
	var txtFirstDW_El = mcCalend_FGetElById(pClId,'txtFirstDW');
	var firstWD = parseInt(txtFirstDW_El.value);
	var f = new Date(y, m, 1);//first day of the month
	var wd = f.getDay();// day of the week
	//alert("first day month: " + wd);
	var wd = (wd==0)?7:wd;//0-6 -> 1-7
	var st = 1 - wd + firstWD;//1;
	if( st > 1 ) st = st - 7;
	//alert("st: " + st);
	//alert(wd+" + "+st);
	var s, s_tr, n, curDay, curWeekDay, curMonth, curYear, tag1, tag2, delta='';
	var wr_attr = null;

	var txtDay0_El, txtMonth0_El, txtYear0_El;	
	if(SelDateWeek)
	{
		//week
		txtDay0_El = mcCalend_FGetElById(pClId,'txtDay0');
		txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth0');
		txtYear0_El = mcCalend_FGetElById(pClId,'txtYear0');
		//wrapping
		wr_attr = txtFirstDW_El.mcCalend_AWrap;
		if(wr_attr == null)
		{
			if( txtFirstDW_El.getAttribute("mcCalend_AWrap") ) wr_attr = txtFirstDW_El.getAttribute("mcCalend_AWrap");
		}	
	}
	else
	{
		//day
		txtDay0_El = mcCalend_FGetElById(pClId,'txtDay');
		txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth');
		txtYear0_El = mcCalend_FGetElById(pClId,'txtYear');	
	}
	var g_iYear0, g_iMonth0, g_iDay0;
	g_iYear0 = parseInt( txtYear0_El.value );
	g_iMonth0 = parseInt( txtMonth0_El.value );
	g_iDay0 = parseInt( txtDay0_El.value );

	var rowToSel = 0;

	var tdHeaderM_El = mcCalend_FGetElById(pClId,'tdHeaderM');
	var tdHeaderY_El = mcCalend_FGetElById(pClId,'tdHeaderY');
	if (tdHeaderY_El) 
	{
		tdHeaderM_El.innerHTML = mcCalend_FGetMonFNbyId(m,pClId);
		tdHeaderY_El.innerHTML = y;
	}
	else tdHeaderM_El.innerHTML = mcCalend_FGetMonFNbyId(m,pClId) + " " + y;

	//paint WDs
	var trWDs_El = mcCalend_FGetElById(pClId,'trWDs');
	var fwdi=firstWD>0?firstWD:7;
	for (var i=0; i<7; i++)
	{
		trWDs_El.cells[i].innerHTML = mcCalend_FGetDayWeekSNbyId(i,pClId);
		if(fwdi>5)trWDs_El.cells[i].style.color="#cc3300";
		fwdi++;if(fwdi>7)fwdi=1;
	}

	var tdwrap,istart,tdToSel;
	var start_row = trWDs_El.rowIndex + 1;
	for (var j=start_row; j<start_row+6; j++)
	{
		s = "";
		tdwrap=-1;
		tdToSel=1;
		for (i=0; i<7; i++)
		{
			s = "";
			n = new Date(y, m, st);
			curDay = n.getDate();
			curWeekDay = n.getDay();
			curWeekDay = (curWeekDay==0)?7:curWeekDay;
			curMonth = n.getMonth();
			curYear = n.getFullYear();
			tag1 = "";
			tag2 = "";

			if ( g_iYear0 == curYear && g_iMonth0 == curMonth && g_iDay0 == curDay )
			{
				rowToSel = j;
				if(!SelDateWeek)
				{
					el.rows[j].cells[i].style.backgroundColor =g_mcCalend_ElsSelColor;
					tag1 = "<b>";
					tag2 = "</b>";			
				}
			}
			else
			{
				if(!SelDateWeek)
					el.rows[j].cells[i].style.backgroundColor =g_mcCalend_ElsBgColor;
			}
						
			if ( rowToSel == j && SelDateWeek && tdToSel )
			{
				tag1 = "<b>";
				tag2 = "</b>";
			}	

			el.rows[j].cells[i].setAttribute("class","");
			if ( m == curMonth )
			{
				if (curWeekDay < 6) cls = "mcCalend_StdDay1";
				else cls = "mcCalend_StdDay2";

				el.rows[j].cells[i].className = cls;
				delta='0';
			}
			else
			{
				el.rows[j].cells[i].className = "mcCalend_StdDay0";
				if( j > start_row + 2 ) delta='1';
				else delta='-1';
			}
			st++;
			if(!SelDateWeek)
			{
				el.rows[j].cells[i].onmouseover = function(){ mcCalend_FtdOver( this ); };
				el.rows[j].cells[i].onmouseout = function(){ mcCalend_FtdOut( this ); };
				el.rows[j].cells[i].onclick = new Function( "mcCalend_FtdClick( '"+pClId+"',this,"+delta+" );" );
			}
			else
			{
				//wrapping
				if (wr_attr)
				{
					if (rowToSel == j && tdwrap>0 && tdToSel)
						el.rows[j].cells[i].style.backgroundColor =g_mcCalend_ElsSelColor;
					else
						el.rows[j].cells[i].style.backgroundColor ="";
					if(curMonth==11 && curDay==31)
					{
						tdwrap=i+1;
						if(rowToSel==j)
						{
							tdToSel = 0;
							for (var ij=0; ij<=i; ij++)
								el.rows[j].cells[ij].style.backgroundColor =g_mcCalend_ElsSelColor;
						}
					}
				}
			}
			el.rows[j].cells[i].innerHTML = tag1 + n.getDate() + tag2;
		}//i
		//select row
		if(SelDateWeek)
		{
			if (wr_attr)
			{
				if(tdwrap>0) el.rows[j].mcCalend_TDWrap = tdwrap;
				else el.rows[j].mcCalend_TDWrap = "";
			}
			if(rowToSel == j && tdwrap==-1) el.rows[j].bgColor =g_mcCalend_ElsSelColor;
			else el.rows[j].bgColor =g_mcCalend_ElsBgColor;
		}
	}//j

    if(document.getElementById("txtBox1"))
		document.getElementById("txtBox1").value = el.innerHTML;
}

function mcCalend_FGetMonFNbyId(num,pClId)
{
	var pName;
	for (var vI = 0; vI < g_mcCalend_MonFN.length; vI++)
	{
		if (g_mcCalend_MonFN[vI] == pClId)
		{
			if( vI < g_mcCalend_MonFN.length-1 ) pName = g_mcCalend_MonFN[vI+1][num];
			break;
		}
	}
	if (pName)
		return pName;
	else
		return null;
} 

function mcCalend_FGetDayWeekSNbyId(num,pClId)
{
	var pName;
	for (var vI = 0; vI < g_mcCalend_DayWeekSN.length; vI++)
	{
		if (g_mcCalend_DayWeekSN[vI] == pClId)
		{
			if( vI < g_mcCalend_DayWeekSN.length-1 ) pName = g_mcCalend_DayWeekSN[vI+1][num];
			break;
		}
	}
	if (pName)
		return pName;
	else
		return null;
} 
function mcCalend_FConstructSDP(pClId,d,M,y)
{
	var pName;//array
	for (var vI = 0; vI < g_mcCalend_SDP.length; vI++)
	{
		if (g_mcCalend_SDP[vI] == pClId)
		{
			if( vI < g_mcCalend_SDP.length-1 ) pName = g_mcCalend_SDP[vI+1];
			break;
		}
	}
	if (!pName)
		return null;
	var retVal = pName[0];

	var str_d = "" + d;
	if( pName[4] > 1 && d < 10 ) str_d = "0" + d;
	var str_M = "" + M;
	if( pName[5] > 1 && M < 10 ) str_M = "0" + M;
	var str_y = "" + y;
	if( pName[6] < 4)
	{
		if(str_y.length>2)
		{
			str_y = str_y.substr(2,str_y.length);
			y = parseInt( str_y,10 );
		}
		if( pName[6] > 1 && y < 10 ) str_y = "0" + y;
	}

	retVal = retVal.replace(pName[1],str_d);
	retVal = retVal.replace(pName[2],str_M);
	retVal = retVal.replace(pName[3],str_y);

	return retVal;
} 

function mcCalend_FtrOver(el,pE)
{
	var wr_attr = null;
	wr_attr = el.mcCalend_TDWrap;
	if(wr_attr == null)
	{
		if( el.getAttribute("mcCalend_TDWrap") ) wr_attr = el.getAttribute("mcCalend_TDWrap");
	}
	if (wr_attr)
	{
		var ist=0,ien=6,iwr=parseInt(wr_attr,10);

		pE = (pE) ? pE : ((window.event) ? event : null);
		if (pE)
		{
			var elem = (pE.target) ? pE.target : ((pE.srcElement) ? pE.srcElement : null);
			if (elem)
			{
				if(elem.parentNode != el) 
					elem = elem.parentNode;
				if(elem.parentNode == el)
				{
					var elemInd = elem.cellIndex;
					if(el.cells[elemInd] != elem)//Safari
					{
						for(var i=0;i<6;i++)
							if(el.cells[i] == elem) break;
						elemInd=i;
					}
					if(elemInd < iwr) ien=iwr-1;
					else ist=iwr;
				}
			}
		}
		for (var j=ist; j<=ien; j++) el.cells[j].style.backgroundColor =g_mcCalend_ElsHLColor;
	}
	else el.bgColor =g_mcCalend_ElsHLColor;
}

function mcCalend_FtrOut(el,pE)
{
	var pickprev=-1;
	if(el.cells[0].getElementsByTagName("B")[0]) pickprev=0;
	var wr_attr = null;
	wr_attr = el.mcCalend_TDWrap;
	if(wr_attr == null)
	{
		if( el.getAttribute("mcCalend_TDWrap") ) wr_attr = el.getAttribute("mcCalend_TDWrap");
	}
	if (wr_attr)
	{
		var ist=0,ien=6,iwr=parseInt(wr_attr,10);
		if(el.cells[el.cells.length-1].getElementsByTagName("B")[0]) pickprev=1;
		for(var j=ist; j<=ien; j++) el.cells[j].style.backgroundColor ="";
		if(pickprev>=0)
		{
			if(pickprev==0) ien=iwr-1;
			if(pickprev==1) ist=iwr;
			for(var j=ist; j<=ien; j++)
				el.cells[j].style.backgroundColor =g_mcCalend_ElsSelColor;
		}
		return;
	}
	if(pickprev>=0)
		el.bgColor =g_mcCalend_ElsSelColor;
	else el.bgColor =g_mcCalend_ElsBgColor;
}

function mcCalend_FtrClick(pClId,el,pE)
{
	var rowI=el.rowIndex;//alert(el);

	var ist=0,ien=el.cells.length-1,iwr=0,last_year=-1;

	var wr_attr = null;
	wr_attr = el.mcCalend_TDWrap;
	if(wr_attr == null)
	{
		if( el.getAttribute("mcCalend_TDWrap") ) wr_attr = el.getAttribute("mcCalend_TDWrap");
	}
	if (wr_attr)
	{
		iwr=parseInt(wr_attr,10);

		pE = (pE) ? pE : ((window.event) ? event : null);
		if (pE)
		{
			var elem = (pE.target) ? pE.target : ((pE.srcElement) ? pE.srcElement : null);
			if (elem)
			{
				if(elem.parentNode != el) 
					elem = elem.parentNode;
				if(elem.parentNode == el)
				{
					var elemInd = elem.cellIndex;
					if(el.cells[elemInd] != elem)//Safari
					{
						for(var i=0;i<6;i++)
							if(el.cells[i] == elem) break;
						elemInd=i;
					}
					if(elemInd < iwr)
					{
						last_year=1;
						ien=iwr-1;
					}
					else
					{
						ist=iwr;
						last_year=0;
					}
				}
			}
		}
	}

	var trWDs_El = mcCalend_FGetElById(pClId,'trWDs');
	var row_first = trWDs_El.rowIndex + 1;

	var rowLast = row_first + 6;

	var day_first, day_last;

	var g_iDay0,g_iDay1,picknew=0;
	if( el.cells[ist].getElementsByTagName("B")[0] )
		g_iDay0 = parseInt( el.cells[ist].getElementsByTagName("B")[0].innerHTML );
	else
	{
		g_iDay0 = parseInt( el.cells[ist].innerHTML );
		picknew=1;
	}
	if( el.cells[ien].getElementsByTagName("B")[0] )
		g_iDay1 = parseInt( el.cells[ien].getElementsByTagName("B")[0].innerHTML );
	else g_iDay1 = parseInt( el.cells[ien].innerHTML );

	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');	
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');

	var g_iMonth = txtShowMonth_El.value;
	var g_iYear = txtShowYear_El.value;
	
	var curMonth_temp = txtShowMonth_El.value;
	var curYear_temp = txtShowYear_El.value;
	
	if( g_iDay0 > g_iDay1)
	{
		if(rowI >= rowLast - 2)
		{
			//alert("change next");
			day_first = new Date(g_iYear, g_iMonth, g_iDay0);
			day_last = new Date(g_iYear, g_iMonth, g_iDay0 + 7 - 1);
		}
		else
		{
			//alert("change prev ");
			curMonth_temp--;
			if (curMonth_temp==-1) 
			{
				curMonth_temp = 11
				curYear_temp--;
			}
			day_first = new Date(curYear_temp, curMonth_temp, g_iDay0);
			day_last = new Date(curYear_temp, curMonth_temp, g_iDay0 + 7 - 1);
		}
	}
	else
	{
		//alert("without change");
		if( g_iDay0 < 15 && rowI >= rowLast - 2 )
		{
			//alert("change next ");
			curMonth_temp++;
			if (curMonth_temp==12) 
			{
				curMonth_temp = 0
				curYear_temp++;
			}
			day_first = new Date(curYear_temp, curMonth_temp, g_iDay0);
			day_last = new Date(curYear_temp, curMonth_temp, g_iDay1);
		}
		else
		{
			if(last_year>0  && rowI < rowLast - 2)
			{
				curMonth_temp--;
				if (curMonth_temp==-1) 
				{
					curMonth_temp = 11
					curYear_temp--;
				}
			}
			day_first = new Date(curYear_temp, curMonth_temp, g_iDay0);
			day_last = new Date(curYear_temp, curMonth_temp, g_iDay1);
		}
	}

	var g_iMonth0 = g_iMonth;
	var g_iMonth1 = g_iMonth;
	var g_iYear0 = g_iYear;
	var g_iYear1 = g_iYear;

	g_iDay0 = day_first.getDate();
	g_iMonth0 = day_first.getMonth();
	g_iYear0 = day_first.getFullYear();

	g_iDay1 = day_last.getDate();
	g_iMonth1 = day_last.getMonth();
	g_iYear1 = day_last.getFullYear();

	var g_oDate1 = "".concat(g_iDay0<10?"0":"",g_iDay0, "/", g_iMonth0<9?"0":"", g_iMonth0+1, "/", g_iYear0<500?g_iYear0+1900:g_iYear0);
	var g_oDate2 = "".concat(g_iDay1<10?"0":"",g_iDay1, "/", g_iMonth1<9?"0":"", g_iMonth1+1, "/", g_iYear1<500?g_iYear1+1900:g_iYear1);

	//set values
	var txtYear0_El = mcCalend_FGetElById(pClId,'txtYear0');
	txtYear0_El.value = g_iYear0;
	var txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth0');
	txtMonth0_El.value = g_iMonth0;
	var txtDay0_El = mcCalend_FGetElById(pClId,'txtDay0');
	txtDay0_El.value = g_iDay0;

	var txtYear1_El = mcCalend_FGetElById(pClId,'txtYear1');
	txtYear1_El.value = g_iYear1;
	var txtMonth1_El = mcCalend_FGetElById(pClId,'txtMonth1');
	txtMonth1_El.value = g_iMonth1;
	var txtDay1_El = mcCalend_FGetElById(pClId,'txtDay1');
	txtDay1_El.value = g_iDay1;

	var dvMain_El = mcCalend_FGetElById(pClId,'dvMain');
	if(dvMain_El.style.position=="absolute")
	{
		dvMain_El.style.display="none";
		mcCalend_FShowSelects();
	}
	else
	{
		if(picknew>0) mcCalend_Fpaint(pClId, txtShowYear_El.value, txtShowMonth_El.value);
	}

	var txtDtInt_El = mcCalend_FGetElById(pClId,'txtDtInt');
	if(txtDtInt_El)
	{
		var temp_value = mcCalend_FConstructSDP(pClId,g_iDay0,g_iMonth0+1,g_iYear0) + "-" + mcCalend_FConstructSDP(pClId,g_iDay1,g_iMonth1+1,g_iYear1);
		if( txtDtInt_El.value == temp_value ) return;
		{
			mcCalend_FOnChangeErrorFree(txtDtInt_El);
			txtDtInt_El.value = temp_value;
		}
		//txtDtInt_El.value = mcCalend_FConstructSDP(pClId,g_iDay0,g_iMonth0+1,g_iYear0) + "-" + mcCalend_FConstructSDP(pClId,g_iDay1,g_iMonth1+1,g_iYear1);
	}
	mcCalend_FdoPostBack(pClId,picknew);
}

function mcCalend_FtdOver(el)
{
	el.style.backgroundColor =g_mcCalend_ElsHLColor;
}

function mcCalend_FtdOut(el)
{
	if(el.getElementsByTagName("B")[0])
		el.style.backgroundColor =g_mcCalend_ElsSelColor;
	else el.style.backgroundColor =g_mcCalend_ElsBgColor;
}

function mcCalend_FtdClick(pClId,el,delta)
{
	el.style.backgroundColor =g_mcCalend_ElsHLColor;
	var g_iDay,picknew=0;
	if( el.getElementsByTagName("B")[0] )
		g_iDay = parseInt( el.getElementsByTagName("B")[0].innerHTML );
	else
	{
		g_iDay = parseInt( el.innerHTML );
		picknew=1;
	}

	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');	
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');

	var g_iMonth = parseInt(txtShowMonth_El.value);
	var g_iYear = parseInt(txtShowYear_El.value);

	g_iMonth = g_iMonth + parseInt(delta);
	if (g_iMonth==12) 
	{
		g_iMonth = 0;
		g_iYear++;
	}
	if (g_iMonth==-1) 
	{
		g_iMonth = 11
		g_iYear--;
	}

	var g_oDate = "".concat(g_iDay<10?"0":"",g_iDay, "/", g_iMonth<9?"0":"", g_iMonth+1, "/", g_iYear<500?g_iYear+1900:g_iYear);

	//set values
	var txtYear_El = mcCalend_FGetElById(pClId,'txtYear');
	txtYear_El.value = g_iYear;
	var txtMonth_El = mcCalend_FGetElById(pClId,'txtMonth');
	txtMonth_El.value = g_iMonth;
	var txtDay_El = mcCalend_FGetElById(pClId,'txtDay');
	txtDay_El.value = g_iDay;

	var dvMain_El = mcCalend_FGetElById(pClId,'dvMain');
	if(dvMain_El.style.position=="absolute")
	{
		dvMain_El.style.display="none";
		mcCalend_FShowSelects();
	}
	else
	{
		if(picknew>0) mcCalend_Fpaint(pClId, txtShowYear_El.value, txtShowMonth_El.value);
	}

	var txtDtInt_El = mcCalend_FGetElById(pClId,'txtDtInt');
	if(txtDtInt_El)
	{
		var temp_value = mcCalend_FConstructSDP(pClId,g_iDay,g_iMonth+1,g_iYear);
		if( txtDtInt_El.value == temp_value ) return;
		{
			mcCalend_FOnChangeErrorFree(txtDtInt_El);
			txtDtInt_El.value = temp_value;
		}
		//txtDtInt_El.value = mcCalend_FConstructSDP(pClId,g_iDay,g_iMonth+1,g_iYear);
	}
	mcCalend_FdoPostBack(pClId,picknew);
}

function mcCalend_FdoPostBack(pClId,picknew)
{
	var calControl = mcCalend_FGetById(pClId);
	if(picknew == 0)
	{
		var pbov_attr = null;
		pbov_attr = calControl.mcCalend_PostBackOldValue;
		if(pbov_attr == null)
		{
			if( calControl.getAttribute("mcCalend_PostBackOldValue") )
				pbov_attr = calControl.getAttribute("mcCalend_PostBackOldValue");
		}
		if (pbov_attr == null) return;
		pbov_attr = parseInt(pbov_attr,10);
		if (pbov_attr != 1) return;
	}
	var pb_attr = null;
	pb_attr = calControl.mcCalend_doPostBack;
	if(pb_attr == null)
	{
		if( calControl.getAttribute("mcCalend_doPostBack") ) pb_attr = calControl.getAttribute("mcCalend_doPostBack");
	}
	if (pb_attr) eval(pb_attr);
}

function mcCalend_FButArrowPos(el,pix)
{
	el.parentNode.style.top=pix + "px";
	el.parentNode.style.left=pix + "px";
}
function mcCalend_FButArrowOver(el,color)
{
	//el.style.backgroundColor=color;
	mcCalend_FButArrowPos(el,0);
}
function mcCalend_FButArrowOut(el)
{
	//el.style.backgroundColor='';
	mcCalend_FButArrowPos(el,0);
}
function mcCalend_FButArrowDown(el,color)
{
	//el.style.backgroundColor=color;
	mcCalend_FButArrowPos(el,1);
}
function mcCalend_FButArrowUp(el,color)
{
	//el.style.backgroundColor=color;
	mcCalend_FButArrowPos(el,0);
}

function mcCalend_FincMonth(pClId)
{
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
    var curYear = parseInt( txtShowYear_El.value );
    var curMonth = parseInt( txtShowMonth_El.value );

	curMonth++;
	if (curMonth==12) 
	{
		curMonth = 0;
		curYear++;
	}
	mcCalend_pEvcanbub=true;

	txtShowMonth_El.value = curMonth;
	txtShowYear_El.value = curYear;
	mcCalend_Fpaint(pClId, curYear, curMonth);
}

function mcCalend_FdecMonth(pClId)
{
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
    var curYear = parseInt( txtShowYear_El.value );
    var curMonth = parseInt( txtShowMonth_El.value );

	curMonth--;
	if (curMonth==-1) 
	{
		curMonth = 11
		curYear--;
	}
	mcCalend_pEvcanbub=true;

	txtShowMonth_El.value = curMonth;
	txtShowYear_El.value = curYear;
	mcCalend_Fpaint(pClId, curYear, curMonth);
}

function mcCalend_FincYear(pClId)
{
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
    var curYear = parseInt( txtShowYear_El.value );
    var curMonth = parseInt( txtShowMonth_El.value );

	curYear++;
	mcCalend_pEvcanbub=true;

	txtShowMonth_El.value = curMonth;
	txtShowYear_El.value = curYear;
	mcCalend_Fpaint(pClId, curYear, curMonth);
}

function mcCalend_FdecYear(pClId)
{
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
    var curYear = parseInt( txtShowYear_El.value );
    var curMonth = parseInt( txtShowMonth_El.value );

	curYear--;
	mcCalend_pEvcanbub=true;

	txtShowMonth_El.value = curMonth;
	txtShowYear_El.value = curYear;
	mcCalend_Fpaint(pClId, curYear, curMonth);
}

function mcCalend_FGetKeyCode(pE)
{
	var vKeyCode = null;
	if (pE.keyCode)
		vKeyCode = pE.keyCode;
	else if (pE.which)
		vKeyCode = pE.which;
	else if (pE.charCode)
		vKeyCode = pE.charCode;
	return vKeyCode;
}

function mcCalend_FdateOnKeyDown(pClId,el,pE)
{
	var vKC = mcCalend_FGetKeyCode(pE);
	var vKCStr = String.fromCharCode(vKC);
	var txtSelDayWeek_El = mcCalend_FGetElById(pClId,'txtSelDayWeek');
	var SelDateWeek = parseInt(txtSelDayWeek_El.value,10);
	//alert(vKC + "-" + vKCStr);
	if ( (vKC >= 33) && (vKC <= 47) )
	{
		switch (vKC) 
		{
			case 38:
			//alert("up");
				mcCalend_FdateDayChangeProcess(pClId,el,1);
				break;
			case 40:
			//alert("down");
				mcCalend_FdateDayChangeProcess(pClId,el,-1);
				break;
			case 33:
			//alert("pgup");
				mcCalend_FdateMonthChangeProcess(pClId,el,1);
				break;
			case 34:
			//alert("pgdown");
				mcCalend_FdateMonthChangeProcess(pClId,el,-1);
				break;
			default:
			//alert(vKC);
			return false;
			break;
		}
		mcCalend_FdoPostBack(pClId,1);
		//mcCalend_pEvcanbub = true;
		//document.onkeydown  = function () { mcCalend_Fdoconkeydown(); };
		//mcCalend_FStopEvent(pE);
	}
}

function mcCalend_FdateDayChangeProcess(pClId,el,delta)
{
	var txtDay0_El, txtMonth0_El, txtYear0_El;	
	var txtSelDayWeek_El = mcCalend_FGetElById(pClId,'txtSelDayWeek');
	var SelDateWeek = parseInt(txtSelDayWeek_El.value,10);
	if(SelDateWeek)
	{
		//week
		txtDay0_El = mcCalend_FGetElById(pClId,'txtDay0');
		txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth0');
		txtYear0_El = mcCalend_FGetElById(pClId,'txtYear0');	
	}
	else
	{
		//day
		txtDay0_El = mcCalend_FGetElById(pClId,'txtDay');
		txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth');
		txtYear0_El = mcCalend_FGetElById(pClId,'txtYear');	
	}
	var g_iYear0, g_iMonth0, g_iDay0;
	g_iYear0 = parseInt( txtYear0_El.value );
	g_iMonth0 = parseInt( txtMonth0_El.value );
	g_iDay0 = parseInt( txtDay0_El.value );
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');	
	var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
	var n,n1;
	if(SelDateWeek)
	{
		n = new Date(g_iYear0, g_iMonth0, g_iDay0+(7*delta));
		n1 = new Date(g_iYear0, g_iMonth0, g_iDay0+(7*delta)+6);
		el.value = mcCalend_FConstructSDP(pClId,n.getDate(),n.getMonth()+1,n.getFullYear()) + "-" + mcCalend_FConstructSDP(pClId,n1.getDate(),n1.getMonth()+1,n1.getFullYear());
		txtYear0_El.value = n.getFullYear();
		txtMonth0_El.value = n.getMonth();
		txtDay0_El.value = n.getDate();
		//todo - add end date
		var txtYear1_El = mcCalend_FGetElById(pClId,'txtYear1');
		var txtMonth1_El = mcCalend_FGetElById(pClId,'txtMonth1');
		var txtDay1_El = mcCalend_FGetElById(pClId,'txtDay1');
		txtYear1_El.value = n1.getFullYear();
		txtMonth1_El.value = n1.getMonth();
		txtDay1_El.value = n1.getDate();
	}
	else
	{
		n = new Date(g_iYear0, g_iMonth0, g_iDay0+delta);
		el.value = mcCalend_FConstructSDP(pClId,n.getDate(),n.getMonth()+1,n.getFullYear());
	}
	txtYear0_El.value = n.getFullYear();
	txtMonth0_El.value = n.getMonth();
	txtDay0_El.value = n.getDate();
	txtShowYear_El.value = n.getFullYear();
	txtShowMonth_El.value = n.getMonth();
}

function mcCalend_FdateMonthChangeProcess(pClId,el,delta)
{
	var txtDay0_El, txtMonth0_El, txtYear0_El;	
	var txtSelDayWeek_El = mcCalend_FGetElById(pClId,'txtSelDayWeek');
	var SelDateWeek = parseInt(txtSelDayWeek_El.value,10);
	if(SelDateWeek)
	{
		//week
		return;
	}
	else
	{
		//day
		txtDay0_El = mcCalend_FGetElById(pClId,'txtDay');
		txtMonth0_El = mcCalend_FGetElById(pClId,'txtMonth');
		txtYear0_El = mcCalend_FGetElById(pClId,'txtYear');	
	}
	var g_iYear0, g_iMonth0, g_iDay0;
	g_iYear0 = parseInt( txtYear0_El.value );
	g_iMonth0 = parseInt( txtMonth0_El.value );
	g_iDay0 = parseInt( txtDay0_El.value );
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');	
	var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
	var n = new Date(g_iYear0, g_iMonth0+delta, g_iDay0);
	el.value = mcCalend_FConstructSDP(pClId,n.getDate(),n.getMonth()+1,n.getFullYear());
	txtYear0_El.value = n.getFullYear();
	txtMonth0_El.value = n.getMonth();
	txtDay0_El.value = n.getDate();
	txtShowYear_El.value = n.getFullYear();
	txtShowMonth_El.value = n.getMonth();
}

function mcCalend_FdateOnChange(pClId,el)
{
	var strToDo = el.value;
	strToDo = strToDo.replace(" ","");
	if(strToDo == "")
	{
		mcCalend_FOnChangeErrorFree(el);
		return;
	}

	var pName;//array
	for (var vI = 0; vI < g_mcCalend_SDP.length; vI++)
	{
		if (g_mcCalend_SDP[vI] == pClId)
		{
			if( vI < g_mcCalend_SDP.length-1 ) pName = g_mcCalend_SDP[vI+1];
			break;
		}
	}
	if (!pName)
		return null;
	var retVal = pName[0];

	var strToDo_Array = strToDo.split(pName[7]);
	//alert(strToDo_Array.join("-"));
	if( isNaN(strToDo_Array[pName[8]]) || isNaN(strToDo_Array[pName[9]]) || isNaN(strToDo_Array[pName[10]]) )
	{
		//alert("format not valid");
		mcCalend_FOnChangeError(el);
		return;
	}
	var g_iDay = parseInt( strToDo_Array[pName[8]],10 );
	var g_iMonth = parseInt( strToDo_Array[pName[9]],10 );
	var g_iYear = parseInt( strToDo_Array[pName[10]],10 );
	var dvld = false;
	if(g_iYear<100 || g_iYear>9999)
	{
		//alert("year is not valid");
		dvld = true;
	}
	if(g_iMonth<1 || g_iMonth>12)
	{
		//alert("month is not valid");
		dvld = true;
	}
	if( !mcCalend_FDateGood(g_iYear,g_iMonth,g_iDay) )
	{
		//alert("date is not valid");
		dvld = true;
	}
	//alert(g_iDay + "-" + g_iMonth + "-" + g_iYear);
	if(dvld) 
	{
		mcCalend_FOnChangeError(el);
		return;
	}
	var txtShowYear_El = mcCalend_FGetElById(pClId,'txtShowYear');
    var txtShowMonth_El = mcCalend_FGetElById(pClId,'txtShowMonth');
	txtShowMonth_El.value = g_iMonth-1;
	txtShowYear_El.value = g_iYear;
	//set values
	var txtYear_El = mcCalend_FGetElById(pClId,'txtYear');
	txtYear_El.value = g_iYear;
	var txtMonth_El = mcCalend_FGetElById(pClId,'txtMonth');
	txtMonth_El.value = g_iMonth-1;
	var txtDay_El = mcCalend_FGetElById(pClId,'txtDay');
	txtDay_El.value = g_iDay;

	mcCalend_FOnChangeErrorFree(el);
	el.value = mcCalend_FConstructSDP(pClId,g_iDay,g_iMonth,g_iYear);
	mcCalend_FdoPostBack(pClId,1);
}

function mcCalend_FDateGood(y, m, d)
{
	with (new Date(y, --m, d))
		return ( getMonth()==m && getDate()==d && getFullYear()==y );
}

function mcCalend_FOnChangeError(el)
{
	var el_attr = null;
	el_attr = el.mc_Calend_OCError;
	if(el_attr == null)
	{
		if( el.getAttribute("mc_Calend_OCError") ) el_attr = el.getAttribute("mc_Calend_OCError");
	}
	if( el_attr != null ) 
	{
		if( el_attr != 1) 
		{
			el.mc_Calend_OCErrBgColor = el.style.backgroundColor;
			el.mc_Calend_OCErrColor = el.style.color;
		}
	}
	else
	{
		el.mc_Calend_OCErrBgColor = el.style.backgroundColor;
		el.mc_Calend_OCErrColor = el.style.color;
	}
	el.mc_Calend_OCError = 1;
	el.style.backgroundColor = "#ffcccc";
	el.style.color = "#ff0000";
}

function mcCalend_FOnChangeErrorFree(el)
{
	var el_attr = null;
	el_attr = el.mc_Calend_OCError;
	if(el_attr == null)
	{
		if( el.getAttribute("mc_Calend_OCError") ) el_attr = el.getAttribute("mc_Calend_OCError");
	}
	if( el_attr != null ) 
	{
		if( el_attr > 0) 
		{
			if(el.mc_Calend_OCErrBgColor)
				el.style.backgroundColor = el.mc_Calend_OCErrBgColor;
			else
				el.style.backgroundColor ="";
			if(el.mc_Calend_OCErrColor)
				el.style.color = el.mc_Calend_OCErrColor;
			else
				el.style.color ="";
			el.mc_Calend_OCError = 0;
		}
	}
}

function mcCalend_Fdoconclick()
{
	//alert("document.onclick: entering..."+mcCalend_pEvcanbub);
	if (mcCalend_pEvcanbub) mcCalend_pEvcanbub = false;
	else mcCalend_FHideCalendars();

	if(g_mcCalend_SpinCnrtl!="")
	{
		if(g_mcCalend_SpinStop)
			g_mcCalend_SpinStop=false;
		else
			mcCalend_FspinInActive();
	}
}
function mcCalend_FtablCalonclick()
{
	//alert("talble.onclick: entering...");
	if (!mcCalend_pEvcanbub) mcCalend_pEvcanbub = true;
}