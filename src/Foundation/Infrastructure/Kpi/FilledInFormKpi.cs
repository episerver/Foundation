using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Events;
using EPiServer.Framework.Localization;
using EPiServer.Marketing.KPI.Exceptions;
using EPiServer.Marketing.KPI.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Foundation.Infrastructure.Kpi
{
    [DataContract]
    public class FilledInFormKpi : EPiServer.Marketing.KPI.Manager.DataClass.Kpi
    {
        private readonly IContentRepository _contentRepository;
        private readonly IEPiServerFormsCoreConfig _formsConfig;
        private readonly IFormRepository _formRepository;
        private readonly LocalizationService _localization;
        private readonly FormsEvents _formsEvents;

        //This is needed by the KPI engine
        public FilledInFormKpi()
        {
            _formRepository = _servicelocator.GetInstance<IFormRepository>();
            _contentRepository = _servicelocator.GetInstance<IContentRepository>();
            _formsConfig = _servicelocator.GetInstance<IEPiServerFormsCoreConfig>();
            _localization = _servicelocator.GetInstance<LocalizationService>();
        }

        public FilledInFormKpi(
            IFormRepository formRepository,
            IContentRepository contentRepository,
            IEPiServerFormsCoreConfig formsConfig,
            LocalizationService localization,
            FormsEvents formsEvents)
        {
            _formRepository = formRepository;
            _contentRepository = contentRepository;
            _formsConfig = formsConfig;
            _localization = localization;
            _formsEvents = formsEvents;
        }

        [DataMember]
        public override string UiMarkup
        {
            get
            {
                // This is used when setting up the custom Kpi in the UI
                var sb = new StringBuilder();

                sb.Append("<p>");
                sb.Append("<select data-dojo-type=\"dijit/form/ComboBox\" id=\"FormKPIFormId\" name=\"FormKPIFormId\">");
                foreach (var form in GetAllFormNamesandIds())
                {
                    sb.Append("<option value=\"" + form.Item1 + "\">" + WebUtility.HtmlEncode(form.Item2) + "</option>");
                }
                sb.Append("</select>");
                sb.Append("</p>");

                return sb.ToString();
            }
        }

        [DataMember]
        public override string UiReadOnlyMarkup
        {
            get
            {
                var content = _localization.GetString("/formkpi/readonlytext", "Has submitted the form");
                var foundFormId = GetAllFormNamesandIds()
                    .FirstOrDefault(x => x.Item1 == FormGuid.ToString());

                if (foundFormId == null)
                {
                    return "<div>" + content + "</div>";
                }

                return "<div>" + content + ": <strong>\"" + foundFormId.Item2 + "\"</strong></div>";
            }
        }

        [DataMember]
        public override string Description => _localization.GetString(
            "/formkpi/description",
            "Conversion goal is activated when a user submits a completed (finalised) form");

        [DataMember]
        public new string FriendlyName => _localization.GetString("/formkpi/friendlyname", "Submits form");

        [DataMember]
        public Guid FormGuid;

        private EventHandler<FormsEventArgs> _eventHander;

        /// <summary>
        /// Attach to the form submission finalised event
        /// </summary>
        public override event EventHandler EvaluateProxyEvent
        {
            add
            {
                _eventHander = value.Invoke;
                _formsEvents.FormsSubmissionFinalized += _eventHander;
            }
            remove => _formsEvents.FormsSubmissionFinalized -= _eventHander;
        }

        public override IKpiResult Evaluate(object sender, EventArgs e)
        {
            var conversionResult = new KpiConversionResult { KpiId = Id, HasConverted = false };
            try
            {
                if (e is FormsSubmittedEventArgs formSubmission)
                {
                    conversionResult.HasConverted = formSubmission.FormsContent.ContentGuid == FormGuid;
                }
            }
            catch
            {
                // ignored
            }

            return conversionResult;
        }

        public override void Validate(Dictionary<string, string> responseData)
        {
            // For some unknown reason the Combobox Dojo dijit only supplies us with the
            // selected text and not the value of the selected item, so we need to
            // do a reverse look up based on the text to get the actual form id
            var foundFormId = GetAllFormNamesandIds()
                .FirstOrDefault(x => x.Item2 == responseData["FormKPIFormId"]);

            if (foundFormId == null)
            {
                throw new KpiValidationException(
                    _localization.GetString(
                    "/formkpi/errors/couldnotfindformid",
                    "Could not look up form id, check it hasn't been deleted and select again"));
            }

            var formId = foundFormId.Item1;
            if (!string.IsNullOrEmpty(formId) && Guid.TryParse(formId, out var formGuid))
            {
                FormGuid = formGuid;
            }
            else
            {
                throw new KpiValidationException(_localization.GetString("/formkpi/errors/selectaform", "Please select a form"));
            }
        }

        private List<Tuple<string, string>> GetAllFormNamesandIds()
        {
            var allForms = new List<Tuple<string, string>>();
            foreach (var form in _formRepository.GetFormsInfo(null))
            {
                allForms.Add(new Tuple<string, string>(
                    form.FormGuid.ToString(),
                    GetFormsPath(form.FormGuid, form.Name)
                    ));
            }

            return allForms;
        }

        private string GetFormsPath(Guid contentGuid, string formPath)
        {
            var currentItem = _contentRepository.Get<IContent>(contentGuid);
            var currentParent = _contentRepository.Get<IContent>(currentItem.ParentLink);
            if (currentItem.ParentLink == _formsConfig.RootFolder)
            {
                return formPath;
            }
            formPath = currentParent.Name + " > " + formPath;
            return GetFormsPath(currentParent.ContentGuid, formPath);
        }
    }
}