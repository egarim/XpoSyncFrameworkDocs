using System;
using DevExpress.ExpressApp.Xpo;

namespace GettingStarted.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
