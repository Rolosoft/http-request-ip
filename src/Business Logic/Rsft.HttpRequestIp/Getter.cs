/*
Copyright 2013 Rolosoft.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

namespace Rsft.HttpRequestIp
{
    using System.Web;

    using Rsft.HttpRequestIp.Entities;
    
    /// <summary>
    /// Gets the best guess of a users (hosts) IP address.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         As HTTP is an application level protocol, it is not always possible to identify a host request IP (as TCP sits much lower in the OSI stack than HTTP) with 100% accuracy.
    ///     </para>
    ///     <para>
    ///         The host IP can be obscured by proxies or firewalls that are HTTP transport specific. Our internal algorithms inspect the HTTP stream for common properties and can make "best guesses" on the details.
    ///     </para>
    ///     <para>
    ///         No HTTP level inspection can promise 100% accuracy but <c>Rsft.HttpRequestIp</c> can deliver high accuracy to detect an end users (host) IP address.
    ///     </para>
    ///     <para>
    ///         Uses of this component include IP Geo targeting, IP blocking and user auditing.
    ///     </para>
    /// </remarks>
    public static class Getter
    {
        /// <summary>
        /// Gets the best guess IP.
        /// </summary>
        /// <value>
        /// The best guess IP.
        /// </value>
        /// <exception cref="System.Web.HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        private static string BestGuessIp
        {
            get
            {
                var rtn = string.Empty;

                if (!string.IsNullOrWhiteSpace(GetServerVariables().RemoteAddressHeader))
                {
                    rtn = GetServerVariables().RemoteAddressHeader;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(GetServerVariables().HttpXForwardedForHeader))
                    {
                        rtn = GetServerVariables().HttpXForwardedForHeader;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(GetServerVariables().HttpForwardedHeader))
                        {
                            rtn = GetServerVariables().HttpForwardedHeader;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(GetServerVariables().HttpXForwardedForHeader))
                            {
                                rtn = GetServerVariables().HttpXForwardedForHeader;
                            }
                        }
                    }
                }

                return rtn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is proxy detected.
        /// </summary>
        /// <exception cref="System.Web.HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        private static bool IsProxyDetected
        {
            get
            {
                var proxyDetected = !string.IsNullOrWhiteSpace(GetServerVariables().HttpXForwardedForHeader)
                                    || !string.IsNullOrWhiteSpace(GetServerVariables().HttpForwardedHeader)
                                    || !string.IsNullOrWhiteSpace(GetServerVariables().HttpForwardedForHeader)
                                    || !string.IsNullOrWhiteSpace(GetServerVariables().HttpViaHeader);

                return proxyDetected;
            }
        }
        

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="RequestInfo"/>.
        /// </returns>
        /// <exception cref="System.Web.HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        public static RequestInfo Get()
        {
            return new RequestInfo
                       {
                           BestGuessIp = BestGuessIp,
                           IsProxied = IsProxyDetected,
                           ServerVariables = GetServerVariables()
                       };
        }

        /// <summary>
        /// The get server variable.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="System.Web.HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        private static string GetServerVariable(string key)
        {
            return HttpContext.Current == null ? null : HttpContext.Current.Request.ServerVariables[key];
        }

        /// <summary>
        /// Gets the server variables.
        /// </summary>
        /// <returns>The <see cref="ServerVariables"/>.</returns>
        /// <exception cref="System.Web.HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        private static ServerVariables GetServerVariables()
        {
            var serverVariables = new ServerVariables
                                      {
                                          HttpForwardedForHeader =
                                              GetServerVariable("HTTP_FORWARDED"),
                                          HttpForwardedHeader = GetServerVariable("HTTP_FROM"),
                                          HttpViaHeader = GetServerVariable("HTTP_VIA"),
                                          HttpXForwardedForHeader =
                                              GetServerVariable("HTTP_X_FORWARDED_FOR"),
                                          RemoteAddressHeader =
                                              GetServerVariable("REMOTE_ADDR")
                                      };

            return serverVariables;
        }
    }
}