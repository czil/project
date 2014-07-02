using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;


namespace WebBrowserTest
{
    public partial class Form1 : Form
    {

        StreamWriter srWrite = null;

        string parag = null;

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            

            string fileNew = @"hurricaneIsaacN.txt";

            this.webBrowser1.ScriptErrorsSuppressed = true;

            this.webBrowser1.Navigate("www.noslang.com");

            srWrite = new StreamWriter(fileNew, false, Encoding.Default);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //this.textBox1.Text = "lol";
            

            string fileName = @"hurricaneIsaac.txt";

            string myLine = null; 
            
            try{

            using (StreamReader srRead = new StreamReader(fileName, System.Text.Encoding.Default))
             {
                 
                 //StreamWriter srWriteDel = new StreamWriter(fileNewDel, false, Encoding.Default);

               

                 int count = 0;

                 //while (count < 639)
                 //{
                 //    myLine = srRead.ReadLine();

                 //    count++;
                 //}


                 while ((myLine = srRead.ReadLine()) != null) // && count != 5)
                 {


                     //myLine = srRead.ReadLine();

                     this.webBrowser1.Document.GetElementById("p").SetAttribute("value", myLine);

                     HtmlElementCollection el = this.webBrowser1.Document.GetElementsByTagName("input");

                     HtmlElementCollection frm = this.webBrowser1.Document.GetElementsByTagName("form");

                     foreach (HtmlElement btn in el)
                     {
                         if (btn.Name == "submit")
                         {
                             btn.InvokeMember("Click");

                             //foreach (HtmlElement f in frm)
                             //{
                             //    if (f.Name == "TheForm")

                             //        f.InvokeMember("submit");
                             //}

                         }

                     }

                     //while (this.webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                     //{

                     //   System.Threading.Thread.Sleep(10000); 
                     //}

                     MessageBoxTemporal.Show(this.webBrowser1.ReadyState.ToString(), "tit", 5, false);
                 }
                         //count++;
            }
            }
             
            catch(NullReferenceException ex)
            {

                MessageBox.Show(ex.Message);
            }
                 
        }
     
        
        
           

        private void button2_Click(object sender, EventArgs e)
        {


           
            /*this.webBrowser1.Document.GetElementById("p").SetAttribute("value", textBox1.Text);

            HtmlElementCollection el = this.webBrowser1.Document.GetElementsByTagName("form");

            foreach (HtmlElement frm in el)
            {
                if (frm.Name == "TheForm")
                {
                    frm.InvokeMember("submit");
                }

            }

            HtmlElementCollection elSpan = this.webBrowser1.Document.GetElementsByTagName("span");

            this.textBox2.Text = elSpan.Count.ToString();

            foreach (HtmlElement btn in elSpan)
            {
                this.textBox2.Text = btn.InnerHtml;

            }*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(10000);

            this.button1.PerformClick();

            System.Threading.Thread.Sleep(10000);

          

            System.Threading.Thread.Sleep(10000);

            this.button2.PerformClick();
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            HtmlElementCollection elSpan = this.webBrowser1.Document.GetElementsByTagName("span");

            this.textBox2.Text = elSpan.Count.ToString();

            foreach (HtmlElement btn in elSpan)
            {
                //this.textBox2.Text = btn.InnerHtml;

                    if (parag != btn.InnerHtml)
                    {
                        srWrite.WriteLine(btn.InnerHtml);

                        parag = btn.InnerHtml;
                    }

            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (srWrite != null)
            {
                srWrite.Close();
            }
        }


        }





    public class MessageBoxTemporal
    {
        System.Threading.Timer IntervaloTiempo;
        string TituloMessageBox;
        string TextoMessageBox;
        int TiempoMaximo;
        IntPtr hndLabel = IntPtr.Zero;
        bool MostrarContador;

        MessageBoxTemporal(string texto, string titulo, int tiempo, bool contador)
        {
            TituloMessageBox = titulo;
            TiempoMaximo = tiempo;
            TextoMessageBox = texto;
            MostrarContador = contador;

            if (TiempoMaximo > 99) return; //Máximo 99 segundos
            IntervaloTiempo = new System.Threading.Timer(EjecutaCada1Segundo,
                null, 1000, 1000);
            if (contador)
            {
                DialogResult ResultadoMensaje = MessageBox.Show(texto + "\r\nEste mensaje se cerrará dentro de " +
                    TiempoMaximo.ToString("00") + " segundos ...", titulo);
                if (ResultadoMensaje == DialogResult.OK) IntervaloTiempo.Dispose();
            }
            else
            {
                DialogResult ResultadoMensaje = MessageBox.Show(texto + "...", titulo);
                if (ResultadoMensaje == DialogResult.OK) IntervaloTiempo.Dispose();
            }
        }
        public static void Show(string texto, string titulo, int tiempo, bool contador)
        {
            new MessageBoxTemporal(texto, titulo, tiempo, contador);
        }
        void EjecutaCada1Segundo(object state)
        {
            TiempoMaximo--;
            if (TiempoMaximo <= 0)
            {
                IntPtr hndMBox = FindWindow(null, TituloMessageBox);
                if (hndMBox != IntPtr.Zero)
                {
                    SendMessage(hndMBox, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    IntervaloTiempo.Dispose();
                }
            }
            else if (MostrarContador)
            {
                // Ha pasado un intervalo de 1 seg:
                if (hndLabel != IntPtr.Zero)
                {
                    SetWindowText(hndLabel, TextoMessageBox +
                        "\r\nEste mensaje se cerrará dentro de " +
                        TiempoMaximo.ToString("00") + " segundos");
                }
                else
                {
                    IntPtr hndMBox = FindWindow(null, TituloMessageBox);
                    if (hndMBox != IntPtr.Zero)
                    {
                        // Ha encontrado el MessageBox, busca ahora el texto
                        hndLabel = FindWindowEx(hndMBox, IntPtr.Zero, "Static", null);
                        if (hndLabel != IntPtr.Zero)
                        {
                            // Ha encontrado el texto porque el MessageBox
                            // solo tiene un control "Static".
                            SetWindowText(hndLabel, TextoMessageBox +
                                "\r\nEste mensaje se cerrará dentro de " +
                                TiempoMaximo.ToString("00") + " segundos");
                        }
                    }
                }
            }
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll",
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true,
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
            string lpszClass, string lpszWindow);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true,
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool SetWindowText(IntPtr hwnd, string lpString);
    }
    }

