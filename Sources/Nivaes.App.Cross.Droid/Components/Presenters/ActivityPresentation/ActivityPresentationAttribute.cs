using System.Diagnostics;

namespace Nivaes.App.Cross.Droid;

[AttributeUsage(AttributeTargets.Class)]
public class ActivityPresentationAttribute 
    : BasePresentationAttribute
{
    public ActivityPresentationAttribute()
    {
    }

    public static Bundle? DefaultExtras { [DebuggerHidden]get; }

    /// <summary>
    /// Add extras to the Intent that will be started for this Activity
    /// </summary>
    public Bundle? Extras { [DebuggerHidden]get; [DebuggerHidden]set; } = DefaultExtras;
}