using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MyFirstAthenticationAthorize.Security
{
    public class UserIdentity : IIdentity, IPrincipal
    {
        private readonly decimal m_Id;
        private readonly string m_Username;
        private readonly string[] m_Roles;
        private readonly string m_Name;
        private readonly bool _IsAuthenticated;
        private readonly decimal m_isActive;


        public string AuthenticationType { get { return "Forms"; } }
        public bool IsAuthenticated { get { return this._IsAuthenticated; } }
        public string Name { get { return this.m_Name; } }
        public string Username { get { return this.m_Username; } }
        public decimal Id { get { return this.m_Id; } }
        public string[] RoleName { get { return this.m_Roles; } }
        public int RoleCount { get { return this.m_Roles != null ? this.m_Roles.Length : 0; } }
        public decimal isActive { get { return this.m_isActive; } }


        public UserIdentity(decimal id, string username, string[] role, string Name, bool IsAuthenticated, decimal isActive)
        {
            this.m_Id = id;
            this.m_Username = username;
            this.m_Roles = role;
            this.m_Name = Name;
            this.m_isActive = isActive;

            _IsAuthenticated = IsAuthenticated;
        }


        public IIdentity Identity { get { return this; } }

        //public IIdentity Identity => throw new NotImplementedException();

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role)) return false;
            if (this.RoleCount <= 0) return false;
            return (this.m_Roles.Any(x => x == role));
        }
    }
}