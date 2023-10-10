namespace schoolbus_mobile.Pages
{
    public partial class Home
    {
        private Timer? r_tim;

        protected override void OnAfterRender(bool p_1st)
        {
            if (!p_1st) { return; }
            JsRuntime.InvokeAsync<string>("f_load_map", new object[] { 30, 31 });

            r_tim = new Timer(async (stateInfo) =>
            {
                var l_rnd = new Random();
                double l_lat = (l_rnd.NextDouble() / 100) + 30;
                double l_lng = (l_rnd.NextDouble() / 100) + 31;

                await JsRuntime.InvokeAsync<string>("f_move_marker", new object[] { l_lat, l_lng });
            }, new AutoResetEvent(false), 3000, 3000);
        }

        async Task v_center()
        {
            await JsRuntime.InvokeAsync<string>("f_center_map", new object[] { });
        }
    }
}