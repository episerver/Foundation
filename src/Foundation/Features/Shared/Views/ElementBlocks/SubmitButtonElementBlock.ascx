<%--
    ====================================
    Version: 4.5.0.0. Modified: 20170315
    ====================================
--%>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ Control Language="C#" Inherits="ViewUserControl<SubmitButtonElementBlock>" %>

<%
    var formElement = Model.FormElement;
    var buttonText = Model.Label;

    var buttonDisableState = Model.GetFormSubmittableStatus(ViewContext.Controller.ControllerContext.HttpContext);
%>

<button id="<%: formElement.Guid %>" name="submit" type="submit" value="<%: formElement.Guid %>" data-f-is-finalized="<%: Model.FinalizeForm.ToString().ToLower() %>"
    data-f-is-progressive-submit="true" data-f-type="submitbutton"
    <%= Model.AttributesString %> <%: buttonDisableState %>
    <% if (Model.Image == null) 
    { %>
        class="Form__Element FormExcludeDataRebind FormSubmitButton button-transparent-black">
        <%: buttonText %>
    <% } else { %>
        class="Form__Element FormExcludeDataRebind FormSubmitButton FormImageSubmitButton">
        <img src="<%: Model.Image.Path %>" data-f-is-progressive-submit="true" data-f-is-finalized="<%: Model.FinalizeForm.ToString().ToLower() %>" />
    <% } %>
</button>
