using EPiServer.ConnectForCampaign.Services.Implementation;
using EPiServer.Logging;
using Foundation.Commerce.Customer.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Foundation.Infrastructure.Services
{
    public class CampaignService : ICampaignService
    {
        protected readonly IRecipientListService _campaignRecipientList;
        protected readonly IRecipientService _campaignRecipient;
        protected readonly ICustomerService _customerService;

        private readonly string recipientName = ConfigurationManager.AppSettings["Campaign.Newsletter.Name"];
        private readonly string lastOrderDate = ConfigurationManager.AppSettings["Campaign.Newsletter.LastOrderUpdate"];
        private readonly string lastLogin = ConfigurationManager.AppSettings["Campaign.Newsletter.LastLogin"];
        private readonly string bonusPoints = ConfigurationManager.AppSettings["Campaign.Newsletter.BonusPoints"];
        private readonly string score = ConfigurationManager.AppSettings["Campaign.Newsletter.Score"];

        public CampaignService(IRecipientListService campaignRecipientLists,
            IRecipientService campaignRecipient,
            ICustomerService customerService)
        {
            _campaignRecipientList = campaignRecipientLists;
            _campaignRecipient = campaignRecipient;
            _customerService = customerService;
        }

        /// <summary>
        /// Updates the last login date.
        /// </summary>
        /// <param name="email">The email.</param>
        public void UpdateLastLoginDate(string email) => UpdateCurrentDate(lastLogin, email);

        /// <summary>
        /// Updates the last order date.
        /// </summary>
        public void UpdateLastOrderDate()
        {
            var email = _customerService.GetCurrentContact()?.Email;
            if (!string.IsNullOrEmpty(email))
            {
                UpdateCurrentDate(lastOrderDate, email);
            }
        }

        /// <summary>
        /// Updates bonus point and score.
        /// </summary>
        /// <param name="point">Extra point to bonus and score.</param>
        public void UpdatePoint(int point)
        {
            try
            {
                var email = _customerService.GetCurrentContact()?.Email;
                if (string.IsNullOrEmpty(email))
                {
                    return;
                }

                var currentRecipientListId = GetRecipientListId(recipientName);

                // Get current point of recipient
                var currentPoints = _campaignRecipient.GetRecipientDetails(currentRecipientListId, email, new List<string>() { bonusPoints, score }).ToArray();

                // Update recipient
                if (currentRecipientListId != -1)
                {
                    var b = long.TryParse(currentPoints[0], out var oldBonusPoints);
                    var s = long.TryParse(currentPoints[1], out var oldScore);

                    var data = new Dictionary<string, string>
                    {
                        { bonusPoints, (oldBonusPoints + point).ToString() },
                        { score, (oldScore + point).ToString() },
                        { "Email", email }
                    };

                    AddOrUpdateRecipient(data, currentRecipientListId);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Adds the new recipient list.
        /// </summary>
        /// <param name="email">The email which identifies the recipient.</param>
        public void AddNewRecipient(string email)
        {
            var currentRecipientListId = GetRecipientListId(recipientName);
            var data = new Dictionary<string, string>
            {
                { "Email", email }
            };

            AddOrUpdateRecipient(data, currentRecipientListId);
        }

        /// <summary>
        /// Gets the recipient list identifier.
        /// </summary>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        private long GetRecipientListId(string recipientName)
        {
            var allRecipientIds = _campaignRecipientList.GetAllRecipientLists();
            return allRecipientIds.FirstOrDefault(x => x.Name.Equals(recipientName))?.Id ?? 0;
        }

        /// <summary>
        /// Adds the or update recipient.
        /// </summary>
        /// <param name="profileValues">The profile values.</param>
        /// <param name="recipientListId">The recipient list identifier.</param>
        private string AddOrUpdateRecipient(Dictionary<string, string> profileValues, long recipientListId)
        {
            _campaignRecipient.UpsertRecipient(profileValues, recipientListId, 0, out var result);
            return result;
        }

        /// <summary>
        /// Updates a field on recipient campaign with current datetime.
        /// </summary>
        /// <param name="dateField">Field name on recipient of campaign.</param>
        /// <param name="email">The email on recipient of campaign.</param>
        private void UpdateCurrentDate(string dateField, string email)
        {
            try
            {
                var currentRecipientListId = GetRecipientListId(recipientName);

                // Update recipient
                if (currentRecipientListId != -1)
                {
                    var dictionary = new Dictionary<string, string>
                    {
                        { dateField, DateTime.Now.ToString("o") },
                        { "Email", email }
                    };

                    AddOrUpdateRecipient(dictionary, currentRecipientListId);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
            }
        }
    }
}