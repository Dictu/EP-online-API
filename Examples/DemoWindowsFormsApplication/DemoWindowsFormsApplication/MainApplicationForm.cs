using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace DemoWindowsFormsApplication
{
    public partial class MainApplicationForm : Form
    {
        private string apiHostUrl;

        public MainApplicationForm()
        {
            InitializeComponent();
            Cef.Initialize(new CefSettings());
        }

        private async void GetTokenButtonClick(object sender, EventArgs e)
        {
            if (!IsApiHostUrlSet())
                return;

            var visitor = new TaskStringVisitor();
            using (var frm = new RetrieveTokenForm($"{apiHostUrl}","/token", visitor))
            {
                frm.Show(this);
                var bearer = await visitor.Task;
                bearerTextBox.Text = bearer;
            }
        }

        private async void ToevoegenButtonClick(object sender, EventArgs e)
        {
            if (!IsApiHostUrlSet())
                return;

            var statusCode = await CallApi("toevoegen");
            MessageBox.Show(statusCode.ToString(), "Toevoegen");
        }

        private async void UitbreidenButtonClick(object sender, EventArgs e)
        {
            if (!IsApiHostUrlSet())
                return;

            var statusCode = await CallApi("uitbreiden");
            MessageBox.Show(statusCode.ToString(), "Uitbreiden");
        }

        private async void VervangenButtonClick(object sender, EventArgs e)
        {
            if (!IsApiHostUrlSet())
                return;

            var statusCode = await CallApi("vervangen");
            MessageBox.Show(statusCode.ToString(), "Vervangen");
        }

        private bool IsApiHostUrlSet()
        {
            apiHostUrl = hostTextBox.Text;
            if (string.IsNullOrWhiteSpace(apiHostUrl))
            {
                MessageBox.Show("API host url is verreist.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private async Task<HttpStatusCode> CallApi(string method)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTextBox.Text);
                var response = await httpClient.PostAsync($"{apiHostUrl}/api/v1/Energielabels/{method}", new StringContent("<EPC></EPC>", Encoding.UTF8, "application/xml"));
                return response.StatusCode;
            }
        }
    }
}
