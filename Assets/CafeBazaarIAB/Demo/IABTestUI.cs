using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using BazaarPlugin;

public class IABTestUI : MonoBehaviour
{
#if UNITY_ANDROID

    // Enter all the available skus from the CafeBazaar Developer Portal in this array so that item information can be fetched for them
    string[] skus = { "test100"
                , "consume1"};

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, Screen.width - 15f, Screen.height - 15f));
        GUI.skin.button.fixedHeight = 50;
        GUI.skin.button.fontSize = 20;

        if (Button("Initialize IAB"))
        {
            ObscuredString key = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwC/9PLsRWemh41DzeBh+X1yfm5jGymrJaN9MBwExLaUSMoE6VHmLd9+h4xWyUl7ghLHj1OD3Cw7WHwJBL7/n4/oIaHxZ7OHko7Q7d/wVY2LMmADHV7Ekj0QAwJWblT9bxA7gtvH+Mt+V9U6R9fSxRYR1FF076Q7bWdmbyhzLPaQDkLPv2JE5NTnuBUHtCydkjK3BI4Kb30JTs9nA1vjbQ6vMzPbxOyRmdtd9wtVWGECAwEAAQ==";
            //Debug.Log("key : " + key);
            BazaarIAB.init(key);
        }

        if (Button("Query Inventory"))
        {
            BazaarIAB.queryInventory(skus);
        }

        if (Button("Query SkuDetails"))
        {
            BazaarIAB.querySkuDetails(skus);
        }

        if (Button("Query Purchases"))
        {
            BazaarIAB.queryPurchases();
        }

        if (Button("Are subscriptions supported?"))
        {
            //Debug.Log("subscriptions supported: " + BazaarIAB.areSubscriptionsSupported());
        }

        if (Button("Purchase Product Test1"))
        {
            BazaarIAB.purchaseProduct("test100");
        }

        if (Button("Purchase Product Test2"))
        {
            BazaarIAB.purchaseProduct("consume1");
        }

        if (Button("Consume Purchase Test1"))
        {
            BazaarIAB.consumeProduct("consume1");
        }

        if (Button("Consume Purchase Test2"))
        {
            BazaarIAB.consumeProduct("com.fanafzar.bazaarplugin.test2");
        }

        if (Button("Consume Multiple Purchases"))
        {
            var skus = new string[] { "com.fanafzar.bazaarplugin.test1", "com.fanafzar.bazaarplugin.test2" };
            BazaarIAB.consumeProducts(skus);
        }

        if (Button("Test Unavailable Item"))
        {
            BazaarIAB.purchaseProduct("com.fanafzar.bazaarplugin.unavailable");
        }

        if (Button("Purchase Monthly Subscription"))
        {
            BazaarIAB.purchaseProduct("consume1", "subscription payload");
        }

        if (Button("Purchase Annually Subscription"))
        {
            BazaarIAB.purchaseProduct("com.fanafzar.bazaarplugin.annually_subscribtion_test", "subscription payload");
        }

        if (Button("Enable High Details Logs"))
        {
            BazaarIAB.enableLogging(true);
        }

        GUILayout.EndArea();
    }

    bool Button(string label)
    {
        GUILayout.Space(5);
        return GUILayout.Button(label);
    }

#endif

}

