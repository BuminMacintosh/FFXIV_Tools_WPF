using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ffxivlib;

namespace FFXIV.Tools.AlertVoice.Log
{
    public class LogParser
    {
        private FFXIVLog Log { get; set; }

        /// <summary>
        /// テキストパーサー
        /// </summary>
        /// <param name="entry">FFXIVLIBのログエントリオブジェクト</param>
        /// <returns></returns>
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
        /// テキストにアクションが含まれるかどうか
        /// </summary>
        /// <param name="keyAction">調査対象アクション名</param>
        /// <returns>含まれればTrue</returns>
        public bool ContainsAction(params string[] keyActions)
        {
            // アクション表記がある場合
            if (this.Log.IsBattle()
                && this.Log.IsFromMonster()
                && this.Log.Text.Contains("「")
                && this.Log.Text.Contains("」"))
            {
                var match = new Regex(@"(?<target>[^！⇒]+)(?<conjunction>[のには]+)[「](?<action>[^」]+)[」](?<etc>[^！]*)").Match(this.Log.Text);

                return keyActions.Any(k => k == match.Groups["action"].Value.Trim());
            }

            return false;
        }
    }
}
