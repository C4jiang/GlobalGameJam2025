using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vietlabs.fr2;
using Object = UnityEngine.Object;
using System.IO;
using System.Reflection;
using I2.Loc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;
using System.Linq;

public static class FinderRefactorUtil {
    //[MenuItem("Tools/搜寻卡牌/列出所有卡牌名")]
    //public static void SearchAllCardName() {
    //    Debug.Log("列出所有卡牌名开始运行...");
    //    var cardInfos = FR2_Ref.FindUsedBy(new[] { "c1642e75f7764674eac30cf8dcf1c4e2" });
    //    foreach (var kvp in cardInfos) {
    //        if (kvp.Value.depth == 1) {
    //            var path = kvp.Value.asset.assetPath;
    //            var card = AssetDatabase.LoadAssetAtPath<CardInfo>(path);
    //            Debug.Log(card.cardName);
    //        }
    //    }
    //    Debug.Log("列出所有卡牌名运行结束");
    //}
}