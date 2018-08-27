using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viveport.Internal;
using System;
using Steamworks;
using System.ComponentModel;

namespace IMRE.Wrappers
{
	/// <summary>
	/// This script serves as a wrapper for Achievements in Steamworks and Viveport for any IMRE scene.
	/// The main contributor(s) to this script is CB
	/// Status: SHELVED
	/// </summary>
	public class Achievements : MonoBehaviour
	{

        public static Achievements ins;

        static string VIVEPORT_ID = "5e9252c5-848d-44a1-9445-0cf8c540805b";
        static readonly int STEAMWORKS_ID_INT = 207207;
        static AppId_t STEAMWORKS_ID
        {
            get
            {
                //APPID IS INVALID UNTIL WE HAVE A PROPER INT ASSIGNED.
                return AppId_t.Invalid;
                //return new AppId_t((uint)STEAMWORKS_ID_INT);
            }
        }

        public enum achievementMode { steamworks, viveport, drmfree };
        public achievementMode currentMode;

		#region private warpper variables
		/// <summary>
		/// Variables for viveport integration.
		/// </summary>
		private static bool bInit = true, bIsReady = false;

		/// <summary>
		/// Variables for steamworks integration.
		/// </summary>
		private static bool s_EverInialized;

        private bool m_bInitialized;
        public static bool Initialized
        {
            get
            {
                return ins.m_bInitialized;
            }
        }

        private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
        private static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }
        #endregion

        #region initialization
        // Use this for initialization
        void Awake()
		{
            ins = this;
            //this is set to viveport until we get steam support.
            currentMode = achievementMode.viveport;
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    Api.Init(InitStatusHandler, VIVEPORT_ID);
                    break;
                case achievementMode.viveport:
                    checkSteamworksValid();
                    break;
				case achievementMode.drmfree:
					break;
                default:
                    Debug.LogWarning("NOT INTERGATED WTIH VIVEPORT OR STEAMWORKS");
                    break;
            }
			// We want our SteamManager Instance to persist across scenes.
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region General Wrapper
        public void setAchievement(string API_key, bool value)
		{
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    SteamUserStats.SetAchievement(API_key);
                    SteamAPI.RunCallbacks();
                    break;
                case achievementMode.viveport:
                    UserStats.IsReady(IsReadyHandler);
                    UserStats.SetAchievement(API_key);
                    UserStats.UploadStats(UploadStatsHandler);
                    break;
				case achievementMode.drmfree:
					break;
            }
		}

		public bool getAchievement(string API_key)
		{
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    SteamAPI.RunCallbacks();
                    bool result;
                    SteamUserStats.GetAchievement(API_key, out result);
                    break;
                case achievementMode.viveport:
                    UserStats.IsReady(IsReadyHandler);
                    UserStats.DownloadStats(DownloadStatsHandler);
                    int unlockTime = 0;
                    return UserStats.GetAchievementUnlockTime(API_key, ref unlockTime) == 0;
				case achievementMode.drmfree:
					break;

            }
            return false;

		}
		public DateTime getAchievementTime(string API_key)
		{
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    SteamAPI.RunCallbacks();
                    bool done;
                    uint unlockTime;
                    SteamUserStats.GetAchievementAndUnlockTime(API_key, out done, out unlockTime);
                    return FromUnixTime((int)unlockTime);
                case achievementMode.viveport:
                    UserStats.IsReady(IsReadyHandler);
                    UserStats.DownloadStats(DownloadStatsHandler);
                    int unlockTime2 = 0;
                    UserStats.GetAchievementUnlockTime(API_key, ref unlockTime2);
                    return FromUnixTime(unlockTime2);
				case achievementMode.drmfree:
					break;
            }
            return new DateTime(1990, 1, 1);

		}

		public void setStat(string API_key, float value)
		{
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    SteamUserStats.SetStat(API_key, value);
                    SteamAPI.RunCallbacks();
                    break;
                case achievementMode.viveport:
                    UserStats.IsReady(IsReadyHandler);
                    UserStats.SetStat(API_key, value);
                    UserStats.UploadStats(UploadStatsHandler);
                    break;
				case achievementMode.drmfree:
					break;
            }

		}

		public float getStat(string API_key)
		{
            switch (currentMode)
            {
                case achievementMode.steamworks:
                    SteamAPI.RunCallbacks();
                    float value = 0f;
                    SteamUserStats.GetStat(API_key, out value);
                    return value;
                case achievementMode.viveport:
                    UserStats.IsReady(IsReadyHandler);
                    UserStats.DownloadStats(DownloadStatsHandler);
                    float value2 = 0f;
                    UserStats.GetStat(API_key, ref value2);
                    return value2;
				case achievementMode.drmfree:
					break;
            }
            return float.NaN;
		}

        public void incrementStat(string API_key)
        {

            float value = getStat(API_key);
            value++;
            setStat(API_key, value);
        }
        #endregion
        
        #region  Viveport Wrapper Functions
        public static DateTime FromUnixTime(long unixTime)
		{
			return epoch.AddSeconds(unixTime);
		}
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static void InitStatusHandler(int nResult)
		{
			if (nResult == 0)
			{
				bInit = true;
				bIsReady = false;
				Viveport.Core.Logger.Log("InitStatusHandler is successful");
			}
			else
			{
				bInit = false;
				Viveport.Core.Logger.Log("InitStatusHandler error : " + nResult);
			}
		}

		private static void IsReadyHandler(int nResult)
		{
			if (nResult == 0)
			{
				bIsReady = true;
				Viveport.Core.Logger.Log("IsReadyHandler is successful");
			}
			else
			{
				bIsReady = false;
				Viveport.Core.Logger.Log("IsReadyHandler error: " + nResult);
			}
		}

		private static void ShutdownHandler(int nResult)
		{
			if (nResult == 0)
			{
				bInit = false;
				bIsReady = false;
				Viveport.Core.Logger.Log("ShutdownHandler is successful");
			}
			else
			{
				Viveport.Core.Logger.Log("ShutdownHandler error: " + nResult);
			}
		}

		private static void DownloadStatsHandler(int nResult)
		{
			if (nResult == 0)
				Viveport.Core.Logger.Log("DownloadStatsHandler is successful ");
			else
				Viveport.Core.Logger.Log("DownloadStatsHandler error: " + nResult);
		}

		private static void UploadStatsHandler(int nResult)
		{
			if (nResult == 0)
				Viveport.Core.Logger.Log("UploadStatsHandler is successful");
			else
				Viveport.Core.Logger.Log("UploadStatsHandler error: " + nResult);
		}
        #endregion
        #region Steamworks WrapperFunctions
        private void checkSteamworksValid()
        {
            if (s_EverInialized)
            {
                // This is almost always an error.
                // The most common case where this happens is when SteamManager gets destroyed because of Application.Quit(),
                // and then some Steamworks code in some other OnDestroy gets called afterwards, creating a new SteamManager.
                // You should never call Steamworks functions in OnDestroy, always prefer OnDisable if possible.
                throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
            }

            if (!Packsize.Test())
            {
                Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
            }

            if (!DllCheck.Test())
            {
                Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
            }

            try
            {
                // If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the
                // Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.

                // Once you get a Steam AppID assigned by Valve, you need to replace AppId_t.Invalid with it and
                // remove steam_appid.txt from the game depot. eg: "(AppId_t)480" or "new AppId_t(480)".
                // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                if (SteamAPI.RestartAppIfNecessary(STEAMWORKS_ID))
                {
                    Application.Quit();
                    return;
                }
            }
            catch (System.DllNotFoundException e)
            { // We catch this exception here, as it will be the first occurence of it.
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

                Application.Quit();
                return;
            }

            // Initializes the Steamworks API.
            // If this returns false then this indicates one of the following conditions:
            // [*] The Steam client isn't running. A running Steam client is required to provide implementations of the various Steamworks interfaces.
            // [*] The Steam client couldn't determine the App ID of game. If you're running your application from the executable or debugger directly then you must have a [code-inline]steam_appid.txt[/code-inline] in your game directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the [code-inline]steam_appid.txt[/code-inline] file.
            // [*] Your application is not running under the same OS user context as the Steam client, such as a different user or administration access level.
            // [*] Ensure that you own a license for the App ID on the currently active Steam account. Your game must show up in your Steam library.
            // [*] Your App ID is not completely set up, i.e. in [code-inline]Release State: Unavailable[/code-inline], or it's missing default packages.
            // Valve's documentation for this is located here:
            // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
            m_bInitialized = SteamAPI.Init();
            if (!m_bInitialized)
            {
                Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

                return;
            }

            s_EverInialized = true;
        }

        // This should only ever get called on first load and after an Assembly reload, You should never Disable the Steamworks Manager yourself.
        private void OnEnable()
        {
            if (!m_bInitialized)
            {
                return;
            }

            if (m_SteamAPIWarningMessageHook == null)
            {
                // Set up our callback to recieve warning messages from Steam.
                // You must launch with "-debug_steamapi" in the launch args to recieve warnings.
                m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
            }
        }

        // OnApplicationQuit gets called too early to shutdown the SteamAPI.
        // Because the SteamManager should be persistent and never disabled or destroyed we can shutdown the SteamAPI here.
        // Thus it is not recommended to perform any Steamworks work in other OnDestroy functions as the order of execution can not be garenteed upon Shutdown. Prefer OnDisable().
        private void OnDestroy()
        {
            if (ins != this)
            {
                return;
            }

            ins = null;

            if (!m_bInitialized)
            {
                return;
            }

            Viveport.Api.Shutdown(ShutdownHandler);
            SteamAPI.Shutdown();
        }
        #endregion
    }

}