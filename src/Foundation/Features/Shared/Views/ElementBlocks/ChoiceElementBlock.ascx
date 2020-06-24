<%--
    ====================================
    Version: 4.5.0.0. Modified: 20170321
    ====================================
--%>

<%@ import namespace="System.Web.Mvc" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Implementation.Elements" %>
<%@ control language="C#" inherits="ViewUserControl<ChoiceElementBlock>" %>

<%
    var formElement = Model.FormElement;
    var labelText = Model.Label;
    var items = Model.GetItems();
%>

<% using(Html.BeginElement(Model, new { id=formElement.Guid, @class="FormChoice" + Model.GetValidationCssClasses(), data_f_type="choice" }, true)) { %>
    <%  if(!string.IsNullOrEmpty(labelText)) { %>
    <span class="Form__Element__Caption"><%: labelText %></span>
    <% } 
        foreach (var item in items)
        {
            var defaultCheckedString = Model.GetDefaultSelectedString(item);
            var checkedString = string.IsNullOrEmpty(defaultCheckedString) ? string.Empty : "checked";
    %>
    <div class="<%:(Model.AllowMultiSelect ? "checkbox" : "radio")%>">
    <label>
        <% if(Model.AllowMultiSelect) { %>
        <input type="checkbox" name="<%: formElement.ElementName %>" value="<%: item.Value %>" class="FormChoice__Input FormChoice__Input--Checkbox checkbox" <%: checkedString %> <%: defaultCheckedString %> data-f-datainput />
        <% } else  { %>
        <input type="radio" name="<%: formElement.ElementName %>" value="<%: item.Value %>" class="FormChoice__Input FormChoice__Input--Radio" <%: checkedString %> <%: defaultCheckedString %> data-f-datainput />
        <% } %>
        <%: item.Caption %></label></div>

    <% } %>
    <%= Html.ValidationMessageFor(Model) %>
<% } %>