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

namespace Rsft.HttpRequestIp.Entities
{
    /// <summary>
    /// The server variables.
    /// </summary>
    public class ServerVariables
    {
        /// <summary>
        /// Gets or sets the http forwarded for header.
        /// </summary>
        public string HttpForwardedForHeader { get; set; }

        /// <summary>
        /// Gets or sets the http forwarded header.
        /// </summary>
        public string HttpForwardedHeader { get; set; }

        /// <summary>
        /// Gets or sets the http via header.
        /// </summary>
        public string HttpViaHeader { get; set; }

        /// <summary>
        /// Gets or sets the http x forwarded for header.
        /// </summary>
        public string HttpXForwardedForHeader { get; set; }

        /// <summary>
        /// Gets or sets the remote address header.
        /// </summary>
        public string RemoteAddressHeader { get; set; }

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
    }
}