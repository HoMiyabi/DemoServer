using Kirara.Network;

public class MsgMeta : IMsgMeta
{
    public Google.Protobuf.MessageParser GetParser(uint cmdId)
    {
        return cmdId switch
        {
            MsgCmdId.Result => Result.Parser,
            MsgCmdId.Ping => Ping.Parser,
            MsgCmdId.Pong => Pong.Parser,
            MsgCmdId.NPlayer => NPlayer.Parser,
            MsgCmdId.NOtherPlayer => NOtherPlayer.Parser,
            MsgCmdId.NMaterialItem => NMaterialItem.Parser,
            MsgCmdId.NCurrencyItem => NCurrencyItem.Parser,
            MsgCmdId.NWeaponItem => NWeaponItem.Parser,
            MsgCmdId.NDiscItem => NDiscItem.Parser,
            MsgCmdId.NFloat3 => NFloat3.Parser,
            MsgCmdId.NPosRot => NPosRot.Parser,
            MsgCmdId.NWeaponAttr => NWeaponAttr.Parser,
            MsgCmdId.NDiscAttr => NDiscAttr.Parser,
            MsgCmdId.MsgGachaWeapon => MsgGachaWeapon.Parser,
            MsgCmdId.MsgGachaDisc => MsgGachaDisc.Parser,
            MsgCmdId.ReqRegister => ReqRegister.Parser,
            MsgCmdId.RspRegister => RspRegister.Parser,
            MsgCmdId.ReqLogin => ReqLogin.Parser,
            MsgCmdId.RspLogin => RspLogin.Parser,
            MsgCmdId.ReqGetPlayerData => ReqGetPlayerData.Parser,
            MsgCmdId.RspGetPlayerData => RspGetPlayerData.Parser,
            MsgCmdId.NSyncRole => NSyncRole.Parser,
            MsgCmdId.NSyncPlayer => NSyncPlayer.Parser,
            MsgCmdId.MsgEnterRoom => MsgEnterRoom.Parser,
            MsgCmdId.MsgUpdateFromAutonomous => MsgUpdateFromAutonomous.Parser,
            MsgCmdId.NotifyUpdateFromAuthority => NotifyUpdateFromAuthority.Parser,
            MsgCmdId.NotifyAddSimulatedPlayers => NotifyAddSimulatedPlayers.Parser,
            MsgCmdId.NotifyRemoveSimulatedPlayers => NotifyRemoveSimulatedPlayers.Parser,
            MsgCmdId.MsgRolePlayAction => MsgRolePlayAction.Parser,
            MsgCmdId.NotifyOtherRolePlayAction => NotifyOtherRolePlayAction.Parser,
            MsgCmdId.MsgSwitchRole => MsgSwitchRole.Parser,
            MsgCmdId.ReqGetExchangeItems => ReqGetExchangeItems.Parser,
            MsgCmdId.RspGetExchangeItems => RspGetExchangeItems.Parser,
            MsgCmdId.NExchangeItem => NExchangeItem.Parser,
            MsgCmdId.ReqExchange => ReqExchange.Parser,
            MsgCmdId.RspExchange => RspExchange.Parser,
            MsgCmdId.ReqSearchPlayer => ReqSearchPlayer.Parser,
            MsgCmdId.RspSearchPlayer => RspSearchPlayer.Parser,
            MsgCmdId.ReqGetFriendInfos => ReqGetFriendInfos.Parser,
            MsgCmdId.RspGetFriendInfos => RspGetFriendInfos.Parser,
            MsgCmdId.ReqSendAddFriend => ReqSendAddFriend.Parser,
            MsgCmdId.RspSendAddFriend => RspSendAddFriend.Parser,
            MsgCmdId.ReqAcceptAddFriend => ReqAcceptAddFriend.Parser,
            MsgCmdId.RspAcceptAddFriend => RspAcceptAddFriend.Parser,
            MsgCmdId.ReqRefuseAddFriend => ReqRefuseAddFriend.Parser,
            MsgCmdId.RspRefuseAddFriend => RspRefuseAddFriend.Parser,
            MsgCmdId.ReqDeleteFriend => ReqDeleteFriend.Parser,
            MsgCmdId.RspDeleteFriend => RspDeleteFriend.Parser,
            MsgCmdId.ReqModifySignature => ReqModifySignature.Parser,
            MsgCmdId.RspModifySignature => RspModifySignature.Parser,
            MsgCmdId.ReqModifyPassword => ReqModifyPassword.Parser,
            MsgCmdId.RspModifyPassword => RspModifyPassword.Parser,
            MsgCmdId.ReqModifyAvatar => ReqModifyAvatar.Parser,
            MsgCmdId.RspModifyAvatar => RspModifyAvatar.Parser,
            MsgCmdId.NChatMsg => NChatMsg.Parser,
            MsgCmdId.NChatMsgRecord => NChatMsgRecord.Parser,
            MsgCmdId.ReqSendChatMsg => ReqSendChatMsg.Parser,
            MsgCmdId.RspSendChatMsg => RspSendChatMsg.Parser,
            MsgCmdId.NotifyReceiveChatMsg => NotifyReceiveChatMsg.Parser,
            MsgCmdId.ReqGetChatRecords => ReqGetChatRecords.Parser,
            MsgCmdId.RspGetChatRecords => RspGetChatRecords.Parser,
            MsgCmdId.NRole => NRole.Parser,
            MsgCmdId.ReqRoleRemoveDisc => ReqRoleRemoveDisc.Parser,
            MsgCmdId.RspRoleRemoveDisc => RspRoleRemoveDisc.Parser,
            MsgCmdId.ReqRoleEquipDisc => ReqRoleEquipDisc.Parser,
            MsgCmdId.RspRoleEquipDisc => RspRoleEquipDisc.Parser,
            MsgCmdId.ReqRoleRemoveWeapon => ReqRoleRemoveWeapon.Parser,
            MsgCmdId.RspRoleRemoveWeapon => RspRoleRemoveWeapon.Parser,
            MsgCmdId.ReqRoleEquipWeapon => ReqRoleEquipWeapon.Parser,
            MsgCmdId.RspRoleEquipWeapon => RspRoleEquipWeapon.Parser,
            MsgCmdId.NotifySpawnMonster => NotifySpawnMonster.Parser,
            MsgCmdId.MsgMonsterTakeDamage => MsgMonsterTakeDamage.Parser,
            MsgCmdId.NotifySyncMonsterTakeDamage => NotifySyncMonsterTakeDamage.Parser,
            MsgCmdId.NotifyMonsterDie => NotifyMonsterDie.Parser,
            MsgCmdId.ReqStartQuest => ReqStartQuest.Parser,
            MsgCmdId.RspStartQuest => RspStartQuest.Parser,
            MsgCmdId.ReqUpgradeDisc => ReqUpgradeDisc.Parser,
            MsgCmdId.RspUpgradeDisc => RspUpgradeDisc.Parser,
            MsgCmdId.MsgGatherMaterial => MsgGatherMaterial.Parser,
            MsgCmdId.MsgCompleteQuestChain => MsgCompleteQuestChain.Parser,
            MsgCmdId.NotifyObtainItems => NotifyObtainItems.Parser,
            _ => null
        };
    }
}

public partial class Result : IMsg
{
    public uint CmdId => MsgCmdId.Result;
}
public partial class Ping : IMsg
{
    public uint CmdId => MsgCmdId.Ping;
}
public partial class Pong : IRsp
{
    public uint CmdId => MsgCmdId.Pong;
}
public partial class NPlayer : IMsg
{
    public uint CmdId => MsgCmdId.NPlayer;
}
public partial class NOtherPlayer : IMsg
{
    public uint CmdId => MsgCmdId.NOtherPlayer;
}
public partial class NMaterialItem : IMsg
{
    public uint CmdId => MsgCmdId.NMaterialItem;
}
public partial class NCurrencyItem : IMsg
{
    public uint CmdId => MsgCmdId.NCurrencyItem;
}
public partial class NWeaponItem : IMsg
{
    public uint CmdId => MsgCmdId.NWeaponItem;
}
public partial class NDiscItem : IMsg
{
    public uint CmdId => MsgCmdId.NDiscItem;
}
public partial class NFloat3 : IMsg
{
    public uint CmdId => MsgCmdId.NFloat3;
}
public partial class NPosRot : IMsg
{
    public uint CmdId => MsgCmdId.NPosRot;
}
public partial class NWeaponAttr : IMsg
{
    public uint CmdId => MsgCmdId.NWeaponAttr;
}
public partial class NDiscAttr : IMsg
{
    public uint CmdId => MsgCmdId.NDiscAttr;
}
public partial class MsgGachaWeapon : IMsg
{
    public uint CmdId => MsgCmdId.MsgGachaWeapon;
}
public partial class MsgGachaDisc : IMsg
{
    public uint CmdId => MsgCmdId.MsgGachaDisc;
}
public partial class ReqRegister : IMsg
{
    public uint CmdId => MsgCmdId.ReqRegister;
}
public partial class RspRegister : IRsp
{
    public uint CmdId => MsgCmdId.RspRegister;
}
public partial class ReqLogin : IMsg
{
    public uint CmdId => MsgCmdId.ReqLogin;
}
public partial class RspLogin : IRsp
{
    public uint CmdId => MsgCmdId.RspLogin;
}
public partial class ReqGetPlayerData : IMsg
{
    public uint CmdId => MsgCmdId.ReqGetPlayerData;
}
public partial class RspGetPlayerData : IRsp
{
    public uint CmdId => MsgCmdId.RspGetPlayerData;
}
public partial class NSyncRole : IMsg
{
    public uint CmdId => MsgCmdId.NSyncRole;
}
public partial class NSyncPlayer : IMsg
{
    public uint CmdId => MsgCmdId.NSyncPlayer;
}
public partial class MsgEnterRoom : IMsg
{
    public uint CmdId => MsgCmdId.MsgEnterRoom;
}
public partial class MsgUpdateFromAutonomous : IMsg
{
    public uint CmdId => MsgCmdId.MsgUpdateFromAutonomous;
}
public partial class NotifyUpdateFromAuthority : IMsg
{
    public uint CmdId => MsgCmdId.NotifyUpdateFromAuthority;
}
public partial class NotifyAddSimulatedPlayers : IMsg
{
    public uint CmdId => MsgCmdId.NotifyAddSimulatedPlayers;
}
public partial class NotifyRemoveSimulatedPlayers : IMsg
{
    public uint CmdId => MsgCmdId.NotifyRemoveSimulatedPlayers;
}
public partial class MsgRolePlayAction : IMsg
{
    public uint CmdId => MsgCmdId.MsgRolePlayAction;
}
public partial class NotifyOtherRolePlayAction : IMsg
{
    public uint CmdId => MsgCmdId.NotifyOtherRolePlayAction;
}
public partial class MsgSwitchRole : IMsg
{
    public uint CmdId => MsgCmdId.MsgSwitchRole;
}
public partial class ReqGetExchangeItems : IMsg
{
    public uint CmdId => MsgCmdId.ReqGetExchangeItems;
}
public partial class RspGetExchangeItems : IRsp
{
    public uint CmdId => MsgCmdId.RspGetExchangeItems;
}
public partial class NExchangeItem : IMsg
{
    public uint CmdId => MsgCmdId.NExchangeItem;
}
public partial class ReqExchange : IMsg
{
    public uint CmdId => MsgCmdId.ReqExchange;
}
public partial class RspExchange : IRsp
{
    public uint CmdId => MsgCmdId.RspExchange;
}
public partial class ReqSearchPlayer : IMsg
{
    public uint CmdId => MsgCmdId.ReqSearchPlayer;
}
public partial class RspSearchPlayer : IRsp
{
    public uint CmdId => MsgCmdId.RspSearchPlayer;
}
public partial class ReqGetFriendInfos : IMsg
{
    public uint CmdId => MsgCmdId.ReqGetFriendInfos;
}
public partial class RspGetFriendInfos : IRsp
{
    public uint CmdId => MsgCmdId.RspGetFriendInfos;
}
public partial class ReqSendAddFriend : IMsg
{
    public uint CmdId => MsgCmdId.ReqSendAddFriend;
}
public partial class RspSendAddFriend : IRsp
{
    public uint CmdId => MsgCmdId.RspSendAddFriend;
}
public partial class ReqAcceptAddFriend : IMsg
{
    public uint CmdId => MsgCmdId.ReqAcceptAddFriend;
}
public partial class RspAcceptAddFriend : IRsp
{
    public uint CmdId => MsgCmdId.RspAcceptAddFriend;
}
public partial class ReqRefuseAddFriend : IMsg
{
    public uint CmdId => MsgCmdId.ReqRefuseAddFriend;
}
public partial class RspRefuseAddFriend : IRsp
{
    public uint CmdId => MsgCmdId.RspRefuseAddFriend;
}
public partial class ReqDeleteFriend : IMsg
{
    public uint CmdId => MsgCmdId.ReqDeleteFriend;
}
public partial class RspDeleteFriend : IRsp
{
    public uint CmdId => MsgCmdId.RspDeleteFriend;
}
public partial class ReqModifySignature : IMsg
{
    public uint CmdId => MsgCmdId.ReqModifySignature;
}
public partial class RspModifySignature : IRsp
{
    public uint CmdId => MsgCmdId.RspModifySignature;
}
public partial class ReqModifyPassword : IMsg
{
    public uint CmdId => MsgCmdId.ReqModifyPassword;
}
public partial class RspModifyPassword : IRsp
{
    public uint CmdId => MsgCmdId.RspModifyPassword;
}
public partial class ReqModifyAvatar : IMsg
{
    public uint CmdId => MsgCmdId.ReqModifyAvatar;
}
public partial class RspModifyAvatar : IRsp
{
    public uint CmdId => MsgCmdId.RspModifyAvatar;
}
public partial class NChatMsg : IMsg
{
    public uint CmdId => MsgCmdId.NChatMsg;
}
public partial class NChatMsgRecord : IMsg
{
    public uint CmdId => MsgCmdId.NChatMsgRecord;
}
public partial class ReqSendChatMsg : IMsg
{
    public uint CmdId => MsgCmdId.ReqSendChatMsg;
}
public partial class RspSendChatMsg : IRsp
{
    public uint CmdId => MsgCmdId.RspSendChatMsg;
}
public partial class NotifyReceiveChatMsg : IMsg
{
    public uint CmdId => MsgCmdId.NotifyReceiveChatMsg;
}
public partial class ReqGetChatRecords : IMsg
{
    public uint CmdId => MsgCmdId.ReqGetChatRecords;
}
public partial class RspGetChatRecords : IRsp
{
    public uint CmdId => MsgCmdId.RspGetChatRecords;
}
public partial class NRole : IMsg
{
    public uint CmdId => MsgCmdId.NRole;
}
public partial class ReqRoleRemoveDisc : IMsg
{
    public uint CmdId => MsgCmdId.ReqRoleRemoveDisc;
}
public partial class RspRoleRemoveDisc : IRsp
{
    public uint CmdId => MsgCmdId.RspRoleRemoveDisc;
}
public partial class ReqRoleEquipDisc : IMsg
{
    public uint CmdId => MsgCmdId.ReqRoleEquipDisc;
}
public partial class RspRoleEquipDisc : IRsp
{
    public uint CmdId => MsgCmdId.RspRoleEquipDisc;
}
public partial class ReqRoleRemoveWeapon : IMsg
{
    public uint CmdId => MsgCmdId.ReqRoleRemoveWeapon;
}
public partial class RspRoleRemoveWeapon : IRsp
{
    public uint CmdId => MsgCmdId.RspRoleRemoveWeapon;
}
public partial class ReqRoleEquipWeapon : IMsg
{
    public uint CmdId => MsgCmdId.ReqRoleEquipWeapon;
}
public partial class RspRoleEquipWeapon : IRsp
{
    public uint CmdId => MsgCmdId.RspRoleEquipWeapon;
}
public partial class NotifySpawnMonster : IMsg
{
    public uint CmdId => MsgCmdId.NotifySpawnMonster;
}
public partial class MsgMonsterTakeDamage : IMsg
{
    public uint CmdId => MsgCmdId.MsgMonsterTakeDamage;
}
public partial class NotifySyncMonsterTakeDamage : IMsg
{
    public uint CmdId => MsgCmdId.NotifySyncMonsterTakeDamage;
}
public partial class NotifyMonsterDie : IMsg
{
    public uint CmdId => MsgCmdId.NotifyMonsterDie;
}
public partial class ReqStartQuest : IMsg
{
    public uint CmdId => MsgCmdId.ReqStartQuest;
}
public partial class RspStartQuest : IRsp
{
    public uint CmdId => MsgCmdId.RspStartQuest;
}
public partial class ReqUpgradeDisc : IMsg
{
    public uint CmdId => MsgCmdId.ReqUpgradeDisc;
}
public partial class RspUpgradeDisc : IRsp
{
    public uint CmdId => MsgCmdId.RspUpgradeDisc;
}
public partial class MsgGatherMaterial : IMsg
{
    public uint CmdId => MsgCmdId.MsgGatherMaterial;
}
public partial class MsgCompleteQuestChain : IMsg
{
    public uint CmdId => MsgCmdId.MsgCompleteQuestChain;
}
public partial class NotifyObtainItems : IMsg
{
    public uint CmdId => MsgCmdId.NotifyObtainItems;
}
