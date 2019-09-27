// JScript File
function Mediachase_CatalogClient() {
    // Properties

    // Method Mappings   
    this.CreateCatalog = function (source) {
        CSManagementClient.ChangeView('Catalog', 'Edit');
    };

    this.CreateNode = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'Node-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid);
    };

    this.CreateProductItem = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'Product-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&type=Product');
    };

    this.CreateVariationItem = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'Variation-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&type=Variation');
    };

    this.CreatePackageItem = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'CatalogPackage-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&type=Package');
    };

    this.CreateProductBundleItem = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'Bundle-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&type=Bundle');
    };

    this.CreateDynPackageItem = function (source) {
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'DynamicPackage-Edit', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&type=DynamicPackage');
    };

    this.OpenItem = function (type, id, extParams) {
        type = type.trim().toLowerCase();
        var catalogid = CSManagementClient.QueryString("catalogid");
        var catalognodeid = CSManagementClient.QueryString("catalognodeid");

        if (type == 'product')
            CSManagementClient.ChangeView("Catalog", "Product-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&catalogentryid=' + id + '&' + extParams);
        else if (type == 'variation')
            CSManagementClient.ChangeView("Catalog", "Variation-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&catalogentryid=' + id + '&' + extParams);
        else if (type == 'package')
            CSManagementClient.ChangeView("Catalog", "CatalogPackage-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&catalogentryid=' + id + '&' + extParams);
        else if (type == 'bundle')
            CSManagementClient.ChangeView("Catalog", "Bundle-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&catalogentryid=' + id + '&' + extParams);
        else if (type == 'dynamicpackage')
            CSManagementClient.ChangeView("Catalog", "DynamicPackage-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&catalogentryid=' + id + '&' + extParams);
        else if (type == 'node' || type == 'levelup') {
            if (id == catalognodeid)
                CSManagementClient.ChangeView("Catalog", "Node-Edit", 'catalogid=' + catalogid + '&catalognodeid=&nodeid=' + id);
            else
                CSManagementClient.ChangeView("Catalog", "Node-Edit", 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid + '&nodeid=' + id);
        }
    };

    this.ViewItem = function (type, id) {
        type = type.trim().toLowerCase();
        var catalogid = CSManagementClient.QueryString("catalogid");

        if (type == 'node')
            CSManagementClient.ChangeView("Catalog", "Node-List", 'catalogid=' + catalogid + '&catalognodeid=' + id);
        else
            this.OpenItem(type, id);
    };

    this.OpenItem2 = function (params) {
        var id = '';
        var type = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            id = cmdObj.CommandArguments.ID;
            type = cmdObj.CommandArguments.Type;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for function OpenItem');
            return;
        }
        this.OpenItem(type, id);
    };

    this.EditCatalog = function (params) {
        var id = '';

        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            id = cmdObj.CommandArguments.CatalogId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for function EditCatalog');
            return;
        }

        CSManagementClient.ChangeView("Catalog", "Edit", 'catalogid=' + id);
    };

    this.MoveCopyOpenDialog = function (source) {
        if (CSManagementClient.ListHasItemsSelected(source)) {
            MoveCopy_OpenDialog();
        }
    };

    this.CSVImportCatalog = function () {
        CSManagementClient.ChangeView('Catalog', 'Catalog-CSVImport', '');
    };

    this.ImportCatalog = function () {
        CSManagementClient.ChangeView('Catalog', 'Catalog-Import', '');
    };

    this.ExportCatalog = function (params) {
        if (params != null) {
            var items = Toolbar_GetSelectedGridItems(params);
            if (items) {
                var splitter = ';';

                // remove trailing splitter
                if ((items.length > 1) && (items.lastIndexOf(splitter) == items.length - 1))
                    items = items.substring(0, items.length - 1);

                // get array of selected items
                var selectedItems = items.split(splitter);
                if (selectedItems.length == 0) {
                    alert("You must select a catalog before you can perform export.");
                    return;
                }
                else if (selectedItems.length > 1) {
                    alert('You must select only one catalog!');
                    return;
                }
                else if (selectedItems.length == 1) {
                    var keys = selectedItems[0].split(CSManagementClient.EcfListView_PrimaryKeySeparator);
                    if (keys && (keys.length >= 1)) {
                        var id = keys[0];

                        CSManagementClient.ChangeView('Catalog', 'Catalog-Export', 'catalogid=' + id);
                    }
                    else
                        alert('Invalid primaryKey!');
                    return;
                }
            }
        }
    };

    this.ExportCatalog2 = function (params) {
        var catalogId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            catalogId = cmdObj.CommandArguments.CatalogId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for function ExportCatalog2');
            return;
        }
        CSManagementClient.ChangeView('Catalog', 'Catalog-Export', 'catalogid=' + catalogId);
    };

    this.CatalogSaveRedirect = function (catalogid, catalognodeid) {
        var catalogid = catalogid != null && catalogid.length > 0 ? catalogid : CSManagementClient.QueryString("catalogid");
        var catalognodeid = catalognodeid != null && catalognodeid.length > 0 ? catalognodeid : CSManagementClient.QueryString("catalognodeid");
        CSManagementClient.ChangeView('Catalog', 'Node-List', 'catalogid=' + catalogid + '&catalognodeid=' + catalognodeid);
    };

    //-------------------------- Currencies -------------------------------------
    this.NewCurrency = function () {
        CSManagementClient.ChangeView('Catalog', 'Currency-Edit', '');
    };

    this.EditCurrency = function (currencyId) {
        CSManagementClient.ChangeView('Catalog', 'Currency-Edit', 'currencyid=' + currencyId);
    };

    this.EditCurrency2 = function (params) {
        var currencyId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            currencyId = cmdObj.CommandArguments.CurrencyId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for method EditCurrency2');
            return;
        }
        this.EditCurrency(currencyId);
    };

    this.CurrencySaveRedirect = function () {
        CSManagementClient.ChangeView('Catalog', 'Currencies-List', '');
    };
};

var CSCatalogClient = new Mediachase_CatalogClient();
