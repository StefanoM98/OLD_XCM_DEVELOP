using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class AgentAuthorizationJsonModel
    {
        public long AgentAuthorization_ID { get; set; }
        public string FK_AgentAuthorization_User_ID { get; set; }
        public bool AgentAuthorization_OrderConfirmation { get; set; }
        public System.DateTime AgentAuthorization_CreationDate { get; set; }
        public System.DateTime AgentAuthorization_LastModifiedDate { get; set; }
        public string AgentAuthorization_LastModifiedUserID { get; set; }
    }
}