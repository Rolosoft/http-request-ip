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
    /// The Address Guess Resolver Cloud Flare Reverse Proxy
    /// </summary>
    /// <seealso cref="AddressGuessResolverBase" />
    internal class AddressGuessResolverCloudFlareReverseProxy : AddressGuessResolverBase
    {
        /// <summary>
        /// Gets the guess.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>
        /// The <see cref="TResponse" />.
        /// </returns>
        public override AddressGuessResolverResponse GetGuess(AddressGuessResolverRequest request)
        {
            var serverVariables = GetServerVariables(request.ServerVariablesNameValueCollection);

            var rtnObj = new AddressGuessResolverResponse
                             {
                                 BestGuessIp = serverVariables.RemoteAddressHeader,
                                 IpCountry = serverVariables.IpCountry,
                                 IsProxied = this.IsProxyDetected(serverVariables),
                                 ServerVariables = serverVariables
                             };

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
            //Unable to use CloudFlare for HTTP proxy detection.
            return false;
        }
    }
}