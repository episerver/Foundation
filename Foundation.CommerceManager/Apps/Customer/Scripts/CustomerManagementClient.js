function Mediachase_CustomerManagementClient() 
{
	this.EditContact2 = function(primaryKeyId) 
	{
		CSManagementClient.ChangeBafView('Contact', 'Edit', 'ObjectId=' + encodeURI(primaryKeyId));
	};
	this.EditContact = function(params) 
	{
		var primaryKeyId = '';
		try 
		{
			var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
			primaryKeyId = cmdObj.CommandArguments.primaryKeyId;
		}
		catch (e) 
		{
			alert('A problem occured with retrieving parameters for method EditContact');
			return;
		}
		this.EditContact2(primaryKeyId);
	};

	this.EditOrganization2 = function(primaryKeyId) 
	{
		CSManagementClient.ChangeBafView('Organization', 'Edit', 'ObjectId=' + encodeURI(primaryKeyId));
	};
	this.EditOrganization = function(params) 
	{
		var primaryKeyId = '';
		try 
		{
			var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
			primaryKeyId = cmdObj.CommandArguments.primaryKeyId;
		}
		catch (e) 
		{
			alert('A problem occured with retrieving parameters for method EditOrganization');
			return;
		}
		this.EditOrganization2(primaryKeyId);
	};

	// Roles
	this.NewRole = function()
	{
		var type = CSManagementClient.QueryString("");
		CSManagementClient.ChangeBafView('Customer', 'Role-Edit', '');
	};

	this.EditRole = function(roleId)
	{
		CSManagementClient.ChangeBafView('Customer', 'Role-Edit', 'RoleId=' + encodeURI(roleId));
	};

	this.EditRole2 = function(params)
	{
		var roleId = '';
		try
		{
			var cmdObj = Sys.Serialization.JavaScriptSerializer.deserialize(params);
			roleId = cmdObj.CommandArguments.Name;
		}
		catch (e)
		{
			alert('A problem occured with retrieving parameters for method EditRole2');
			return;
		}
		this.EditRole(roleId);
	};
};

var CSCustomerManagementClient = new Mediachase_CustomerManagementClient();