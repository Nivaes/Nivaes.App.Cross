namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using Java.Util.Zip;

    [Register("nivaes.app.cross.CrossContextWrapper")]
    public class CrossContextWrapper : ContextWrapper
    {
        private LayoutInflater? _inflater;
        //private readonly IMvxBindingContextOwner _bindingContextOwner;

        public CrossContextWrapper(Context? context/*, IMvxBindingContextOwner bindingContextOwner*/)
            : base(context)
        {
        }

        public override Java.Lang.Object? GetSystemService([StringDef(Type = "Android.Content.Context", Fields = new[] { "PowerService", "WindowService", "LayoutInflaterService", "AccountService", "ActivityService", "AlarmService", "NotificationService", "AccessibilityService", "CaptioningService", "KeyguardService", "LocationService", "HealthconnectService", "SearchService", "SensorService", "StorageService", "StorageStatsService", "WallpaperService", "VibratorManagerService", "VibratorService", "ConnectivityService", "IpsecService", "VpnManagementService", "NetworkStatsService", "WifiService", "WifiAwareService", "WifiP2pService", "WifiRttRangingService", "NsdService", "AudioService", "FingerprintService", "BiometricService", "MediaRouterService", "TelephonyService", "TelephonySubscriptionService", "CarrierConfigService", "EuiccService", "TelecomService", "ClipboardService", "InputMethodService", "TextServicesManagerService", "TextClassificationService", "AppwidgetService", "DropboxService", "DevicePolicyService", "UiModeService", "DownloadService", "NfcService", "BluetoothService", "UsbService", "LauncherAppsService", "InputService", "DisplayService", "UserService", "RestrictionsService", "AppOpsService", "RoleService", "CameraService", "PrintService", "ConsumerIrService", "TvInteractiveAppService", "TvInputService", "UsageStatsService", "MediaSessionService", "MediaCommunicationService", "BatteryService", "JobSchedulerService", "PersistentDataBlockService", "MediaProjectionService", "MidiService", "HardwarePropertiesService", "ShortcutService", "SystemHealthService", "CompanionDeviceService", "VirtualDeviceService", "CrossProfileAppsService", "LocaleService", "MediaMetricsService", "DisplayHashService", "CredentialService", "DeviceLockService", "GrammaticalInflectionService", "SecurityStateService", "ContactKeysService" }), StringDef(Type = "Android.Content.Context", Fields = new[] { "PowerService", "WindowService", "LayoutInflaterService", "AccountService", "ActivityService", "AlarmService", "NotificationService", "AccessibilityService", "CaptioningService", "KeyguardService", "LocationService", "HealthconnectService", "SearchService", "SensorService", "StorageService", "StorageStatsService", "WallpaperService", "VibratorManagerService", "VibratorService", "ConnectivityService", "IpsecService", "VpnManagementService", "NetworkStatsService", "WifiService", "WifiAwareService", "WifiP2pService", "WifiRttRangingService", "NsdService", "AudioService", "FingerprintService", "BiometricService", "MediaRouterService", "TelephonyService", "TelephonySubscriptionService", "CarrierConfigService", "EuiccService", "TelecomService", "ClipboardService", "InputMethodService", "TextServicesManagerService", "TextClassificationService", "AppwidgetService", "DropboxService", "DevicePolicyService", "UiModeService", "DownloadService", "NfcService", "BluetoothService", "UsbService", "LauncherAppsService", "InputService", "DisplayService", "UserService", "RestrictionsService", "AppOpsService", "RoleService", "CameraService", "PrintService", "ConsumerIrService", "TvInteractiveAppService", "TvInputService", "UsageStatsService", "MediaSessionService", "MediaCommunicationService", "BatteryService", "JobSchedulerService", "PersistentDataBlockService", "MediaProjectionService", "MidiService", "HardwarePropertiesService", "ShortcutService", "SystemHealthService", "CompanionDeviceService", "VirtualDeviceService", "CrossProfileAppsService", "LocaleService", "MediaMetricsService", "DisplayHashService", "CredentialService", "DeviceLockService", "GrammaticalInflectionService", "SecurityStateService", "ContactKeysService" })] string? name)
        {
            if (string.Equals(name, LayoutInflaterService, StringComparison.InvariantCulture))
            {
                return _inflater ??=
                    new CrossLayoutInflater(LayoutInflater.From(BaseContext), this);
            }

            return base.GetSystemService(name);
        }
    }
}
