using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class JsonDal
    {
        public async static Task<string> GetJson(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            return streamReader.ReadToEnd();
        }
    }
}
