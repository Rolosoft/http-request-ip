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

namespace Rsft.HttpRequestIp.Entities
{
    /// <summary>
    /// Address Guess Resolver Response
    /// </summary>
    internal sealed class AddressGuessResolverResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is proxied.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is proxied; otherwise, <c>false</c>.
        /// </value>
        public bool IsProxied { get; set; }

        /// <summary>
        /// Gets or sets the best guess IP.
        /// </summary>
        /// <value>
        /// The best guess IP.
        /// </value>
        public string BestGuessIp { get; set; }

        /// <summary>
        /// Gets or sets the IP country.
        /// </summary>
        /// <value>
        /// The IP country.
        /// </value>
        /// <remarks>
        /// Provided only if using CloudFlare.
        /// </remarks>
        public string IpCountry { get; set; }

        /// <summary>
        /// Gets or sets the server variables.
        /// </summary>
        /// <value>
        /// The server variables.
        /// </value>
        public ServerVariables ServerVariables { get; set; }
    }
}