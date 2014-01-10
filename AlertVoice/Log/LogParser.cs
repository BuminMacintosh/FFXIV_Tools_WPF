using System.Text.RegularExpressions;
using ffxivlib;

namespace FFXIV.Tools.AlertVoice.Log
{
    public class LogParser
    {
        public FFXIVLog Log { get; private set; }

        /// <summary>
        /// ログエントリをメンバーへ設定します
        /// </summary>
        /// <param name="entry">FFXIVLIBのログエントリオブジェクト</param>
        public LogParser(Chatlog.Entry entry)
        {
            this.Log = new FFXIVLog()
            {
                Code = entry.Code,
                Timestamp = entry.Timestamp,
                Text = entry.Text.Replace(" HQ ", "⇒"),
            };
        }

        /// <summary>
        /// テキストにアクションが含まれればヒットしたアクション名を返します
        /// </summary>
        /// <param name="keyActions">調査対象アクション名</param>
        /// <returns>含まれればHitしたアクション、なければstring.Empty</returns>
        public string GetHitAction(params string[] keyActions)
        {
            string hitAction = string.Empty;

            // アクション表記がある場合
            if (this.Log.IsBattle()
                && this.Log.IsFromMonster()
                && this.Log.IsStartAction()
                && this.Log.Text.Contains("「")
                && this.Log.Text.Contains("」"))
            {
                var match = new Regex(@"(?<target>[^！⇒]+)(?<conjunction>[のには]+)[「](?<action>[^」]+)[」](?<etc>[^！]*)").Match(this.Log.Text);
                var targetAction = match.Groups["action"].Value.Trim();

                foreach (var word in keyActions)
                {
                    if (!string.IsNullOrWhiteSpace(word.Trim()) && targetAction == word.Trim())
                    {
                        hitAction = word + match.Groups["etc"].Value.Trim();
                        break; 
                    }
                }        
            }

            return hitAction;
        }
    }
}
