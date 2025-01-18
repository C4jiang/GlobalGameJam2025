using System.Collections.Generic;
using System.Linq;

public static class MsgType
{
    public static string BattleFx = nameof(BattleFx);
    [System.Obsolete("请用BattleFx")] public static string BattleAttackFx = nameof(BattleFx);
    public static string BattleHurt = nameof(BattleHurt);
    public static string BattleDefence = nameof(BattleDefence);
    public static string MessureChanged = nameof(MessureChanged);
    public static string WaitSettleDown = nameof(WaitSettleDown);

    //arg1:GameObject entity, arg2: GameObject attacker
    public static string EntityDead = nameof(EntityDead);

    // arg1:GameObject entity, arg2: EffectTargetZone zone, arg3 slotIndex, arg4: EntityEffectType effectType, arg5 EntityEffectArgs args
    public static string EntityEffectToSlot = nameof(EntityEffectToSlot);

    public static string EntityActivateOver = nameof(EntityActivateOver);
    public static string EntityActivateOneAction = nameof(EntityActivateOneAction);

    public static string SendEmotion = nameof(SendEmotion);

    //卡牌区域整理的时候发送事件
    public static string CardFieldArrange = nameof(CardFieldArrange);

    public static string BattleLeave = nameof(BattleLeave);
    //玩家造成情绪影响事件 arg1:trust arg2:impression
    public static string EmotionAffect = nameof(EmotionAffect);
    public static string PostEmotionAffect = nameof(PostEmotionAffect);

    public static string OnEnterBattle = nameof(OnEnterBattle);
    public static string OnBattleBegin = nameof(OnBattleBegin);
    public static string OnFirstRoundEnd = nameof(OnFirstRoundEnd);
    public static string OnPlayerRoundStart = nameof(OnPlayerRoundStart);
    public static string OnPlayerRoundEnd = nameof(OnPlayerRoundEnd);
    public static string OnCardSettlementEnd = nameof(OnCardSettlementEnd);
    public static string OnActivateCardStart = nameof(OnActivateCardStart);
    public static string OnEntityRoundStart = nameof(OnEntityRoundStart);

    public static string PlayBGM = nameof(PlayBGM);
    public static string PlaySE = nameof(PlaySE);
    public static string StopBGM = nameof(StopBGM);
    public static string LowerBGMVolumn = nameof(LowerBGMVolumn);
    public static string ResumeBGMVolumn = nameof(ResumeBGMVolumn);

    public static string CardPosChange = nameof(CardPosChange);
    public static string PickUpCard = nameof(PickUpCard);
    public static string UnTouchCard = nameof(UnTouchCard);
    public static string DropCard = nameof(DropCard);
    public static string PlayerMoveCard = nameof(PlayerMoveCard);
    public static string CardEnterBattleField = nameof(CardEnterBattleField);
    public static string InsertTrashCard = nameof(InsertTrashCard);
    
    public static string DrawCard = nameof(DrawCard);

    public static string ChangeLanguage = nameof(ChangeLanguage);
    public static string EntityChatCompleted = nameof(EntityChatCompleted);
    public static string EntityMaidChatStart = nameof(EntityMaidChatStart);
    public static string AttackTweenFinish = nameof(AttackTweenFinish);
    public static string WaitNextCharChanged = nameof(WaitNextCharChanged);

    public static string ChestIconOpeningReady = nameof(ChestIconOpeningReady);
    public static string RelationPhaseRewardInvested = nameof(RelationPhaseRewardInvested);

    public static string CardAbilitiesChanged = nameof(CardAbilitiesChanged);

    //GameResult
    public static string CloseGameResult = nameof(CloseGameResult);

    //GameProgressPlayerInput
    public static string NewGameStart = nameof(NewGameStart);
    public static string GameStart = nameof(GameStart);
    public static string GameEnd = nameof(GameEnd);
    public static string GameOverPlayerDead = nameof(GameOverPlayerDead);
    public static string OnBeforeGameStartFirstDraw = nameof(OnBeforeGameStartFirstDraw);
    public static string OnAfterGameStartFirstDraw = nameof(OnAfterGameStartFirstDraw);
    public static string RestartGame = nameof(RestartGame);
    public static string ContinueGame = nameof(ContinueGame);
    public static string GiveUpGame = nameof(GiveUpGame);
    public static string ReturnMainMenu = nameof(ReturnMainMenu);
    public static string EndingStaffList = nameof(EndingStaffList);

    //PlayManager
    public static string ScriptOver = nameof(ScriptOver);
    public static string AdvanceModeTrigger = nameof(AdvanceModeTrigger);
    public static string AdvanceModeCheck = nameof(AdvanceModeCheck);
    public static string GetCoin = nameof(GetCoin);
    public static string GetCoinWithNum = nameof(GetCoinWithNum);

    public static string TestSpineTrigger = nameof(TestSpineTrigger);
    public static string PlayerChatComplete = nameof(PlayerChatComplete);

    // Test Only
    public static string TestEvent = nameof(TestEvent);

    // Water Module
    public static string RefreshWaterCnt = nameof(RefreshWaterCnt); // 刷新UI界面魔药份数
    public static string BlowSuccess = nameof(BlowSuccess); // 稳定度小于等于阈值，吹泡泡成功
    public static string BlowFail = nameof(BlowFail); // 稳定度大于阈值，吹泡泡失败

    // Bubble Module
    public static string BubbleSuccess = nameof(BubbleSuccess); // 泡泡匹配成功
    public static string BubbleFail = nameof(BubbleFail); // 泡泡匹配失败

    // Customer Module
    public static string CreateCustomer = nameof(CreateCustomer); // 创建顾客
    public static string NoCustomer = nameof(NoCustomer); // 无顾客
    public static string DialogComplete = nameof(DialogComplete); // 对话结束

    // intro & ending
    public static string StartIntro = nameof(StartIntro);   // 播放片头曲
    public static string StartEnding = nameof(StartEnding); // 播放结束曲

    public static string IntroEnd = nameof(IntroEnd);   // 片头曲结束
    public static string EndingEnd = nameof(EndingEnd); // 结束曲结束


    private static IEnumerable<string> _allEventNames;
    public static IEnumerable<string> GetEventNames()
    {
        if(_allEventNames == null)
        {
            _allEventNames = typeof(MsgType).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Select(x => x.GetValue(null).ToString());
        }
        return _allEventNames;
    }
}