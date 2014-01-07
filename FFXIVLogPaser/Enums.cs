using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLogPaser
{
    //[who 6][to 2] [stat1][info3][?1][num 3]
    /// <summary>
    /// ログカテゴリWHO
    /// </summary>
    public enum LOG_CATEGORY_WHO
    {
        SYSTEM,
        MYSELF,
        PTMEMBER,
        ALLY,
        OTHER,
        ENEMY,
        NPC
    }

    /// <summary>
    /// ログカテゴリTO
    /// </summary>
    public enum LOG_CATEGORY_TO
    {
        EMPTY_OR_ME,
        PTMEMBER,
        ENEMY,
        OTHER
    }

    /// <summary>
    /// ログカテゴリTargetStatus
    /// </summary>
    public enum LOG_CATEGORY_TARGET_STATUS
    {
        NORMAL,
        FIGHTING
    }

    /// <summary>
    /// ログのカテゴリ
    /// </summary>
    public enum LOG_CATEGORY_TYPE
    {
        UNKNOWN1,
        UNKNOWN2,
        BATTLE,
        GAME_EVENT,
        SYSTEM_EVENT
    }

    /// <summary>
    /// 戦闘ログのタイプ
    /// </summary>
    public enum LOG_BATTLE_TYPE
    {
        UNKNOWN,
        HIT,
        MISS,
        DONE,
        ITEM,
        HEAL,
        EFFECT1,
        EFFECT2
    }

    public enum LOG_GAME_EVENT_TYPE
    {
        UNKNOWN,//38
        EVENT,//39
        DOWN_KO,//3A
        UNKNOWN2,//3B
        ATTENTION,//3C
        UNKOWN3,//3D
        GETITEM,//3E
        UNKNOWN4//3F

    }

    public enum LOG_SYSTEM_EVENT_TYPE
    {
        EXP,//40
        DICE,//41
        UNKNOWN1,//42
        UNKNOWN2,//43
        UNKNOWN3,//44
        LOGIN_LOGOUT,//45
    }

    public enum LOG_CATEGORY_UNKNOWN_FLG
    {
        ZERO,
        ONE
    }
}
