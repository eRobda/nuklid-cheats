using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;

namespace nuklidO2_among_us
{
    public partial class Form1 : Form
    {
        #region hovna
        public const string WINDOW_NAME = "Among Us";

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public static IntPtr handle = FindWindow(null, WINDOW_NAME);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        public static RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }
        #endregion

        static bool ukazuje = true;
        bool isRunning = true;

        public Form1()
        {
            InitializeComponent();
            nameLabel.Parent = label3;
            nameLabel.BackColor = Color.Transparent;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //speedhack
        }   

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] procs = Process.GetProcesses();
            bool obsahuje = false;
            foreach (Process proc in procs)
            {
                if (proc.ProcessName == "Among Us")
                {
                    obsahuje = true;
                }
            }
            if (obsahuje == false)
            {
                MessageBox.Show("Among Us isn't running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(Environment.ExitCode);
            }

            CheckForIllegalCrossThreadCalls = false;
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle);

            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);

            this.Left = rect.left;
            this.Top = rect.top;

            Thread th = new Thread(HideShow);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            
            Thread t = new Thread(Hover);
            t.Start();

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                GetWindowRect(handle, out rect);
                this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);

                this.Left = rect.left;
                this.Top = rect.top;
                Thread.Sleep(10);
            }
        }
        void HideShow()
        {
            while (isRunning)
            {
                if ((Keyboard.GetKeyStates(Key.Insert) & KeyStates.Down) > 0 && ukazuje == true)
                {
                    this.Hide();
                    ukazuje = false;
                
                    Thread.Sleep(200);
                }
                else if((Keyboard.GetKeyStates(Key.Insert) & KeyStates.Down) > 0 && ukazuje == false)
                {
                    this.Show();
                    ukazuje = true;

                    Thread.Sleep(200);
                }

                Process[] procs = Process.GetProcesses();
                bool obsahuje = false;
                foreach (Process proc in procs)
                {
                    if (proc.ProcessName == "Among Us")
                    {
                        obsahuje = true;
                    }
                }
                if (obsahuje == false)
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }
        }

        void Hover()
        {
            while (isRunning)
            {

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseHover(object sender, EventArgs e)
        {
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle);
        }
    }
}
