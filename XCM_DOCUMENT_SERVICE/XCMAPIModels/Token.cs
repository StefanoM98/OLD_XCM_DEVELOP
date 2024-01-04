﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCM_DOCUMENT_SERVICE.XCMAPIModels
{

    public class Token
    {
        public TokenResult result { get; set; }
        public TokenUser user { get; set; }
    }

    public class TokenResult
    {
        public object[] messages { get; set; }
        public bool status { get; set; }
        public object info { get; set; }
        public int maxPages { get; set; }
    }

    public class TokenUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lang { get; set; }
        public int type { get; set; }
        public string filter { get; set; }
        public DateTime expire { get; set; }
        public string token { get; set; }
        public object settings { get; set; }
        public string agency { get; set; }
    }
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}