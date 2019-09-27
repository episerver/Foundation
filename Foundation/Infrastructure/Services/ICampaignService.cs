namespace Foundation.Infrastructure.Services
{
    public interface ICampaignService
    {
        void UpdateLastLoginDate(string email);
        void UpdateLastOrderDate();
        void UpdatePoint(int point);
        void AddNewRecipient(string email);
    }
}
