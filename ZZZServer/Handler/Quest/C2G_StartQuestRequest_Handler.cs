using cfg.main;

using Kirara.Network;
using Serilog;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Quest;

public class ReqStartQuest_Handler : RpcHandler<ReqStartQuest, RspStartQuest>
{
    protected override void Run(Session session, ReqStartQuest req, RspStartQuest rsp, Action reply)
    {
        var player = (Player)session.Data;

        var questConfig = ConfigMgr.tb.TbQuestChainConfig[req.QuestChainCid].Quests
            .Find(questConfig => questConfig.QuestCid == req.QuestCid);
        if (questConfig == null)
        {
            rsp.Result = new Result { Code = 1, Msg = "任务不存在" };
            return;
        }

        if (player.Room == null)
        {
            rsp.Result = new Result { Code = 2, Msg = "不在房间中" };
            return;
        }

        if (questConfig is DefeatQuestConfig defeatQuestConfig)
        {
            var monsterConfig = ConfigMgr.tb.TbMonsterConfig[defeatQuestConfig.MonsterCid];

            NFloat3 pos;
            if (req.QuestChainCid == 1)
            {
                pos = new NFloat3()
                {
                    X = 0,
                    Y = 0,
                    Z = 0,
                };
            }
            else if (req.QuestChainCid == 2)
            {
                // 任务第二章，临时先这样。
                pos = new NFloat3()
                {
                    X = 50,
                    Y = 0,
                    Z = 0,
                };
            }
            else
            {
                pos = new NFloat3()
                {
                    X = 0,
                    Y = 0,
                    Z = 0,
                };
                Log.Warning($"错误的任务cid {defeatQuestConfig.QuestCid}");
            }

            for (int i = 0; i < defeatQuestConfig.Count; i++)
            {
                player.Room.SpawnMonster(monsterConfig.Id, new NPosRot()
                {
                    Pos = pos,
                    Rot = new NFloat3()
                });
            }
        }
    }
}