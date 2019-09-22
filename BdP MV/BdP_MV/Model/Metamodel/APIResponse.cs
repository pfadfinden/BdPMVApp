using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Model.Metamodel
{
  
        public class APIResponse
    {
            public object servicePrefix { get; set; }
            public object methodCall { get; set; }
            public JObject response { get; set; }
            public int statusCode { get; set; }
            public string statusMessage { get; set; }
            public string apiSessionName { get; set; }
            public object apiSessionToken { get; set; }
            public int minorNumber { get; set; }
            public int majorNumber { get; set; }
        }

     
}
