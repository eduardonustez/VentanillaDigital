using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace ServiciosDistribuidos.CrossPlatform.Controllers
{
    public class TextToSpeechController : ApiController
    {
        Thread p1;
        string strTexto;
        string strNombre;
    
        public async Task<bool> Put(string texto,string nombre)
        {
            //var response = new HttpResponseMessage(HttpStatusCode.OK);
            //  SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            //using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            //{
            //    synthesizer.SetOutputToWaveFile(@"C:\temp\" + nombre);
            //    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            //    synthesizer.SetOutputToNull();
            //    synthesizer.SpeakAsync(texto);
                
            //    synthesizer.Dispose();
            //}
            //byte[] fileBytes;
            //string base64 = null;
            //using (MemoryStream memStream = new MemoryStream())
            //{
            //    synthesizer.SetOutputToAudioStream(memStream, new SpeechAudioFormatInfo(EncodingFormat.ALaw,
            //        8000, 8, 1, 8000, 1, null));
            //    synthesizer.Speak(texto);
            //    memStream.Seek(0, SeekOrigin.Begin);
            //    fileBytes = memStream.ToArray();
            //    base64 = Convert.ToBase64String(fileBytes);
            //    response.Content = new StreamContent(memStream);
            //    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/mp3");
            //}
            //return base64;
            //File(fileBytes, "audio/mp4");
            strTexto = texto;
            strNombre = nombre;
            p1 = new Thread(new ThreadStart(Hilo1));
            p1.Start();
            
            return true; 

        }
        private void Hilo1()
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToWaveFile(@"C:\temp\" + strNombre);
                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                synthesizer.Speak(strTexto);
                synthesizer.Dispose();
            }
            //p1.Abort();
        }

    }
}
