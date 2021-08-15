using System;
using DevExpress.ExpressApp.Xpo;

namespace DemoApp.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
