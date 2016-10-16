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

    using Entities;
    using HttpRequestIp.Logic;

    [TestFixture]
    public class AddressGuessResolverNoneReverseProxyTests
    {
        private static readonly AddressGuessResolverNoneReverseProxy TestObj = new AddressGuessResolverNoneReverseProxy();

        [Test]
        public static void GetGuess_WhenNonProxied_ExpectNoProxyDetected()
        {
            // arrange
            
            // act
            var addressGuessResolverResponse = TestObj.GetGuess(
                new AddressGuessResolverRequest { ServerVariablesNameValueCollection = StandardServerVarsNonProxied });

            // assert
            Assert.That(addressGuessResolverResponse.IpCountry == string.Empty);
            Assert.That(addressGuessResolverResponse.IsProxied == false);
            StringAssert.AreEqualIgnoringCase("176.253.76.186", addressGuessResolverResponse.BestGuessIp);
            Assert.That(addressGuessResolverResponse.ServerVariables != null);
        }

        [Test]
        public static void GetGuess_WhenProxied_ExpectProxyDetected()
        {
            // arrange

            // act
            var addressGuessResolverResponse = TestObj.GetGuess(
                new AddressGuessResolverRequest { ServerVariablesNameValueCollection = StandardServerVarsHttpProxied });

            // assert
            Assert.That(addressGuessResolverResponse.IpCountry == string.Empty);
            Assert.That(addressGuessResolverResponse.IsProxied);
            StringAssert.AreEqualIgnoringCase("47.88.104.219", addressGuessResolverResponse.BestGuessIp);
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
        private static NameValueCollection StandardServerVarsNonProxied
        {
            get
            {
                var rtn = new NameValueCollection();

                rtn.Add("REMOTE_ADDR", "176.253.76.186");
                rtn.Add("REMOTE_HOST", "176.253.76.186");
                rtn.Add("HTTP_X_FORWARDED_FOR", "176.253.76.186:58968");

                return rtn;
            }
        }

        private static NameValueCollection StandardServerVarsHttpProxied
        {
            get
            {
                var rtn = new NameValueCollection();

                rtn.Add("REMOTE_ADDR", "47.88.104.219");
                rtn.Add("REMOTE_HOST", "47.88.104.219");
                rtn.Add("HTTP_X_FORWARDED_FOR", "185.1.43.2:33990");
                rtn.Add("HTTP_FROM", "185.1.43.2");

                return rtn;
            }
        }

    }
}