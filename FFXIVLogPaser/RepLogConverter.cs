using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ffxivlib;

namespace FFXIVLogPaser
{
    public class RepLogConverter
    {
        /// <summary>
        /// FFXIVLIBのログエントリオブジェクト1つを集計用オブジェクトに変換する
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public FF14RepLog Convert(Chatlog.Entry entry)
        {
            FF14RepLog result = new FF14RepLog();

            result.Code = entry.Code;
            result.Timestamp = entry.Timestamp;
            result.Text = entry.Text.Replace(" HQ ", "⇒");

            // アクションを抽出
            this.SetAction(result);

            // ヒットダメージを抽出
            this.SetHitDamage(result);

            // HP回復を抽出
            this.SetCure(result);

            return result;
        }


        /// <summary>
        /// テキストからアクションがある場合は抽出する
        /// </summary>
        /// <param name="log"></param>
        private void SetAction(FF14RepLog log)
        {
            if (log.WHO == LOG_CATEGORY_WHO.SYSTEM) return;
            if (log.LOG_TYPE != LOG_CATEGORY_TYPE.BATTLE) return;
            if (log.BATTLE_TYPE != LOG_BATTLE_TYPE.EFFECT1
                && log.BATTLE_TYPE != LOG_BATTLE_TYPE.EFFECT2
                && log.BATTLE_TYPE != LOG_BATTLE_TYPE.DONE
                && log.BATTLE_TYPE != LOG_BATTLE_TYPE.UNKNOWN) return;

            // アクションがある場合
            if (log.Text.Contains("「") && log.Text.Contains("」"))
            {
                var match = new Regex(@"(?<target>[^！⇒]+)(?<conjunction>[のには]+)[「](?<action>[^」]+)[」](?<etc>[^！]*)").Match(log.Text);

                log.Action = match.Groups["action"].Value.Trim();

                // 接続詞の判定
                switch (match.Groups["conjunction"].Value.Trim())
                {
                    case "の":
                    case "は":
                        log.From = match.Groups["target"].Value.Trim();
                        break;
                    case "に":
                        log.To = match.Groups["target"].Value.Trim();
                        break;
                }

                if (match.Groups["etc"].Value.Trim() == "の効果。")
                {
                    // アクションの効果の場合は対象がFROM自身 且つ 実行開始
                    log.From = null;
                    log.To = match.Groups["target"].Value.Trim();
                    log.ActionStart = true;
                }
                else if (match.Groups["etc"].Value.Trim() == "が切れた。")
                {
                    // アクションの効果切れの場合は対象がFROM自身 且つ 実行終了
                    log.From = null;
                    log.To = match.Groups["target"].Value.Trim();
                    log.ActionEnd = true;
                }
                else if (match.Groups["etc"].Value.Trim() == "を唱えた。")
                {
                    // アクションが詠唱開始の場合は対象無し
                    log.From = match.Groups["target"].Value.Trim();
                    log.To = null;
                    log.ActionStart = true;
                }
                else if (match.Groups["etc"].Value.Trim() == "の詠唱を中断した。")
                {
                    // アクションが詠唱開始の場合は対象無し
                    log.From = null;
                    log.To = match.Groups["target"].Value.Trim();
                    log.ActionEnd = true;
                }
                else if (match.Groups["etc"].Value.Trim() == "の構え。")
                {
                    // アクションの構えの場合は対象がFROM自身
                    if (string.IsNullOrEmpty(log.To)) log.To = log.From;
                }
            }
        }

        /// <summary>
        /// テキストからヒットダメージがある場合は抽出する
        /// </summary>
        /// <param name="log"></param>
        private void SetHitDamage(FF14RepLog log)
        {
            if (log.WHO == LOG_CATEGORY_WHO.SYSTEM) return;
            if (log.LOG_TYPE != LOG_CATEGORY_TYPE.BATTLE) return;
            if (log.BATTLE_TYPE != LOG_BATTLE_TYPE.HIT) return;
            
            // ダメージがある場合
            if (log.Text.Contains("ダメージ。"))
            {
                var match = new Regex(@"(|(?<from>[^！⇒]+)[の](?<action>[^\n]+)([⇒]))(?<to>[^！⇒]+)[はに]([^\n0-9]*)(?<dmg>\d+)(|\((?<bonus>[+-]\d+)\%\))ダメージ。").Match(log.Text);
                log.From = match.Groups["from"].Value.Trim();
                log.Action = match.Groups["action"].Value.Trim();
                log.To = match.Groups["to"].Value.Trim();
                log.Damage = int.Parse(match.Groups["dmg"].Value.Trim());
            }
        }

        /// <summary>
        /// テキストから回復がある場合は抽出する
        /// </summary>
        /// <param name="log"></param>
        private void SetCure(FF14RepLog log)
        {
            if (log.WHO == LOG_CATEGORY_WHO.SYSTEM) return;
            if (log.LOG_TYPE != LOG_CATEGORY_TYPE.BATTLE) return;
            if (log.BATTLE_TYPE != LOG_BATTLE_TYPE.HEAL) return;

            // ＨＰ回復がある場合
            if (log.Text.Contains("ＨＰ回復。"))
            {
                var match = new Regex(@"(?<to>[^！⇒]+)は(?<cure>\d+)ＨＰ回復。").Match(log.Text);
                log.To = match.Groups["to"].Value.Trim();
                log.Cure = int.Parse(match.Groups["cure"].Value.Trim());
            }
        }

    }
}
