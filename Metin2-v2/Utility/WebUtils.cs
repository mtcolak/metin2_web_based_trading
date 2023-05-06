using Metin2_v2.Helper;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace Metin2_v2.Utility
{
    public static class WebUtils
    {
        #region PostWebRequest

        public static string PostWebRequest(string destinationUrl, string data, string contentType, string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            //request.Timeout = 3000;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            request.ContentType = contentType;
            request.ContentLength = bytes.Length;
            request.Method = method;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }

        #endregion

        #region GetWebRequest

        public static string GetWebRequest(string destinationUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            //request.Timeout = 1000;
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Method = "GET";
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                //Logging.Logger.Write("Request", string.Concat(destinationUrl, " : ", responseStr));
                return responseStr;
            }
            return null;
        }

        #endregion

        #region GetWebRequestToFile

        public static Tuple<int, string> GetWebRequestToFile(string url, string filename)
        {
            try
            {
                var s = "";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    s = client.DownloadString(url);
                }

                //string pathForSaving = Server.MapPath("~/Temp");
                //if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    //filename = pathForSaving + "/" + Guid.NewGuid().ToString() + ".xml";
                    System.IO.File.WriteAllText(filename, s);
                }

                return new Tuple<int, string>(0, string.Empty);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(1, ex.Message);
            }
        }

        #endregion

        #region CreatePassword

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijkmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        #endregion

        #region EncryptData

        public static string EncryptData(string Message, string secretKey)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(secretKey));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            //TDESAlgorithm.IV = iv;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        #endregion

        #region EncryptDataSHA256

        public static string EncryptDataSHA256(string Message)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(Message);
            using (var hash = System.Security.Cryptography.SHA256.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 64, because 256 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(64);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        #endregion

        #region EncryptDataSHA512

        public static string EncryptDataSHA512(string Message)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(Message);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        #endregion

        #region DecryptString

        public static string DecryptString(string Message, string secretKey)
        {
            Message = Message.Replace(" ", "+");

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(secretKey));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            //TDESAlgorithm.IV = iv;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);
        }

        #endregion

        #region CreateValidateCode

        public static string CreateValidateCode()
        {
            Random rnd = new Random();
            string ValidateCode = "";
            for (int i = 0; i < 6; i++)
            {
                ValidateCode += rnd.Next(0, 10).ToString();
            }
            return ValidateCode;
        }

        #endregion

        #region PostWebResult

        public static void PostWebResult(string SessionID, string Result, string postUrl, string Operator)
        {
            try
            {
                string postData = "SessionID=" + SessionID + "&" + "Result=" + Result + "&" + "Operator=" + Operator;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(postUrl);

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);

                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;

                using (Stream newStream = httpWReq.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                }
            }
            catch
            { }
        }

        public static void PostWebResult(string SessionID, string Result, string postUrl, string Operator, string Parameters)
        {
            try
            {
                string postData = "SessionID=" + SessionID + "&" + "Result=" + Result + "&" + "Operator=" + Operator + "&Parameters=" + Parameters;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(postUrl);

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);

                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;

                using (Stream newStream = httpWReq.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                }
            }
            catch
            { }
        }

        #endregion

        #region SplitParametersForUrl
        public static string SplitParametersForUrl(string parameters)
        {

            string resultString = String.Empty;
            try
            {

                string[] delimiter1 = { ";;" };
                string[] split1 = parameters.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);
                string strSplit1 = "";
                foreach (var item in split1)
                {
                    strSplit1 += item + "||";
                }

                string[] delimiter2 = { "||" };
                string[] split2 = strSplit1.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i <= split2.Length - 2; i = i + 2)
                {
                    resultString += "&" + split2[i] + "=" + split2[i + 1];
                }

            }
            catch /*(Exception ex)*/
            {

            }
            return resultString;
        }
        #endregion

        #region Encrypt

        static string desKey = "tete0A2o";

        public static string Encrypt(string value)
        {
            UTF8Encoding utf8Enc = new UTF8Encoding();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = utf8Enc.GetBytes(desKey);
            des.Mode = CipherMode.ECB;
            ICryptoTransform encryptor = des.CreateEncryptor();
            byte[] arrayByte = utf8Enc.GetBytes(value);
            byte[] enc = encryptor.TransformFinalBlock(arrayByte, 0, arrayByte.Length);
            return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(enc));
        }

        #endregion

        #region CalculateMD5Hash

        public static string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        #region CalculateMD5HashASCII

        /// <summary>
        /// Dikkat ASCII encoding kullanılır sadece user loginde kullanılıyor
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CalculateMD5HashASCII(string str)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        #region HttpGet

        public static string HttpGet(string URI)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        #endregion

        #region CheckPhoneNumber

        public static PhoneResult CheckPhoneNumber(string phoneNumber)
        {
            PhoneResult res = new PhoneResult();
            res.gsm = phoneNumber;
            res.result = false;

            //00|9|0|5326001020
            if (phoneNumber.Length == 10 && phoneNumber.StartsWith("5"))
            {
                res.gsm = "90" + phoneNumber;
                res.result = true;
            }
            else if (phoneNumber.Length == 11 && phoneNumber.StartsWith("05"))
            {
                res.gsm = "9" + phoneNumber;
                res.result = true;
            }
            else if (phoneNumber.Length == 12 && phoneNumber.StartsWith("905"))
            {
                res.gsm = phoneNumber;
                res.result = true;
            }
            else if (phoneNumber.Length > 12)
            {
                if (phoneNumber.Substring(phoneNumber.Length - 10).StartsWith("5"))
                {
                    res.gsm = "90" + phoneNumber.Substring(phoneNumber.Length - 10);
                    res.result = true;
                }
            }

            return res;
        }

        #endregion

        #region SplitResponse

        private static NameValueCollection SplitResponse(string testStr)
        {
            NameValueCollection nvColleciton = new NameValueCollection();

            string[] strSplitByAmp = testStr.Split('&');
            foreach (string item in strSplitByAmp)
            {
                string[] strSplitByEq = item.Split('=');
                nvColleciton[strSplitByEq[0]] = strSplitByEq[1];
            }
            return nvColleciton;
        }

        #endregion SplitResponse


        //public static string GetIPAddress(HttpContext context)
        //{
        //    if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
        //    {
        //        return context.Request.Headers["X-Forwarded-For"];
        //    }
        //    return context.Connection?.RemoteIpAddress?.ToString();
        //}


        #region UserIPIsProxy

        //public static bool UserIPIsProxy
        //{
        //    get
        //    {
        //        String IP = SessionHelper. HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //        return IP != null && IP.ToLower() != "unknown";
        //    }
        //}

        #endregion

        #region UserIP

        //public static string UserIP
        //{
        //    get
        //    {
        //        try
        //        {
        //            string IP = String.Empty;

        //            if (UserIPIsProxy)
        //                IP = SessionHelper.GetCurrentHttpRequest().HttpContext.Connection.RemoteIpAddress.ToString(); // ServerVariables["HTTP_X_FORWARDED_FOR"];
        //            else if (SessionHelper.GetCurrentHttpRequest().HttpContext.Connection.LocalIpAddress.ToString().Length != 0) //   HttpContext.Current.Request.UserHostAddress.Length != 0)
        //                IP = SessionHelper.GetCurrentHttpRequest().HttpContext.Connection.LocalIpAddress.ToString(); // HttpContext.Current.Request.UserHostAddress;
        //            else
        //            {
        //                IP = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
        //                if (string.IsNullOrEmpty(IP))
        //                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //            }

        //            return IP;
        //            //return "213.74.118.226";
        //        }
        //        catch (Exception) { return string.Empty; }
        //    }
        //}

        public static string UserIP()
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //

            ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && SessionHelper.GetCurrentHttpRequestConnection()?.RemoteIpAddress != null)
                ip = SessionHelper.GetCurrentHttpRequestConnection().RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = "Okunmadı!";
            }

            return ip;
        }

        public static T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (SessionHelper.GetCurrentHttpRequest().Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!String.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }


        #endregion

        #region PhoneResult

        public class PhoneResult
        {
            public string gsm { get; set; }
            public bool result { get; set; }
        }

        #endregion

        #region Base64Encode

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #endregion

        #region Base64Decode

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion

        #region MessageBoxScript

        public static string MessageBoxScript(int MessageType, string Icon, string Message, string OkText, string CancelText, string OkFunction, string CancelFunction)
        {
            return String.Format("<script>showPTGMessage({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\", {5}, {6}); </script>",
                                 MessageType.ToString(),
                                 (Icon ?? ""),
                                 (Message ?? "Mesaj giriniz!"),
                                 (OkText ?? ""),
                                 (CancelText ?? ""),
                                 (string.IsNullOrWhiteSpace(OkFunction) ? "null" : "function() { " + OkFunction + " }"),
                                 (string.IsNullOrWhiteSpace(CancelFunction) ? "null" : "function() { " + CancelFunction + " }")
                                );
        }

        #endregion

        #region PrepareURL

        public static string PrepareURL(string url)
        {
            var tempUrl = (url ?? "").Trim();
            if (!tempUrl.Contains("?"))
            {
                tempUrl = tempUrl + "?";
            }
            else
            {
                if (tempUrl.Substring(tempUrl.Length - 1, 1) != "?" && tempUrl.Substring(tempUrl.Length - 1, 1) != "&")
                {
                    tempUrl = tempUrl + "&";
                }
            }
            return tempUrl;
        }

        #endregion
    }
}
