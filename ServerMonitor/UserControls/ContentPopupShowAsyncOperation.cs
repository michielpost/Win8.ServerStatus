using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ServerMonitor.UserControls
{
    public class ContentPopupShowAsyncOperation : IAsyncOperation<string>
    {
        private string _result = string.Empty;


        public AsyncOperationCompletedHandler<string> Completed
        {
            get;
            set;
        }


        internal void SetResult(string result)
        {
            _result = result;
        }


        public string GetResults()
        {
            return _result;
        }


        public void Cancel()
        {
            Completed(this, AsyncStatus.Canceled);
        }


        public void Close()
        {
            Completed(this, AsyncStatus.Completed);
        }


        public Exception ErrorCode
        {
            get;
            set;
        }


        public uint Id
        {
            get;
            set;
        }


        public AsyncStatus Status
        {
            get;
            set;
        }
    }

}
