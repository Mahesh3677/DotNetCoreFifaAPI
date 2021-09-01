using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Contracts.Responses
{
    public class ErrorModel
    {
        public string FieldName { get; set; }

        public string Messsage { get; set; }
    }
}
