﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Threading.Tasks;

namespace DevNet.Data
{
    /// <summary>
    /// Get's instance of Ldap Base.
    /// </summary>
    public sealed class Ldap : IDisposable
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
            try
            {
                GetPrincipalContext = new PrincipalContext(ContextType.Domain, DomainName, Directory, ContextOptions.SimpleBind, AuthUsername, AuthPassword);
            }
            catch (PrincipalServerDownException ex)
            {
                Debug.WriteLine($"Server Down for {DomainName} ~ {ex.Message}");
                DomainName = ConfigurationManager.AppSettings["LdapIPAddress"];
                GetPrincipalContext = new PrincipalContext(ContextType.Domain, DomainName, Directory, ContextOptions.SimpleBind, AuthUsername, AuthPassword);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Exception: {ex.Message}");
            }
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

        public void Dispose()
        {
            try
            {
                GetPrincipalContext.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not dispose LDAP Object : {ex.Message}");
            }
        }
        #endregion
    }
}