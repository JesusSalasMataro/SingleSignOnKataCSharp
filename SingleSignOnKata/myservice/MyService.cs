using SingleSignOnKata.sso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOnKata.myservice
{
    public class MyService
    {
        private SingleSignOnRegistry _ssoRegistry;
        private AuthenticationGateway _authGateway;

        public MyService(SingleSignOnRegistry registry, AuthenticationGateway authGateway)
        {
            _ssoRegistry = registry;
            _authGateway = authGateway;
        }

        public SSOToken InitService(String username, String password)
        {
            if (_authGateway.credentialsAreValid(username, password)) 
            {
                return _ssoRegistry.register_new_session(username, password);
            }

            return null;
        }

        public Response handleRequest(Request request)
        {
            string responseText;

            if (_ssoRegistry.is_valid(request.getSSOToken()))
            {
                responseText = "hello " + request.getName() + "!";
            }
            else
            {
                responseText = "Invalid token";
            }

            return new Response(responseText);
        }

        public void DisposeService(SSOToken token)
        {
            _ssoRegistry.unregister(token);
        }

    }
}
