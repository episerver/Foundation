<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.EPiServerLogin" CodeBehind="EPiServerLogin.aspx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<script runat="server">
    protected override void OnPreInit(EventArgs e)
    {
        // add P3P header, in order to allow third-party cookies, especially for IE to allow third-party cookies, which will be set through iFrame
        // http://www.p3pwriter.com/LRN_111.asp
        // the compact policy string using here is taken from http://msdn.microsoft.com/en-us/library/ms537341%28v=vs.85%29.aspx
        System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"NOI ADM DEV PSAi COM NAV OUR OTR STP IND DEM\"");
        
        // check if we have EPiServer SSO request, otherwise, process eveything like normal
        if (!String.IsNullOrEmpty(Request["esso_t"]) || !String.IsNullOrEmpty(Request["esso_u"]))
        {
            string redirectType = Request["esso_type"] as string;
            if (redirectType == "GenerateToken") // got generate token request
            {
                Server.Transfer("~/Apps/EPiServerSSO/GenerateToken.aspx");
            }
            else // got SSO login request
            {
                Server.Transfer("~/Apps/EPiServerSSO/Authenticate.aspx");
            }
        }
        else
        {
            base.OnPreInit(e);
        }
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EPiServer Commerce Manager 5.2 Login Page</title>

    <style type="text/css">
		table.login
		{
		    border-style: none;
		    border-width: 0;
		    width: 100%;
		}
		table.login td
		{
		    padding-top: 1px;
		    padding-bottom: 1px;
		}
	</style>
	
	
    <script type="text/javascript">
        getEcfMainFrame = function() {
            var win = window;
            var retVal = null;

            // use try catch to ignore permission denied error when call cross-frame scripting
            try {
                while (win.parent != win) {
                    if (typeof (CSManagementClient) != "undefined")
                        retVal = win;
                    win = win.parent;
                }
            }
            catch (err) {
            }

            return retVal;
        };
        if (getEcfMainFrame() != null && getEcfMainFrame().location.href != self.location.href)
            getEcfMainFrame().location.href = self.location.href;
    </script>

    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FontStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FormStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/GeneralStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/LoginStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/BusinessFoundation/Theme.css") %>" type="text/css" rel="stylesheet" />
    
    <!-- EPi Style START -->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
	<!-- EPi Style END -->	
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div id="epi-ecf-banner">
	        <div style="float:left;width:350px;padding:5px;">
	        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/"><img alt="EPiServer Commerce" align="absmiddle" src="../EPi/Shell/Light/Resources/EPiServer_ECF-NEG.png" width="300" height="40" /></asp:HyperLink>
            </div>
<%--	        <div style="clear:both;"></div>--%>
        </div>
<%--    <div style="width: 100%; height: 50px; background-image: url(<%=ResolveClientUrl("~/Apps/Shell/Styles/Images/Shell/up_bg.gif") %>);
        background-repeat: repeat-y; background-color: #B4CAF4;">
        <div style="float: left; padding: 15px 0 0 10px;">
            <asp:Label ID="lblPageTitle" runat="server" CssClass="ibn-pagetitle" Text="EPiServer Commerce Manager"></asp:Label>
        </div>
        <div style="float: right; padding: 7px" id="rightPart">
        </div>
    </div>--%>
    <div class="LoginPanel">
        <div class="LoginTable">
            <table cellspacing="0" cellpadding="0" align="left">
                <tr>
                    <td>
                        <h1>CONDOR - Operational Access</h1>
                        <asp:Label ID="Label2" runat="server" CssClass="text" Text="Secure access to the commerce management tools"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="text" width="90%">
                        <br />
                        Fully integrated commerce tools for creating catalogs, managing products, taking
                        orders, processing financial transactions, fulfilling orders, and measuring success.
                        <br />
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="550">
                        <asp:Panel ID="LoginPanel" runat="server" DefaultButton="LoginCtrl$LoginButton">
                            <asp:Login ID="LoginCtrl" runat="server" TitleText="" BorderStyle="None" >
                                <LoginButtonStyle   ></LoginButtonStyle>
                                <HyperLinkStyle        Font-Size=".9em" Font-Names="Verdana" font-underline="true" ForeColor="#585880" BackColor="transparent"></HyperLinkStyle>
                                <CheckBoxStyle         CssClass="text" ForeColor="#000000" BorderColor="transparent" BackColor="transparent"></CheckBoxStyle>
                                <InstructionTextStyle  Font-Size=".9em" Font-Names="Verdana" ForeColor="#585880" font-italic="True" BackColor="transparent"></InstructionTextStyle>
                                <FailureTextStyle      Font-Size=".9em" Font-Names="Verdana" ForeColor="#585880"  BackColor="transparent" Font-Bold="True"></FailureTextStyle>
                                <TextBoxStyle          Width="300px"></TextBoxStyle>
                                <TitleTextStyle        Font-Size=".9em" Font-Names="Verdana" Font-Bold="True" ForeColor="#585880" BorderColor="#CCCCCC" BorderWidth="1pt" cssclass="form-header"></TitleTextStyle>
                                <LabelStyle        Width="150px" HorizontalAlign="Left" CssClass="text" ForeColor="#000000" BackColor="transparent"></LabelStyle>
                                <LayoutTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                        width="100%">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table class="login" cellpadding="0">
                                                        <tbody>
                                                            <tr>
                                                                <td width="150px" align="left">
                                                                    <asp:Label Width="150px" ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox Width="300px" ID="UserName" runat="server" autocomplete="off" />
                                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="LoginCtrl">*</asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="150px" align="left">
                                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox Width="300px" ID="Password" runat="server" TextMode="Password" autocomplete="off" />
                                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LoginCtrl">*</asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="ApplicationRow">
                                                                <td width="150px" align="left">
                                                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="Application">Application:</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox Width="300px" ID="Application" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Application"
                                                                        ErrorMessage="Application is required." ToolTip="Application is required." ValidationGroup="LoginCtrl">*</asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="loginText">
                                                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" style="color: red">
                                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td align="left">
                                                                    <asp:Button ID="LoginButton" Width="120" runat="server" CommandName="Login" Text="Log In"
                                                                        ValidationGroup="LoginCtrl" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                            </asp:Login>
                        </asp:Panel>
                        <asp:Panel ID="RegisterPanel" Visible="false" runat="server">
                            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server">
                                <WizardSteps>
                                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                                width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td align="left" colspan="2">
                                                                        No accounts have been found in the system for eCommerce Framework administration,
                                                                        use the form below to create a new administrative account.
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="150px" align="left">
                                                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="UserName" Width="300px" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="150px" align="left">
                                                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="Password" runat="server" Width="300px" TextMode="Password"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="150px" align="left">
                                                                        <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="ConfirmPassword" Width="300px" runat="server" TextMode="Password"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                                            ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                                                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="150px" align="left">
                                                                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="Email" runat="server" Width="300px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                                                            ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                                            ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                                                            ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2" style="color: Red;">
                                                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                        <asp:Button ID="StepNextButton" runat="server" Width="120" CommandName="MoveNext" Text="Create User"
                                                                            ValidationGroup="CreateUserWizard1" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <CustomNavigationTemplate>
                                        </CustomNavigationTemplate>
                                    </asp:CreateUserWizardStep>
                                </WizardSteps>
                            </asp:CreateUserWizard>
                        </asp:Panel>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="text">
                        <p>
                            <br />
                            Note: Please click <asp:HyperLink ID="HyperLinkSysReqs" Target="_blank" runat="server" NavigateUrl="http://world.episerver.com/Documentation/">here</asp:HyperLink> for browser requirements.</p>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="LoginFooter">
        <asp:HyperLink Target="_blank" runat="server" NavigateUrl="http://www.episerver.com"><%=Mediachase.Commerce.FrameworkContext.ProductName%></asp:HyperLink>
        <br />
        Version:
        <%=Mediachase.Commerce.FrameworkContext.ProductVersionDesc%><br /><br />
        &copy; <%=DateTime.Now.Year.ToString()%> EPiServer AB.  All Rights Reserved.
    </div>
    </form>
</body>
</html>
