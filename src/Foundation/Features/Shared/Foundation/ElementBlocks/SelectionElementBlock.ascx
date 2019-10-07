<%--
    ====================================
    Version: 4.15.1. Modified: 20180726
    ====================================
--%>

<%@ import namespace="System.Web.Mvc" %>
<%@ Import Namespace="EPiServer.Shell.Web.Mvc.Html" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ control language="C#" inherits="ViewUserControl<SelectionElementBlock>" %>

<%
    var formElement = Model.FormElement; 
    var labelText = Model.Label;
    var placeholderText = Model.PlaceHolder;
    var defaultOptionItemText = !string.IsNullOrWhiteSpace(placeholderText) ? placeholderText : Html.Translate(string.Format("/episerver/forms/viewmode/selection/{0}", Model.AllowMultiSelect ? "selectoptions" : "selectanoption"));
    var defaultOptionSelected = !Model.AllowMultiSelect && !Model.Items.Any(x => x.Checked.HasValue && x.Checked.Value) ? "selected=\"selected\"" : "";
    var items = Model.GetItems();
    var defaultValue = Model.GetDefaultValue();
%>

<% using(Html.BeginElement(Model, new { @class="FormSelection" + Model.GetValidationCssClasses(), data_f_type="selection" })) { %>
    <label for="<%: formElement.Guid %>" class="Form__Element__Caption"><%: labelText %></label>
    <select class="select" name="<%: formElement.ElementName %>" id="<%: formElement.Guid %>" <%: Model.AllowMultiSelect ? "multiple" : "" %>  <%= Model.AttributesString %> data-f-datainput>
        <option disabled="disabled" <%= defaultOptionSelected %> value=""><%: defaultOptionItemText %></option>
        <%
        foreach (var item in items)
        {
            var defaultSelectedString = Model.GetDefaultSelectedString(item, defaultValue);
            var selectedString = string.IsNullOrEmpty(defaultSelectedString) ? string.Empty : "selected";
        %>
        <option value="<%: item.Value %>" <%= selectedString %> <%= defaultSelectedString %> data-f-datainput><%: item.Caption %></option>
        <% } %>
    </select>
    <%= Html.ValidationMessageFor(Model) %>
<% } %>