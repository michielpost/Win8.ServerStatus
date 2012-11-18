using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServerMonitor.Services.Interfaces;
using ServerMonitor.Services.Models;
using ServerMonitor.UserControls;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Networking.Connectivity;
using GalaSoft.MvvmLight.Threading;

namespace ServerMonitor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ILocalStoreService StorageService { get; set; }

        private ObservableCollection<RemoteServer> _servers;

        public ObservableCollection<RemoteServer> Servers
        {
            get { return _servers; }
            set { _servers = value;
            RaisePropertyChanged("Servers");
                 CheckServers();
            }
        }

        public RelayCommand AddServerCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }


        private bool _hasInternet;

        public bool HasInternet
        {
            get { return _hasInternet; }
            set
            {
                _hasInternet = value;
                RaisePropertyChanged("HasInternet");

                CheckServers();
            }
        }

       



        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ILocalStoreService storage)
        {
            StorageService = storage;

            Servers = new ObservableCollection<RemoteServer>();

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Servers.Add(new RemoteServer());
                Servers.Add(new RemoteServer());
                Servers.Add(new RemoteServer());

            }
            else
            {
                // Code runs "for real"
            }


            LoadServers();

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            SetInternetStatus();

            // Code runs in Blend --> create design time data.



            AddServerCommand = new RelayCommand(AddServerCommandAction);
            RefreshCommand = new RelayCommand(RefreshCommandAction);


        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            SetInternetStatus();
        }

        private void SetInternetStatus()
        {
            var profiles = NetworkInformation.GetConnectionProfiles();
            var internetProfile = NetworkInformation.GetInternetConnectionProfile();

            bool hasIn = profiles.Any(s => s.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess) || (internetProfile != null && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);

            DispatcherHelper.InvokeAsync(() =>
           {
               HasInternet = hasIn;
           });

        }

        private async void LoadServers()
        {
            var get = await StorageService.GetAll();
            if (get == null)
                get = new List<RemoteServer>();

           this.Servers = new ObservableCollection<RemoteServer>(get);
        }

        public void CheckServers()
        {
            if (HasInternet)
            {
                foreach (var server in this.Servers)
                {
                    server.CheckOnline();
                }
            }
        }

        public void RefreshCommandAction()
        {
            CheckServers();
        }

        public async void AddServerCommandAction()
        {
            RemoteServer newServer = new RemoteServer();
            newServer.Id = Guid.NewGuid();
            newServer.Type = ServerType.Http;
            newServer.Url = "http://";

            AddServerPopupControl popup = new AddServerPopupControl();
            popup.DataContext = newServer;

            string result = await popup.ShowAsync();
            popup.ClosePopup();

            if (result == "ok")
            {
                this.Servers.Add(newServer);

                StorageService.Add(newServer);

                CheckServers();
            }
        }

        internal void DeleteServer(RemoteServer s)
        {
            this.Servers.Remove(s);

            StorageService.Delete(s);
        }

        internal void DeleteServers(List<RemoteServer> list)
        {
            foreach (var s in list)
            {
                this.Servers.Remove(s);

            }

            StorageService.DeleteMany(list);
        }
    }
}