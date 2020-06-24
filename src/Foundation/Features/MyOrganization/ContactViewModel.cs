using Foundation.Commerce.Customer;
using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Features.MyOrganization.Organization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyOrganization
{
    public class ContactViewModel
    {
        public ContactViewModel(FoundationContact contact)
        {
            ContactId = contact.ContactId;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.Email;
            Organization = contact.FoundationOrganization != null ? new OrganizationModel(contact.FoundationOrganization) : null;
            UserRole = contact.UserRole;
            Budget = contact.Budget != null ? new BudgetViewModel(contact.Budget) : null;
            Location = contact.UserLocationId;
        }

        public ContactViewModel()
        {
        }

        public Guid ContactId { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "First name *:")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last name *:")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Display(Name = "Email *:")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Display(Name = "Organization")]
        [Required(ErrorMessage = "Organization is required")]
        public string OrganizationId { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        public B2BUserRoles Role => Enum.TryParse(UserRole, out B2BUserRoles role) ? role : B2BUserRoles.None;

        public bool IsAdmin => Role == B2BUserRoles.Admin;
        public string UserRole { get; set; }
        public OrganizationModel Organization { get; set; }
        public BudgetViewModel Budget { get; set; }
        public bool ShowOrganizationError { get; set; }
    }
}