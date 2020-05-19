<%@ import namespace="System.Web.Mvc" %>
<%@ import namespace="EPiServer.Core" %>
<%@ import namespace="EPiServer.Web.Mvc.Html" %>
<%@ import namespace="EPiServer.Forms.Core" %>
<%@ import namespace="EPiServer.Forms.Core.Models" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Samples.Implementation.Elements" %>
<%@ import namespace="EPiServer.Forms.Samples.EditView" %>

<%@ control language="C#" inherits="ViewUserControl<RecaptchaElementBlock>" %>

<%
    var formElement = Model.FormElement;
    var errorMessage = Model.GetErrorMessage();
%>

<div class="Form__Element Form__CustomElement FormRecaptcha" data-epiforms-element-name="<%: formElement.ElementName %>" data-epiforms-sitekey="<%: Model.SiteKey %>" id="<%: formElement.Guid %>">
    <% if (EPiServer.Editor.PageEditing.PageIsInEditMode) 
    {  %>
        <span class="EditView__InvisibleElement"><%: Model.EditViewFriendlyTitle  %></span>
    <% }
    else
    {  %>
        <div class="g-recaptcha"></div>
        <span role="alert" aria-live="polite" data-epiforms-linked-name="<%: formElement.ElementName %>" class="Form__Element__ValidationError" style="<%: string.IsNullOrEmpty(errorMessage) ? "display:none" : "" %>;"><%: errorMessage %></span>
    <% }  %>
</div>