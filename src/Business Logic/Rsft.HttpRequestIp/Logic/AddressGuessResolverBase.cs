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
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Entities;
    using Interfaces;

    /// <summary>
    /// The Address Guess Resolver Base.
    /// </summary>
    /// <seealso cref="Interfaces.IAddressGuessResolver{Rsft.HttpRequestIp.Entities.AddressGuessResolverRequest, Rsft.HttpRequestIp.Entities.AddressGuessResolverResponse}" />
    internal abstract class AddressGuessResolverBase : IAddressGuessResolver<AddressGuessResolverRequest, AddressGuessResolverResponse>
    {
        private static readonly char[] Splitter1 = { ',' };

        /// <summary>
        /// Gets the guess.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>
        /// The <see cref="TResponse" />.
        /// </returns>
        public abstract AddressGuessResolverResponse GetGuess(AddressGuessResolverRequest request);

        /// <summary>
        /// Determines whether [is proxy detected] [the specified server variables].
        /// </summary>
        /// <param name="serverVariables">The server variables.</param>
        /// <returns>
        ///   <c>true</c> if [is proxy detected] [the specified server variables]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsProxyDetected(ServerVariables serverVariables);

        /// <summary>
        /// Gets the server variables.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>The <see cref="ServerVariables"/>.</returns>
        protected static ServerVariables GetServerVariables(NameValueCollection nameValueCollection)
        {
            Contract.Requires(nameValueCollection != null);

            var serverVariables = new ServerVariables
                                      {
                                          HttpForwardedForHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_FORWARDED", false) }, nameValueCollection),
                                          HttpForwardedHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_FROM", false) }, nameValueCollection),
                                          HttpViaHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_VIA", false) }, nameValueCollection),
                                          HttpXForwardedForHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_X_FORWARDED_FOR", true) }, nameValueCollection),
                                          RemoteAddressHeader = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_CF_CONNECTING_IP", false), new Tuple<int, string, bool>(1, "REMOTE_ADDR", false) }, nameValueCollection),
                                          IpCountry = GetServerVariable(new List<Tuple<int, string, bool>> { new Tuple<int, string, bool>(0, "HTTP_CF_IPCOUNTRY", false) }, nameValueCollection)
                                      };

            return serverVariables;
        }

        /// <summary>
        /// The get server variable.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        private static string GetServerVariable(List<Tuple<int, string, bool>> keys, NameValueCollection nameValueCollection)
        {
            Contract.Requires(nameValueCollection != null);

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

                var i = nameValueCollection[variable.Item2];

                // if asked to take first in possible comma delimited, parse and take else take raw value;
                rtn = variable.Item3 ? GetFirstDelimitedFromServerVar(i) : i;

                if (!string.IsNullOrWhiteSpace(rtn))
                {
                    break;
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets the first delimited from server variable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The first or default value from a delimited string</returns>
        private static string GetFirstDelimitedFromServerVar(string value)
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