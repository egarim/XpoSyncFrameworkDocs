using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
namespace XafXamarinCustomAuth.Module.BusinessObjects
{
    [DomainComponent, Serializable]
    [System.ComponentModel.DisplayName("Log In")]
    public class CustomLogonParameters : INotifyPropertyChanged, ISerializable
    {
        private Company company;
        private Employee employee;
        private string password;

        [ImmediatePostData]
        public Company Company
        {
            get { return company; }
            set
            {
                if (value == company) return;
                company = value;
                if (Employee?.Company != company)
                {
                    Employee = null;
                }
                OnPropertyChanged(nameof(Company));
            }
        }
        [DataSourceProperty("Company.Employees"), ImmediatePostData]
        public Employee Employee
        {
            get { return employee; }
            set
            {
                if (value == employee) return;
                employee = value;
                Company = employee?.Company;
                UserName = employee?.UserName;
                OnPropertyChanged(nameof(Employee));
            }
        }
        [Browsable(false)]
        public String UserName { get; set; }
        [PasswordPropertyText(true)]
        public string Password
        {
            get { return password; }
            set
            {
                if (password == value) return;
                password = value;
            }
        }
        public CustomLogonParameters() { }
        // ISerializable 
        public CustomLogonParameters(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount > 0)
            {
                UserName = info.GetString("UserName");
                Password = info.GetString("Password");
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [System.Security.SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserName", UserName);
            info.AddValue("Password", Password);
        }
        public void RefreshPersistentObjects(IObjectSpace objectSpace)
        {
            Employee = (UserName == null) ? null : objectSpace.FirstOrDefault<Employee>(e => e.UserName == UserName);
        }
    }
}
