using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Universal.RestSharp
{

    /// <summary>
    /// Specifies how a body paremeter should be encoded before transmission.
    /// </summary>
    public enum ParameterEncoding
    {

        /// <summary>
        /// 
        /// </summary>
        Base64,

        /// <summary>
        /// 
        /// </summary>
        ByteArray,

        /// <summary>
        /// 
        /// </summary>
        UriEncoded,

        /// <summary>
        /// 
        /// </summary>
        Unencoded
    }
}
