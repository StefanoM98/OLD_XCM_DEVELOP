using API_XCM.Models.XCM.CRM;
using API_XCM.Models.XCM.CRM.SyncroDB_CRM;
using API_XCM.Models.XCM.CRM.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API_XCM.Models.XCM;

namespace API_XCM.Code.CRM
{
    public class JsonInterpreter
    {

        #region Customer
        public static List<CustomerJsonModel> ListCustomerJsonInterpreter(List<Customer> customerOnDb)
        {
            var response = new List<CustomerJsonModel>();
            foreach (var cs in customerOnDb)
            {
                var newCs = new CustomerJsonModel()
                {
                    Customer_id = cs.Customer_id,
                    Customer_address = cs.Customer_address,
                    Customer_Authorization = CustomerAuthorizationJsonInterpreter(cs.Customer_Authorization),
                    Customer_country = cs.Customer_country,
                    Customer_CreationDate = cs.Customer_CreationDate,
                    Customer_defaultPriceListId = cs.Customer_defaultPriceListId,
                    Customer_description = cs.Customer_description,
                    Customer_district = cs.Customer_district,
                    Customer_isEnable = cs.Customer_isEnable,
                    Customer_IsEnableCRM = cs.Customer_IsEnableCRM,
                    Customer_LastModifiedDate = cs.Customer_LastModifiedDate,
                    Customer_LastModifiedUserID = cs.Customer_LastModifiedUserID,
                    Customer_location = cs.Customer_location,
                    Customer_Logo = cs.Customer_Logo,
                    Customer_SessionID = cs.Customer_SessionID,
                    Customer_vatCode = cs.Customer_vatCode,
                    Customer_zipCode = cs.Customer_zipCode
                };
                response.Add(newCs);
            }
            return response;

        }
        public static CustomerAuthorizationJsonModel CustomerAuthorizationJsonInterpreter(Customer_Authorization customerAuthorization)
        {
            if (customerAuthorization != null)
            {
                var response = new CustomerAuthorizationJsonModel()
                {
                    Customer_Id = customerAuthorization.Customer_Id,
                    LastModifiedDate = customerAuthorization.LastModifiedDate,
                    LastModifiedUserID = customerAuthorization.LastModifiedUserID,
                    Orders_Active = Convert.ToBoolean(customerAuthorization.Orders_Active),
                    Warehouse_Active = Convert.ToBoolean(customerAuthorization.Warehouse_Active),
                    Tracking_Active = Convert.ToBoolean(customerAuthorization.Tracking_Active),
                    Agents_Active = Convert.ToBoolean(customerAuthorization.Agents_Active),
                    ExpireDateTracking = customerAuthorization.ExpireDateTracking,
                    ExpireDateOrders = customerAuthorization.ExpireDateOrders,
                    ExpireDateWarehouse = customerAuthorization.ExpireDateWarehouse,
                    ExpireDateAgents = customerAuthorization.ExpireDateAgents

                };

                return response;
            }
            return new CustomerAuthorizationJsonModel();

        }
        #endregion

        #region User
        public static List<UserJsonModel> ListUserJsonInterepreter(List<User> usersOnDb)
        {
            List<UserJsonModel> response = new List<UserJsonModel>();

            foreach (var user in usersOnDb)
            {
                var newUser = new UserJsonModel()
                {
                    FK_User_Customer_ID = user.FK_User_Customer_ID,
                    FK_User_Role_ID = user.FK_User_Role_ID,
                    User_Active = user.User_Active,
                    User_CreationDate = user.User_CreationDate,
                    User_Email = user.User_Email,
                    User_FirstName = user.User_FirstName,
                    User_HashPassword = user.User_HashPassword,
                    User_ID = user.User_ID,
                    User_LastModifiedDate = user.User_LastModifiedDate,
                    User_Salt = user.User_Salt,
                    User_Surname = user.User_Surname,
                    User_Username = user.User_Username,
                    User_PasswordRecovered = user.User_PasswordRecovered
                };
                response.Add(newUser);

            }
            return response;
        }
        public static UserJsonModel UserJsonInterepreter(User userOnDb)
        {
            var newUser = new UserJsonModel()
            {
                FK_User_Customer_ID = userOnDb.FK_User_Customer_ID,
                FK_User_Role_ID = userOnDb.FK_User_Role_ID,
                User_Active = userOnDb.User_Active,
                User_CreationDate = userOnDb.User_CreationDate,
                User_Email = userOnDb.User_Email,
                User_FirstName = userOnDb.User_FirstName,
                User_HashPassword = userOnDb.User_HashPassword,
                User_ID = userOnDb.User_ID,
                User_LastModifiedDate = userOnDb.User_LastModifiedDate,
                User_Salt = userOnDb.User_Salt,
                User_Surname = userOnDb.User_Surname,
                User_Username = userOnDb.User_Username,
                User_PasswordRecovered = userOnDb.User_PasswordRecovered
            };

            return newUser;
        }
        #endregion

        #region Agent
        public static List<AgentJsonModel> ConvertListUserAgentToJsonModel(List<User> usersOnDb)
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            List<AgentJsonModel> response = new List<AgentJsonModel>();

            foreach (var user in usersOnDb)
            {
                AgentAuthorizationJsonModel newAgentAuth = null;
                var agentAuth = entity.AgentAuthorization.FirstOrDefault(x => x.FK_AgentAuthorization_User_ID == user.User_ID);
                if (agentAuth != null)
                {
                    newAgentAuth = new AgentAuthorizationJsonModel()
                    {
                        FK_AgentAuthorization_User_ID = agentAuth.FK_AgentAuthorization_User_ID,
                        AgentAuthorization_CreationDate = agentAuth.AgentAuthorization_CreationDate,
                        AgentAuthorization_ID = agentAuth.AgentAuthorization_ID,
                        AgentAuthorization_LastModifiedDate = agentAuth.AgentAuthorization_LastModifiedDate,
                        AgentAuthorization_LastModifiedUserID = agentAuth.AgentAuthorization_LastModifiedUserID,
                        AgentAuthorization_OrderConfirmation = agentAuth.AgentAuthorization_OrderConfirmation
                    };

                }
                var newUser = new AgentJsonModel()
                {
                    FK_User_Customer_ID = user.FK_User_Customer_ID,
                    FK_User_Role_ID = user.FK_User_Role_ID,
                    User_Active = user.User_Active,
                    User_CreationDate = user.User_CreationDate,
                    User_Email = user.User_Email,
                    User_FirstName = user.User_FirstName,
                    User_HashPassword = user.User_HashPassword,
                    User_ID = user.User_ID,
                    User_LastModifiedDate = user.User_LastModifiedDate,
                    User_Salt = user.User_Salt,
                    User_Surname = user.User_Surname,
                    User_Username = user.User_Username,
                    User_PasswordRecovered = user.User_PasswordRecovered,
                    User_AgentAuth = newAgentAuth
                };
                response.Add(newUser);
            }
            return response;
        }
        public static AgentJsonModel ConvertUserAgentToJsonModel(User user)
        {
            return new AgentJsonModel()
            {
                FK_User_Customer_ID = user.FK_User_Customer_ID,
                FK_User_Role_ID = user.FK_User_Role_ID,
                User_Active = user.User_Active,
                User_CreationDate = user.User_CreationDate,
                User_Email = user.User_Email,
                User_FirstName = user.User_FirstName,
                User_HashPassword = user.User_HashPassword,
                User_ID = user.User_ID,
                User_LastModifiedDate = user.User_LastModifiedDate,
                User_Salt = user.User_Salt,
                User_Surname = user.User_Surname,
                User_Username = user.User_Username,
                User_PasswordRecovered = user.User_PasswordRecovered
            };
        }

        public static User ConvertAgentJsonToEntityModel(AgentJsonModel user)
        {
            return new User()
            {
                FK_User_Customer_ID = user.FK_User_Customer_ID,
                FK_User_Role_ID = user.FK_User_Role_ID,
                User_Active = user.User_Active,
                User_CreationDate = user.User_CreationDate,
                User_Email = user.User_Email,
                User_FirstName = user.User_FirstName,
                User_HashPassword = user.User_HashPassword,
                User_ID = user.User_ID,
                User_LastModifiedDate = user.User_LastModifiedDate,
                User_Salt = user.User_Salt,
                User_Surname = user.User_Surname,
                User_Username = user.User_Username,
                User_PasswordRecovered = user.User_PasswordRecovered
            };
        }

        #endregion

        #region Role
        public static List<RoleJsonModel> ListRoleJsonInterpreter(List<Role> roles)
        {
            List<RoleJsonModel> response = new List<RoleJsonModel>();
            foreach (var role in roles)
            {
                var newRole = new RoleJsonModel()
                {
                    Role_Active = role.Role_Active,
                    Role_CreationDate = role.Role_CreationDate,
                    Role_ID = role.Role_ID,
                    Role_LastModifiedDate = role.Role_LastModifiedDate,
                    Role_Name = role.Role_Name
                };
                response.Add(newRole);
            }
            return response;
        }
        #endregion

        #region Location
        public static List<LocationJsonModel> ConvertCustomerLocationToLocationJsonModel(List<Customer_Location> locations)
        {
            List<LocationJsonModel> list = new List<LocationJsonModel>();

            foreach(var location in locations)
            {
                var newLocation =  new LocationJsonModel()
                {
                    FK_Location_Customer_ID = location.FK_Location_Customer_ID,
                    Location_Address = location.Location_Address,
                    Location_Country = location.Location_Country,
                    Location_District = location.Location_District,
                    Location_IsDefault = location.Location_IsDefault,
                    Location_Location = location.Location_Location,
                    Location_LastModifiedDate = DateTime.Now,
                    Location_LastModifiedUserId = location.Location_LastModifiedUserId,
                    Location_Name = location.Location_Name,
                    Location_Region = location.Location_Region,
                    Location_ZipCode = location.Location_ZipCode,
                    Location_GespeLocationId = location.Location_GespeLocationId,
                    Location_Id = location.Location_Id,
                    Location_IsActive = location.Location_IsActive,
                    Location_CreationDate = location.Location_CreationDate
                };

                list.Add(newLocation);
            }

            return list;
            
        }

        public static Customer_Location ConvertLocationJsonModelToCustomerLocation(LocationJsonModel location)
        {
            return new Customer_Location()
            {
                FK_Location_Customer_ID = location.FK_Location_Customer_ID,
                Location_Address = location.Location_Address,
                Location_Country = location.Location_Country,
                Location_District = location.Location_District,
                Location_IsDefault = location.Location_IsDefault,
                Location_Location = location.Location_Location,
                Location_LastModifiedDate = DateTime.Now,
                Location_LastModifiedUserId = location.Location_LastModifiedUserId,
                Location_Name = location.Location_Name,
                Location_Region = location.Location_Region,
                Location_ZipCode = location.Location_ZipCode,
                Location_GespeLocationId = location.Location_GespeLocationId,
                Location_CreationDate = location.Location_CreationDate,
                Location_IsActive = location.Location_IsActive,
                Location_Id = location.Location_Id
            };
        }
        #endregion

        #region Product
        public static ProductJsonModel ConvertAnagrafica_ProdottiToJsonModel(Anagrafica_Prodotti product)
        {
            return new ProductJsonModel()
            {
                CODICE_PRODOTTO = product.CODICE_PRODOTTO,
                DATA_CREAZIONE = product.DATA_CREAZIONE,
                DATA_ULTIMA_MODIFICA = product.DATA_ULTIMA_MODIFICA,
                DESCRIZIONE_PRODOTTO = product.DESCRIZIONE_PRODOTTO,
                GESPE_CUSTOMERID = product.GESPE_CUSTOMERID,
                ID_ANAGRAFICA_PRODOTTO = product.ID_ANAGRAFICA_PRODOTTO,
                PREZZO_UNITARIO = product.PREZZO_UNITARIO != null ? product.PREZZO_UNITARIO.Value : 0,
            };
        }

        public static Anagrafica_Prodotti ConvertProductJsonModelToEntityModel(ProductJsonModel product)
        {
            return new Anagrafica_Prodotti()
            {
                CODICE_PRODOTTO = product.CODICE_PRODOTTO,
                DATA_CREAZIONE = product.DATA_CREAZIONE,
                DATA_ULTIMA_MODIFICA = product.DATA_ULTIMA_MODIFICA,
                DESCRIZIONE_PRODOTTO = product.DESCRIZIONE_PRODOTTO,
                GESPE_CUSTOMERID = product.GESPE_CUSTOMERID,
                ID_ANAGRAFICA_PRODOTTO = product.ID_ANAGRAFICA_PRODOTTO,
                PREZZO_UNITARIO = product.PREZZO_UNITARIO
            };
        }
        #endregion

        #region GEO
        public static GeoITJsonModel ConvertGeoITtoJsonModel(GEO_IT source)
        {
            return new GeoITJsonModel()
            {
                GeoIT_Id = source.ID_GEOGRAFICO,
                GeoIT_Cap = source.CAP,
                GeoIT_Localita = source.CITTA,
                GeoIT_Provincia = source.PROVINCIA,
                GeoIT_Region = source.REGIONE,
                GeoIT_Nazione = "IT"
            };
        }
        #endregion

        #region AgentAuthorization
        public static AgentAuthorizationJsonModel ConvertAgentAuthorizationToJsonModel(AgentAuthorization src)
        {
            return new AgentAuthorizationJsonModel()
            {
                AgentAuthorization_ID = src.AgentAuthorization_ID,
                AgentAuthorization_OrderConfirmation = src.AgentAuthorization_OrderConfirmation,
                FK_AgentAuthorization_User_ID = src.FK_AgentAuthorization_User_ID,
                AgentAuthorization_CreationDate = src.AgentAuthorization_CreationDate,
                AgentAuthorization_LastModifiedDate = src.AgentAuthorization_LastModifiedDate,
                AgentAuthorization_LastModifiedUserID = src.AgentAuthorization_LastModifiedUserID
            };
        }

        public static AgentAuthorization ConvertAgentAuthorizationToEntityModel(AgentAuthorizationJsonModel src)
        {
            return new AgentAuthorization()
            {
                AgentAuthorization_ID = src.AgentAuthorization_ID,
                AgentAuthorization_OrderConfirmation = src.AgentAuthorization_OrderConfirmation,
                FK_AgentAuthorization_User_ID = src.FK_AgentAuthorization_User_ID,
                AgentAuthorization_CreationDate = src.AgentAuthorization_CreationDate,
                AgentAuthorization_LastModifiedDate = src.AgentAuthorization_LastModifiedDate,
                AgentAuthorization_LastModifiedUserID = src.AgentAuthorization_LastModifiedUserID
            };
        }
        #endregion


        #region Orders
        //public static List<RootObjectOrder> ListOrdersJsonInterpreter(List<RootObjectOrder> Ordini)
        //{
        //    var resp = new List<OrderJsonModel>();
        //    foreach (var doc in Ordini)
        //    {
        //        var nr = new OrderJsonModel()
        //        {
        //            Orders_consigneeDes = doc.Orders_consigneeDes,
        //            Orders_customerDes = doc.Orders_customerDes,
        //            Orders_customerID = doc.Orders_customerID,
        //            Orders_docDate = (doc.Orders_docDate != null) ? doc.Orders_docDate : DateTime.MinValue,
        //            Orders_docNumber = doc.Orders_docNumber,
        //            Orders_GespeID = doc.Orders_GespeID,
        //            Orders_reference = doc.Orders_reference,
        //            Orders_referenceDate = (doc.Orders_referenceDate != null) ? doc.Orders_referenceDate : DateTime.MinValue,
        //            Orders_reTypeID = doc.Orders_reTypeID,
        //            Orders_senderDes = doc.Orders_senderDes,
        //            Orders_statusDes = doc.Orders_statusDes,
        //            Orders_unloadDes = doc.Orders_unloadDes
        //        };
        //        resp.Add(nr);
        //    }
        //    return resp;
        //}
        public static RootObjectOrderViewModel ClonaFromCRMOrderDBToJson(Orders Ordini)
        {
            try
            {
                var nH = new HeaderOrderViewModel()
                {
                    consigneeDes = Ordini.Orders_consigneeDes,
                    customerDes = Ordini.Orders_customerDes,
                    customerID = Ordini.Orders_customerID,
                    docDate = Ordini.Orders_docDate,
                    docNumber = Ordini.Orders_docNumber,
                    id = Ordini.Orders_GespeUniq,
                    reference = Ordini.Orders_reference,
                    referenceDate = Ordini.Orders_referenceDate,
                    regTypeID = Ordini.Orders_reTypeID,
                    senderDes = Ordini.Orders_senderDes,
                    statusDes = Ordini.Orders_statusDes,
                    unLoadDes = Ordini.Orders_unloadDes,
                    consigneeAddress = Ordini.Orders_consigneeAddress,
                    consigneeCountry = Ordini.Orders_consigneeCountry,
                    consigneeDistrict = Ordini.Orders_consigneeDistrict,
                    consigneeID = Ordini.Orders_consigneeID,
                    consigneeLocation = Ordini.Orders_consigneeLocation,
                    consigneeRegion = Ordini.Orders_consigneeRegion,
                    consigneeZipCode = Ordini.Orders_consigneeZipCode,
                    coverage = Ordini.Orders_coverage,
                    deliveryNote = Ordini.Orders_deliveryNote,
                    executed = Ordini.Orders_executed,
                    externalID = Ordini.Orders_externalID,
                    info1 = Ordini.Orders_info1,
                    info2 = Ordini.Orders_info2,
                    info3 = Ordini.Orders_info3,
                    info4 = Ordini.Orders_info4,
                    info5 = Ordini.Orders_info5,
                    info6 = Ordini.Orders_info6,
                    info7 = Ordini.Orders_info7,
                    info8 = Ordini.Orders_info8,
                    info9 = Ordini.Orders_info9,
                    internalNote = Ordini.Orders_internalNote,
                    ownerDes = Ordini.Orders_ownerDes,
                    ownerID = Ordini.Orders_ownerID,
                    planned = Ordini.Orders_planned,
                    publicNode = Ordini.Orders_publicNote,
                    reference2 = Ordini.Orders_reference2,
                    reference2Date = Ordini.Orders_reference2Date,
                    rowsNo = Ordini.Orders_rowsNo,
                    senderAddress = Ordini.Orders_senderAddress,
                    senderCountry = Ordini.Orders_senderCountry,
                    senderDistrict = Ordini.Orders_senderDistrict,
                    senderID = Ordini.Orders_senderID,
                    senderLocation = Ordini.Orders_senderLocation,
                    senderRegion = Ordini.Orders_senderRegion,
                    senderZipCode = Ordini.Orders_senderZipCode,
                    shipDocNumber = Ordini.Orders_shipDocNumber,
                    shipID = Ordini.Orders_shipID,
                    siteID = Ordini.Orders_siteID,
                    statusId = Ordini.Orders_statusId,
                    totalBoxes = Ordini.Orders_totalBoxes,
                    totalCube = Ordini.Orders_totalCube,
                    totalGrossWeight = Ordini.Orders_totalGrossWeight,
                    totalNetWeight = Ordini.Orders_totalNetWeight,
                    totalPacks = Ordini.Orders_totalPacks,
                    totalQty = Ordini.Orders_totalQty,
                    tripDocNumber = Ordini.Orders_tripDocNumber,
                    tripID = Ordini.Orders_tripID,
                    unloadAddress = Ordini.Orders_unloadAddress,
                    unloadCountry = Ordini.Orders_unloadCountry,
                    unloadDistrict = Ordini.Orders_unloadDistrict,
                    unloadID = Ordini.Orders_unloadID,
                    unloadLocation = Ordini.Orders_unloadLocation,
                    unloadRegion = Ordini.Orders_unloadRegion,
                    unloadZipCode = Ordini.Orders_unloadZipCode
                };
                var nO = new RootObjectOrderViewModel()
                {
                    header = nH
                };

                return nO;

            }
            catch (Exception ee)
            {

                throw;
            }
        }
        public static Orders ClonaFromJsonToCRMOrderDB(RootObjectOrderViewModel ordine)
        {
            try
            {
                return new Orders()
                {
                    Orders_consigneeDes = ordine.header.consigneeDes,
                    Orders_customerDes = ordine.header.customerDes,
                    Orders_customerID = ordine.header.customerID,
                    Orders_docDate = ordine.header.docDate,
                    Orders_docNumber = ordine.header.docNumber,
                    Orders_GespeUniq = ordine.header.id,
                    Orders_reference = ordine.header.reference,
                    Orders_referenceDate = ordine.header.referenceDate,
                    Orders_reTypeID = ordine.header.regTypeID,
                    Orders_senderDes = ordine.header.senderDes,
                    Orders_statusDes = ordine.header.statusDes,
                    Orders_unloadDes = ordine.header.unLoadDes,
                    Orders_consigneeAddress = ordine.header.consigneeAddress,
                    Orders_consigneeCountry = ordine.header.consigneeCountry,
                    Orders_consigneeDistrict = ordine.header.consigneeDistrict,
                    Orders_consigneeID = ordine.header.consigneeID,
                    Orders_consigneeLocation = ordine.header.consigneeLocation,
                    Orders_consigneeRegion = ordine.header.consigneeRegion,
                    Orders_consigneeZipCode = ordine.header.consigneeZipCode,
                    Orders_coverage = ordine.header.coverage,
                    Orders_deliveryNote = ordine.header.deliveryNote,
                    Orders_executed = ordine.header.executed,
                    Orders_externalID = ordine.header.externalID,
                    Orders_info1 = ordine.header.info1,
                    Orders_info2 = ordine.header.info2,
                    Orders_info3 = ordine.header.info3,
                    Orders_info4 = ordine.header.info4,
                    Orders_info5 = ordine.header.info5,
                    Orders_info6 = ordine.header.info6,
                    Orders_info7 = ordine.header.info7,
                    Orders_info8 = ordine.header.info8,
                    Orders_info9 = ordine.header.info9,
                    Orders_internalNote = ordine.header.internalNote,
                    Orders_ownerDes = ordine.header.ownerDes,
                    Orders_ownerID = ordine.header.ownerID,
                    Orders_planned = ordine.header.planned,
                    Orders_publicNote = ordine.header.publicNode,
                    Orders_reference2 = ordine.header.reference2,
                    Orders_reference2Date = ordine.header.reference2Date,
                    Orders_rowsNo = ordine.header.rowsNo,
                    Orders_senderAddress = ordine.header.senderAddress,
                    Orders_senderCountry = ordine.header.senderCountry,
                    Orders_senderDistrict = ordine.header.senderDistrict,
                    Orders_senderID = ordine.header.senderID,
                    Orders_senderLocation = ordine.header.senderLocation,
                    Orders_senderRegion = ordine.header.senderRegion,
                    Orders_senderZipCode = ordine.header.senderZipCode,
                    Orders_shipDocNumber = ordine.header.shipDocNumber,
                    Orders_shipID = ordine.header.shipID,
                    Orders_siteID = ordine.header.siteID,
                    Orders_statusId = ordine.header.statusId,
                    Orders_totalBoxes = ordine.header.totalBoxes,
                    Orders_totalCube = ordine.header.totalCube,
                    Orders_totalGrossWeight = ordine.header.totalGrossWeight,
                    Orders_totalNetWeight = ordine.header.totalNetWeight,
                    Orders_totalPacks = ordine.header.totalPacks,
                    Orders_totalQty = ordine.header.totalQty,
                    Orders_tripDocNumber = ordine.header.tripDocNumber,
                    Orders_tripID = ordine.header.tripID,
                    Orders_unloadAddress = ordine.header.unloadAddress,
                    Orders_unloadCountry = ordine.header.unloadCountry,
                    Orders_unloadDistrict = ordine.header.unloadDistrict,
                    Orders_unloadID = ordine.header.unloadID,
                    Orders_unloadLocation = ordine.header.unloadLocation,
                    Orders_unloadRegion = ordine.header.unloadRegion,
                    Orders_unloadZipCode = ordine.header.unloadZipCode

                };

            }
            catch (Exception ee)
            {

                throw;
            }
        }

        public static TempOrderJsonModel ConvertTempOrdersToJsonModel(Temp_Orders order)
        {
            return new TempOrderJsonModel()
            {
                AgentID = order.AgentID,
                OrderConfirmed = order.OrderConfirmed,
                ConsigneeLocationID = order.ConsigneeLocationID,
                CustomerID = order.CustomerID,
                DeliveryNote = order.DeliveryNote,
                OrderReference = order.OrderReference,
                OrderReferenceDate = order.OrderReferenceDate,
                OrderSended = order.OrderSended,
                OrderType = order.OrderType,
                UnloadLocationID = order.UnloadLocationID,
                XCMNote = order.XCMNote,
                OrderID = order.OrderID
            };
        }

        public static TempOrderProductsJsonModel ConvertTempOrderProductsToJsonModel(Temp_Order_Products products)
        {
            return new TempOrderProductsJsonModel()
            {
                ID = products.ID,
                Order_ID = products.Order_ID,
                Product_PartNumber = products.Product_PartNumber,
                Product_Price = products.Product_Price,
                Product_Quantity = products.Product_Quantity,
                Product_Des = products.Product_Des,
                Product_Discount = products.Product_Discount,
                Product_Iva = products.Product_Iva
                
            };
        }
        #endregion

        #region Shipments
        public static ShipmentList ClonaFromJsonToShipmentDB(ShipmentListJson daJson)
        {
            return new ShipmentList()
            {
                Shipment_AttachCount = daJson.attachCount,
                Shipment_CashCurrency = daJson.cashCurrency,
                Shipment_CashNote = daJson.cashNote,
                Shipment_CashPayment = daJson.cashPayment,
                Shipment_CashValue = daJson.cashValue,
                Shipment_ConsineeDes = daJson.consigneeDes,
                Shipment_ConsineeID = daJson.consigneeID,
                Shipment_Cube = daJson.cube,
                Shipment_CustomerDes = daJson.customerDes,
                Shipment_CustomerID = daJson.customerID,
                Shipment_DeliveryDateTime = daJson.deliveryDateTime,
                Shipment_DocDate = daJson.docDate,
                Shipment_DocNumber = daJson.docNumber,
                Shipment_ExternalRef = daJson.externRef,
                Shipment_FirstStopDes = daJson.firstStopDes,
                Shipment_FirstStopID = daJson.firstStopID,
                Shipment_FloorPallets = daJson.floorPallets,
                Shipment_GespeID_UNITEX = daJson.id,
                Shipment_GrossWeight = daJson.grossWeight,
                Shipment_InsideRef = daJson.insideRef,
                Shipment_LastStopDes = daJson.lastStopDes,
                Shipment_LastStopID = daJson.lastStopID,
                Shipment_Meters = daJson.meters,
                Shipment_NetWeight = daJson.netWeight,
                Shipment_OwnerAgency = daJson.ownerAgency,
                Shipment_Packs = daJson.packs,
                Shipment_PickupDateTime = daJson.pickupDateTime,
                Shipment_PickupSupplierDes = daJson.pickupSupplierDes,
                Shipment_PickupSupplierID = daJson.pickupSupplierID,
                Shipment_SenderDes = daJson.senderDes,
                Shipment_SenderID = daJson.senderID,
                Shipment_ServiceType = daJson.serviceType,
                Shipment_StatusDes = daJson.statusDes,
                Shipment_StatusID = daJson.statusId,
                Shipment_StatusType = daJson.statusType,
                Shipment_TotalPallets = daJson.totalPallets,
                Shipment_TransportType = daJson.transportType,
                Shipment_WebOrderID = daJson.webOrderID,
                Shipment_WebOrderNumber = daJson.webOrderNumber,
                Shipment_WebStatusID = daJson.webStatusId,
                Shipment_WebStatusType = daJson.webStatusType
            };
        }

        public static ShipmentJsonModel ConvertShipmentToJsonModel(ShipmentList ship)
        {
            return new ShipmentJsonModel()
            {
                Shipment_AttachCount = ship.Shipment_AttachCount,
                Shipment_CashCurrency = ship.Shipment_CashCurrency,
                Shipment_CashNote = ship.Shipment_CashNote,
                Shipment_CashPayment = ship.Shipment_CashPayment,
                Shipment_CashValue = ship.Shipment_CashValue,
                Shipment_ConsineeDes = ship.Shipment_ConsineeDes,
                Shipment_ConsineeID = ship.Shipment_ConsineeID,
                Shipment_Cube = ship.Shipment_Cube,
                Shipment_CustomerDes = ship.Shipment_CustomerDes,
                Shipment_CustomerID = ship.Shipment_CustomerID,
                Shipment_DeliveryDateTime = ship.Shipment_DeliveryDateTime,
                Shipment_DocDate = ship.Shipment_DocDate,
                Shipment_DocNumber = ship.Shipment_DocNumber,
                Shipment_ExternalRef = ship.Shipment_ExternalRef,
                Shipment_FirstStopDes = ship.Shipment_FirstStopDes,
                Shipment_FirstStopID = ship.Shipment_FirstStopID,
                Shipment_FloorPallets = ship.Shipment_FloorPallets,
                Shipment_GespeID_UNITEX = ship.Shipment_GespeID_UNITEX,
                Shipment_GrossWeight = ship.Shipment_GrossWeight,
                Shipment_InsideRef = ship.Shipment_InsideRef,
                Shipment_LastStopDes = ship.Shipment_LastStopDes,
                Shipment_LastStopID = ship.Shipment_LastStopID,
                Shipment_Meters = ship.Shipment_Meters,
                Shipment_NetWeight = ship.Shipment_NetWeight,
                Shipment_OwnerAgency = ship.Shipment_OwnerAgency,
                Shipment_Packs = ship.Shipment_Packs,
                Shipment_PickupDateTime = ship.Shipment_PickupDateTime,
                Shipment_PickupSupplierDes = ship.Shipment_PickupSupplierDes,
                Shipment_PickupSupplierID = ship.Shipment_PickupSupplierID,
                Shipment_SenderDes = ship.Shipment_SenderDes,
                Shipment_SenderID = ship.Shipment_SenderID,
                Shipment_ServiceType = ship.Shipment_ServiceType,
                Shipment_StatusDes = ship.Shipment_StatusDes,
                Shipment_StatusID = ship.Shipment_StatusID,
                Shipment_StatusType = ship.Shipment_StatusType,
                Shipment_TotalPallets = ship.Shipment_TotalPallets,
                Shipment_TransportType = ship.Shipment_TransportType,
                Shipment_WebOrderID = ship.Shipment_WebOrderID,
                Shipment_WebOrderNumber = ship.Shipment_WebOrderNumber,
                Shipment_WebStatusID = ship.Shipment_WebStatusID,
                Shipment_WebStatusType = ship.Shipment_WebStatusType,
                Shipment_CustomerID_XCM = ship.Shipment_CustomerID_XCM,
                Shipment_GespeID_XCM = ship.Shipment_GespeID_XCM,
                ID_SHIPMENT = ship.ID_SHIPMENT
            };
        }

        public static TrackingJsonModel ConvertTrackingToJsonModel(Tracking track)
        {
            return new TrackingJsonModel()
            {
                Tracking_Data = track.Tracking_Data,
                ID_TRACKING = track.ID_TRACKING,
                Tracking_ShipmentID = track.Tracking_ShipmentID,
                Tracking_StatusDes = track.Tracking_StatusDes,
                Tracking_StatusID = track.Tracking_StatusID
            };
        }

        #endregion

        #region ClientUser
        public static ClientJsonModel ConvertClientUserToClientJsonModel(Client_User clientUsersDb)
        {

            return new ClientJsonModel()
            {
                FK_UserClient_Customer_ID = clientUsersDb.FK_UserClient_Customer_ID,
                UserClient_CreationDate = clientUsersDb.UserClient_CreationDate,
                UserClient_LastModifiedDate = clientUsersDb.UserClient_LastModifiedDate,
                UserClient_LastModifiedUserID = clientUsersDb.UserClient_LastModifiedUserID,
                UserClient_Name = clientUsersDb.UserClient_Name,
                UserClient_UserID = clientUsersDb.UserClient_UserID

            };
        }
        public static Client_User ConvertClientJsonModelToClientUser(ClientJsonModel clientUsersDb)
        {

            return new Client_User()
            {
                FK_UserClient_Customer_ID = clientUsersDb.FK_UserClient_Customer_ID,
                UserClient_CreationDate = clientUsersDb.UserClient_CreationDate,
                UserClient_LastModifiedDate = clientUsersDb.UserClient_LastModifiedDate,
                UserClient_LastModifiedUserID = clientUsersDb.UserClient_LastModifiedUserID,
                UserClient_Name = clientUsersDb.UserClient_Name,
                UserClient_UserID = clientUsersDb.UserClient_UserID

            };
        }
        #endregion

    }
}