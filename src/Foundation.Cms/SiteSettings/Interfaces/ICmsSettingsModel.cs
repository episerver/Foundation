namespace Foundation.Cms.SiteSettings.Interfaces
{
    public interface ICmsSettingsModel
    {
        ICmsLayoutSettings LayoutSettings { get; set; }
        ISelectionSettings SelectionSettings { get; set; }
    }
}
