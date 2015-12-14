using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Speech.Synthesis;

namespace Delta.PECS.WebCSC.Site {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateWavStream : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            if (context.Request.QueryString["TextToSpeech"] != null) {
                var textToSpeech = HttpUtility.UrlDecode(context.Request.QueryString["TextToSpeech"]);
                var bytes = this.CreateStream(textToSpeech);
                if (bytes != null) {
                    context.Response.ClearContent();
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.ContentType = "audio/wav";
                    context.Response.BinaryWrite(bytes);
                    context.Response.Flush();
                    context.Response.End();
                }
            }
        }

        public bool IsReusable {
            get { return false; }
        }

        /// <summary>
        /// Create Wav Stream
        /// </summary>
        /// <param name="textToSpeech">textToSpeech</param>
        /// <returns>Wav Stream</returns>
        private byte[] CreateStream(string textToSpeech) {
            byte[] bytes = null;
            var t = new System.Threading.Thread(() => {
                try {
                    using (var speaker = new SpeechSynthesizer()) {
                        using (var stream = new MemoryStream()) {
                            speaker.SetOutputToWaveStream(stream);
                            speaker.Speak(textToSpeech);
                            speaker.SetOutputToNull();
                            bytes = stream.ToArray();
                        }
                    }
                } catch { }
            });

            t.Start();
            t.Join();
            return bytes;
        }
    }
}