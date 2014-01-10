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
            }
            catch (COMException)
            {
                throw new InvalidOperationException(
                    "合成音声が利用できません。" + Environment.NewLine
                    + "Microsoft Speech Platform - Runtime (Version 11) をインストールしてください。"
                );
            }

            // 合成音声エンジンで日本語を話す人を設定する
            this.SetJapaneseVoice(this.VoiceSpeeach);
        }

        /// <summary>
        /// 合成音声エンジンで日本語を話す人を探す
        /// </summary>
        /// <param name="voiceSpeeach">COMオブジェクト</param>
        private void SetJapaneseVoice(SpeechLib.SpVoice voiceSpeeach)
        {
            foreach (SpObjectToken voiceperson in voiceSpeeach.GetVoices())
            {
                if ("411" == voiceperson.GetAttribute("Language"))
                {
                    this.VoiceSpeeach.Voice = voiceperson;
                    return;
                }
            }

            // 見つからないならエラー
            throw new InvalidOperationException(
                "日本語合成音声が利用できません。" + Environment.NewLine
                + "日本語合成音声 MSSpeech_TTS_ja-JP をインストールしてください。"
            );
        }

        /// <summary>
        /// しゃべくりがおわるまで待機します
        /// </summary>
        public void WaitUntilSpoken()
        {
            while (this.VoiceSpeeach.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
            {
                this.Delay(100);
            }
        }

        /// <summary>
        /// Task.Delayが.NET 4だと搭載されていないので擬似関数作成
        /// </summary>
        /// <param name="milliseconds"></param>
        private void Delay(int milliseconds)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
            new System.Threading.Timer(_ => tcs.SetResult(null)).Change(milliseconds, -1);
            tcs.Task.Wait();
        }

        /// <summary>
        /// 指定した文章を既定の音声で読み上げます
        /// </summary>
        /// <param name="text">XML TTSで書かれた文章</param>
        public void TalkByDefaultVoice(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                this.WaitUntilSpoken();

                this.VoiceSpeeach.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }
    }
}
