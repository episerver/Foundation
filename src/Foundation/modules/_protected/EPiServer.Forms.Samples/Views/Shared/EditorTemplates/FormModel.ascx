<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EPiServer.Forms.Samples.Criteria.FormModel>" %>
<%@ Assembly Name="EPiServer.Forms.Samples" %>
<%@ Import Namespace="EPiServer.Personalization.VisitorGroups.Criteria" %>
<%@ Import Namespace="EPiServer.Personalization.VisitorGroups" %>
 
<div id='FormModel'>
    <div class="epi-critera-block">
        <span class="epi-criteria-inlineblock">
            <%= Html.LabelFor(model => model.SubmissionStatus, Html.Translate("/episerver/forms/samples/criteria/formcriterion/submissionstatus"), new { @class = "episize200" })%>
        </span>
        <span class="epi-criteria-inlineblock">
            <%= Html.DojoEditorFor(model => model.SubmissionStatus)%>
        </span>
    </div>
    <div class="epi-critera-block">
        <span class="epi-criteria-inlineblock">
            <%= Html.LabelFor(model => model.SelectedForm, Html.Translate("/episerver/forms/samples/criteria/formcriterion/selectedform"), new { @class = "episize200" })%>
        </span>
        <span class="epi-criteria-inlineblock">
            <%= Html.DojoEditorFor(model => model.SelectedForm)%>
        </span>
    </div>
</div>