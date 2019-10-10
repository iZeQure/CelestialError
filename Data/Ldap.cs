using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace DevNet.Data
{
    /// <summary>
    /// Get's instance of Ldap Base.
    /// </summary>
    public sealed class Ldap
    {
        private static Ldap instance = new Ldap();
        private static readonly object ldapLock = new object();

        #region Attributes        
        /// <summary>
        /// The domain name
        /// </summary>
        private string domainName = ConfigurationManager.AppSettings["LdapDomainName"];

        /// <summary>
        /// The directory
        /// </summary>
        private string directory = ConfigurationManager.AppSettings["LdapDirectory"];

        /// <summary>
        /// The authentication username
        /// </summary>
        private string authUsername = ConfigurationManager.AppSettings["LdapAuthUser"];

        /// <summary>
        /// The authentication password
        /// </summary>
        private string authPassword = ConfigurationManager.AppSettings["LdapAuthPass"];

        /// <summary>
        /// The principal context
        /// </summary>
        private PrincipalContext principalContext;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the name of the domain.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        public string DomainName
        {
            get { return domainName; }
            private set { domainName = value; }
        }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory
        {
            get { return directory; }
            private set { directory = value; }
        }

        /// <summary>
        /// Gets the authentication username.
        /// </summary>
        /// <value>
        /// The authentication username.
        /// </value>
        public string AuthUsername
        {
            get { return authUsername; }
            private set { authUsername = value; }
        }

        /// <summary>
        /// Gets the authentication password.
        /// </summary>
        /// <value>
        /// The authentication password.
        /// </value>
        public string AuthPassword
        {
            get { return authPassword; }
            private set { authPassword = value; }
        }

        /// <summary>
        /// Gets the get principal context.
        /// </summary>
        /// <value>
        /// The get principal context.
        /// </value>
        public PrincipalContext GetPrincipalContext
        {
            get { return principalContext; }
            private set { principalContext = value; }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Prevents a default instance of the <see cref="Ldap"/> class from being created.
        /// Get's current or new instance of Principal Context.
        /// </summary>
        private Ldap()
        {
            GetPrincipalContext = new PrincipalContext(ContextType.Domain, DomainName, Directory, AuthUsername, AuthPassword);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Ldap Instance
        {
            get
            {
                lock (ldapLock)
                {
                    if (instance == null)
                    {
                        instance = new Ldap();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ldap"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="authUsername">The authentication username.</param>
        /// <param name="authPassword">The authentication password.</param>
        //public Ldap(string domainName, string directory, string authUsername, string authPassword)
        //{
        //    DomainName = domainName;
        //    Directory = directory;
        //    AuthUsername = authUsername;
        //    AuthPassword = authPassword;
        //}
        #endregion
    }
}