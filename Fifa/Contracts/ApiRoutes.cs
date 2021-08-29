using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root+"/"+Version;
        public static class Posts 
        {
            public const string Getall =Base + "/posts";

            public const string Create = Base + "/posts";

            public const string Update = Base + "/posts/{postId}";

            public const string Get = Base + "/posts/{postId}";

            public const string Delete = Base + "/posts/{postId}";
        }
    }
}
