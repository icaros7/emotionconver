using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ConsoleTest
{
    class Program
    {
        static void Main()
        {
            //Console.Write("인식할 사진 (JPG권장) : ");
            string imageFilePath = "D:\\1.jpg";

            MakeRequest(imageFilePath);

            //Console.WriteLine("\n\n\n\tㄱㄷㄱㄷ 값 불러오는중 엔터 누름 취소됨 주의\n\n\n");
            Console.ReadLine();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async void MakeRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "6909027b92a44e21b73f5060958e7457");

            // NOTE: You must use the same region in your REST call as you used to obtain your subscription keys.
            //   For example, if you obtained your subscription keys from westcentralus, replace "westus" in the 
            //   URI below with "westcentralus".
            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            //A peak at the JSON response.
            string value = responseContent;

            Console.WriteLine(responseContent);
            Console.WriteLine("\n\n\n첫번째 사람만 인식함 ㅅㄱ");
            int first_pos = value.IndexOf("scores") + 17;
            int end_pos = value.IndexOf(",\"contempt\":");
            string anger_tmp = value.Substring(first_pos, end_pos - first_pos);
            if (anger_tmp.IndexOf("E-") != 0)
            {
                int tmp = anger_tmp.IndexOf("E-");
                decimal anger = decimal.Parse(anger_tmp.Substring(tmp+2, tmp+5));
                
            }
            Console.WriteLine("화남 : " + anger_tmp);
            first_pos = end_pos + 12;
            end_pos = value.IndexOf(",\"disgust\":");
            string contempt = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("경멸적 : " + contempt);
            first_pos = end_pos + 11;
            end_pos = value.IndexOf(",\"fear\":");
            string disgust = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("부정적 : " + disgust);
            first_pos = end_pos + 8;
            end_pos = value.IndexOf(",\"happiness\":");
            string fear = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("두려움 : " + fear);
            first_pos = end_pos + 13;
            end_pos = value.IndexOf(",\"neutral\":");
            string happyness = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("행복함 : " + happyness);
            first_pos = end_pos + 11;
            end_pos = value.IndexOf(",\"sadness\":");
            string neutral = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("자연스러움 : " + neutral);
            first_pos = end_pos + 11;
            end_pos = value.IndexOf(",\"surprise\":");
            string sadness = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("슬픔 : " + sadness);
            first_pos = end_pos + 11;
            end_pos = value.IndexOf("}}");
            string surprise = value.Substring(first_pos, end_pos - first_pos);
            Console.WriteLine("놀라움 : " + surprise);
        }
    }
}
