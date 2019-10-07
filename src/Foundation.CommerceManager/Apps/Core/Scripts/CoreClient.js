// JScript File
function Mediachase_CoreClient()
{   
    // Method Mappings   
    
    // User Methods
    this.CreateUser = function(source)
    {
        CSManagementClient.OpenWindow('Core', 'User-New', '', '', 100, 250);
    };

    this.CreateMetaClass = function(appId, viewId,
                                    classNamespace, fieldNamespace, mfViewId /* id of metafields view*/)
    {
    	//var ns = CSManagementClient.QueryString("namespace");
    	CSManagementClient.ChangeView(appId, viewId, 'namespace=' + classNamespace + '&fieldnamespace=' + fieldNamespace + '&mfview=' + mfViewId);
    };
    
    this.CreateMetaField = function(appId, viewId, 
                                            fieldNamespace)
    {
        /*var ns = CSManagementClient.QueryString("fieldnamespace");
        var mfview = CSManagementClient.QueryString("mfview");*/
        
        CSManagementClient.ChangeView(appId, viewId, 'fieldnamespace='+fieldNamespace);
    };
    
    /*------------- Import/Export MetaData : START ----------------------------------------------------------*/
    this.ExportMetaData = function(appId)
    {
        CSManagementClient.ChangeView(appId, appId+'MetaData-Export');
    };
    
    this.ImportMetaData = function(appId)
    {
        CSManagementClient.ChangeView(appId, appId+'MetaData-Import');
    };
    /*------------- Import/Export MetaData : END ----------------------------------------------------------*/
    
    
};

var CSCoreClient = new Mediachase_CoreClient();

