using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helper
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            congratsMessage.Visible = false;
            buttonOK.Enabled = false;
        }

        private Task ProcessData(List<string> list, IProgress<ProgressReport> progress)
        {
            int index = 1;
            int totalProcess = list.Count;
            var progressReport = new ProgressReport();
            return Task.Run(() =>
            {
                for (int i = 0; i < totalProcess; i++)
                {
                    progressReport.PercentComplete = index++ * 100 / totalProcess;
                    progress.Report(progressReport);
                    Thread.Sleep(10);
                }
            });
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
                list.Add(i.ToString());
            labelStatus.Text = "Working...";
            var progress = new Progress<ProgressReport>();
            progress.ProgressChanged += (o, report) =>
              {
                  labelStatus.Text = string.Format("Processing...{0}%", report.PercentComplete);
                  progressBar.Value = report.PercentComplete;
                  progressBar.Update();
              };
            await ProcessData(list, progress);
            Thread.Sleep(500);
            labelStatus.Text = "Done!";

            congratsMessage.Visible = true;
            buttonOK.Enabled = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }


        public class ProgressReport
        {
            public int PercentComplete { get; set; }
        }

    }
}
