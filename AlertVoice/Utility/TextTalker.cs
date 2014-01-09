using System;
using System.Runtime.InteropServices;
using SpeechLib;

namespace FFXIV.Tools.AlertVoice.Utility
{
    public class TextTalker
    {
        //合成音声ライブラリ
        private SpeechLib.SpVoice VoiceSpeeach { get; set; }

        public TextTalker()
        {
            try
            {
                this.VoiceSpeeach = new SpeechLib.SpVoice();
                this.VoiceSpeeach.WaitUntilDone(1000);
            }
            catch (COMException)
            {
                throw new InvalidOperationException("合成音声が利用できません。\r\nMicrosoft Speech Platform - Runtime (Version 11) をインストールしてください。\r\n");
            }

            // 合成音声エンジンで日本語を話す人を探す
            var voice = this.FindJapaneseVoice(this.VoiceSpeeach);

            if (null == voice)
            {
                throw new InvalidOperationException("日本語合成音声が利用できません。\r\n日本語合成音声 MSSpeech_TTS_ja-JP をインストールしてください。\r\n");
            }
            else
            {
                this.VoiceSpeeach.Voice = voice;
            }
        }

        private SpObjectToken FindJapaneseVoice(SpeechLib.SpVoice voiceSpeeach)
        {
            foreach (SpObjectToken voiceperson in voiceSpeeach.GetVoices())
            {
                if ("411" == voiceperson.GetAttribute("Language"))
                {
                    return voiceperson;
                }
            }

            return null;
        }

        /// <summary>
        /// 音声出力が終わるまで待機します
        /// </summary>
        public void WaitSpoken()
        {
            while (this.VoiceSpeeach.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 指定した文章を既定の音声で読み上げます
        /// </summary>
        /// <param name="text"></param>
        public void TalkByDefaultVoice(string text)
        {
            this.WaitSpoken();

            this.VoiceSpeeach.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
        }
    }
}
