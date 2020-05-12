<%@ import namespace="System.Web.Mvc" %>
<%@ import namespace="EPiServer.Core" %>
<%@ import namespace="EPiServer.Web.Mvc.Html" %>
<%@ import namespace="EPiServer.Forms.Core" %>
<%@ import namespace="EPiServer.Forms.Core.Models" %>
<%@ import namespace="EPiServer.Forms.Helpers.Internal" %>
<%@ import namespace="EPiServer.Forms.Samples.Implementation.Elements" %>
<%@ import namespace="EPiServer.Forms.Samples.Implementation.Models" %>
<%@ import namespace="EPiServer.Forms.Samples.EditView" %>

<%@ control language="C#" inherits="ViewUserControl<AddressesElementBlock>" %>

<%  
    var formElement = Model.FormElement;
    var errorMessage = Model.GetErrorMessage();
    var addressInfo = Model.GetDefaultAddressInfo();
    var addressDetail = addressInfo.address;
    var route = addressInfo.street; 
    var city = addressInfo.city; 
    var state = addressInfo.state; 
    var postalCode = addressInfo.postalCode; 
    var country = addressInfo.country; 
%>

    <fieldset  class="Form__Element Form__CustomElement FormAddressElement <%: Model.GetValidationCssClasses() %>" data-epiforms-element-name="<%: formElement.ElementName %>">

        <legend class="Form__Element__Caption visually-hidden"><%: Model.Label %></legend>
    
    <!-- Address detail-->
    <label for="<%: formElement.Guid + "_address" %>" class="Form__Element__Caption"><%: Model.AddressLabel %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_address" %>" type="text" class="Form__CustomInput FormAddressElement__Address" value="<%: addressDetail %>" <%: Html.Raw(Model.AttributesString) %> />
    <!-- Route-->
    <label for="<%: formElement.Guid + "_route" %>" class="Form__Element__Caption"><%: Model.StreetLabel %></label>
    <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_route" %>" type="text" class="Form__CustomInput FormAddressElement__Route" value="<%: route %>" <%: Html.Raw(Model.AttributesString) %> />
     <!-- City -->
     <label for="<%: formElement.Guid + "_locality" %>" class="Form__Element__Caption"><%: Model.CityLabel %></label>
     <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_locality" %>" type="text" class="Form__CustomInput FormAddressElement__Locality" value="<%: city %>" <%: Html.Raw(Model.AttributesString) %> />
     <!-- State -->
     <label for="<%: formElement.Guid + "_administrative_area_level_1" %>" class="Form__Element__Caption"><%: Model.StateLabel %></label>
     <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_administrative_area_level_1" %>" type="text" class="Form__CustomInput FormAddressElement__State" value="<%: state %>" <%: Html.Raw(Model.AttributesString) %> />
     <!-- Zip code-->
     <label for="<%: formElement.Guid + "_postal_code" %>" class="Form__Element__Caption"><%: Model.PostalLabel %></label>
     <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_postal_code" %>" type="text" class="Form__CustomInput FormAddressElement__ZipCode" value="<%: postalCode %>" <%: Html.Raw(Model.AttributesString) %> />
     <!-- Country-->
     <label for="<%: formElement.Guid + "_country" %>" class="Form__Element__Caption"><%: Model.CountryLabel %></label>
     <input name="<%: formElement.ElementName %>" id="<%: formElement.Guid + "_country" %>" type="text" class="Form__CustomInput FormAddressElement__Country" value="<%: country %>" <%: Html.Raw(Model.AttributesString) %> />
     <!-- Map -->
     <label for="<%: formElement.Guid + "_map" %>" class="Form__Element__Caption"></label>
     <div style="width: <%: Model.MapWidth + "px" %>; height: <%: Model.MapHeight + "px" %>; display: none;" id="<%: formElement.Guid + "_map" %>" class="Form__CustomInput FormAddressElement__Map"></div>
    
     <span role="alert" aria-live="polite" data-epiforms-linked-name="<%: formElement.ElementName %>" class="Form__Element__ValidationError" style="<%: string.IsNullOrEmpty(errorMessage) ? "display:none" : "" %>;"><%: errorMessage %></span>
    
    <% if (!EPiServer.Editor.PageEditing.PageIsInEditMode) 
       {
    %>
      
        <script type="text/javascript">
            var __SamplesAddressElements = __SamplesAddressElements || [];
            __SamplesAddressElements.push({
                guid: "<%: formElement.Guid %>"
            });
        </script>
    <% } %>

        </fieldset>