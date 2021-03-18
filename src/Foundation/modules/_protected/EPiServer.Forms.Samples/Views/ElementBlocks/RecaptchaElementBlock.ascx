<%@ import namespace="EPiServer.Forms.Samples.Implementation.Elements" %>

<%@ control language="C#" inherits="ViewUserControl<RecaptchaElementBlock>" %>

<%
    var formElement = Model.FormElement;
    var errorMessage = Model.GetErrorMessage();
%>

<div id="<%: formElement.Guid %>" 
    class="Form__Element Form__CustomElement FormRecaptcha" 
    data-epiforms-element-name="<%: formElement.ElementName %>" 
    data-epiforms-sitekey="<%: Model.SiteKey %>" >

    <% if (EPiServer.Editor.PageEditing.PageIsInEditMode) 
    {  %>
        <span class="EditView__InvisibleElement"><%: Model.EditViewFriendlyTitle  %></span>
    <% }
    else
    {  %>
        <span role="alert" 
            aria-live="polite" 
            data-epiforms-linked-name="<%: formElement.ElementName %>" 
            class="Form__Element__ValidationError" 
            style="<%: string.IsNullOrEmpty(errorMessage) ? "display:none" : "" %>;"><%: errorMessage %>
        </span>
    <% }  %>
</div>