using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;
//using System.ComponentModel.DataAnnotations;

namespace IORPromoteTool.Helper
{
    public class LDAPProfileInfo
    {
        public string LdapServer { get; set; }
        public int Port { get; set; }
        public bool IsAnonymous { get; set; }
        public string LdapUser { get; set; }
        public string LdapPassword { get; set; }
        public string BaseDN { get; set; }
        public bool IsSSL { get; set; }

        public LDAPProfileInfo()
        {
            Port = 389;
        }
    }

    public class LDAPHelper : IDisposable
    {
        LDAPProfileInfo profile;
        LdapConnection connection;

        const string EmailFilter = "(mail={0})";
        const string UserLogonNameFilter = "(SAMAccountName={0})";
        const string DNFilter = "(distinguishedName={0})";

        public LDAPHelper(LDAPProfileInfo profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");
            this.profile = profile;
        }

        public bool Authenticate(string dn, string password)
        {
            using (PrincipalContext context = new PrincipalContext(
                ContextType.Domain,
                profile.LdapServer,
                profile.BaseDN))
            {
                ContextOptions options = ContextOptions.SimpleBind;
                if (profile.IsSSL)
                    options |= ContextOptions.SecureSocketLayer;
                return context.ValidateCredentials(dn, password, options);
            }
        }

        public void Connect()
        {
            Search(string.Format(DNFilter, profile.BaseDN));
        }

        public string GetDistinguishNameByUserLogonName(string username)
        {
            foreach (SearchResultEntry entry in Search(string.Format(UserLogonNameFilter, username)))
                return entry.DistinguishedName;
            return string.Empty;
        }

        public Dictionary<string, string> GetUserInfoByUserLogonName(string username)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            foreach (SearchResultEntry entry in Search(string.Format(UserLogonNameFilter, username)))
            {
                string email = string.Empty;
                if (entry.Attributes.Contains("Mail") && entry.Attributes["Mail"].Count > 0)
                {
                    email = entry.Attributes["Mail"][0].ToString();
                }
                info.Add("Email", email);
                string name = string.Empty;
                if (entry.Attributes.Contains("DisplayName") && entry.Attributes["DisplayName"].Count > 0)
                {
                    name = entry.Attributes["DisplayName"][0].ToString();
                }
                info.Add("Name", name);
                break;
            }
            return info;
        }

        SearchResultEntryCollection Search(string ldapFilter, params string[] attributes)
        {

            LdapConnection conn = GetConnection();
            SearchRequest req = new SearchRequest(profile.BaseDN, ldapFilter, System.DirectoryServices.Protocols.SearchScope.Subtree, attributes);
            SearchResponse response = conn.SendRequest(req, TimeSpan.MaxValue) as SearchResponse;
            if (response.ResultCode != ResultCode.Success)
                throw new Exception(response.ErrorMessage);
            return response.Entries;
        }

        LdapConnection GetConnection()
        {
            if (connection == null)
            {
                LdapDirectoryIdentifier ldapDI = null;
                if (profile.Port > 0)
                    ldapDI = new LdapDirectoryIdentifier(profile.LdapServer, profile.Port);
                else
                    ldapDI = new LdapDirectoryIdentifier(profile.LdapServer);
                if (!profile.IsAnonymous)
                {
                    string[] domainAndUser = profile.LdapUser.Split('\\');
                    NetworkCredential credential = null;
                    if (domainAndUser.Length > 1)
                        credential = new NetworkCredential(domainAndUser[1], profile.LdapPassword ?? string.Empty, domainAndUser[0]);
                    else
                        credential = new NetworkCredential(domainAndUser[0], profile.LdapPassword ?? string.Empty, string.Empty);
                    connection = new LdapConnection(ldapDI, credential, AuthType.Basic);
                }
                else
                {
                    connection = new LdapConnection(ldapDI);
                    connection.AuthType = AuthType.Anonymous;
                }
                connection.SessionOptions.ProtocolVersion = 3;
                connection.SessionOptions.SecureSocketLayer = profile.IsSSL;
                connection.SessionOptions.VerifyServerCertificate = ((conn, certificate) => true);
            }
            return connection;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

        #endregion
    }
}