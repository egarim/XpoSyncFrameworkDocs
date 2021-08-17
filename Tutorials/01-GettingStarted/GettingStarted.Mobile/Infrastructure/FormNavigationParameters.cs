using Prism.Common;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GettingStarted.Mobile.Infrastructure
{

    public class FormNavigationParameters : NavigationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormNavigationParameters"/> class.
        /// </summary>
        public FormNavigationParameters()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormNavigationParameters"/> class with a query string.
        /// </summary>
        /// <param name="query">The query string.</param>
        public FormNavigationParameters(string query) : base(query)
        {
           
        }
        public FormNavigationParameters(bool IsNewObject,string ObjectTypeName,object CurrentObjectKey)
        {
            this.Add(nameof(IsNewObject), IsNewObject);
            this.Add(nameof(ObjectTypeName), ObjectTypeName);
            this.Add(nameof(CurrentObjectKey), CurrentObjectKey);
        }
     
        public bool IsNewObject =>this.GetValue<bool>(nameof(IsNewObject)) ;
        public string ObjectTypeName => this.GetValue<string>(nameof(ObjectTypeName));
        public object CurrentObjectKey => this.GetValue<object>(nameof(CurrentObjectKey));

    }
}
