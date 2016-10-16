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

namespace Rsft.HttpRequestIp.Tests.Unit.Logic
{
    using System.Collections.Specialized;

    using NUnit.Framework;

    using HttpRequestIp.Logic;

    using Entities;

    [TestFixture]
    public class AddressGuessResolverCloudFlareReverseProxyTests
    {

        /// <summary>
        /// The test object
        /// </summary>
        private static readonly AddressGuessResolverCloudFlareReverseProxy TestObj = new AddressGuessResolverCloudFlareReverseProxy();

        [Test]
        public static void GetGuess_WhenNonProxied_ExpectNoProxyDetected()
        {
            // arrange

            // act
            var addressGuessResolverResponse = TestObj.GetGuess(
                new AddressGuessResolverRequest { ServerVariablesNameValueCollection = CloudFlareServerVarsNonProxied });

            // assert
            StringAssert.AreEqualIgnoringCase("GB", addressGuessResolverResponse.IpCountry);
            Assert.That(addressGuessResolverResponse.IsProxied == false);
            StringAssert.AreEqualIgnoringCase("176.253.76.186", addressGuessResolverResponse.BestGuessIp);
            Assert.That(addressGuessResolverResponse.ServerVariables != null);
        }

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }

        /// <summary>
        /// Gets the standard server vars non proxied.
        /// </summary>
        /// <value>
        /// The standard server vars non proxied.
        /// </value>
        private static NameValueCollection CloudFlareServerVarsNonProxied
        {
            get
            {
                var rtn = new NameValueCollection();

                rtn.Add("REMOTE_ADDR", "141.101.98.177");
                rtn.Add("REMOTE_HOST", "141.101.98.177");
                rtn.Add("HTTP_X_FORWARDED_FOR", "176.253.76.186, 141.101.98.177:32389");
                rtn.Add("HTTP_CF_CONNECTING_IP", "176.253.76.186");
                rtn.Add("HTTP_CF_IPCOUNTRY", "GB");

                return rtn;
            }
        }
    }
}