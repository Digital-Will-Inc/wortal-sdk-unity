using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalExample
{
    /// <summary>
    /// Demo scene that examples how to use the Wortal SDK in your game.
    /// </summary>
    public class WortalExample : MonoBehaviour
    {
#region UI References
        [Header("Log")]
        [SerializeField]
        private Text _log;

        [Header("Ads")]
        [SerializeField]
        private Button _interstitial;
        [SerializeField]
        private Button _rewarded;

        [Header("Analytics")]
        [SerializeField]
        private Button _levelStart;
        [SerializeField]
        private Button _levelEnd;

        [Header("Context")]
        [SerializeField]
        private Button _contextChoose;
        [SerializeField]
        private Button _contextShare;
        [SerializeField]
        private Button _contextUpdate;
        [SerializeField]
        private Button _contextCreate;
        [SerializeField]
        private Button _contextSwitch;

        [Header("Leaderboard")]
        [SerializeField]
        private Button _leaderboardGet;
        [SerializeField]
        private Button _leaderboardAdd;
        [SerializeField]
        private Button _leaderboardPlayer;
        [SerializeField]
        private Button _leaderboardEntries;
        [SerializeField]
        private Button _leaderboardCount;

        [Header("IAP")]
        [SerializeField]
        private Button _iapGet;
        [SerializeField]
        private Button _iapCatalog;
        [SerializeField]
        private Button _iapBuy;
        [SerializeField]
        private Button _iapConsume;

        [Header("Player")]
        [SerializeField]
        private Button _playerId;
        [SerializeField]
        private Button _playerSetData;
        [SerializeField]
        private Button _playerGetData;
        [SerializeField]
        private Button _playerFriends;
        [SerializeField]
        private Button _playerSigned;

        [Header("Session")]
        [SerializeField]
        private Button _sessionEntryData;
        [SerializeField]
        private Button _sessionTraffic;
        [SerializeField]
        private Button _sessionSetData;
        [SerializeField]
        private Button _sessionEntryPoint;

        private string _purchaseToken;
#endregion UI References
#region Initialization
        private void Start()
        {
            _interstitial.onClick.AddListener(OnInterstitial);
            _rewarded.onClick.AddListener(OnRewarded);
            _levelStart.onClick.AddListener(OnLevelStart);
            _levelEnd.onClick.AddListener(OnLevelEnd);
            _contextChoose.onClick.AddListener(OnContextChoose);
            _contextShare.onClick.AddListener(OnContextShare);
            _contextUpdate.onClick.AddListener(OnContextUpdate);
            _contextCreate.onClick.AddListener(OnContextCreate);
            _contextSwitch.onClick.AddListener(OnContextSwitch);
            _leaderboardGet.onClick.AddListener(OnLeaderboardGet);
            _leaderboardAdd.onClick.AddListener(OnLeaderboardAdd);
            _leaderboardPlayer.onClick.AddListener(OnLeaderboardPlayer);
            _leaderboardEntries.onClick.AddListener(OnLeaderboardEntries);
            _leaderboardCount.onClick.AddListener(OnLeaderboardCount);
            _iapGet.onClick.AddListener(OnIAPGet);
            _iapCatalog.onClick.AddListener(OnIAPCatalog);
            _iapBuy.onClick.AddListener(OnIAPPurchase);
            _iapConsume.onClick.AddListener(OnIAPConsume);
            _playerId.onClick.AddListener(OnPlayerId);
            _playerSetData.onClick.AddListener(OnPlayerSetData);
            _playerGetData.onClick.AddListener(OnPlayerGetData);
            _playerFriends.onClick.AddListener(OnPlayerFriends);
            _playerSigned.onClick.AddListener(OnPlayerSigned);
            _sessionTraffic.onClick.AddListener(OnSessionTraffic);
            _sessionEntryData.onClick.AddListener(OnSessionEntryData);
            _sessionEntryPoint.onClick.AddListener(OnSessionEntryPoint);
            _sessionSetData.onClick.AddListener(OnSessionSetData);
        }
#endregion Initialization

#region Ads
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT GAME FLOW IN REWARDED ADS:
        // Some scenarios might require resuming the game in adDismissedCallback/adViewedCallback.
        // For example: A revive & continue rewarded ad placement after the player has lost.
        // The game flow will depend on the outcome of the ad - does the player watch it or not?
        // If they watched, continue the current level in adViewedCallback.
        // If they skipped, end the level in adDismissedCallback.
        // If the game flow is not dependent on whether or not the player watches, such as bonus points or coins,
        // then you should resume the game in afterAdCallback & noFillCallback.
        ////////////////////////////////////////////////////////////////////////

        private void OnInterstitial()
        {
            Wortal.Ads.ShowInterstitial(Placement.Next, "NextLevel",
                () => Log("BeforeAd"),
                () => Log("AfterAd"));
        }

        private void OnRewarded()
        {
            Wortal.Ads.ShowRewarded("ReviveAndContinue",
                () => Log("BeforeAd"),
                () => Log("AfterAd"),
                () => Log("AdDismissed"),
                () => Log("AdViewed"));
        }
#endregion Ads

#region Analytics
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT LEVEL ANALYTICS:
        // When you call LogLevelStart() a timer is started. This is to track how
        // long it takes the player to finish the level. On the next call to
        // LogLevelEnd() the timer is stopped, but the time tracked will only be
        // added to the event if the level param matches the level param passed to
        // LogLevelStart() in the previous call. If not, the timePlayed will be set to 0.
        ////////////////////////////////////////////////////////////////////////

        private void OnLevelStart()
        {
            Wortal.Analytics.LogLevelStart("Demo");
            Log("OnLevelStart");
        }

        private void OnLevelEnd()
        {
            Wortal.Analytics.LogLevelEnd("Demo", "100", true);
            Log("OnLevelEnd");
        }
#endregion Analytics

#region Context
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT CONTEXT:
        // ContextPayload has lots of options for customizing the payload and filtering.
        // Some of these options are only used for specific calls such as Share or Choose.
        // Text and Image are required for the payload to be sent.
        // Image needs to be a Data URL for a base64 encoded image.
        // Text does not need to be localized, but takes a LocalizableContent param to support this.
        // If there is no localization to pass, then just use the Default member and omit Localizations.
        ////////////////////////////////////////////////////////////////////////

        private void OnContextChoose()
        {
            var payload = new ContextPayload
            {
                Image = WortalExampleUtils.GetImage(),
                Text = new LocalizableContent
                {
                    Default = "Invite",
                },
                Caption = new LocalizableContent
                {
                    Default = "Play!",
                    Localizations = new Dictionary<string, string>
                    {
                        { "en_US", "English" },
                        { "ja_JP", "Japanese" },
                    },
                },
                Data = new Dictionary<string, object>
                {
                    { "current_level", 1 },
                },
            };
            Wortal.Context.Choose(payload,
                () => Log("New context: " + Wortal.Context.GetID()),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnContextShare()
        {
            var payload = new ContextPayload
            {
                Image = WortalExampleUtils.GetImage(),
                Text = new LocalizableContent
                {
                    Default = "Share",
                },
                Caption = new LocalizableContent
                {
                    Default = "Let's Play!",
                },
            };
            Wortal.Context.Share(payload,
                result => Log("Shares: " + result),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnContextUpdate()
        {
            var payload = new ContextPayload
            {
                Image = WortalExampleUtils.GetImage(),
                Text = new LocalizableContent
                {
                    Default = "Update",
                },
                Caption = new LocalizableContent
                {
                    Default = "We Played!",
                },
            };
            Wortal.Context.Update(payload,
                () => Log("New context: " + Wortal.Context.GetID()),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnContextCreate()
        {
            Wortal.Context.Create("somePlayerId",
                () => Log("New context: " + Wortal.Context.GetID()),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnContextSwitch()
        {
            Wortal.Context.Switch("someContextId",
                () => Log("New context: " + Wortal.Context.GetID()),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }
#endregion Context

#region Leaderboard
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT LEADERBOARD:
        // Leaderboards need to be created on a per-platform basis before they can be used.
        // Contact your support representative to have these created before accessing the API.
        ////////////////////////////////////////////////////////////////////////

        private void OnLeaderboardGet()
        {
            Wortal.Leaderboard.GetLeaderboard("global",
                board => Log("Leaderboard: " + board.Name),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnLeaderboardAdd()
        {
            Wortal.Leaderboard.SendEntry("global", 100,
                entry => Log("Score: " + entry.Score),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnLeaderboardPlayer()
        {
            Wortal.Leaderboard.GetPlayerEntry("global",
                entry => Log("Score: " + entry.Score),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnLeaderboardEntries()
        {
            Wortal.Leaderboard.GetEntries("global", 10,
                entries => Log("Score: " + entries[0].Score),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnLeaderboardCount()
        {
            Wortal.Leaderboard.GetEntryCount("global",
                count => Log("Count: " + count),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }
#endregion Leaderboard

#region IAP
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT IN-APP PURCHASING:
        // IAP needs to be setup on a per-platform basis before they can be used.
        // Contact your support representative to have these created before accessing the API.
        ////////////////////////////////////////////////////////////////////////

        private void OnIAPGet()
        {
            Wortal.IAP.GetPurchases(
                purchases =>
                {
                    _purchaseToken = purchases[0].PurchaseToken;
                    Log(purchases[0].ProductID);
                },
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnIAPCatalog()
        {
            Wortal.IAP.GetCatalog(
                catalog => Log(catalog[0].Title),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnIAPPurchase()
        {
            Wortal.IAP.MakePurchase(new WortalIAP.PurchaseConfig
                {
                    ProductID = "jp.rgames.wortalsdktest.purchase.llama_pack_1.test",
                },
                purchase =>
                {
                    _purchaseToken = purchase.PurchaseToken;
                    Log(purchase.PurchaseToken);
                },
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnIAPConsume()
        {
            Wortal.IAP.ConsumePurchase(_purchaseToken,
                () => Log("Purchase Consumed"),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }
#endregion IAP

#region Player
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT PLAYER DATA:
        // Player data saved to the platform can be of any type with any values.
        // The maximum size is 1MB per player.
        // Data can be a complex JS object, with nested objects of various types. Due to this
        // the serialization/de-serialization method is not as simple as we would like.
        //
        // For Player.SetData: You should create a Dictionary<string, object> and add your data to that.
        // You can nest additional collections inside this root dictionary for more complex data models.
        //
        // For Player.GetData: The data returned from the JS SDK will be de-serialized into IDictionary<string, object>.
        // You can then use GetType on the values of the dictionary to see what type(s) the data has.
        ////////////////////////////////////////////////////////////////////////

        private void OnPlayerId()
        {
            Log(Wortal.Player.GetID());
        }

        private void OnPlayerSetData()
        {
            Dictionary<string, object> data = new()
            {
                {
                    "items", new Dictionary<string, int>
                    {
                        { "coins", 100 },
                        { "boosters", 2 },
                    }
                },
                { "lives", 3 },
            };
            Wortal.Player.SetData(data,
                () => Log("Data set"),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnPlayerGetData()
        {
            Wortal.Player.GetData(new[] { "items", "lives" },
                data =>
                {
                    foreach (KeyValuePair<string, object> kvp in data)
                    {
                        Debug.Log("Key name: " + kvp.Key);
                        Debug.Log("Value type: " + kvp.Value.GetType());
                    }
                    var items = (Dictionary<string, object>)data["items"];
                    Log("Coins: " + items["coins"]);
                },
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnPlayerFriends()
        {
            var payload = new WortalPlayer.PlayerPayload
            {
                Filter = WortalPlayer.PlayerFilter.ALL,
            };
            Wortal.Player.GetConnectedPlayers(payload,
                players => Log(players[0].GetName()),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }

        private void OnPlayerSigned()
        {
            Wortal.Player.GetSignedPlayerInfo(
                (id, signature) => Log("ID: " + id + "\nSignature: " + signature),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }
#endregion Player

#region Session
        ////////////////////////////////////////////////////////////////////////
        // NOTE ABOUT SESSION DATA:
        // Data saved using SetSessionData does not persist outside the game session.
        // It is available for the platform's webhooks, which need to be setup in advance.
        // Contact your support representative to have these created before accessing the API.
        ////////////////////////////////////////////////////////////////////////

        private void OnSessionTraffic()
        {
            WortalSession.TrafficSource trafficSource = Wortal.Session.GetTrafficSource();
            Log(trafficSource.EntryPoint + "\n" + trafficSource.UTMSource);
        }

        private void OnSessionSetData()
        {
            Dictionary<string, object> data = new()
            {
                { "referrerId", "friend1" },
            };
            Wortal.Session.SetSessionData(data);
        }

        private void OnSessionEntryData()
        {
            IDictionary<string, object> data = Wortal.Session.GetEntryPointData();
            foreach (KeyValuePair<string, object> kvp in data)
            {
                Debug.Log(kvp.Key);
                Debug.Log(kvp.Value);
            }
        }

        private void OnSessionEntryPoint()
        {
            Wortal.Session.GetEntryPoint(
                entryPoint => Log(entryPoint),
                error => Log("Error Code: " + error.Code + "\nError: " + error.Message));
        }
#endregion Session

        private void Log(string message)
        {
            _log.text = message;
            Debug.Log(message);
        }
    }
}
