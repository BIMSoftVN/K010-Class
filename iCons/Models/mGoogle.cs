using iCons.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iCons.Models
{
    public class mGoogle
    {
        public static async Task<(bool IsSuccess, string Message)> Dich(string Content)
        {
            bool isSuccess = false;
            string message = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Content))
                {
                    message = "Nội dung không được để trống";
                    return (isSuccess, message);
                }

                string Url = "https://translate.google.com/m?sl=auto&tl=en&hl=vi&q=" + Content;
                string DivResult = "<div class=\"result-container\">";

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var response = await client.GetAsync(Url);
                    if (response.IsSuccessStatusCode)
                    {
                        var htmlContent = await response.Content.ReadAsStringAsync();
                        int startIndex = htmlContent.IndexOf(DivResult);
                        if(startIndex!=-1)
                        {
                            startIndex += DivResult.Length;
                            int endIndex = htmlContent.IndexOf("</div>", startIndex);
                            if (endIndex != -1)
                            {
                                Content = htmlContent.Substring(startIndex, endIndex - startIndex);
                                isSuccess = true;
                            }
                            else
                            {
                                message = "Không dịch được";
                            }
                        }    
                        else
                        {
                            message = "Không dịch được";
                        }    
                    }
                    else
                    {
                        message = "Không dịch được";
                    }
                }


                return (true,Content);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return (isSuccess, message);
        }

    }
}
