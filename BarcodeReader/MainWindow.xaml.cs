using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;

namespace BarcodeReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string TheApp =Properties.Settings.Default.pathToZbar;
        Boolean Exists;
        

        public MainWindow()
        {
            InitializeComponent();
            Exists = File.Exists(TheApp);

        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Exists)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = TheApp;
                startInfo.Arguments = "--raw  -Sdisable -Sqrcode.enable";
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = false;
                process.StartInfo = startInfo;
                process.Start();

                textBox1.Text = process.StandardOutput.ReadLine();
                System.Media.SystemSounds.Beep.Play();
                try
                {
                    process.Kill();
                }
                catch
                {
                    textBox1.Text = "Error Reading Barcode";
                }
                System.Windows.Clipboard.SetText(textBox1.Text);
            }
            else
            {
                textBox1.Text = "App Missing\r Please install or select";
                
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Title = "Select the ZBarCam.exe Application";
                myDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
                if (myDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text += myDialog.FileName;
                    TheApp = myDialog.FileName;
                    BarcodeReader.Properties.Settings.Default.pathToZbar = TheApp;
                    BarcodeReader.Properties.Settings.Default.Save();
                    Exists = File.Exists(TheApp);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TheApp = "";
            BarcodeReader.Properties.Settings.Default.pathToZbar = TheApp;
            BarcodeReader.Properties.Settings.Default.Save();
            Exists = File.Exists(TheApp);
        }
    }
}
