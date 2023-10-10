using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace schoolbus_mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async Task v_msg(string p_msg)
        {
            await DisplayAlert("في الطريق", p_msg, "تم", FlowDirection.RightToLeft);
        }

        public async Task<Boolean> f_confirm(string p_msg, string p_cnf)
        {
            return await DisplayAlert("في الطريق", p_msg, p_cnf, "إلغاء", FlowDirection.RightToLeft);
        }

        public async Task<(string g_res, Boolean g_rtr)> f_prompt(string p_ttl, string p_msg, string g_scd)
        {
            string l_res = await DisplayPromptAsync(p_ttl, p_msg, g_scd, "إلغاء");

            return (l_res, !(string.IsNullOrWhiteSpace(l_res)));
        }

        public async Task<string> f_sheet(string p_ttl, string[] p_opt)
        {
            return await DisplayActionSheet(p_ttl, "إلغاء", null, p_opt);
        }

        public async Task<string> f_contact()
        {
            try
            {
                await Permissions.RequestAsync<Permissions.ContactsRead>();
                Contact l_ctc = await Communication.Contacts.Default.PickContactAsync();

                if (l_ctc == null)
                { return string.Empty; }

                List<ContactPhone> l_lst = l_ctc.Phones;

                string[] l_phn = (from i_phn in l_lst
                                  select i_phn.PhoneNumber.Replace(" ", string.Empty)).ToArray();

                string l_res = await f_sheet("اختر رقم الهاتف", l_phn);
                if (string.IsNullOrWhiteSpace(l_res)) { return string.Empty; }

                char l_chr = l_res[0];
                if (!(char.IsDigit(l_chr) || l_chr == '+')) { return string.Empty; }

                return l_res;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task v_launch(string p_url)
        {
            await Launcher.OpenAsync(p_url);
        }

        public async Task<string> f_json(string p_nam)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(p_nam + ".json");
                using var reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
            catch { }

            return null;
        }

        public async Task<string> f_get_secure(string g_key)
        {
            try
            {
                return await SecureStorage.Default.GetAsync(g_key);
            }
            catch
            {
                return null;
            }
        }

        public async Task v_set_secure(string p_key, string p_val)
        {
            await SecureStorage.Default.SetAsync(p_key, p_val);
        }
    }
}