using ExcelWorkbook2.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWorkbook2.Models
{
    public class mWeather
    {

        public static async Task<List<clWeatherInfo>> GetWeatherByLatLon(string ViDo, string KinhDo)
        {
            var reList = new List<clWeatherInfo>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = string.Format(@"https://api.openweathermap.org/data/2.5/forecast?lang=vi&units=metric&lat={0}&lon={1}&appid=11aa4b4963ff5453cf74ef722bb48abf", ViDo, KinhDo);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                    var res = await client.GetAsync(Url);

                    if (res.IsSuccessStatusCode)
                    {
                        var JsonString = await res.Content.ReadAsStringAsync();
                        var JData = JToken.Parse(JsonString);
                        var JList = JData["list"].ToArray();

                        foreach (var Jitem in JList)
                        {
                            var unixTime = (long)Jitem["dt"];
                            var dtOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
                            var dt = dtOffset.UtcDateTime;


                            var tt = new clWeatherInfo();
                            tt.wDate = dt;
                            tt.wMota = Jitem["weather"][0]["description"].ToString();
                            tt.wTemp = double.Parse(Jitem["main"]["temp"].ToString());
                            tt.wWind = double.Parse(Jitem["wind"]["speed"].ToString());

                            reList.Add(tt);
                        }    
                    }    
                }    
            }
            catch
            {

            }
            return reList;
        }
    }
}
