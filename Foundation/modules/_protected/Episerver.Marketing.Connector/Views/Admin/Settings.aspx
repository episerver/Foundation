<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Settings.aspx.cs" Inherits="Episerver.Marketing.Connector.Framework.Views.Admin.Settings" %>

<asp:content runat="server" contentplaceholderid="FullRegion">
<script type="text/javascript">
    $(document).ready(function () {
        $('#chkHideSubmissionError').change(function () {
            $('#divFailureMessage').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
       });
</script>
    <div class="epi-contentContainer epi-padding">
         <div class="epi-contentContainer epi-padding">
              <h1 class="EP-prefix">
                  <%= Translate("/episerver/marketing/connectors/admin/globalsettings/displayname") %>
             </h1>
             <p class="EP-systemInfo">
               <%= Translate("/episerver/marketing/connectors/admin/globalsettings/description") %>
             </p>
             <div class="epi-formArea">
               <div class="epi-size25">
                 <div>
                  <asp:Label runat="server" ID="lblEnableFormsAutofill" AssociatedControlID="chkEnableFormsAutofill"></asp:Label>
                  <asp:CheckBox ID="chkEnableFormsAutofill" runat="server" />
                 </div>
                   <div>
                    <asp:Label runat="server" ID="lblHideSubmissionError" AssociatedControlID="chkHideSubmissionError"></asp:Label>
                    <asp:CheckBox ID="chkHideSubmissionError" runat="server" clientidmode="Static"/>
                   </div>
                   <div id="divFailureMessage" runat="server" clientidmode="Static">
                    <asp:Label runat="server" ID="lblFormSubmissionFailureMessage" AssociatedControlID="uxFormSubmissionFailureMessage"></asp:Label>
                    <asp:TextBox runat="server" ID="uxFormSubmissionFailureMessage"></asp:TextBox>
                   </div>
                </div>
               </div>
         </div>
        <div id="statusMessage" runat="server" class="EP-systemMessage EP-systemMessage-Warning" visible="false"></div>
       <div class="epi-buttonContainer">
                    <span class="epi-cmsButton">
                        <asp:Button runat="server" ID="uxSave" OnClick="uxSave_OnClick" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save" Text="<%$ Resources: EPiServer, button.save %>" ToolTip="<%$ Resources: EPiServer, button.save %>" /></span>
       </div>
    </div>
   
</asp:content>
