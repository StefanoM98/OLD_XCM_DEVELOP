using API_XCM.Models.UNITEX;
using API_XCM.Models.XCM;
using API_XCM.Models.XCM.API;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API_XCM.Models.XCM.CRM;

namespace API_XCM.Code
{
    public class ApplicationUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long? RoleId { get; set; }
        public string CustomerGespeId { get; set; }
    }

    public class AuthHelper
    {
        static UNITEXEntities dbU = new UNITEXEntities();
        static Entities db = new Entities();

        public static bool Login(string username, string password)
        {

            var user = dbU.USERS.FirstOrDefault(x => x.EMAIL == username);
            if (user == null) return false;
            if (Crypt.Encrypt(password) == user.PASSWORD)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Logger
        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");
        internal static Logger _loggerAPI = LogManager.GetLogger("LogAPI");
        #endregion

        public static bool SignIn(string userName, string password)
        {
            //var user = db.Users.FirstOrDefault(x => x.User_Email == userName);
            XCM_CRMEntities entity = new XCM_CRMEntities();
            var user = entity.User.FirstOrDefault(x => x.User_Email == userName);

            if (user != null)
            {
                if (Hashing.VerifyPassword(password, user.User_HashPassword, user.User_Salt))
                {
                    if(HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session["Users"] = CreateApplicationUser(user);  // Mock user 
                    }
                    return true;
                }
                return false;

            }
            return false;
        }
        public static void SignOut()
        {
            HttpContext.Current.Session.Clear();
        }
        public static bool IsAuthenticated()
        {
            return GetLoggedInUserInfo() != null;
        }
        public static ApplicationUser GetLoggedInUserInfo()
        {
            return HttpContext.Current.Session["Users"] as ApplicationUser;
        }
        private static ApplicationUser CreateApplicationUser(Users src)
        {
            return new ApplicationUser
            {
                UserId = src.User_ID,
                UserName = src.User_Username,
                FirstName = src.User_FirstName,
                LastName = src.User_Surname,
                Email = src.User_Email,
                RoleId = src.FK_User_Role_ID,
                CustomerGespeId = src.User_CustomerId
            };
        }

        private static ApplicationUser CreateApplicationUser(API_XCM.Models.XCM.CRM.User src)
        {
            return new ApplicationUser
            {
                UserId = src.User_ID,
                UserName = src.User_Username,
                FirstName = src.User_FirstName,
                LastName = src.User_Surname,
                Email = src.User_Email,
                RoleId = src.FK_User_Role_ID,
                CustomerGespeId = src.FK_User_Customer_ID
            };
        }

        #region RoleAuthorization
        public static string GetCustomerGespeId()
        {
            if (HttpContext.Current.Session["Users"] != null)
            {
                var response = HttpContext.Current.Session["Users"] as ApplicationUser;
                return response.CustomerGespeId;
            }
            else
            {
                //TODO: gestire
                return "";
            }
        }

        public static bool IsAgent()
        {
            return GetLoggedInUserInfo() != null && GetLoggedInUserInfo().RoleId == 3;
        }

        public static bool IsAdmin()
        {
            return GetLoggedInUserInfo() != null && GetLoggedInUserInfo().RoleId == 2;
        }

        public static bool IsRoot()
        {
            return GetLoggedInUserInfo() != null && GetLoggedInUserInfo().RoleId == 1;
        }

        public static bool IsClient()
        {
            return GetLoggedInUserInfo() != null && GetLoggedInUserInfo().RoleId == 4;
        }

        public static bool IsCrm()
        {
            return GetLoggedInUserInfo() != null && GetLoggedInUserInfo().RoleId == 5;
        }
        #endregion

        public static bool Register(RegisterViewModel model)
        {
            Users userExsist = db.Users.FirstOrDefault(x => x.User_Email == model.Email);

            if (userExsist == null)
            {
                Users user = new Users();
                user.User_CreationDate = DateTime.Now;

                user.User_ID = Guid.NewGuid().ToString();

                HashSalt hashSalt = Hashing.GenerateSaltedHash(model.Password);
                user.User_HashPassword = hashSalt.Hash;
                user.User_Salt = hashSalt.Salt;

                user.User_LastModifiedDate = DateTime.Now;

                user.User_Email = model.Email;
                user.User_Username = model.UserName;
                user.User_FirstName = model.FirstName;
                user.User_Surname = model.LastName;

                user.User_CustomerId = model.Customer_ID;
                user.FK_User_Role_ID = model.Role_ID;

                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            return false;

        }

        public static List<CustomerViewModel> GetCustomers()
        {
            var customers = CRM.CRM.GetCustomers();

            var response = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                var newItem = new CustomerViewModel()
                {
                    Customer_ID = customer.Customer_id,
                    Customer_description = customer.Customer_description
                };

                response.Add(newItem);

            }
            return response;
        }

        public static List<RoleViewModel> GetRoles()
        {
            var roles = db.Roles.ToList();
            var response = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                var newItem = new RoleViewModel()
                {
                    Role_ID = role.Role_ID,
                    Role_Active = role.Role_Active,
                    Role_CreationDate = role.Role_CreationDate,
                    Role_LastModifiedDate = role.Role_LastModifiedDate,
                    Role_Name = role.Role_Name
                };

                response.Add(newItem);

            }
            return response;
        }

        public static ApplicationUser GetUser(string email)
        {
            //var user = db.Users.FirstOrDefault(x => x.User_Email == email);

            XCM_CRMEntities entity = new XCM_CRMEntities();
            var user = entity.User.FirstOrDefault(x => x.User_Email == email);

            if (user != null)
            {
                var newUser = new ApplicationUser()
                {
                    CustomerGespeId = user.FK_User_Customer_ID,
                    Email = user.User_Email,
                    FirstName = user.User_FirstName,
                    LastName = user.User_Surname,
                    RoleId = user.FK_User_Role_ID,
                    UserId = user.User_ID, 
                    UserName = user.User_Username
                };

                return newUser;
            }

            return new ApplicationUser();
        }

        public static string GetRoleName(long? roleId)
        {
            var roleName = db.Roles.FirstOrDefault(x => x.Role_ID == roleId);
            
            return roleName != null ? roleName.Role_Name : "";  
        }

    }
}
