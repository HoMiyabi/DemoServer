using cfg.main;

using Kirara.Network;
using Mathd;
using Serilog;
using ZZZServer.Model;

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

            Vector3d position = new Vector3d();
            if (req.QuestChainCid == 1)
            {
                position = new Vector3d(13.5, -1.65, 19.65);
            }
            else if (req.QuestChainCid == 2)
            {
                // 任务第二章，临时先这样。
                position = new Vector3d(13.5, -1.65, 19.65);
            }
            else
            {
                Log.Warning($"错误的任务cid {defeatQuestConfig.QuestCid}");
            }

            for (int i = 0; i < defeatQuestConfig.Count; i++)
            {
                var room = player.Room;
                var monster = new ZZZServer.SVEntity.Monster(monsterConfig.Id, room, room.NextMonsterId,
                    position, Quaterniond.identity);
                room.monsters.Add(monster);
            }
        }
    }
}