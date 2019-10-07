function Browseris () {
	var agt = navigator.userAgent.toLowerCase();
        this.osver = 1.0;
        if (agt)
        {
            var stOSVer = agt.substring(agt.indexOf("windows ") + 11);
	    this.osver = parseFloat(stOSVer);
        }
	this.major = parseInt(navigator.appVersion);
	this.nav = ((agt.indexOf('mozilla')!=-1)&&((agt.indexOf('spoofer')==-1) && (agt.indexOf('compatible')==-1)));
 	this.nav2 = (this.nav && (this.major == 2));
	this.nav3 = (this.nav && (this.major == 3));
	this.nav4 = (this.nav && (this.major == 4));
	this.nav6 = this.nav && (this.major == 5);
	this.nav6up = this.nav && (this.major >= 5);
	this.nav7up = false;
	if (this.nav6up)
	{
		var navIdx = agt.indexOf("netscape/");
		if (navIdx >=0 )
			this.nav7up = parseInt(agt.substring(navIdx+9)) >= 7;
	}
	this.ie = (agt.indexOf("msie")!=-1);
	this.aol = this.ie && agt.indexOf(" aol ")!=-1;
	if (this.ie)
		{
		var stIEVer = agt.substring(agt.indexOf("msie ") + 5);
		this.iever = parseInt(stIEVer);
		this.verIEFull = parseFloat(stIEVer);
		}
	else
		this.iever = 0;
	this.ie3 = ( this.ie && (this.major == 2));
	this.ie4 = ( this.ie && (this.major == 4));
	this.ie4up = this.ie && (this.major >=4);
	this.ie5up = this.ie && (this.iever >= 5);
	this.ie55up = this.ie && (this.verIEFull >= 5.5);
    this.win16 = ((agt.indexOf("win16")!=-1)
               || (agt.indexOf("16bit")!=-1) || (agt.indexOf("windows 3.1")!=-1)
               || (agt.indexOf("windows 16-bit")!=-1) );
    this.win31 = (agt.indexOf("windows 3.1")!=-1) || (agt.indexOf("win16")!=-1) ||
                 (agt.indexOf("windows 16-bit")!=-1);
    this.win98 = ((agt.indexOf("win98")!=-1)||(agt.indexOf("windows 98")!=-1));
    this.win95 = ((agt.indexOf("win95")!=-1)||(agt.indexOf("windows 95")!=-1));
    this.winnt = ((agt.indexOf("winnt")!=-1)||(agt.indexOf("windows nt")!=-1));
    this.win32 = this.win95 || this.winnt || this.win98 || 
                 ((this.major >= 4) && (navigator.platform == "Win32")) ||
                 (agt.indexOf("win32")!=-1) || (agt.indexOf("32bit")!=-1);
    this.os2   = (agt.indexOf("os/2")!=-1) 
                 || (navigator.appVersion.indexOf("OS/2")!=-1)  
                 || (agt.indexOf("ibm-webexplorer")!=-1);
    this.mac    = (agt.indexOf("mac")!=-1);
    this.mac68k = this.mac && ((agt.indexOf("68k")!=-1) || 
                               (agt.indexOf("68000")!=-1));
    this.macppc = this.mac && ((agt.indexOf("ppc")!=-1) || 
                               (agt.indexOf("powerpc")!=-1));
    this.w3c = this.nav6up;
}
var browseris = new Browseris();
