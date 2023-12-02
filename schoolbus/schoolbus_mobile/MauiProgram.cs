#if ANDROID
using Android.Views;
#endif
using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace schoolbus_mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                 {
#if ANDROID
                     events.AddAndroid(android =>
                     {
                         android.OnCreate((activity, bundle) => 
                         {
                             activity.Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
                             activity.Window.AddFlags(WindowManagerFlags.TranslucentStatus);

                             StatusBarVisibility flags = default;
                             flags |= (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                             activity.Window.DecorView.SystemUiVisibility = flags;
                         });
                     });
#endif
                 });

#if DEBUG
#endif

            return builder.Build();
        }
    }
}