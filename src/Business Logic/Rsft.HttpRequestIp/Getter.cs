/*
Copyright 2013 - 2016 Rolosoft.com

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
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Entities;
    using Interfaces;
    using Logic;

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
        /// The address guess resolver none lazy
        /// </summary>
        private static readonly Lazy<AddressGuessResolverNoneReverseProxy> AddressGuessResolverNoneLazy = new Lazy<AddressGuessResolverNoneReverseProxy>(() => new AddressGuessResolverNoneReverseProxy());

        /// <summary>
        /// The address guess resolver cloud flare lazy
        /// </summary>
        private static readonly Lazy<AddressGuessResolverCloudFlareReverseProxy> AddressGuessResolverCloudFlareLazy = new Lazy<AddressGuessResolverCloudFlareReverseProxy>(() => new AddressGuessResolverCloudFlareReverseProxy());

        /// <summary>
        /// The address resolver lazy
        /// </summary>
        private static Lazy<IAddressGuessResolver<AddressGuessResolverRequest, AddressGuessResolverResponse>> addressResolverLazy;

        /// <summary>
        /// My reverse proxy type
        /// </summary>
        private static ReverseProxyType myReverseProxyType;

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="reverseProxyType">the <see cref="ReverseProxyType" /> of the reverse proxy.
        /// <remarks>Default is ReverseProxyType.AutoDetect</remarks></param>
        /// <param name="serverVars">The server variables.</param>
        /// <returns>
        /// The <see cref="RequestInfo" />.
        /// </returns>
        public static RequestInfo Get(ReverseProxyType reverseProxyType = ReverseProxyType.AutoDetect, NameValueCollection serverVars = null)
        {
            Contract.Requires(Enum.IsDefined(typeof(ReverseProxyType), reverseProxyType));

            myReverseProxyType = reverseProxyType;

            var rtn = new RequestInfo();

            var varsToUse = serverVars ?? HttpContext.Current.Request.ServerVariables;

            var addressGuessResolver = GuessResolver(varsToUse);

            var addressGuessResolverResponse = addressGuessResolver.GetGuess(new AddressGuessResolverRequest { ServerVariablesNameValueCollection = varsToUse });

            rtn.IpCountry = addressGuessResolverResponse.IpCountry;
            rtn.BestGuessIp = addressGuessResolverResponse.BestGuessIp;
            rtn.IsProxied = addressGuessResolverResponse.IsProxied;
            rtn.ServerVariables
                = new ServerVariables
                        {
                            HttpForwardedForHeader = addressGuessResolverResponse.ServerVariables.HttpForwardedForHeader,
                            IpCountry = addressGuessResolverResponse.ServerVariables.IpCountry,
                            RemoteAddressHeader = addressGuessResolverResponse.ServerVariables.RemoteAddressHeader,
                            HttpForwardedHeader = addressGuessResolverResponse.ServerVariables.HttpForwardedHeader,
                            HttpViaHeader = addressGuessResolverResponse.ServerVariables.HttpViaHeader,
                            HttpXForwardedForHeader = addressGuessResolverResponse.ServerVariables.HttpXForwardedForHeader
                        };

            return rtn;
        }

        /// <summary>
        /// Gets the guess resolver.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>The <see cref="IAddressGuessResolver&lt;AddressGuessResolverRequest, AddressGuessResolverResponse&gt;"/></returns>
        /// <value>
        /// The guess resolver.
        /// </value>
        private static IAddressGuessResolver<AddressGuessResolverRequest, AddressGuessResolverResponse> GuessResolver(NameValueCollection nameValueCollection)
        {
            if (addressResolverLazy != null)
            {
                return addressResolverLazy.Value;
            }

            IAddressGuessResolver<AddressGuessResolverRequest, AddressGuessResolverResponse> rtn;

            switch (myReverseProxyType)
            {
                case ReverseProxyType.None:
                    rtn = AddressGuessResolverNoneLazy.Value;
                    break;
                case ReverseProxyType.CloudFlare:
                    rtn = AddressGuessResolverCloudFlareLazy.Value;
                    break;
                case ReverseProxyType.AutoDetect:
                    if (nameValueCollection["HTTP_CF_CONNECTING_IP"] != null)
                    {
                        rtn = AddressGuessResolverCloudFlareLazy.Value;
                    }
                    else
                    {
                        rtn = AddressGuessResolverNoneLazy.Value;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (addressResolverLazy == null)
            {
                addressResolverLazy = new Lazy<IAddressGuessResolver<AddressGuessResolverRequest, AddressGuessResolverResponse>>(() => rtn);
            }

            return addressResolverLazy.Value;
        }
    }
}