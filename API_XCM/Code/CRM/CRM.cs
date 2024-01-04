using API_XCM.Models.XCM;
using API_XCM.Models.XCM.CRM.SyncroDB_CRM;
using API_XCM.Models.XCM.CRM.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using API_XCM.Models.XCM.CRM;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using NLog;

namespace API_XCM.Code.CRM
{
    public class CRM
    {
        #region Logger
        internal static Logger _logger = LogManager.GetLogger("loggerCode");
        #endregion

        #region Customer
        public static List<CustomerJsonModel> GetCustomers()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            try
            {
                var customers = entity.Customer.ToList();
                return JsonInterpreter.ListCustomerJsonInterpreter(customers);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetCustomers");
            }

            return new List<CustomerJsonModel>();
        }
        public static bool UpdateCustomer(CustomerJsonModel customer)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            try
            {
                Customer cs = entity.Customer.FirstOrDefault(x => x.Customer_SessionID == customer.Customer_SessionID);

                if (cs != null)
                {
                    if (!string.IsNullOrEmpty(customer.Customer_IsEnableCRM.ToString()) && cs.Customer_IsEnableCRM != customer.Customer_IsEnableCRM)
                    {
                        cs.Customer_IsEnableCRM = customer.Customer_IsEnableCRM;
                    }
                    if (customer.Customer_Logo != null && cs.Customer_Logo != customer.Customer_Logo)
                    {
                        cs.Customer_Logo = customer.Customer_Logo;
                    }

                    var customerAuthorization = entity.Customer_Authorization.FirstOrDefault(x => x.Customer_Id == cs.Customer_SessionID);

                    if (customerAuthorization == null)
                    {
                        if (cs.Customer_Authorization == null)
                        {
                            Customer_Authorization ca = new Customer_Authorization()
                            {
                                Customer_Id = cs.Customer_SessionID,
                                //Orders_Active = Convert.ToInt32(customer.Customer_Authorization.Orders_Active),
                                //Warehouse_Active = Convert.ToInt32(customer.Customer_Authorization.Warehouse_Active),
                                //Tracking_Active = Convert.ToInt32(customer.Customer_Authorization.Tracking_Active),
                                LastModifiedDate = DateTime.Now,
                                ExpireDateOrders = customer.Customer_Authorization.ExpireDateOrders == DateTime.MinValue ? DateTime.Now : customer.Customer_Authorization.ExpireDateOrders,
                                ExpireDateTracking = customer.Customer_Authorization.ExpireDateTracking == DateTime.MinValue ? DateTime.Now : customer.Customer_Authorization.ExpireDateTracking,
                                ExpireDateWarehouse = customer.Customer_Authorization.ExpireDateWarehouse == DateTime.MinValue ? DateTime.Now : customer.Customer_Authorization.ExpireDateWarehouse

                            };
                            entity.Customer_Authorization.Add(ca);
                            entity.SaveChanges();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.Orders_Active.ToString()) && customerAuthorization.Orders_Active != Convert.ToInt32(customer.Customer_Authorization.Orders_Active))
                        {
                            customerAuthorization.Orders_Active = Convert.ToInt32(customer.Customer_Authorization.Orders_Active);
                        }
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.ExpireDateOrders.ToString()) && customerAuthorization.ExpireDateOrders != customer.Customer_Authorization.ExpireDateOrders)
                        {
                            customerAuthorization.ExpireDateOrders = customer.Customer_Authorization.ExpireDateOrders;
                        }
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.Tracking_Active.ToString()) && customerAuthorization.Tracking_Active != Convert.ToInt32(customer.Customer_Authorization.Tracking_Active))
                        {
                            customerAuthorization.Tracking_Active = Convert.ToInt32(customer.Customer_Authorization.Tracking_Active);
                        }
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.ExpireDateTracking.ToString()) && customerAuthorization.ExpireDateTracking != customer.Customer_Authorization.ExpireDateTracking)
                        {
                            customerAuthorization.ExpireDateTracking = customer.Customer_Authorization.ExpireDateTracking;
                        }
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.Warehouse_Active.ToString()) && customerAuthorization.Warehouse_Active != Convert.ToInt32(customer.Customer_Authorization.Warehouse_Active))
                        {
                            customerAuthorization.Warehouse_Active = Convert.ToInt32(customer.Customer_Authorization.Warehouse_Active);
                        }
                        if (!string.IsNullOrEmpty(customer.Customer_Authorization.ExpireDateWarehouse.ToString()) && customerAuthorization.ExpireDateWarehouse != customer.Customer_Authorization.ExpireDateWarehouse)
                        {
                            customerAuthorization.ExpireDateWarehouse = customer.Customer_Authorization.ExpireDateWarehouse;
                        }

                    }

                    entity.SaveChanges();
                    return Response.True;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateCustomers - {customer.Customer_SessionID}");
            }
            return Response.False;
        }
        public static string GetCustomerGespeId(string csId)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            string response = "";
            try
            {
                var customer = entity.Customer.Where(x => x.Customer_SessionID == csId).FirstOrDefault();
                if (customer != null)
                {
                    response = customer.Customer_id;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetCustomerGespeId - {csId}");
            }
            return response;

        }
        public static CustomerAuthorizationJsonModel GetCustomerAuthorization(string csId)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            CustomerAuthorizationJsonModel response = new CustomerAuthorizationJsonModel();
            try
            {
                var customerAuthorization = entity.Customer_Authorization.FirstOrDefault(x => x.Customer_Id == csId);
                if (customerAuthorization != null)
                {
                    response = JsonInterpreter.CustomerAuthorizationJsonInterpreter(customerAuthorization);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetCustomerAuthorization - {csId}");
            }
            return response;
        }
        public static string GetCustomerSessionIdByGespeId(string customerGespeId)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            string response = "";
            try
            {
                var exist = entity.Customer.FirstOrDefault(x => x.Customer_id == customerGespeId);
                if (exist != null)
                {
                    response = exist.Customer_SessionID;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetCustomerSessionIdByGespeId - {customerGespeId}");
                response = ex.Message;
            }
            return response;
        }
        #endregion

        #region Users
        public static List<UserJsonModel> GetUsers()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            List<User> usersDb = new List<User>();
            List<UserJsonModel> response = new List<UserJsonModel>();
            try
            {
                usersDb = entity.User.ToList();
                response = JsonInterpreter.ListUserJsonInterepreter(usersDb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetUsers");
            }
            return response;
        }
        public static bool UpdateUser(UserJsonModel user)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                User userExist = entity.User.Where(x => x.User_ID == user.User_ID).FirstOrDefault();

                if (userExist != null)
                {
                    if (!string.IsNullOrEmpty(user.User_Username) && userExist.User_Username != user.User_Username)
                    {
                        userExist.User_Username = user.User_Username;
                    }
                    if (!string.IsNullOrEmpty(user.User_Email) && userExist.User_Email != user.User_Email)
                    {
                        userExist.User_Email = user.User_Email;
                    }
                    if (!string.IsNullOrEmpty(user.User_FirstName) && userExist.User_FirstName != user.User_FirstName)
                    {
                        userExist.User_FirstName = user.User_FirstName;
                    }
                    if (!string.IsNullOrEmpty(user.User_Surname) && userExist.User_Surname != user.User_Surname)
                    {
                        userExist.User_Surname = user.User_Surname;
                    }
                    if (!string.IsNullOrEmpty(user.FK_User_Customer_ID) && userExist.FK_User_Customer_ID != user.FK_User_Customer_ID)
                    {
                        userExist.FK_User_Customer_ID = user.FK_User_Customer_ID;
                    }
                    if (user.FK_User_Role_ID != 0 && userExist.FK_User_Role_ID != user.FK_User_Role_ID)
                    {
                        userExist.FK_User_Role_ID = user.FK_User_Role_ID;
                    }
                    if (!string.IsNullOrEmpty(user.User_Active.ToString()) && userExist.User_Active != user.User_Active)
                    {
                        userExist.User_Active = user.User_Active;
                    }

                    if (!string.IsNullOrEmpty(user.User_PasswordRecovered.ToString()) && userExist.User_PasswordRecovered != user.User_PasswordRecovered)
                    {
                        userExist.User_PasswordRecovered = user.User_PasswordRecovered;
                    }

                    if (!string.IsNullOrEmpty(user.User_HashPassword))
                    {
                        userExist.User_HashPassword = user.User_HashPassword;
                        userExist.User_Salt = user.User_Salt;
                    }

                    userExist.User_LastModifiedDate = DateTime.Now;

                    entity.SaveChanges();
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateUser - {user.User_ID}");
            }
            return Response.False;

        }
        public static bool InsertUser(User user)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            try
            {
                User userExsist = entity.User.Where(x => x.User_ID == user.User_ID).FirstOrDefault();

                if (userExsist == null)
                {
                    user.User_CreationDate = DateTime.Now;
                    user.User_LastModifiedDate = DateTime.Now;
                    //TODO: rimuovere username
                    user.User_Username = user.User_Email;
                    entity.User.Add(user);
                    entity.SaveChanges();
                    return Response.True;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InsertUser - {user.User_ID}");
            }
            return Response.False;
        }
        public static UserJsonModel GetUserByEmail(string email)
        {
            UserJsonModel response = new UserJsonModel();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var userDb = entity.User.Where(x => x.User_Email == email).FirstOrDefault();
                if (userDb != null)
                {
                    response = JsonInterpreter.UserJsonInterepreter(userDb);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetUserByEmail - {email}");
            }
            return response;

        }
        #endregion

        #region Agents
        public static bool InsertAgent(AgentJsonModel agent)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                var userExist = entity.User.FirstOrDefault(x => x.User_ID == agent.User_ID);

                if (userExist == null)
                {
                    var user = new User()
                    {
                        FK_User_Customer_ID = agent.FK_User_Customer_ID,
                        FK_User_Role_ID = agent.FK_User_Role_ID,
                        User_Active = agent.User_Active,
                        User_CreationDate = agent.User_CreationDate,
                        User_Email = agent.User_Email,
                        User_FirstName = agent.User_FirstName,
                        User_HashPassword = agent.User_HashPassword,
                        User_LastModifiedDate = agent.User_LastModifiedDate,
                        User_PasswordRecovered = agent.User_PasswordRecovered,
                        User_Salt = agent.User_Salt,
                        User_Surname = agent.User_Surname,
                        User_TempPassword = agent.User_Surname,
                        User_Username = agent.User_Username,
                        User_ID = agent.User_ID
                    };

                    var agentAuth = new AgentAuthorization()
                    {
                        AgentAuthorization_CreationDate = DateTime.Now,
                        AgentAuthorization_LastModifiedDate = DateTime.Now,
                        AgentAuthorization_OrderConfirmation = agent.User_AgentAuth.AgentAuthorization_OrderConfirmation,
                        FK_AgentAuthorization_User_ID = agent.User_ID,
                        //TODO: aggiungere User ID che ha modificato
                        AgentAuthorization_LastModifiedUserID = "999"
                    };

                    entity.User.Add(user);
                    entity.AgentAuthorization.Add(agentAuth);
                    entity.SaveChanges();
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InsertAgent - {agent.User_ID}");
            }
            return Response.False;
        }
        public static bool UpdateAgent(AgentJsonModel agent)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var userExist = entity.User.FirstOrDefault(x => x.User_ID == agent.User_ID);

                if (userExist != null)
                {
                    if (!string.IsNullOrEmpty(agent.User_Username) && userExist.User_Username != agent.User_Username)
                    {
                        userExist.User_Username = agent.User_Username;
                    }
                    if (!string.IsNullOrEmpty(agent.User_Email) && userExist.User_Email != agent.User_Email)
                    {
                        userExist.User_Email = agent.User_Email;
                    }
                    if (!string.IsNullOrEmpty(agent.User_FirstName) && userExist.User_FirstName != agent.User_FirstName)
                    {
                        userExist.User_FirstName = agent.User_FirstName;
                    }
                    if (!string.IsNullOrEmpty(agent.User_Surname) && userExist.User_Surname != agent.User_Surname)
                    {
                        userExist.User_Surname = agent.User_Surname;
                    }
                    if (!string.IsNullOrEmpty(agent.FK_User_Customer_ID) && userExist.FK_User_Customer_ID != agent.FK_User_Customer_ID)
                    {
                        userExist.FK_User_Customer_ID = agent.FK_User_Customer_ID;
                    }
                    if (agent.FK_User_Role_ID != 0 && userExist.FK_User_Role_ID != agent.FK_User_Role_ID)
                    {
                        userExist.FK_User_Role_ID = agent.FK_User_Role_ID;
                    }
                    if (!string.IsNullOrEmpty(agent.User_Active.ToString()) && userExist.User_Active != agent.User_Active)
                    {
                        userExist.User_Active = agent.User_Active;
                    }

                    if (!string.IsNullOrEmpty(agent.User_PasswordRecovered.ToString()) && userExist.User_PasswordRecovered != agent.User_PasswordRecovered)
                    {
                        userExist.User_PasswordRecovered = agent.User_PasswordRecovered;
                    }

                    if (!string.IsNullOrEmpty(agent.User_HashPassword))
                    {
                        userExist.User_HashPassword = agent.User_HashPassword;
                        userExist.User_Salt = agent.User_Salt;
                    }

                    userExist.User_LastModifiedDate = DateTime.Now;

                    entity.SaveChanges();

                    var agentAuth = entity.AgentAuthorization.FirstOrDefault(x => x.FK_AgentAuthorization_User_ID == agent.User_ID);
                    if (agentAuth != null)
                    {
                        if (agent.User_AgentAuth.AgentAuthorization_OrderConfirmation != agentAuth.AgentAuthorization_OrderConfirmation)
                        {
                            agentAuth.AgentAuthorization_OrderConfirmation = agent.User_AgentAuth.AgentAuthorization_OrderConfirmation;
                        }

                        agentAuth.AgentAuthorization_LastModifiedDate = DateTime.Now;
                        //TODO: USER ID
                        //agentAuth.AgentAuthorization_LastModifiedUserID = "999";
                        entity.SaveChanges();
                    }
                    else
                    {
                        var newAgentAuth = new AgentAuthorization()
                        {
                            AgentAuthorization_CreationDate = DateTime.Now,
                            AgentAuthorization_LastModifiedDate = DateTime.Now,
                            //TODO: USER ID
                            AgentAuthorization_LastModifiedUserID = "999",
                            FK_AgentAuthorization_User_ID = agent.User_ID,
                            AgentAuthorization_OrderConfirmation = agent.User_AgentAuth.AgentAuthorization_OrderConfirmation
                        };
                        entity.AgentAuthorization.Add(newAgentAuth);
                        entity.SaveChanges();
                    }
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateAgent - {agent.User_ID}");
            }
            return Response.False;
        }
        public static List<AgentJsonModel> GetAgents(string csId)
        {
            List<AgentJsonModel> agents = new List<AgentJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                List<User> usersDb = new List<User>();
                usersDb = entity.User.Where(x => x.FK_User_Role_ID == 3 && x.FK_User_Customer_ID == csId).ToList();
                agents = JsonInterpreter.ConvertListUserAgentToJsonModel(usersDb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetAgents - {csId}");
            }
            return agents;

        }
        public static List<AgentJsonModel> GetAgents()
        {
            List<AgentJsonModel> agents = new List<AgentJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                List<User> usersDb = new List<User>();
                usersDb = entity.User.Where(x => x.FK_User_Role_ID == 3).ToList();
                agents = JsonInterpreter.ConvertListUserAgentToJsonModel(usersDb);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetAgents");
            }
            return agents;

        }
        public static AgentAuthorizationJsonModel GetAgentAuthorization(string userID)
        {
            AgentAuthorizationJsonModel response = new AgentAuthorizationJsonModel();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                var exist = entity.AgentAuthorization.FirstOrDefault(x => x.FK_AgentAuthorization_User_ID == userID);
                if (exist != null)
                {
                    response = JsonInterpreter.ConvertAgentAuthorizationToJsonModel(exist);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetAgentAuthorization - {userID}");
            }
            return response;
        }
        public static bool InsertAgentAuthorization(AgentAuthorizationJsonModel agentAuthorization)
        {

            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                User userExsist = entity.User.FirstOrDefault(x => x.User_ID == agentAuthorization.FK_AgentAuthorization_User_ID);

                if (userExsist != null)
                {
                    agentAuthorization.AgentAuthorization_CreationDate = DateTime.Now;
                    agentAuthorization.AgentAuthorization_LastModifiedDate = DateTime.Now;
                    //TODO: aggiugnere user id di chi ha aggiunto

                    entity.AgentAuthorization.Add(JsonInterpreter.ConvertAgentAuthorizationToEntityModel(agentAuthorization));
                    entity.SaveChanges();
                    return Response.True;

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InsertAgentAuthorization - {agentAuthorization.AgentAuthorization_ID}");
            }
            return Response.False;

        }
        public static bool UpdateAgentAuthorization(AgentAuthorizationJsonModel agentAuthorization)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                AgentAuthorization authorization = entity.AgentAuthorization.FirstOrDefault(x => x.FK_AgentAuthorization_User_ID == agentAuthorization.FK_AgentAuthorization_User_ID);

                if (authorization != null)
                {
                    if (agentAuthorization.AgentAuthorization_OrderConfirmation != authorization.AgentAuthorization_OrderConfirmation)
                    {
                        authorization.AgentAuthorization_OrderConfirmation = agentAuthorization.AgentAuthorization_OrderConfirmation;
                    }
                    //TODO: aggiugnere user id di chi ha aggiunto
                    authorization.AgentAuthorization_LastModifiedDate = DateTime.Now;
                    entity.SaveChanges();
                    return Response.True;
                }
                else
                {
                    var userExist = entity.User.FirstOrDefault(x => x.User_ID == agentAuthorization.FK_AgentAuthorization_User_ID);
                    if (userExist != null)
                    {
                        var newAuth = new AgentAuthorization()
                        {
                            AgentAuthorization_CreationDate = DateTime.Now,
                            FK_AgentAuthorization_User_ID = agentAuthorization.FK_AgentAuthorization_User_ID,
                            AgentAuthorization_OrderConfirmation = agentAuthorization.AgentAuthorization_OrderConfirmation,
                            AgentAuthorization_LastModifiedDate = DateTime.Now,
                        };
                        entity.AgentAuthorization.Add(newAuth);
                        entity.SaveChanges();
                        return Response.True;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateAgentAuthorization - {agentAuthorization.AgentAuthorization_ID}");
            }
            return Response.False;

        }
        public static string GetAgentName(string agentId)
        {
            string agentName = "";
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var agent = entity.User.FirstOrDefault(x => x.User_ID == agentId);
                if (agent != null)
                {
                    agentName = $"{agent.User_FirstName} {agent.User_Surname}";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetAgentName - {agentId}");
            }
            return agentName;
        }
        #endregion

        #region Roles
        public static List<RoleJsonModel> GetRoles()
        {
            List<RoleJsonModel> response = new List<RoleJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var roles = entity.Role.ToList();
                response = JsonInterpreter.ListRoleJsonInterpreter(roles);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetRoles");
            }
            return response;
        }
        #endregion

        #region Location
        public static void InitClientLocationTable()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            GnXcmEntities gnXcmProd = new GnXcmEntities();
            var locations = gnXcmProd.TmsLocationDetail.Where(x => x.AnaType == 1).ToList();

            List<Customer_Location> list = new List<Customer_Location>();
            List<Customer_Location> csLocations = new List<Customer_Location>();
            var customerSessionId = "";
            var clientId = "";
            foreach (var location in locations)
            {

                if (!string.IsNullOrEmpty(location.AnaID))
                {
                    customerSessionId = GetCustomerSessionIdByGespeId(location.AnaID);
                    if (string.IsNullOrEmpty(customerSessionId))
                        continue;
                }
                if (!string.IsNullOrEmpty(location.Name))
                {
                    clientId = GetClientUserId(location.Name);
                    if (string.IsNullOrEmpty(clientId))
                        continue;
                }
                var newLocation = new Customer_Location()
                {
                    FK_Location_Customer_ID = customerSessionId,
                    Location_Address = location.Address,
                    Location_Country = location.Country,
                    Location_CreationDate = DateTime.Now,
                    Location_District = location.District,
                    Location_IsDefault = location.IsDefault,
                    Location_Location = location.Location,
                    Location_LastModifiedDate = DateTime.Now,
                    Location_LastModifiedUserId = "999",
                    Location_Name = location.Name,
                    Location_Region = location.Region,
                    Location_ZipCode = location.ZipCode,
                    Location_GespeLocationId = location.LocationID,
                    //FK_Location_Client_ID = clientId,
                    Location_IsActive = true

                };
                csLocations.Add(newLocation);
            }

            entity.Customer_Location.AddRange(csLocations);
            entity.SaveChanges();

        }
        public static List<LocationJsonModel> GetLocationsByCustomerId(string csId)
        {
            List<LocationJsonModel> response = new List<LocationJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                response = JsonInterpreter.ConvertCustomerLocationToLocationJsonModel(entity.Customer_Location.Where(x => x.FK_Location_Customer_ID == csId).ToList());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetLocationsByCustomerId");
            }
            return response;

        }
        public static List<LocationJsonModel> GetLocations()
        {
            List<LocationJsonModel> list = new List<LocationJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                //SyncronizeNewLocationOnXcmDb();
                list = JsonInterpreter.ConvertCustomerLocationToLocationJsonModel(entity.Customer_Location.ToList());

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetLocations");
            }
            return list;

        }
        public static bool UpdateLocation(LocationJsonModel location)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            try
            {
                var locationExist = entity.Customer_Location.First(x => x.Location_Id == location.Location_Id);
                if (locationExist != null)
                {
                    //locationExist = JsonInterpreter.ConvertLocationJsonModelToCustomerLocation(location);

                    if (!string.IsNullOrEmpty(location.Location_Name) && locationExist.Location_Name != location.Location_Name)
                    {
                        locationExist.Location_Name = location.Location_Name;
                    }
                    if (!string.IsNullOrEmpty(location.Location_Location) && locationExist.Location_Location != location.Location_Location)
                    {
                        locationExist.Location_Location = location.Location_Location;
                    }
                    if (!string.IsNullOrEmpty(location.Location_Address) && locationExist.Location_Address != location.Location_Address)
                    {
                        locationExist.Location_Address = location.Location_Address;
                    }
                    if (!string.IsNullOrEmpty(location.Location_District) && locationExist.Location_District != location.Location_District)
                    {
                        locationExist.Location_District = location.Location_District;
                    }
                    if (!string.IsNullOrEmpty(location.Location_ZipCode) && locationExist.Location_ZipCode != location.Location_ZipCode)
                    {
                        locationExist.Location_ZipCode = location.Location_ZipCode;
                    }
                    if (!string.IsNullOrEmpty(location.Location_Region) && locationExist.Location_Region != location.Location_Region)
                    {
                        locationExist.Location_Region = location.Location_Region;
                    }
                    if (!string.IsNullOrEmpty(location.Location_Country) && locationExist.Location_Country != location.Location_Country)
                    {
                        locationExist.Location_Country = location.Location_Country;
                    }
                    if (location.Location_IsActive != null && locationExist.Location_IsActive != location.Location_IsActive)
                    {
                        locationExist.Location_IsActive = location.Location_IsActive;
                    }
                    if (location.Location_IsDefault != null && locationExist.Location_IsDefault != location.Location_IsDefault)
                    {
                        locationExist.Location_IsDefault = location.Location_IsDefault;
                    }
                    if (!string.IsNullOrEmpty(location.Location_LastModifiedUserId) && locationExist.Location_LastModifiedUserId != location.Location_LastModifiedUserId)
                    {
                        locationExist.Location_LastModifiedUserId = location.Location_LastModifiedUserId;
                    }
                    locationExist.Location_LastModifiedDate = DateTime.Now;

                    entity.SaveChanges();
                    return Response.True;

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateLocation - {location.Location_Id}");
            }
            return Response.False;

        }
        public static bool InsertLocation(LocationJsonModel location)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();

            try
            {
                var newLocation = new Customer_Location()
                {
                    FK_Location_Customer_ID = location.FK_Location_Customer_ID,
                    Location_Address = location.Location_Address,
                    Location_Country = location.Location_Country,
                    Location_CreationDate = DateTime.Now,
                    Location_District = location.Location_District,
                    Location_IsDefault = location.Location_IsDefault,
                    Location_Location = location.Location_Location,
                    Location_LastModifiedDate = DateTime.Now,
                    Location_LastModifiedUserId = location.Location_LastModifiedUserId,
                    Location_Name = location.Location_Name,
                    Location_Region = location.Location_Region,
                    Location_ZipCode = location.Location_ZipCode,
                    Location_GespeLocationId = location.Location_GespeLocationId,
                    Location_IsActive = location.Location_IsActive,
                    //FK_Location_Client_ID = location.FK_Location_Client_ID,
                };
                entity.Customer_Location.Add(newLocation);
                entity.SaveChanges();

                return Response.True;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InsertLocation");
            }
            return Response.False;

        }
        private static void SyncronizeNewLocationOnXcmDb()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            GnXcmEntities gnXcmProd = new GnXcmEntities();
            var crmDb = entity.Customer_Location.ToList();
            var xcmDb = gnXcmProd.TmsLocationDetail.Where(x => x.AnaType == 1).ToList();
            var customerSessionId = "";
            var clientId = "";
            try
            {
                foreach (var location in xcmDb)
                {
                    var isPresente = crmDb.Where(x => x.Location_GespeLocationId == location.LocationID).FirstOrDefault();
                    if (isPresente == null)
                    {
                        if (!string.IsNullOrEmpty(location.AnaID))
                        {
                            customerSessionId = GetCustomerSessionIdByGespeId(location.AnaID);
                            if (string.IsNullOrEmpty(customerSessionId))
                                continue;
                        }
                        if (!string.IsNullOrEmpty(location.Name))
                        {
                            clientId = GetClientUserId(location.Name);
                            if (string.IsNullOrEmpty(clientId))
                                continue;
                        }
                        var newLocation = new Customer_Location()
                        {
                            FK_Location_Customer_ID = customerSessionId,
                            Location_Address = location.Address,
                            Location_Country = location.Country,
                            Location_CreationDate = DateTime.Now,
                            Location_District = location.District,
                            Location_IsDefault = location.IsDefault,
                            Location_Location = location.Location,
                            Location_LastModifiedDate = DateTime.Now,
                            Location_LastModifiedUserId = "999",
                            Location_Name = location.Name,
                            Location_Region = location.Region,
                            Location_ZipCode = location.ZipCode,
                            Location_GespeLocationId = location.LocationID,
                            //FK_Location_Client_ID = clientId,
                        };
                        entity.Customer_Location.Add(newLocation);
                        entity.SaveChanges();

                    }
                }
            }
            catch (Exception ee)
            {


            }


        }
        #endregion

        #region Geo
        public static List<GeoITJsonModel> GetGeo()
        {
            List<GeoITJsonModel> list = new List<GeoITJsonModel>();
            try
            {
                XCM_WMSEntities db = new XCM_WMSEntities();
                list = db.GEO_IT.ToList().Select(x => JsonInterpreter.ConvertGeoITtoJsonModel(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetGeo");
            }
            return list;
        }

        public static bool DistrictIsValid(string district)
        {
            bool response = false;
            try
            {
                XCM_WMSEntities db = new XCM_WMSEntities();
                var exist = db.GEO_IT.FirstOrDefault(x => x.PROVINCIA == district.ToUpper());
                if (exist != null)
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"DistrictIsValid");
            }
            return response;
        }
        #endregion

        #region Orders
        public static List<RootObjectOrderViewModel> GetOrders(string GespeID)
        {
            List<RootObjectOrderViewModel> list = new List<RootObjectOrderViewModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                if (AuthHelper.IsRoot())//TODO: controllare l'utenza del api
                {
                    list = entity.Orders.ToList().Select(x => JsonInterpreter.ClonaFromCRMOrderDBToJson(x)).ToList();
                }
                list = entity.Orders.Where(x => x.Orders_customerID == GespeID).ToList().Select(x => JsonInterpreter.ClonaFromCRMOrderDBToJson(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetOrders - {GespeID}");
            }
            return list;

        }
        public static List<RootObjectOrderViewModel> GetAgentOrders(string GespeID, string UserId)
        {
            List<RootObjectOrderViewModel> list = new List<RootObjectOrderViewModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var agentOrder = entity.Order_Agent.Where(x => x.FK_OrderAgent_AgentID == UserId).ToList();

                foreach (var order in agentOrder)
                {
                    var exist = entity.Orders.FirstOrDefault(x => x.Orders_customerID == GespeID && x.Orders_GespeUniq == order.FK_OrderAgent_OrdersID);
                    if (exist != null)
                    {
                        list.Add(JsonInterpreter.ClonaFromCRMOrderDBToJson(exist));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetAgentOrders - {GespeID} - {UserId}");
            }
            return list;

        }
        public static string UpdateOrder(RootObjectOrderViewModel daAggiornare)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var corr = entity.Orders.FirstOrDefault(x => x.Orders_GespeUniq == daAggiornare.header.id);
                if (corr != null)
                {
                    corr = JsonInterpreter.ClonaFromJsonToCRMOrderDB(daAggiornare);
                    entity.SaveChanges();
                }
                else
                {
                    throw new Exception("documento non trovato");
                }
                return "true";
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
        }
        public static string AddOrder(RootObjectOrderViewModel nOrd)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            XCM xcmcode = new XCM();
            long idDocApi = 0;//Salvare id inserimento in caso di roolback
            try
            {
                var corr = entity.Orders.FirstOrDefault(x => x.Orders_GespeUniq == nOrd.header.id);
                if (corr != null)
                {
                    var no = JsonInterpreter.ClonaFromJsonToCRMOrderDB(nOrd);
                    entity.Orders.Add(no);
                    idDocApi = xcmcode.InviaOrdineAdAPIXCM(no);
                    if (idDocApi > 0)
                    {
                        entity.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("documento già presente");
                }
                return "true";
            }
            catch (Exception ee)
            {
                //elimina documento API idORDAPI
                var ok = xcmcode.CancellaOrdine(idDocApi);
                if (!ok)
                {
                    //invia messaggio email al customer per richiedere la cancellazione direttamente da gespe
                }
                return ee.Message;
            }
        }
        public static bool InsertOrder(RootObjectOrderViewModel order)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            XCM xcmcode = new XCM();
            try
            {
                var corr = entity.Orders.FirstOrDefault(x => x.Orders_GespeUniq == order.header.id);
                if (corr == null)
                {
                    var no = JsonInterpreter.ClonaFromJsonToCRMOrderDB(order);
                    entity.Orders.Add(no);
                    entity.SaveChanges();
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InsertOrder - {order.header.id}");
            }
            return Response.False;
        }
        public static bool InsertOrderAgent(int orderId, string userId)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var exist = entity.Order_Agent.FirstOrDefault(x => x.FK_OrderAgent_OrdersID == orderId && x.FK_OrderAgent_AgentID == userId);
                if (exist == null)
                {
                    var newRecord = new Order_Agent()
                    {
                        FK_OrderAgent_AgentID = userId,
                        FK_OrderAgent_OrdersID = orderId
                    };

                    entity.Order_Agent.Add(newRecord);
                    entity.SaveChanges();
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InserOrderAgent - {orderId} - {userId}");
            }
            return Response.False;
        }

        #region TempOrder
        public static bool InsertTemporaryOrder(TemporaryOrderJsonModel order)
        {
            try
            {
                XCM_CRMEntities db = new XCM_CRMEntities();
                //TODO: come vedo se esiste?
                var newTempOrder = new Temp_Orders()
                {
                    AgentID = order.Data.AgentID,
                    ConsigneeLocationID = order.Data.ConsigneeLocationID,
                    CustomerID = order.Data.CustomerID,
                    DeliveryNote = order.Data.DeliveryNote,
                    OrderConfirmed = false,
                    OrderReference = order.Data.OrderReference,
                    OrderReferenceDate = order.Data.OrderReferenceDate,
                    OrderSended = false,
                    OrderType = order.Data.OrderType,
                    UnloadLocationID = order.Data.UnloadLocationID,
                    XCMNote = order.Data.XCMNote
                };

                db.Temp_Orders.Add(newTempOrder);
                db.SaveChanges();


                var exist = db.Temp_Order_Products.Where(x => x.Order_ID == newTempOrder.OrderID).ToList();
                if (exist.Count() == 0)
                {
                    foreach (var product in order.Products)
                    {
                        if (exist.Count() == 0)
                        {
                            var newOrderProducts = new Temp_Order_Products()
                            {
                                Order_ID = newTempOrder.OrderID,
                                Product_PartNumber = product.CODICE_PRODOTTO,
                                Product_Price = product.PREZZO_UNITARIO,
                                Product_Quantity = product.QUANTITA,
                                Product_Des = product.DESCRIZIONE_PRODOTTO,
                                Product_Discount = product.SCONTO,
                                Product_Iva = product.IVA,
                            };

                            db.Temp_Order_Products.Add(newOrderProducts);
                            db.SaveChanges();
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"InserTemporaryOrder");
            }
            return Response.True;
        }

        public static List<TempOrderJsonModel> GetTempOrders(string GespeID)
        {
            List<TempOrderJsonModel> list = new List<TempOrderJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                list = entity.Temp_Orders.Where(x => x.CustomerID == GespeID && !x.OrderConfirmed).ToList().Select(x => JsonInterpreter.ConvertTempOrdersToJsonModel(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetTempOrders - {GespeID}");
            }
            return list;

        }

        public static List<TempOrderProductsJsonModel> GetTempOrderProducts(long OrderID)
        {
            List<TempOrderProductsJsonModel> list = new List<TempOrderProductsJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                list = entity.Temp_Order_Products.Where(x => x.Order_ID == OrderID).ToList().Select(x => JsonInterpreter.ConvertTempOrderProductsToJsonModel(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetTempOrderProducts - {OrderID}");
            }
            return list;

        }

        public static bool DeleteTempOrder(long OrderID)
        {
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();

                var exist = entity.Temp_Orders.FirstOrDefault(x => x.OrderID == OrderID);
                if (exist != null)
                {
                    entity.Temp_Orders.Remove(exist);

                    var products = entity.Temp_Order_Products.Where(x => x.Order_ID == OrderID).ToList();

                    foreach (var p in products)
                    {
                        entity.Temp_Order_Products.Remove(p);
                    }

                    entity.SaveChanges();
                    return Response.True;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"DeleteTempOrder - {OrderID}");
            }
            return Response.False;

        }
        #endregion
        #endregion

        #region Shipments
        public static List<ShipmentJsonModel> GetShipments(string customerId)
        {
            List<ShipmentJsonModel> list = new List<ShipmentJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                //TODO: controllare se l'user è root
                if (customerId == "00000")
                {
                    list = entity.ShipmentList.ToList().Select(x => JsonInterpreter.ConvertShipmentToJsonModel(x)).ToList();
                }
                list = entity.ShipmentList.Where(x => x.Shipment_CustomerID_XCM == customerId).ToList().Select(x => JsonInterpreter.ConvertShipmentToJsonModel(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetShipments");
            }
            return list;
        }

        public static List<TrackingJsonModel> GetShipmentDetail(long shipId)
        {
            List<TrackingJsonModel> list = new List<TrackingJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                list = entity.Tracking.Where(x => x.Tracking_ShipmentID == shipId).ToList().Select(x => JsonInterpreter.ConvertTrackingToJsonModel(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetShipmentDetail");
            }
            return list;

        }
        #endregion

        #region Products
        public static List<ProductJsonModel> GetProducts(string customerId)
        {
            List<ProductJsonModel> list = new List<ProductJsonModel>();
            try
            {
                XCM_CRMEntities entity = new XCM_CRMEntities();
                var customer = entity.Customer.FirstOrDefault(x => x.Customer_SessionID == customerId);
                if (customer != null)
                {
                    if (customer.Customer_id == "00000")
                    {
                        var products = entity.Anagrafica_Prodotti.ToList();

                        if (products != null)
                        {
                            list = products.ToList().Select(x => JsonInterpreter.ConvertAnagrafica_ProdottiToJsonModel(x)).ToList();
                        }
                    }
                    else
                    {
                        var products = entity.Anagrafica_Prodotti.Where(x => x.GESPE_CUSTOMERID == customer.Customer_id).ToList();

                        if (products != null)
                        {
                            list = products.ToList().Select(x => JsonInterpreter.ConvertAnagrafica_ProdottiToJsonModel(x)).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"GetProducts");
            }
            return list;

        }

        public static bool UpdateProduct(ProductJsonModel product)
        {
            try
            {
                XCM_CRMEntities db = new XCM_CRMEntities();
                var exist = db.Anagrafica_Prodotti.FirstOrDefault(x => x.ID_ANAGRAFICA_PRODOTTO == product.ID_ANAGRAFICA_PRODOTTO);
                if (exist != null)
                {
                    if (!string.IsNullOrEmpty(product.PREZZO_UNITARIO.ToString()) && exist.PREZZO_UNITARIO != product.PREZZO_UNITARIO)
                    {
                        exist.PREZZO_UNITARIO = product.PREZZO_UNITARIO;
                        db.SaveChanges();
                        return Response.True;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"UpdateProduct");

            }
            return Response.False;
        }
        #endregion

        #region ClientUser
        public static void InitClientUserTable()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            GnXcmEntities gnXcmProd = new GnXcmEntities();
            var locations = gnXcmProd.TmsLocationDetail.Where(x => x.AnaType == 1).ToList();

            List<Client_User> clients = new List<Client_User>();

            List<string> clientName = locations.Select(x => x.Name).Distinct().ToList();

            foreach (var name in clientName)
            {
                var location = locations.FirstOrDefault(x => x.Name == name);
                if (location != null && !string.IsNullOrEmpty(location.AnaID))
                {

                    var newClient = new Client_User()
                    {
                        UserClient_Name = name,
                        UserClient_UserID = Guid.NewGuid().ToString(),
                        FK_UserClient_Customer_ID = !string.IsNullOrEmpty(location.AnaID) ? GetCustomerSessionIdByGespeId(location.AnaID) : "",
                        UserClient_CreationDate = DateTime.Now,
                        UserClient_LastModifiedDate = DateTime.Now,
                        UserClient_LastModifiedUserID = "999"
                    };
                    clients.Add(newClient);
                }
            }
            entity.Client_User.AddRange(clients);
            entity.SaveChanges();
        }

        public static List<ClientJsonModel> GetClients()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            return entity.Client_User.ToList().Select(x => JsonInterpreter.ConvertClientUserToClientJsonModel(x)).ToList();
        }

        public static List<ClientJsonModel> GetClientsByCustomerId(string csId)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            return entity.Client_User.Where(x => x.FK_UserClient_Customer_ID == csId).ToList().Select(x => JsonInterpreter.ConvertClientUserToClientJsonModel(x)).ToList();

        }

        //public static List<LocationJsonModel> GetLocationsByCustomerIdAndClientId(string csId, string clientId)
        //{
        //    XCM_CRMEntities entity = new XCM_CRMEntities();
        //    return JsonInterpreter.ListLocationJsonInterpreter(entity.Customer_Location.Where(x => x.FK_Location_Customer_ID == csId && x.FK_Location_Client_ID == clientId).ToList());
        //}

        public static string GetClientUserId(string name)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            var test = entity.Client_User.Where(x => x.UserClient_Name == name).FirstOrDefault();
            if (test != null)
                return test.UserClient_UserID;
            return "";
        }

        public static bool InsertClient(ClientJsonModel user)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            User userExsist = entity.User.Where(x => x.User_ID == user.UserClient_UserID).FirstOrDefault();

            if (userExsist == null)
            {
                user.UserClient_CreationDate = DateTime.Now;
                user.UserClient_LastModifiedDate = DateTime.Now;
                user.UserClient_LastModifiedUserID = user.UserClient_UserID;
                entity.Client_User.Add(JsonInterpreter.ConvertClientJsonModelToClientUser(user));

                //TODO: Aggiungere la località di carico e scarico in GESPE ?

                Customer_Location location = new Customer_Location()
                {
                    //FK_Location_Client_ID = user.UserClient_UserID,
                    FK_Location_Customer_ID = user.FK_UserClient_Customer_ID,
                    Location_Address = user.UserClient_Location.Location_Address,
                    Location_Country = user.UserClient_Location.Location_Country,
                    Location_CreationDate = DateTime.Now,
                    Location_District = user.UserClient_Location.Location_District,
                    Location_IsActive = user.UserClient_Location.Location_IsActive,
                    Location_LastModifiedDate = DateTime.Now,
                    Location_Location = user.UserClient_Location.Location_Location,
                    Location_Name = user.UserClient_Name,
                    Location_ZipCode = user.UserClient_Location.Location_ZipCode,
                    Location_Region = user.UserClient_Location.Location_Region,
                    Location_IsDefault = true,
                    //TODO: UserID API

                };

                entity.Customer_Location.Add(location);

                entity.SaveChanges();
                return true;

            }
            return false;

        }
        #endregion

    }
}