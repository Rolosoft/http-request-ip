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

namespace Rsft.HttpRequestIp.Logic
{
    using Entities;

    /// <summary>
    /// Address Guess Resolver None Reverse Proxy
    /// </summary>
    /// <seealso cref="AddressGuessResolverBase" />
    internal sealed class AddressGuessResolverNoneReverseProxy : AddressGuessResolverBase
    {
        /// <summary>
        /// Gets the guess.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="TResponse" />.
        /// </returns>
        public override AddressGuessResolverResponse GetGuess(AddressGuessResolverRequest request)
        {
            var serverVariables = GetServerVariables(request.ServerVariablesNameValueCollection);

            var rtnObj = new AddressGuessResolverResponse();

            var rtn = string.Empty;

            if (!string.IsNullOrWhiteSpace(serverVariables.RemoteAddressHeader))
            {
                rtn = serverVariables.RemoteAddressHeader;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(serverVariables.HttpXForwardedForHeader))
                {
                    rtn = serverVariables.HttpXForwardedForHeader;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(serverVariables.HttpForwardedHeader))
                    {
                        rtn = serverVariables.HttpForwardedHeader;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(serverVariables.HttpXForwardedForHeader))
                        {
                            rtn = serverVariables.HttpXForwardedForHeader;
                        }
                    }
                }
            }

            rtnObj.BestGuessIp = rtn;
            rtnObj.IsProxied = this.IsProxyDetected(serverVariables);
            rtnObj.IpCountry = string.Empty;
            rtnObj.ServerVariables = serverVariables;

            return rtnObj;
        }

        /// <summary>
        /// Determines whether [is proxy detected] [the specified server variables].
        /// </summary>
        /// <param name="serverVariables">The server variables.</param>
        /// <returns>
        ///   <c>true</c> if [is proxy detected] [the specified server variables]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsProxyDetected(ServerVariables serverVariables)
        {
            var proxyDetected = !string.IsNullOrWhiteSpace(serverVariables.HttpForwardedHeader)
                                    || !string.IsNullOrWhiteSpace(serverVariables.HttpForwardedForHeader)
                                    || !string.IsNullOrWhiteSpace(serverVariables.HttpViaHeader);

            return proxyDetected;
        }
    }
}