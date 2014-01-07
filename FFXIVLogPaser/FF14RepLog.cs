using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFXIVLogPaser
{
    /// <summary>
    /// 集計用ログオブジェクト
    /// </summary>
    public class FF14RepLog
    {
        public DateTime Timestamp { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// アクションタイプ番号
        /// </summary>
        /// <example>
        /// Ex) A9(169) -> ダメージを与えた
        /// </example>
        public int ActionType
        {
            get
            {
                string hex = this.Code.Substring(2, 2);
                return Convert.ToInt32(hex, 16);
            }
        }

        /// <summary>
        /// 誰から誰のタイプ
        /// </summary>
        /// <example>
        /// Ex) 09(9) -> PTメンバーからPTメンバー
        /// </example>
        public int LogType
        {
            get
            {
                string hex = this.Code.Substring(0, 2);
                return Convert.ToInt32(hex, 16);
            }
        }

        private string _from;
        public string From { get { return this.AppendNameHeader(_from); } set { _from = value; } }

        private string _to;
        public string To { get { return this.AppendNameHeader(_to); } set { _to = value; } }

        public int Damage { get; set; }
        public int Cure { get; set; }

        public string Action { get; set; }
        public bool ActionStart { get; set; }
        public bool ActionEnd { get; set; }

        // アクションの実行対象が無い場合は親ログ（子ログと合わせて初めて集計可能。なお親：子は１：ｎの関係）
        public bool IsParent { get { return string.IsNullOrEmpty(this._to); } }

        // アクションの実行者が無い場合は子ログ（親ログと合わせて初めて集計可能。なお親：子は１：ｎの関係）
        public bool IsChild { get { return string.IsNullOrEmpty(this._from); } }

        /// <summary>
        /// ペット名なら頭に"MY","PT"等をつける 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string AppendNameHeader(string name)
        {
            if (!string.IsNullOrEmpty(name)
                && (name.StartsWith("カーバンクル")
                    || name.StartsWith("イフリート・エギ")
                    || name.StartsWith("タイタン・エギ")
                    || name.StartsWith("ガルーダ・エギ")
                    || name.StartsWith("フェアリー・エオス")
                    || name.StartsWith("フェアリー・セレネ")))
            {
                if (this.TO == LOG_CATEGORY_TO.EMPTY_OR_ME)
                {
                    return name + "(MY)";
                }
                else if (this.TO == LOG_CATEGORY_TO.PTMEMBER)
                {
                    return name + "(PT)";
                }
                else if (this.TO == LOG_CATEGORY_TO.ENEMY)
                {
                    return name + "(EN)";
                }

                return name + "(OTHER)";
            }

            return name;
        }

        #region フラグ
        public LOG_CATEGORY_WHO WHO
        {
            get
            {
                return (LOG_CATEGORY_WHO)(this.LogType >> 2);
            }
        }

        public LOG_CATEGORY_TO TO
        {
            get
            {
                return (LOG_CATEGORY_TO)(this.LogType & 3);
            }
        }

        public LOG_CATEGORY_TARGET_STATUS TARGET_STATUS
        {
            get
            {
                return (LOG_CATEGORY_TARGET_STATUS)((this.ActionType >> 7) & 0x1);
            }
        }

        public LOG_CATEGORY_TYPE LOG_TYPE
        {
            get
            {
                return (LOG_CATEGORY_TYPE)((this.ActionType >> 4) & 0x7);
            }
        }

        public LOG_CATEGORY_UNKNOWN_FLG UNKNOWN_FLG
        {
            get
            {
                return (LOG_CATEGORY_UNKNOWN_FLG)((this.ActionType >> 3) & 1);
            }

        }

        public LOG_BATTLE_TYPE BATTLE_TYPE
        {
            get
            {
                return (LOG_BATTLE_TYPE)(this.ActionType & 0x7);
            }
        }

        public LOG_GAME_EVENT_TYPE GAME_EVENT_TYPE
        {
            get
            {
                return (LOG_GAME_EVENT_TYPE)(this.ActionType & 0x7);
            }
        }

        public LOG_SYSTEM_EVENT_TYPE SYSTEM_EVENT_TYPE
        {
            get
            {
                return (LOG_SYSTEM_EVENT_TYPE)(this.ActionType & 0x7);
            }
        }
        #endregion
    }
}
