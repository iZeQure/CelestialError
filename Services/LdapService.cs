using DevNet.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace DevNet.Services
{
    public class LdapService
    {        
        private UserPrincipal userPrincipal;
        private PrincipalSearcher searchUser;
        string[] userInfo = new string[2];

        public string[] LdapConnection(string commonName)
        {
            Ldap ldap = Ldap.Instance;

            PrincipalContext ctx = ldap.GetPrincipalContext;
            userPrincipal = new UserPrincipal(ctx);
            searchUser = new PrincipalSearcher();

            try
            {
                if (commonName != "" && commonName.Length != 0)
                {
                    userPrincipal.EmailAddress = $"{commonName}@zbc.dk";

                    searchUser.QueryFilter = userPrincipal;
                    userPrincipal = (UserPrincipal)searchUser.FindOne();

                    userInfo[0] = userPrincipal.GivenName;
                    userInfo[1] = userPrincipal.Surname;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                userInfo[0] = "User not found.";
                return userInfo;
            }
            finally
            {
                //if (ctx != null)
                //{
                //    ctx.Dispose();
                //    Debug.WriteLine($"Disposed: Context");
                //}
                //if (userPrincipal != null)
                //{
                //    userPrincipal.Dispose();
                //    Debug.WriteLine($"Disposed: User Principal.");
                //}
                //if (searchUser != null)
                //{
                //    searchUser.Dispose();
                //    Debug.WriteLine($"Disposed: Search User.");
                //}
            }
            return userInfo;
        }
    }
}
