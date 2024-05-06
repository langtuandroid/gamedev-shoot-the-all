using System;
using UnityEngine;

namespace Scripts.Gameplay.Managers
{
    public static class PlayerPrefsManager
    {
        public static Action ON_COINS_CHANGED;

        private const string PAID_CURRENCY_KEY = "PAID_CURRENCY";
        private const string IN_GAME_CURRENCY_KEY = "IN_GAME_CURRENCY";
        private const string LEVEL_KEY = "LEVEL";
        private const string SKIN_UNLOCKED_KEY_ = "SKIN_UNLOCKED_";
        private const string EQUIPPED_SKIN = "EQUIPPED_SKIN_";
        private const string AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS = "SKIN_WATCHED_FOR_ADS_";
        private const string STARS_ON_LEVEL_ = "STARS_LEVEL";
        private const string AUDIO = "AUDIO";
        
        public static int GetAudio()
        { return PlayerPrefs.GetInt(AUDIO, 0); }

        public static void SetAudio(int enabled)
        { PlayerPrefs.SetInt(AUDIO, enabled); }
        
        public static int GetLevel()
        { return PlayerPrefs.GetInt(LEVEL_KEY, 1); }

        public static void SetLevel(int level)
        { PlayerPrefs.SetInt(LEVEL_KEY, level); }

        public static int GetPaidCurrency() => PlayerPrefs.GetInt(PAID_CURRENCY_KEY, 0);

        public static void AddPaidCurrency(int currencyAmount)
        {
            PlayerPrefs.SetInt(PAID_CURRENCY_KEY, GetPaidCurrency() + currencyAmount);
            ON_COINS_CHANGED?.Invoke();
        }
        
        public static int GetInGameMoney() => PlayerPrefs.GetInt(IN_GAME_CURRENCY_KEY, 0);

        public static void AddInGameMoney(int currencyAmount)
        {
            PlayerPrefs.SetInt(IN_GAME_CURRENCY_KEY, GetInGameMoney() + currencyAmount);
            ON_COINS_CHANGED?.Invoke();
        }

        public static int GetIsSkinUnlocked(int index)
        { return PlayerPrefs.GetInt(SKIN_UNLOCKED_KEY_ + index, 0); }

        public static void SetSkinUnlocked(int index)
        { PlayerPrefs.SetInt(SKIN_UNLOCKED_KEY_ + index, 1); }

        public static int GetCurrentEquippedSkin()
        { return PlayerPrefs.GetInt(EQUIPPED_SKIN, 0); }

        public static void SetCurrentEquippedSkin(int index)
        { PlayerPrefs.SetInt(EQUIPPED_SKIN, index); }

        public static void SetStars(int level, int stars)
        { PlayerPrefs.SetInt(STARS_ON_LEVEL_ + level,  stars); }

        public static int GetStars(int level)
        { return PlayerPrefs.GetInt(STARS_ON_LEVEL_ + level, 0); }

        public static bool HasStars(int level)
        { return PlayerPrefs.HasKey(STARS_ON_LEVEL_ + level); }

        public static void AddSkinForAdsWatched(int skinIndex) => PlayerPrefs.SetInt(AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS + skinIndex, GetSkinForAdsWatched(skinIndex) + 1);

        public static int GetSkinForAdsWatched(int skinIndex) => PlayerPrefs.GetInt(AMOUNT_OF_TIMES_SKIN_WATCHED_FOR_ADS + skinIndex, 0);
    }
}
