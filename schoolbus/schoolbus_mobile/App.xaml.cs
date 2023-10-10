#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace schoolbus_mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
#if WINDOWS
                var mauiWindow = handler.VirtualView;
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new SizeInt32(450, 1000));

                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(id, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                var CenteredPosition = appWindow.Position;
                CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                appWindow.Move(CenteredPosition);

                appWindow.Move(new PointInt32(590, 10));
#endif
            });

            MainPage = new MainPage();
        }
    }
}