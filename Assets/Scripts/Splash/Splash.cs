using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using CodeStage.AntiCheat.ObscuredTypes;

public class Splash : MonoBehaviour
{
    public GoToScene goToSecne;

    void Start()
    {
        ObscuredPrefs.SetInt("enterFromSplash", 1);
#if UNITY_ANDROID && !UNITY_EDITOR
                GameAnalytics.Initialize();
#endif
        if (ObscuredPrefs.GetBool("set_x", false))
        {
            Debug.Log("Set _x timer");
            ObscuredPrefs.SetBool("set_x", true);
            ObscuredPrefs.SetDouble("speed_2x_for_150s_time", ObscuredPrefs.GetDouble("2x_speed_for_150s_time"));
            ObscuredPrefs.DeleteKey("2x_speed_for_150s_time");
            ObscuredPrefs.SetDouble("earning_5x_for_1m_time", ObscuredPrefs.GetDouble("5x_earning_for_1m_time"));
            ObscuredPrefs.DeleteKey("5x_earning_for_1m_time");
            ObscuredPrefs.SetDouble("earning_5x_for_1m_special_time", ObscuredPrefs.GetDouble("5x_earning_for_1m_special_time"));
            ObscuredPrefs.DeleteKey("5x_earning_for_1m_special_time");
        }
        if (ObscuredPrefs.HasKey("maxXp" + ObscuredPrefs.GetInt("Level", 1)))
        {
            Debug.Log("Set Max Xp");
            ObscuredPrefs.SetInt("maxXp", ObscuredPrefs.GetInt("maxXp" + ObscuredPrefs.GetInt("Level", 1), 118000));
            ObscuredPrefs.DeleteKey("maxXp" + ObscuredPrefs.GetInt("Level", 1));
        }
        goToSecne.go("Main");
    }

}