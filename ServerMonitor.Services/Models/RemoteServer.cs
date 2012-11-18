using Q42.WinRT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Services.Models
{

    public enum ServerType
    {
        None,
        Http,
        Ftp,
        Ping
    }

    public class RemoteServer : BindableBase
    {
        public Guid Id { get; set; }


        public string Name { get; set; }
        public string Url { get; set; }

        public ServerType Type { get; set; }

        public int Port { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public DataLoader DataLoader { get; set; }


        private string _errorMsg;

        [Newtonsoft.Json.JsonIgnore]
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                _errorMsg = value;
                OnPropertyChanged();
            }
        }
        

        public RemoteServer()
        {
            DataLoader = new DataLoader();
        }

        public void CheckOnline()
        {
            if(!DataLoader.IsBusy)
                DataLoader.LoadAsync(() => CheckStatus());
        }

        private Task<bool> CheckStatus()
        {
            switch (Type)
            {
                case ServerType.None:
                    break;
                case ServerType.Http:
                    return CheckHttp();
                case ServerType.Ftp:
                    break;
                case ServerType.Ping:
                    break;
                default:
                    break;
            }

            return Task.FromResult(true);
        }

        private async Task<bool> CheckHttp()
        {
            Uri address = new Uri(this.Url, UriKind.Absolute);
            HttpClient webclient = new HttpClient();
            webclient.MaxResponseContentBufferSize = 9000000;

            try
            {
                string response = await webclient.GetStringAsync(address);
            }
            catch (Exception e)
            {
                ErrorMsg = "Unable to connect to \r\n" + this.Url;

                throw;
            }


            return true;

        }

        public bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(this.Name))
                return false;

            if (string.IsNullOrEmpty(this.Url))
                return false;

            if (!Uri.IsWellFormedUriString(this.Url, UriKind.Absolute))
                return false;

            return isValid;
        }
    }
}
