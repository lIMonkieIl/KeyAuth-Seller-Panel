using Bunifu.UI.WinForms;
using Bunifu.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyAuth_Seller_Panel.SellerPanel.Views.SelectedAppControls
{
    public partial class FilesView : UserControl
    {
        int FileId = 0;
        public FilesView()
        {
            InitializeComponent();

        }
    

        private void UploadBtn_Click(object sender, EventArgs e)
        {
            HomeView.sellerApi.FileUpload(FileUrlTb.Text);
            if (HomeView.sellerApi.response.Success)
            {
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
                SelectedAppView.AppViews.Controls.Clear();
                SelectedAppView.AppViews.Controls.Add(new FilesView());
            }
            else
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
        }

        private void FilesView_Load(object sender, EventArgs e)
        {
            ScrollbarBinder.BindDatagridView(bunifuDataGridView1, bunifuVScrollBar1);
            HomeView.sellerApi.FileViewAll();
            if (HomeView.sellerApi.response.Success)
            {
                foreach (var Files in HomeView.sellerApi.files.All)
                    bunifuDataGridView1.Rows.Insert(0, Files.Id, Files.Url);
                bunifuDataGridView1.Columns[0].Width = 68;
                bunifuDataGridView1.Columns[1].Width = 650;
            }
            else if (HomeView.sellerApi.response.Message.Contains("No application with specified seller key found"))
            {
                bunifuSnackbar1.Show(HomeView.MainForm, "Redirecting to App info.", BunifuSnackbar.MessageTypes.Information, 10000, "", BunifuSnackbar.Positions.MiddleCenter);
                bunifuSnackbar1.Show(HomeView.MainForm, "Your seller key may have been changed please update it.", BunifuSnackbar.MessageTypes.Error, 10000, "", BunifuSnackbar.Positions.MiddleCenter);
                AppStatsView appStatsView = new AppStatsView();
                SelectedAppView.AppViews.Controls.Add(appStatsView);
                SelectedAppView.AppViews.Controls.Remove(this);

            }
            else
                bunifuSnackbar1.Show(HomeView.MainForm, HomeView.sellerApi.response.Message, BunifuSnackbar.MessageTypes.Information, 10000, "", BunifuSnackbar.Positions.MiddleCenter);

            bunifuVScrollBar1.BindTo(bunifuDataGridView1, true);
        }

        private void bunifuDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.SelectedCells.Count > 0)
                FileId = Convert.ToInt32(bunifuDataGridView1.SelectedCells[0].Value);
        }


        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomeView.sellerApi.FileDeleteAll();
            if (HomeView.sellerApi.response.Success)
            {
                SelectedAppView.AppViews.Controls.Clear();
                SelectedAppView.AppViews.Controls.Add(new FilesView());
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
            }

            if (!HomeView.sellerApi.response.Success)
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomeView.sellerApi.FileDelete(FileId);
            if (HomeView.sellerApi.response.Success)
            {
                SelectedAppView.AppViews.Controls.Clear();
                SelectedAppView.AppViews.Controls.Add(new FilesView());
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
            }

            if (!HomeView.sellerApi.response.Success)
                bunifuSnackbar1.Show(new HomeView(), HomeView.sellerApi.response.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 5000, "", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
        }

        private void downloadOverwrightToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (DownloadFileView downloadFileView = new DownloadFileView())
            {
                downloadFileView.ShowDialog();
            }

        }
    }
}
