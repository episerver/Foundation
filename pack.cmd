nuget pack ./packaging/Foundation.campaign.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Cms.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Cms.Personalization.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Commerce.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Commerce.Personalization.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Find.Cms.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Find.Commerce.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
nuget pack ./packaging/Foundation.Social.nuspec -Properties Configuration=Release -Version %1 -OutputDirectory ./artifacts -BasePath .
