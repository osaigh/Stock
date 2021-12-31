using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Stock.WpfClient.Common.Models
{
    public class User : BindableBase
    {
        #region Properties
        private string firstName = string.Empty;
        public string FirstName 
        {
            get { return firstName; }
            set
            {
                SetProperty(ref this.firstName, value);
            }
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get { return lastName; }
            set
            {
                SetProperty(ref this.lastName, value);
            }
        }

        private string userName = string.Empty;
        public string UserName
        {
            get { return userName; }
            set
            {
                SetProperty(ref this.userName, value);
            }
        }

        public string AccessToken { get; set; }
        #endregion
    }
}
