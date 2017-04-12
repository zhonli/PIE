using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace PIEM.API
{
    //Cross-Origin Resource Sharing configuration
    public class PlatformCorsPolicy : ICorsPolicyProvider
    {
        private static PlatformCorsPolicy _instance;
        private CorsPolicy _policy;
        private PlatformCorsPolicy()
        {
            _policy = new CorsPolicy();

            _policy.AllowAnyHeader = true;
            _policy.AllowAnyMethod = true;
            _policy.AllowAnyOrigin = true;
        }

        public static PlatformCorsPolicy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlatformCorsPolicy();
                }
                return _instance;
            }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}