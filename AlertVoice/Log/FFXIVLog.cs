using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFXIV.Tools.AlertVoice.Log
{
    /// <summary>
    /// ログオブジェクト
    /// </summary>
    public class FFXIVLog
    {
        public DateTime Timestamp { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 誰から誰のタイプ
        /// </summary>
        /// <remarks>
        /// Codeの前半2文字が実行元と実行先のフラグ値を表す16進数
        /// </remarks>
        public int LogType
        {
            get
            {
                string hex = this.Code.Substring(0, 2);
                return Convert.ToInt32(hex, 16);
            }
        }

        /// <summary>
        /// アクションタイプ
        /// </summary>
        /// <remarks>
        /// Codeの後半2文字が実行元と実行先のフラグ値を表す16進数
        /// </remarks>
        public int ActionType
        {
            get
            {
                string hex = this.Code.Substring(2, 2);
                return Convert.ToInt32(hex, 16);
            }
        }

        /// <summary>
        /// 赤ネーム/紫ネーム含めたモンスター起因の行動かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsFromMonster()
        {
            // ログを分析した結果から6bit目がモンスター起因かどうかを表すフラグと思われる
            // 例）
            // [2AAB] ボムボルダーは「大爆発」の構え。
            // [2AAB] --（上位2文字を2進数に変換）--> 00101010
            // [282B] ボムボルダーの「大爆発」
            // [282B] --（上位2文字を2進数に変換）--> 00101000 
            return (1 == ((this.LogType >> 5) & 0x1));
        }

        /// <summary>
        /// 戦闘中の行動かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsBattle()
        {
            // ログを分析した結果から6bit目が戦闘中かどうかを表すフラグと思われる
            // 例）
            // [2AAB] ボムボルダーは「大爆発」の構え。
            // [2AAB] --（下位2文字を2進数に変換）--> 10101011
            // [282B] ボムボルダーの「大爆発」
            // [282B] --（下位2文字を2進数に変換）--> 00101011 
            return (1 == ((this.ActionType >> 5) & 0x1));
        }

        /// <summary>
        /// アクションの開始かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsStartAction()
        {
            // ログを分析した結果から8bit目がアクションの開始かどうかを表すフラグと思われる
            // 例）
            // [2AAB] ボムボルダーは「大爆発」の構え。
            // [2AAB] --（下位2文字を2進数に変換）--> 10101011
            // [282B] ボムボルダーの「大爆発」
            // [282B] --（下位2文字を2進数に変換）--> 00101011
            return (1 == ((this.ActionType >> 7) & 0x1));
        }
    }
}
