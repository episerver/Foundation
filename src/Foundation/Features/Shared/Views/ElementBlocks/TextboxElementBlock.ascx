<%--
    ====================================
    Version: 4.5.0.0. Modified: 20170321
    ====================================
--%>

<%@ import namespace="System.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ control language="C#" inherits="ViewUserControl<TextboxElementBlock>" %>

<%
    var formElement = Model.FormElement; 
    var labelText = Model.Label;
%>

<% using(Html.BeginElement(Model, new { @class="FormTextbox form-group" + Model.GetValidationCssClasses(), data_f_type="textbox" })) { %>
    <label for="<%: formElement.Guid %>" class="Form__Element__Caption"><%: labelText %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid %>" type="text" class="FormTextbox__Input textbox"
        placeholder="<%: Model.PlaceHolder %>" value="<%: Model.GetDefaultValue() %>" <%: Html.Raw(Model.AttributesString) %> data-f-datainput />

    <%= Html.ValidationMessageFor(Model) %>
    <%= Model.RenderDataList() %>
<% } %>