namespace Nivaes.App.Cross;

using System.ComponentModel.DataAnnotations;

public enum CameraResolution : short
{
    ///// <summary>The user can select any resolution.</summary>
    //[Display(nameof(AppResources.MaxResolucionLabel), ResourceType = typeof(AppResources))]
    //HighestAvailable = 0,

    /// <summary>The user can select resolutions up to 320 X 240, or a similar 16:9 resolution.</summary>
    [Display(Name = "320x240")]
    VerySmallQvga = 1,

    /// <summary>The user can select resolutions up to 800 X 600, or a similar 16:9 resolution.</summary>
    [Display(Name = "800x600")]
    SmallVga = 2,

    /// <summary>The user can select resolutions up to 1024 X 768, or a similar 16:9 resolution.</summary>
    [Display(Name = "1024x768")]
    MediumXga = 3,

    /// <summary>The user can select resolutions up to 1920 X 1080, or a similar 4:3 resolution.</summary>
    [Display(Name = "1920 X 1080")]
    Large3M = 4,

    ///// <summary>The user can select resolutions up to 5MP.</summary>
    //[Display(Name = "5 MP")]
    //VeryLarge5M = 5

    ///// <summary>The user can select any resolution.</summary>
    //[Display(Name = nameof(SettingsLocalizability.MaxResolucionLabel), ResourceType = typeof(SettingsLocalizability))]
    //HighestAvailable = 100,
}
