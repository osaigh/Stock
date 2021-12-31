using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.WpfClient.Common.Models
{
    public class SignInResult
    {
        /// <summary>
        /// The user's info
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// If an Error occured during the sign in process
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Errors encountered during the sign in process
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
