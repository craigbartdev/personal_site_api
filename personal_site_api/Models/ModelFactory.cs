using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace personal_site_api.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;

        public ModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new UrlHelper(request);
        }
    }
}