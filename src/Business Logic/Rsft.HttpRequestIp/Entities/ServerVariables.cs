// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerVariables.cs" company="Rolosoft Ltd">
//   © Rolosoft Ltd
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Rsft.HttpRequestIp.Entities
{
    /// <summary>
    /// The server variables.
    /// </summary>
    public class ServerVariables
    {
        #region Public Properties

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

        #endregion
    }
}