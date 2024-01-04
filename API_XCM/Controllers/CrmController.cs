using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using API_XCM.Code.CRM;
using API_XCM.Models.XCM;
using API_XCM.Models.XCM.CRM;
using API_XCM.Models.XCM.CRM.JsonModel;
using API_XCM.Models.XCM.CRM.SyncroDB_CRM;
using DevExpress.Web.Mvc;

namespace API_XCM.Controllers
{
    [Authorize(Roles = "Crm, Admin, Root", Users = "CRM, Davide")]
    public class CrmController : ApiController
    {
        
        #region Customer
        [Authorize(Roles = "Root")]
        [HttpGet]
        public void InitCustomerTable()
        {
            InitDBCRM.InitCustomerTable();
        }

        [Authorize(Roles = "Root")]
        [HttpGet]
        public List<CustomerJsonModel> GetCustomers()
        {
            return CRM.GetCustomers();
        }

        [Authorize(Roles = "Root")]
        [HttpPost]
        public bool UpdateCustomer(CustomerJsonModel customer)
        { 
            return CRM.UpdateCustomer(customer);
        }

        [HttpGet]
        public string GetCustomerGespeId(string csId)
        {
            return CRM.GetCustomerGespeId(csId);
        }

        [HttpGet]
        public CustomerAuthorizationJsonModel GetCustomerAuthorization(string csId)
        {
            return CRM.GetCustomerAuthorization(csId);
        }
        #endregion

        #region Users
        [HttpGet]
        public List<UserJsonModel> GetUsers()
        {
            return CRM.GetUsers();
        }


        [Authorize(Roles = "Root")]
        [HttpPost]
        public bool UpdateUser(UserJsonModel user)
        {
            return CRM.UpdateUser(user);
        }

        [Authorize(Roles = "Root")]
        [HttpPost]
        public bool InsertUser(User user)
        {
            return CRM.InsertUser(user);
        }
        
        [Authorize(Roles = "Root")]
        [HttpGet]
        public UserJsonModel GetUserByEmail(string email)
        {
            return CRM.GetUserByEmail(email);
        }
        #endregion

        #region Agent
        [HttpPost]
        public bool InsertAgent(AgentJsonModel agent)
        {
            return CRM.InsertAgent(agent);
        }

        [HttpPost]
        public bool UpdateAgent(AgentJsonModel agent)
        {
            return CRM.UpdateAgent(agent);
        }
        [HttpGet]
        public List<AgentJsonModel> GetAgents(string csId)
        {
            return CRM.GetAgents(csId);
        }

        [HttpGet]
        public List<AgentJsonModel> GetAgents()
        {
            return CRM.GetAgents();
        }

        [HttpGet]
        public string GetAgentName(string agentId)
        {
            return CRM.GetAgentName(agentId);
        }

        #region AgentAuthorization
        [HttpPost]
        public AgentAuthorizationJsonModel GetAgentAuthorization(string userId)
        {
            return CRM.GetAgentAuthorization(userId);
        }

        [HttpPost]
        public bool UpdateAgentAuthorization(AgentAuthorizationJsonModel model)
        {
            return CRM.UpdateAgentAuthorization(model);
        }

        [HttpPost]
        public bool InsertAgentAuthorization(AgentAuthorizationJsonModel model)
        {
            return CRM.InsertAgentAuthorization(model);
        }
        #endregion
        #endregion

        #region Roles
        [HttpGet]
        public List<RoleJsonModel> GetRoles()
        {
            return CRM.GetRoles();
        }
        #endregion

        #region Location
        [Authorize(Roles = "Root")]
        [HttpGet]
        public void InitClientUserTable()
        {
            CRM.InitClientUserTable();
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpGet]
        public List<LocationJsonModel> GetLocationsByCustomerId(string csId)
        {
            return CRM.GetLocationsByCustomerId(csId);
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpGet]
        public List<LocationJsonModel> GetLocations()
        {
            return CRM.GetLocations();
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpPost]
        public bool InsertLocation(LocationJsonModel location)
        {
            return CRM.InsertLocation(location);
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpPost]
        public bool UpdateLocation(LocationJsonModel location)
        {
            return CRM.UpdateLocation(location);
        }

        //[HttpGet]
        //public List<LocationJsonModel> GetLocationsByCustomerIdAndClientId(string csId, string clientId)
        //{
        //    return CRM.GetLocationsByCustomerIdAndClientId(csId, clientId);
        //}

        //[HttpGet]
        //public List<LocationJsonModel> GetClientLocations(string clientId)
        //{
        //    return CRM.GetClientLocations(clientId);
        //}
        #endregion

        #region Client
        [HttpGet]
        public List<ClientJsonModel> GetClients()
        {
            return CRM.GetClients();
        }

        [HttpGet]
        public List<ClientJsonModel> GetClientByCustomerId(string csId)
        {
            return CRM.GetClientsByCustomerId(csId);
        }

        [HttpPost]
        public bool InsertClient(ClientJsonModel client)
        {
            return CRM.InsertClient(client);
        }
        #endregion

        #region Product
        [Authorize(Roles = "Root, Admin, Agent")]
        public List<ProductJsonModel> GetProducts(string customerId)
        {
            return CRM.GetProducts(customerId);
        }
        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpPost]
        public bool UpdateProduct(ProductJsonModel product)
        {
            return CRM.UpdateProduct(product);
        }
        #endregion

        #region GEO
        [Authorize(Roles = "Root, Admin, Agent")]
        public List<GeoITJsonModel> GetGeo()
        {
            return CRM.GetGeo();
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpGet]
        public bool DistrictIsValid(string district)
        {
            return CRM.DistrictIsValid(district);
        }
        #endregion

        #region Orders
        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpGet]
        public List<RootObjectOrderViewModel> GetOrders(string GespeID)
        {
            return CRM.GetOrders(GespeID);
        }

        [Authorize(Roles = "Root, Admin, Agent")]
        [HttpGet]
        public List<RootObjectOrderViewModel> GetAgentOrders(string GespeID, string UserId)
        {
            return CRM.GetAgentOrders(GespeID, UserId);
        }

        [HttpPost]
        public string UpdateOrder(RootObjectOrderViewModel order)
        {
            return CRM.UpdateOrder(order);
        }

        [HttpPost]
        public string AddOrder(RootObjectOrderViewModel order)
        {
            return CRM.AddOrder(order);
        }

        [HttpGet]
        public bool InsertOrderAgent(int orderId, string userId)
        {
            return CRM.InsertOrderAgent(orderId, userId);
        }

        [HttpPost]
        public bool InsertTemporaryOrder(TemporaryOrderJsonModel order)
        {
            return CRM.InsertTemporaryOrder(order);
        }

        [HttpGet]
        public List<TempOrderJsonModel> GetTempOrders(string GespeID)
        {
            return CRM.GetTempOrders(GespeID);
        }

        [HttpGet]
        public List<TempOrderProductsJsonModel> GetTempOrderProducts(long OrderID)
        {
            return CRM.GetTempOrderProducts(OrderID);
        }

        [HttpPost]
        public bool InsertOrder(RootObjectOrderViewModel order)
        {
            return CRM.InsertOrder(order);
        }

        [HttpGet]
        public bool DeleteTempOrder(long OrderID)
        {
            return CRM.DeleteTempOrder(OrderID);
        }
        #endregion

        #region Shipment
        public List<ShipmentJsonModel> GetShipments(string customerId)
        {
            return CRM.GetShipments(customerId);
        }

        public List<TrackingJsonModel> GetShipmentDetail(int shipId)
        {
            return CRM.GetShipmentDetail(shipId);
        }
        #endregion

        #region TEST
        public string GetTest(string tt)
        {
            if (!string.IsNullOrEmpty(tt)) return "true";
            else return "false";
        }
        #endregion

    }
}