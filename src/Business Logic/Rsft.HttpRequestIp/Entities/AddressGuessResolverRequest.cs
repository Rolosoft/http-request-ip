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
    using System.Collections.Specialized;

    /// <summary>
    /// Address Guess Resolver Request
    /// </summary>
    internal sealed class AddressGuessResolverRequest
    {
        /// <summary>
        /// Gets or sets the server variables name value collection.
        /// </summary>
        /// <value>
        /// The server variables name value collection.
        /// </value>
        public NameValueCollection ServerVariablesNameValueCollection { get; set; }
    }
}