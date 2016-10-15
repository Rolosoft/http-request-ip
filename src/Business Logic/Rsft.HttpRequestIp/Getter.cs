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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Entities;

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
        /// The splitter1
        /// </summary>
        private static readonly char[] Splitter1 = { ',' };

        /// <summary>
        /// Gets the best guess IP.
        /// </summary>
        /// <value>
        /// The best guess IP.
        /// </value>
        /// <exception cref="HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
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
        /// <exception cref="HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        public static RequestInfo Get()
        {
            var serverVariables = GetServerVariables();

            return new RequestInfo
                       {
                           BestGuessIp = BestGuessIp,
                           IsProxied = IsProxyDetected,
                           ServerVariables = serverVariables,
                           IpCountry = serverVariables.IpCountry
                       };
        }

        /// <summary>
        /// The get server variable.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        private static string GetServerVariable(List<Tuple<int, string, bool>> keys)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            if (keys == null)
            {
                return null;
            }

            if (!keys.Any())
            {
                return null;
            }

            string rtn = null;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var variable in keys.OrderBy(r => r.Item1))
            {
                if (string.IsNullOrWhiteSpace(variable.Item2))
                {
                    continue;
                }

                var nameValueCollection = HttpContext.Current.Request.ServerVariables;

                var i = nameValueCollection[variable.Item2];

                // if asked to take first in possible comma delimited, parse and take else take raw value;
                rtn = variable.Item3 ? GetFirstDelimitFromServerVar(i) : i;
            }

            return rtn;
        }

        /// <summary>
        /// Gets the server variables.
        /// </summary>
        /// <returns>The <see cref="ServerVariables"/>.</returns>
        /// <exception cref="HttpException">The web application is running under IIS7 in Integrated mode and HttpContext cannot be used in application startup.</exception>
        private static ServerVariables GetServerVariables()
        {
            var serverVariables = new ServerVariables
                                      {
                                          HttpForwardedForHeader =
                                              GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_FORWARDED", false) }),
                                          HttpForwardedHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_FROM", false) }),
                                          HttpViaHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_VIA", false) }),
                                          HttpXForwardedForHeader =
                                              GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_X_FORWARDED_FOR", true) }),
                                          RemoteAddressHeader =
                                              GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_CF_CONNECTING_IP", false), new Tuple<int, string, bool>(1, "REMOTE_ADDR", false) }),
                                          IpCountry = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_CF_IPCOUNTRY", false) })
                                      };

            return serverVariables;
        }

        private static string GetFirstDelimitFromServerVar(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!value.Contains(","))
            {
                return value;
            }

            var strings = value.Split(Splitter1);

            return strings.FirstOrDefault();
        }
    }
}