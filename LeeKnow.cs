using MelonLoader;
using BTD_Mod_Helper;
using LeeKnow;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Models.Profile;
using BTD_Mod_Helper.Api.ModOptions;

[assembly: MelonInfo(typeof(LeeKnow.LeeKnow), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace LeeKnow;
public class LeeKnow : BloonsTD6Mod
{
    public static ModSettingBool Enable = new(true) { displayName = "Enable" };
    public static ModSettingBool SmallTowers = new(true) { displayName = "Unlock Small Towers" };
    public static ModSettingBool SmallBloons = new(true) { displayName = "Unlock Small Bloons" };
    public static ModSettingBool BigTowers = new(true) { displayName = "Unlock Big Towers" };
    public static ModSettingBool BigBloons = new(true) { displayName = "Unlock Big Bloons" };
    public static ModSettingBool DoubleCash = new(true) { displayName = "Unlock Double Cash" };
    public static ModSettingInt MonkeyMoneyAmount = new ModSettingInt(2000000) { displayName = "Monkey Money Amount" };
    public static ModSettingInt MonkeyKnowledgeAmount = new ModSettingInt(134) { displayName = "Monkey Knowledge Amount" };
    public static ModSettingInt TowerXPAmount = new ModSettingInt(1250000) { displayName = "Tower XP Amount" };
    // public static ModSettingInt PlayerXPAmount = new ModSettingInt(180000000) { displayName = "Player XP Amount" };
    // public static ModSettingInt VeteranPlayerXPAmount = new ModSettingInt(1800000000) { displayName = "Player Veteran XP Amount" };
    public static ModSettingInt PowersAmount = new ModSettingInt(990000000) { displayName = "Powers Amount" };

    [HarmonyLib.HarmonyPatch(typeof(ProfileModel), "Validate")]

    public class ProfileModel_Patch
    {
        [HarmonyLib.HarmonyPostfix]
        public static void Postfix(ProfileModel __instance)
        {
            __instance.rank.Value = 155;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(MainMenu), "Open")]

    public class MainMenu_Patch
    {
        [HarmonyLib.HarmonyPostfix]
        public static void Postfix()
        {
            if (!Enable) return;

            Game.instance.playerService.Player.Data.unlockedSmallBloons = SmallBloons;
            Game.instance.playerService.Player.Data.unlockedSmallTowers = SmallTowers;
            Game.instance.playerService.Player.Data.unlockedBigBloons = BigBloons;
            Game.instance.playerService.Player.Data.unlockedBigTowers = BigTowers;

            Game.instance.playerService.Player.Data.purchase.purchasedDoubleCashMode = DoubleCash;

            Game.instance.playerService.Player.Data.KnowledgePoints = MonkeyKnowledgeAmount;
            Game.instance.playerService.Player.Data.monkeyMoney.Value = MonkeyMoneyAmount;

            foreach (var power in Game.instance.model.powers)
            {
                if (power.name is "CaveMonkey" or "DungeonStatue") continue;

                if (Game.Player.IsPowerAvailable(power.name))
                {
                    Game.Player.GetPowerData(power.name).Quantity = PowersAmount;
                }
                else
                {
                    Game.Player.AddPower(power.name, PowersAmount);
                }
            }
            for (int i = 0; i < Game.instance.model.towers.Count; i++)
            {
                Game.instance.playerService.Player.AddTowerXP(Game.instance.model.towers[i].name, 100);
            }
            foreach (var item in Game.instance.playerService.Player.Data.towerXp)
            {
                Game.instance.playerService.Player.Data.towerXp[item.key].Value = TowerXPAmount;
            }
        }
    }
}