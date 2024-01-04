using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class AgentJsonModel
    {
        [Key]
        public string User_ID { get; set; }
        public string User_Username { get; set; }
        public string User_Email { get; set; }
        public string User_HashPassword { get; set; }
        public string User_Salt { get; set; }
        public string User_FirstName { get; set; }
        public string User_Surname { get; set; }
        public System.DateTime User_CreationDate { get; set; }
        public System.DateTime User_LastModifiedDate { get; set; }
        public Nullable<bool> User_Active { get; set; }
        public long FK_User_Role_ID { get; set; }
        public string FK_User_Customer_ID { get; set; }
        public bool User_PasswordRecovered { get; set; } 
        public AgentAuthorizationJsonModel User_AgentAuth { get; set; }
    }
}