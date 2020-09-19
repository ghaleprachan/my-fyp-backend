using Roof_Care.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.AuthenticationDecode
{
    public static class EncodeDecode
    {
        public static dynamic GetUserId(string encodedId)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(encodedId);
                string[] decodedInfo = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Split(':');
                return Int32.Parse(decodedInfo[0]);
            }
            catch (Exception)
            {
                return GlobalDecleration._unauthorized;
            }
        }

        public static dynamic EncodeUserId(int id, string username)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(id + ":" + username);
                string encryptionToken = System.Convert.ToBase64String(plainTextBytes);
                return encryptionToken;
            }
            catch (Exception)
            {
                return GlobalDecleration._internalServerError;
            }
        }
    }
}