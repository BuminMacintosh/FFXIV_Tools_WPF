using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFXIV.Tools.TargetInformation.Model
{
    /// <summary>
    /// ターゲットインフォメーションアプリのモデルクラス
    /// </summary>
    public class TargetInformationModel
    {
        private Exception Ex { get; set; }
        private ffxivlib.Entity Target { get; set; }
        private ffxivlib.PartyMember Me { get; set; }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        /// <param name="target">ターゲットを表すFFXIVのオブジェクト</param>
        /// <param name="me">自分自身を表すFFXIVのオブジェクト</param>
        public TargetInformationModel(ffxivlib.Entity target, ffxivlib.PartyMember me)
        {
            this.Target = target;
            this.Me = me;
            this.Ex = null;
        }

        /// <summary>
        /// 例外が発生したとき用のコンストラクタ
        /// </summary>
        /// <param name="ex"></param>
        public TargetInformationModel(Exception ex)
        {
            this.Target = null;
            this.Me = null;
            this.Ex = ex;
        }

        /// <summary>
        /// ターゲットの名前
        /// </summary>
        public string Name
        {
            get
            {
                if (null != this.Ex) return this.Ex.Message;
                return this.Target.Structure.Name;
            }
        }

        /// <summary>
        /// ターゲットの現在HP
        /// </summary>
        public string CurrentHP
        {
            get
            {
                if (null != this.Ex) return string.Empty;
                return this.Target.Structure.CurrentHP.ToString();
            }
        }

        /// <summary>
        /// ターゲットの最大HP
        /// </summary>
        public string MaxHP
        {
            get
            {
                if (null != this.Ex) return string.Empty;
                return this.Target.Structure.MaxHP.ToString();
            }
        }

        /// <summary>
        /// ターゲットのHP/MaxHPの百分率
        /// </summary>
        public string RateHP
        {
            get
            {
                if (null != this.Ex) return string.Empty;
                return string.Format(
                    "{2:0.00}",
                    (double)this.Target.Structure.CurrentHP / (double)this.Target.Structure.MaxHP * 100
                );
            }
        }

        /// <summary>
        /// ターゲットと自分との距離
        /// </summary>
        public string Distance
        {
            get
            {
                if (null != this.Ex) return string.Empty;
                return string.Format(
                    "{0:0.00}",
                    (double)Math.Sqrt(
                        Math.Pow(this.Me.Structure.X - this.Target.Structure.X, 2)
                        + Math.Pow(this.Me.Structure.Y - this.Target.Structure.Y, 2)
                        + Math.Pow(this.Me.Structure.Z - this.Target.Structure.Z, 2)
                    )
                );
            }
        }
    }
}
