using System.IO;
using UnityEngine;

namespace HTTP
{
    public static class Common
    {
        public const string Domain = "http://172.16.16.253:8080";
        
        // 기본 퀴즈 ID (서버에서 응답이 없을 경우 사용할 fallback 값)
        private const string DEFAULT_QUIZ_ID = "퀴즈아이디가 없어요";
        
        // 최신 퀴즈 ID를 저장하는 변수
        private static string _lastQuizId = DEFAULT_QUIZ_ID;
        
        // 퀴즈 ID에 접근하는 프로퍼티
        public static string LastQuizId 
        { 
            get { return string.IsNullOrEmpty(_lastQuizId) ? DEFAULT_QUIZ_ID : _lastQuizId; }
            set { _lastQuizId = value; }
        }
           
        public static byte[] AudioClipToWav(AudioClip clip)
        {
            var samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            byte[] wavData = ConvertAudioClipDataToWav(samples, clip.channels, clip.frequency);
            return wavData;
        }
             
        private static byte[] ConvertAudioClipDataToWav(float[] samples, int channels, int sampleRate)
        {
            // 기존 코드 유지
            int byteRate = sampleRate * channels * 2;
            int fileSize = 44 + samples.Length * 2;
                 
            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
                writer.Write(fileSize - 8);
                writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
                writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)channels);
                writer.Write(sampleRate);
                writer.Write(byteRate);
                writer.Write((short)(channels * 2));
                writer.Write((short)16);

                writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
                writer.Write(samples.Length * 2);

                foreach (float sample in samples)
                {
                    short intSample = (short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue);
                    writer.Write(intSample);
                }
            }

            return stream.ToArray();
        }
    }
}