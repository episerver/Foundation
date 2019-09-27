// JScript File
function Mediachase_OrderClient()
{
    // Properties
    
    // Method Mappings
    this.NewPurchaseOrder = function (customerId, prevPage) {
        var params = 'CustomerId=' + customerId +
            '&GridViewId=NewOrderLineItem-List' +
            '&ReturnCommand=cmdOrderNewSave' +
            '&class=PurchaseOrder';

        if (prevPage)
            params += '&prevPage=' + prevPage;

        CSManagementClient.ChangeView('Order', 'PurchaseOrder-New', params);
    };

    //this.NewPurchaseOrder = function (customerId, siteId, prevPage) {
	//	var siteIdResult = '';
	//	if (siteId)
	//		siteIdResult = siteId;
        
	//	var params = 'CustomerId=' + customerId +
    //        '&GridViewId=NewOrderLineItem-List' +
    //        '&ReturnCommand=cmdOrderNewSave' +
    //        '&class=PurchaseOrder' +
    //        '&siteId=' + siteIdResult;

	//	if (prevPage)
	//	    params += '&prevPage=' + prevPage;

	//	CSManagementClient.ChangeView('Order', 'PurchaseOrder-New', params);
    //};
    
    this.NewPurchaseOrderWithSelect = function(customerId) {
        CSManagementClient.ChangeView('Order', 'SelectCustomerSite',
            'CustomerId=' + customerId +
            '&GridViewId=NewOrderLineItem-List' +
            '&ReturnCommand=cmdPurchaseOrderNew' +
            '&class=' + 'PurchaseOrder');
    };

    this.NewPaymentPlan = function (customerId) {
        CSManagementClient.ChangeView('Order', 'PaymentPlan-New',
                'CustomerId=' + customerId +
                '&GridViewId=NewOrderLineItem-List' +
                '&ReturnCommand=cmdOrderNewSave' +
                '&class=PaymentPlan');
    };

    //this.NewPaymentPlan = function(customerId, siteId) {
    //    var siteIdResult = '';
    //    if (siteId)
    //        siteIdResult = siteId;
    //    CSManagementClient.ChangeView('Order', 'PaymentPlan-New',
    //            'CustomerId=' + customerId +
    //            '&GridViewId=NewOrderLineItem-List' +
    //            '&ReturnCommand=cmdOrderNewSave' +
    //            '&class=PaymentPlan' +
    //            '&siteId=' + siteIdResult);
    //};
    
    this.NewShoppingCart = function(source) {
        var curFilter = CSManagementClient.QueryString("filter");
        var curClass = CSManagementClient.QueryString("class");
        CSManagementClient.ChangeView('Order', 'ShoppingCart-New', 'filter=' + curFilter + '&class=' + curClass);
    };

    this.DeleteOrder = function(source)
    {
        if(CSManagementClient.ListHasItemsSelected(source))
        {            
            if(CSManagementClient.confirmSubmit("Are you sure you want to delete selected items?"))       
            {       
                var arr = CSManagementClient.GetListCheckedItems(source, 0);
                
                var index = 0;
                for(index = 0; index < arr.length; index++)
                    source.deleteItem(arr[index]);
                    
                source.postback();
            }
        }
    };

    this.NewPaymentMethod = function(source) {
        var langId = CSManagementClient.QueryString("lang");
        CSManagementClient.ChangeView('Order', 'PaymentMethod-Edit', 'lang=' + langId);
    };

    this.EditPaymentMethod = function(pId, langId) {
        CSManagementClient.ChangeView('Order', 'PaymentMethod-Edit', 'pid=' + pId + '&lang=' + langId);
    };

    this.EditPaymentMethod2 = function(params) {
        var pId = '';
        var langId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            pId = cmdObj.CommandArguments.PaymentMethodId;
            langId = cmdObj.CommandArguments.LanguageId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditPaymentMethod');
            return;
        }
        this.EditPaymentMethod(pId, langId);
    };

    this.PaymentMethodSaveRedirect = function(langId) {
        //CSManagementClient.CloseTab();
        CSManagementClient.ChangeView('Order', 'PaymentMethods-List', 'lang=' + langId);
    };

    this.NewPackage = function() {
        CSManagementClient.ChangeView('Order', 'OrderPackage-Edit');
    };

    this.EditPackage = function(params) {
        var packageId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            packageId = cmdObj.CommandArguments.PackageId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditPackage');
            return;
        }
        CSManagementClient.ChangeView('Order', 'OrderPackage-Edit', 'PackageId=' + packageId);
    };

    this.NewShippingMethod = function(source) {
        var langId = CSManagementClient.QueryString("lang");
        CSManagementClient.ChangeView('Order', 'ShippingMethod-Edit', 'lang=' + langId);
    };

    this.EditShippingMethod = function(smId, langId) {
        CSManagementClient.ChangeView('Order', 'ShippingMethod-Edit', 'smid=' + smId + '&lang=' + langId);
    };

    this.EditShippingMethod2 = function(params) {
        var smId = '';
        var langId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            smId = cmdObj.CommandArguments.ShippingMethodId;
            langId = cmdObj.CommandArguments.LanguageId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditShippingMethod');
            return;
        }
        this.EditShippingMethod(smId, langId);
    };

    this.NewShippingOption = function(source) {
        CSManagementClient.ChangeView('Order', 'ShippingOption-Edit');
    };

    this.EditShippingOption = function(soId) {
        CSManagementClient.ChangeView('Order', 'ShippingOption-Edit', 'soid=' + soId);
    };

    this.EditShippingOption2 = function(params) {
        var soId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            soId = cmdObj.CommandArguments.ShippingOptionId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditShippingOption');
            return;
        }
        this.EditShippingOption(soId);
    };

    this.ShippingOptionSaveRedirect = function() {
        CSManagementClient.ChangeView('Order', 'ShippingOptions-List');
    };

    this.ShippingMethodSaveRedirect = function(langId) {
        CSManagementClient.ChangeView('Order', 'ShippingMethodLanguage-List', 'lang=' + langId);
    };

    this.ViewOrder2 = function(className, id, customerid) {
		if (CSManagementClient)
		{
			CSManagementClient.ChangeView('Order', className + '-ObjectView', 'id=' + id + '&customerid=' + customerid);
		}
		else
		{
			if (window.parent && window.parent.CSManagementClient)
				window.parent.CSManagementClient.ChangeView('Order', className + '-ObjectView', 'id=' + id + '&customerid=' + customerid);
			else
				alert('CSManagementClient cant be found');
		}
    };

    this.ViewOrder = function(id, customerid) {
        var orderClass = CSManagementClient.QueryString("class");
        this.ViewOrder2(orderClass, id, customerid);
    };
    
    // Jurisdictions
    this.NewJurisdiction = function() {
        var type = CSManagementClient.QueryString("type");
        CSManagementClient.ChangeView('Order', 'Jurisdiction-Edit', 'type=' + type);
    };

    this.EditJurisdiction = function(jId, type) {
        CSManagementClient.ChangeView('Order', 'Jurisdiction-Edit', 'jid=' + jId + '&type=' + type);
    };

    this.EditJurisdiction2 = function(params) {
        var jId = '';
        var type = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            jId = cmdObj.CommandArguments.JurisdictionId;
            type = cmdObj.CommandArguments.JurisdictionType;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditJurisdiction2');
            return;
        }
        this.EditJurisdiction(jId, type);
    };

    this.JurisdictionSaveRedirect = function() {
        var type = CSManagementClient.QueryString("type");
        CSManagementClient.ChangeView('Order', 'Jurisdictions-List', 'type=' + type);
    };
    
    // Jurisdiction Groups
    this.NewJurisdictionGroup = function() {
        var type = CSManagementClient.QueryString("type");
        CSManagementClient.ChangeView('Order', 'JurisdictionGroup-Edit', 'type=' + type);
    };

    this.EditJurisdictionGroup = function(jId, type) {
        CSManagementClient.ChangeView('Order', 'JurisdictionGroup-Edit', 'jid=' + jId + '&type=' + type);
    };

    this.EditJurisdictionGroup2 = function(params) {
        var jId = '';
        var type = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            jId = cmdObj.CommandArguments.JurisdictionGroupId;
            type = cmdObj.CommandArguments.JurisdictionType;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditJurisdictionGroup2');
            return;
        }
        this.EditJurisdictionGroup(jId, type);
    };

    this.JurisdictionGroupSaveRedirect = function() {
        var type = CSManagementClient.QueryString("type");
        CSManagementClient.ChangeView('Order', 'JurisdictionGroups-List', 'type=' + type);
    };
    
    //-------------------------- Taxes -------------------------------------
    this.NewTax = function(source) {
        CSManagementClient.ChangeView('Order', 'Tax-Edit', '');
    };

    this.EditTax = function(taxId) {
        CSManagementClient.ChangeView('Order', 'Tax-Edit', 'taxid=' + taxId);
    };

    this.EditTax2 = function(params) {
        var taxId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            taxId = cmdObj.CommandArguments.TaxId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditTax2');
            return;
        }
        this.EditTax(taxId);
    };

    this.TaxSaveRedirect = function() {
        //CSManagementClient.CloseTab();
        CSManagementClient.ChangeView('Order', 'Taxes-List', '');
    };

    this.ImportTaxes = function() {
        CSManagementClient.ChangeView('Order', 'Tax-Import', '');
    };

    this.ExportTaxes = function (params) {
        CSManagementClient.ChangeView('Order', 'Tax-Export', '');
    };
    
    //-------------------------- Countries -------------------------------------
    this.NewCountry = function(source) {
        CSManagementClient.ChangeView('Order', 'Country-Edit', '');
    };

    this.EditCountry = function(countryId) {
        CSManagementClient.ChangeView('Order', 'Country-Edit', 'countryid=' + countryId);
    };

    this.EditCountry2 = function(params) {
        var countryId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            countryId = cmdObj.CommandArguments.CountryId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditCountry2');
            return;
        }
        this.EditCountry(countryId);
    };

    this.CountrySaveRedirect = function() {
        CSManagementClient.ChangeView('Order', 'Countries-List', '');
    };
    
    //-------------------------- Return Reasons -------------------------------------
    this.NewReturnReason = function(source) {
        CSManagementClient.ChangeView('Order', 'ReturnReason-Edit', '');
    }

    this.EditReturnReason = function(returnReasonId) {
        CSManagementClient.ChangeView('Order', 'ReturnReason-Edit', 'returnreasonid=' + returnReasonId);
    }

    this.EditReturnReason2 = function(params) {
        var returnReasonId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            returnReasonId = cmdObj.CommandArguments.ReturnReasonId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditReturnReason2');
            return;
        }
        //alert('calling EditReturnReasonJS ' + returnReasonId);
        this.EditReturnReason(returnReasonId)
    };
    
    this.ReturnReasonSaveRedirect = function() {
        CSManagementClient.ChangeView('Order', 'ReturnReasons-List', '');
    };
    
    //-------------------------------------------------------------
    this.InitSelector = function(selectCtrlId, objIdToHide) {
        var objToHide = $(objIdToHide);
        var selectObj = $("#" + selectCtrlId);
        if (selectObj != null && $("#" + selectCtrlId + " option").size() >= 1) {
            $("#" + selectCtrlId).linkselect();
        }
        else {
            if (objToHide != null)
                objToHide.hide();
        }
    };
};

var CSOrderClient = new Mediachase_OrderClient();
