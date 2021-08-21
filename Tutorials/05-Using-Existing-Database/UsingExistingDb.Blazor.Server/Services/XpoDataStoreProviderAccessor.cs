using System;
using DevExpress.ExpressApp.Xpo;

namespace UsingExistingDb.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
