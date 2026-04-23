namespace Foundation.Infrastructure.Display
{
    public abstract class DisplayResolutionBase : IDisplayResolution
    {
        private readonly string _localizationKey;
        private string _name;
        private Injected<LocalizationService> LocalizationService { get; set; }

        protected DisplayResolutionBase(string localizationKey, int width, int height)
        {
            _localizationKey = localizationKey;
            Id = GetType().FullName;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the unique ID for this resolution
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the name of resolution
        /// </summary>
        public string Name
        {
            get => _name ?? Translate(_localizationKey);
            protected set => _name = value;
        }

        /// <summary>
        /// Gets the resolution width in pixels
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets the resolution height in pixels
        /// </summary>
        public int Height { get; protected set; }

        private string Translate(string resourceKey)
        {
            if (!LocalizationService.Service.TryGetString(resourceKey, out var value))
            {
                value = resourceKey;
            }

            return value;
        }
    }
}