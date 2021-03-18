<%@ import namespace="System.Web.Mvc" %>
<%@ import namespace="EPiServer.Core" %>
<%@ import namespace="EPiServer.Web.Mvc.Html" %>
<%@ import namespace="EPiServer.Forms.Core" %>
<%@ import namespace="EPiServer.Forms.Core.Models" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Samples.Implementation.Elements" %>
<%@ import namespace="EPiServer.Forms.Samples.EditView" %>

<%@ control language="C#" inherits="ViewUserControl<DateTimeRangeElementBlock>" %>

<%  
    var formElement = Model.FormElement;
    var defaultValue = Model.GetDefaultValue();
    var values = !string.IsNullOrEmpty(defaultValue) ? defaultValue.Split('|') : null;
    var startValue = (values != null && values.Length > 0) ? values[0] : null;
    var endValue = (values != null && values.Length > 1) ? values[1]: null;
    var placeHolders = !string.IsNullOrEmpty(Model.PlaceHolder) ? Model.PlaceHolder.Split('|') : null;
    var startPlaceHolder = (placeHolders != null && placeHolders.Length > 0) ? placeHolders[0] : null;
    var endPlaceHolder = (placeHolders != null && placeHolders.Length > 1) ? placeHolders[1]: null;

    var errorMessage = Model.GetErrorMessage();
%>

<fieldset class="Form__Element Form__CustomElement FormDateTimeRange <%: Model.GetValidationCssClasses() %>" data-epiforms-element-name="<%: formElement.ElementName %>">
    <legend class="Form__Element__Caption"><%: Model.Label %></legend>

    <label for="<%: formElement.Guid + "_start" %>" class="Form__Element__Caption visually-hidden"><%: Html.Translate("/contenttypes/datetimerangeelementblock/start") %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_start" %>" type="text" class="Form__CustomInput FormDateTimeRange__Start"
           <% if (!string.IsNullOrWhiteSpace(startPlaceHolder)) { %>
				placeholder="<%: startPlaceHolder %>"
			<% } %>
		   value="<%: startValue %>" <%: Html.Raw(Model.AttributesString) %> />
    <span class="FormDateTimeRange_Seperator">:</span>
    <label for="<%: formElement.Guid + "_end" %>" class="Form__Element__Caption visually-hidden"><%: Html.Translate("/contenttypes/datetimerangeelementblock/end") %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_end" %>" type="text" class="Form__CustomInput FormDateTimeRange__End"
           <% if (!string.IsNullOrWhiteSpace(endPlaceHolder)) { %>
				placeholder="<%: endPlaceHolder %>"
			<% } %>
		   value="<%: endValue %>" <%: Html.Raw(Model.AttributesString) %> />
    <span role="alert" aria-live="polite" data-epiforms-linked-name="<%: formElement.ElementName %>" class="Form__Element__ValidationError" style="<%: string.IsNullOrEmpty(errorMessage) ? "display:none" : "" %>;"><%: errorMessage %></span>
    
    <% if (!EPiServer.Editor.PageEditing.PageIsInEditMode) 
       {
        // push this Element information to a global blob. Then Samples.js will init them all at once. 
        var pickerType = ((DateTimePickerType)Model.PickerType).ToString().ToLower(); %>
        <script type="text/javascript">
            var __SamplesDateTimeElements = __SamplesDateTimeElements || [];
            __SamplesDateTimeElements.push({
                guid: "<%: formElement.Guid + "_start" %>",
                pickerType: "<%: pickerType %>"
            },
                {
                    guid: "<%: formElement.Guid + "_end" %>",
                    pickerType: "<%: pickerType %>"
                });
        </script>
    <% } %>
</fieldset>