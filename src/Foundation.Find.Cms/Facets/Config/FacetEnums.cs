namespace Foundation.Find.Cms.Facets.Config
{
    public enum FacetDisplayMode
    {
        [EnumSelectionDescription(Text = "Checkbox", Value = "Checkbox")]
        Checkbox = 1,
        [EnumSelectionDescription(Text = "Button", Value = "Button")]
        Button,
        [EnumSelectionDescription(Text = "Color Swatch", Value = "ColorSwatch")]
        ColorSwatch,
        [EnumSelectionDescription(Text = "Size swatch", Value = "SizeSwatch")]
        SizeSwatch,
        [EnumSelectionDescription(Text = "Numeric range", Value = "Range")]
        Range,
        [EnumSelectionDescription(Text = "Rating", Value = "Rating")]
        Rating,
        [EnumSelectionDescription(Text = "Slider", Value = "Slider")]
        Slider,
        [EnumSelectionDescription(Text = "Price range", Value = "PriceRange")]
        PriceRange,
    }

    public enum FacetContentFieldName
    {
        [EnumSelectionDescription(Text = "Type of content", Value = "PageTypes")]
        ContentType = 1,
        [EnumSelectionDescription(Text = "Category", Value = "ContentCategory")]
        Categories,
        [EnumSelectionDescription(Text = "Interests", Value = "TagList")]
        Interests,
        [EnumSelectionDescription(Text = "Article type", Value = "ArticleType")]
        ArticleType,
    }

    public enum FacetFieldType
    {
        [EnumSelectionDescription(Text = "String", Value = "String")]
        String = 1,
        [EnumSelectionDescription(Text = "List of string", Value = "ListOfString")]
        ListOfString,
        [EnumSelectionDescription(Text = "Integer", Value = "Integer")]
        Integer,
        [EnumSelectionDescription(Text = "2 decimal places", Value = "Double")]
        Double,
        [EnumSelectionDescription(Text = "Boolean", Value = "Boolean")]
        Boolean,
        [EnumSelectionDescription(Text = "Enhanced boolean", Value = "NullableBoolean")]
        NullableBoolean
    }

    public enum FacetDisplayDirection
    {
        [EnumSelectionDescription(Text = "Vertical", Value = "Vertical")]
        Vertical = 1,
        [EnumSelectionDescription(Text = "Horizontal", Value = "Horizontal")]
        Horizontal
    }
}
