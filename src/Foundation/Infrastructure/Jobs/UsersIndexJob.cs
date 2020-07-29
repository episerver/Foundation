using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.MyOrganization.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Jobs
{
    [ScheduledPlugIn(
        DisplayName = "Users Index Job",
        Description = "Index users in the database ",
        SortIndex = 1)]
    public class UsersIndexJob : ScheduledJobBase
    {
        private readonly int _batchSize = 500;

        public UsersIndexJob()
        {
            IsStoppable = false;
        }

        public Injected<ILogger> Logger { get; set; }
        public Injected<ICustomerService> CustomerService { get; set; }
        public Injected<IClient> Find { get; set; }

        public override string Execute()
        {
            OnStatusChanged("Started execution.");

            try
            {
                IndexContacts();

                return "Done";
            }
            catch (Exception ex)
            {
                Logger.Service.Log(Level.Critical, ex.Message, ex);
                throw new Exception("Error: " + ex.Message);
            }
        }

        private void IndexContacts()
        {
            var batchNumber = 0;
            List<FoundationContact> contacts;

            do
            {
                contacts = ReadContacts(batchNumber);

                var contactsToIndex = new List<UserSearchResultModel>(batchNumber);
                contactsToIndex.AddRange(contacts.Select(ConvertToUserSearchResultModel));

                try
                {
                    if (contactsToIndex.Count > 0)
                    {
                        Find.Service.Delete<UserSearchResultModel>(x => x.ContactId.Exists());
                        Find.Service.Index(contactsToIndex);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Service.Log(Level.Error, ex.Message, ex);
                }

                var indexed = batchNumber * _batchSize + contacts.Count;
                OnStatusChanged($"Indexed {indexed} contacts");
                batchNumber++;
            } while (contacts.Count > 0);
        }

        private UserSearchResultModel ConvertToUserSearchResultModel(FoundationContact contact)
        {
            return new UserSearchResultModel
            {
                ContactId = contact.ContactId,
                Email = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                FullName = contact.FullName
            };
        }

        private List<FoundationContact> ReadContacts(int batchNumber)
        {
            return CustomerService.Service.GetContacts()
                .OrderBy(x => x.ContactId)
                .Skip(_batchSize * batchNumber)
                .Take(_batchSize).ToList();
        }
    }
}