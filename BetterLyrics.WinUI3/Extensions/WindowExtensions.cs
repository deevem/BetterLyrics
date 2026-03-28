using BetterLyrics.WinUI3.Enums;
using BetterLyrics.WinUI3.Helper;
using BetterLyrics.WinUI3.Services.LocalizationService;
using BetterLyrics.WinUI3.Services.SettingsService;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.UI;

namespace BetterLyrics.WinUI3.Extensions
{
    public static class WindowExtensions
    {
        extension(Window window)
        {
            public void Init(
                string titleKey = "",
                string title = "",
                TitleBarHeightOption titleBarHeightOption = TitleBarHeightOption.Standard,
                BackdropType backdropType = BackdropType.DesktopAcrylic)
            {
                var localizationService = Ioc.Default.GetRequiredService<ILocalizationService>();

                if (titleKey != "")
                {
                    window.Title = localizationService.GetLocalizedString(titleKey);
                }
                if (title != "")
                {
                    window.Title = title;
                }
                window.AppWindow.TitleBar.PreferredTheme = TitleBarTheme.UseDefaultAppMode;
                window.AppWindow.SetIcons();

                window.ExtendsContentIntoTitleBar = true;
                window.AppWindow.TitleBar.PreferredHeightOption = titleBarHeightOption;

                ApplyTransparentCaptionChrome(window.AppWindow.TitleBar);

                window.SystemBackdrop = SystemBackdropHelper.CreateSystemBackdrop(backdropType);
            }

            /// <summary>
            /// Win10 (and some Win11 builds) leaves a visible light strip behind caption buttons when
            /// <see cref="Window.ExtendsContentIntoTitleBar"/> is true unless caption chrome is fully transparent.
            /// </summary>
            private static void ApplyTransparentCaptionChrome(AppWindowTitleBar titleBar)
            {
                var transparent = Color.FromArgb(0, 0, 0, 0);
                titleBar.BackgroundColor = transparent;
                titleBar.InactiveBackgroundColor = transparent;
                titleBar.ButtonBackgroundColor = transparent;
                titleBar.ButtonInactiveBackgroundColor = transparent;
            }

            public void SyncTheme()
            {
                var settingsService = Ioc.Default.GetRequiredService<ISettingsService>();
                if (settingsService == null || window == null || window.Content == null) return;

                var appTheme = settingsService.AppSettings.GeneralSettings.AppTheme;
                window.AppWindow.TitleBar.PreferredTheme = appTheme.ToTitleBarTheme();
                ((FrameworkElement)window.Content).RequestedTheme = appTheme;
            }

        }
    }
}
