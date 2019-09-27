// JScript File
function Mediachase_MarketsClient() {
    // Properties    

    // Method Mappings

    // Market functions
    this.NewMarket = function (source) {
        CSManagementClient.ChangeView('Markets', 'Market-Edit', '');
    };
    this.EditMarket = function (params) {
        var marketId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            marketId = cmdObj.CommandArguments.MarketId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for function EditMarket');
            return;
        }
        CSManagementClient.ChangeView('Markets', 'Market-Edit', 'MarketId=' + marketId);
    };
    this.CopyMarket = function (params) {
        var marketId = '';
        try {
            var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
            marketId = cmdObj.CommandArguments.MarketId;
        }
        catch (e) {
            alert('A problem occured with retrieving parameters for function CopyMarket');
            return;
        }
        CSManagementClient.ChangeView('Markets', 'Market-Edit', 'MarketId=' + marketId + '&cmd=copy');
    };
};

var CSMarketsClient = new Mediachase_MarketsClient();
