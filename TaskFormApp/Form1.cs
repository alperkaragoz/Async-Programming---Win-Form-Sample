using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskFormApp
{
    public partial class Form1 : Form
    {
        public int counter { get; set; } = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnReadFile_Click(object sender, EventArgs e)
        {
            string data = string.Empty;
            Task<string> read = ReadFileAsync();

            richTextBox1.Text = await new HttpClient().GetStringAsync("https://www.google.com");

            // Await ile aşağıdaki kodu yazarak kodu blokluyoruz.(Ana thread i bloklamadan.)
            // Eğer ilgili thread i o anda sonlandırıp datayı almak istiyorsan await keywordünü kullanıyoruz.Örn: data=await ReadFileAsync(); Eğer arada işlemler yapıp sonrasında datayı almak istiyorsak aşağıdaki şekilde data=await read; olarak kullanıyoruz.
            data = await read;

            rtbBox.Text = data;
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            txtCounter.Text = counter++.ToString();
        }

        private string ReadFile()
        {
            string data = string.Empty;
            using (StreamReader sr = new StreamReader("file.txt"))
            {
                // Ana thread i bloklar(Thread.Sleep()).
                Thread.Sleep(5000);
                data = sr.ReadToEnd();
            }
            return data;
        }

        private async Task<string> ReadFileAsync()
        {
            string data = string.Empty;
            using (StreamReader sr = new StreamReader("file.txt"))
            {
                Task<string> task = sr.ReadToEndAsync();

                // data=await task; koduna gelene kadar .ReadToEndAsync() metotundan dönecek datadan bağımsız işler yapılabilir.

                // İlgili thread i bloklar.(Task.Delay)
                await Task.Delay(1000);

                data = await task;
            }
            return data;
        }
    }
}
