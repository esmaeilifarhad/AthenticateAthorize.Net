using MyFirstAthenticationAthorize.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MyFirstAthenticationAthorize.Security
{
    public class AuthenticationModules : IHttpModule
    {
        public HttpContextBase HttpContext { get; set; }
        public HttpRequestBase Request { get; set; }
        public RouteData RouteData { get; set; }
        private HttpApplication m_Context;


        public static bool Login(string username, string password, int UserID = 0)
        {
            
            if ((((string.IsNullOrEmpty(username)) || (string.IsNullOrEmpty(password))) && UserID <= 0)) return false;

            System.Web.Security.FormsAuthentication.Initialize();

            Models.Entity.DataBase db = new Models.Entity.DataBase();
            var lstUserRoles = (from U in db.users join UR in db.Users_Roles
                                on U.ID equals UR.Users.ID
                                join R in db.Roles on UR.Roles.ID equals R.ID
                                where U.USERNAME==username
                                select new {U.USERNAME,R.TITLE }).ToList();
           // var userContext=null;
            USER dbuser = db.users.SingleOrDefault(q=>q.USERNAME==username);

            
           // dbuser = userContext.Login(username.ToLower(), password);

            if (dbuser == null) return false;
            if (dbuser.ROLE_ID <= 0) return false;

            //decimal roleid = dbuser.ROLE_ID;
            //string name = dbuser.NAME;

            List<string> rolename = new List<string>() ;
            foreach (var item in lstUserRoles)
            {
                rolename.Add(item.TITLE);
            }
            



            var ticket = new System.Web.Security.FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                true,
                Serialize(dbuser.ID, dbuser.USERNAME, rolename.ToArray(), dbuser.NAME, (int)dbuser.IS_ACTIVE),
                System.Web.Security.FormsAuthentication.FormsCookiePath);

            string hash = System.Web.Security.FormsAuthentication.Encrypt(ticket);

            var cookie = new System.Web.HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, hash);
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

            var context = System.Web.HttpContext.Current;
            context.Response.Cookies.Add(cookie);

            context.User = new UserIdentity(dbuser.ID, dbuser.USERNAME, rolename.ToArray(), dbuser.NAME, true, dbuser.IS_ACTIVE);
            //--------------------------new 
            // System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(context.User.Identity, roles);
            
            return true;
        }

        public void Init(HttpApplication context)
        {
            this.m_Context = context;
            this.m_Context.AuthenticateRequest += this.AuthenticateRequest;
        }

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            var context = System.Web.HttpContext.Current;

            if ((context.User == null) || (!context.User.Identity.IsAuthenticated))
            {
                return;
            }

            var identity = context.User.Identity as System.Web.Security.FormsIdentity;
            if (identity == null)
            {
                return;
            }

            decimal id, isactive;
            string username, Name;
            string[] roles;

            if (!Deserialize(identity.Ticket.UserData, out id, out username, out roles, out Name, out isactive)) return;

            context.User = new UserIdentity(id, username, roles, Name, true, isactive);
        }

        public static void Logoff()
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
            catch (Exception)
            {
                //HttpContext.Current.RewritePath("Home/LoginView");
            }

        }

        public static void Expire()
        {
            try
            {
                var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                var urlHelper = new UrlHelper(context);
                var url = urlHelper.Action("Index", "Home");
                System.Web.HttpContext.Current.Response.Redirect(url);
            }
            catch (Exception)
            {

            }

        }

        public static UserIdentity CurrentUser
        {
            get
            {
                try
                {
                    var user = System.Web.HttpContext.Current.User;
                    if (user != null) return user.Identity as UserIdentity;
                }
                catch { }
                return null;
            }
        }
        public static decimal CurrentUserId
        {
            get
            {
                var user = CurrentUser;
                return user != null ? user.Id : 0;
            }
        }

        #region
        private const char SEPARATOR = '>';

        private static string Serialize(decimal id, string username, string[] roles, string Name, decimal isactive)
        {
            var ret = new System.Text.StringBuilder();
            ret.Append(id);
            ret.Append(SEPARATOR);
            ret.Append(username);
            ret.Append(SEPARATOR);
            ret.Append(Name);
            ret.Append(SEPARATOR);
            ret.Append(isactive);

            if (roles.Count() > 0)
            {
                foreach (var r in roles)
                {
                    ret.Append(SEPARATOR);
                    ret.Append(r);
                }

            }


            return ret.ToString();


        }
        private bool Deserialize(string value, out decimal id, out string username, out string[] role, out string Name, out decimal isactive)
        {
            id = 0;
            username = null;
            role = null;
            Name = null;
            isactive = 0;


            if (string.IsNullOrEmpty(value)) return false;

            string[] arr = value.Split(SEPARATOR);

            if ((arr == null) || (arr.Length < 4)) return false;

            //read 'userid'
            if (!decimal.TryParse(arr[0], out id)) return false;
            if (id <= 0) return false;

            //read 'username'
            username = arr[1];
            if (string.IsNullOrEmpty(username)) return false;

            //read 'Full Name ( FirstName + LastName )'
            Name = arr[2];
            if (string.IsNullOrEmpty(Name)) return false;

            //read status ID
            isactive = Convert.ToInt32(arr[3]);
            //FillRoles
            List<string> roles = new List<string>();
            if (arr.Length >= 4)
            {
                for (int i = 4; i < arr.Length; i++)
                {

                    if (!string.IsNullOrEmpty(arr[i]))
                        roles.Add(arr[i]);
                }
                role = roles.ToArray();
            }

            return true;
        }
        #endregion
        public void Dispose()
        {
            if (this.m_Context != null)
            {
                try
                {
                    this.m_Context.AuthenticateRequest -= this.AuthenticateRequest;
                }
                catch
                {

                }
            }
            this.m_Context = null;

            System.GC.SuppressFinalize(this);
        }
    }
}