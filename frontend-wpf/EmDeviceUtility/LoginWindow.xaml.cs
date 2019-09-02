using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmDeviceUtility
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string serverAddr = "http://localhost:3000";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var request = WebRequest.Create(serverAddr + "/login");
            var resp = (HttpWebResponse)request.GetResponse();
            if(resp.StatusCode == HttpStatusCode.OK)
            {
                using (var dataStream = resp.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    var reader = new StreamReader(dataStream);
                    // Read the content.  
                    var responseFromServer = reader.ReadToEnd();
                    // Convert json to object
                    dynamic users = JsonConvert.DeserializeObject(responseFromServer);
                    var str = "";
                    foreach(var user in users)
                    {
                        var u = (string)user;
                        str += u + ",";
                    }
                    MessageBox.Show("Got users from server: " + str);
                }
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please enter username and password", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Refrence - https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class

            var request = WebRequest.Create(serverAddr + "/login");
            request.Method = "POST";
            var outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("username", txtUsername.Text);
            outgoingQueryString.Add("password", txtPassword.Password);
            var data = outgoingQueryString.ToString();            
            var dataArray = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = dataArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(dataArray, 0, dataArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            try
            { 
                var resp = (HttpWebResponse)request.GetResponse();           
                // Successfull login, goto next window
                var dw = new DataWindow();
                dw.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid username or password. Please try again", "Login", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }        
    }
}
