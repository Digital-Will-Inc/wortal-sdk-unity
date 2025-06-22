using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalTournament : IWortalTournament
    {
        private static Action<Tournament> _getCurrentCallback;
        private static Action<Tournament[]> _getAllCallback;
        private static Action _postScoreCallback;
        private static Action<Tournament> _createCallback;
        private static Action _shareCallback;
        private static Action _joinCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public void GetCurrent(Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            _getCurrentCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentGetCurrentJS(TournamentGetCurrentCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Tournament.GetCurrent()");
            TournamentGetCurrentCallback(JsonConvert.SerializeObject(GetMockTournament()));
#endif
        }

        public void GetAll(Action<Tournament[]> callback, Action<WortalError> errorCallback)
        {
            _getAllCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentGetAllJS(TournamentGetAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Tournament.GetAll()");
            Tournament[] tournaments = new Tournament[Random.Range(1, 5)];
            for (int i = 0; i < tournaments.Length; i++)
            {
                tournaments[i] = GetMockTournament();
            }
            TournamentGetAllCallback(JsonConvert.SerializeObject(tournaments));
#endif
        }

        public void PostScore(int score, Action callback, Action<WortalError> errorCallback)
        {
            _postScoreCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentPostScoreJS(score, TournamentPostScoreCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.PostScore({score})");
            TournamentPostScoreCallback();
#endif
        }

        public void Create(CreateTournamentPayload payload, Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            _errorCallback = errorCallback;
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentCreateJS(payloadJson, TournamentCreateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Create({payload})");
            var tournament = new Tournament(payload);
            TournamentCreateCallback(JsonConvert.SerializeObject(tournament));
#endif
        }

        public void Share(ShareTournamentPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _shareCallback = callback;
            _errorCallback = errorCallback;
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentShareJS(payloadJson, TournamentShareCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Share({payload})");
            TournamentShareCallback();
#endif
        }

        public void Join(string tournamentID, Action callback, Action<WortalError> errorCallback)
        {
            _joinCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.TournamentJoinJS(tournamentID, TournamentJoinCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Join({tournamentID})");
            TournamentJoinCallback();
#endif
        }

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void TournamentGetCurrentCallback(string tournament)
        {
            Tournament tournamentObj;

            try
            {
                tournamentObj = JsonConvert.DeserializeObject<Tournament>(tournament);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "TournamentGetCurrentCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getCurrentCallback?.Invoke(tournamentObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void TournamentGetAllCallback(string tournaments)
        {
            Tournament[] tournamentObj;

            try
            {
                tournamentObj = JsonConvert.DeserializeObject<Tournament[]>(tournaments);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "TournamentGetAllCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getAllCallback?.Invoke(tournamentObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void TournamentPostScoreCallback()
        {
            _postScoreCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void TournamentCreateCallback(string tournament)
        {
            Tournament tournamentObj;

            try
            {
                tournamentObj = JsonConvert.DeserializeObject<Tournament>(tournament);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "TournamentCreateCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _createCallback?.Invoke(tournamentObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void TournamentShareCallback()
        {
            _shareCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void TournamentJoinCallback()
        {
            _joinCallback?.Invoke();
        }

        #endregion Callback Methods

        #region Debug Helpers

        private static Tournament GetMockTournament()
        {
            int id = Random.Range(100000, 999999);
            int contextID = Random.Range(100000, 999999);

            DateTime futureTime = DateTime.Now.AddMinutes(Random.Range(5, 120));
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = futureTime - unixEpoch;
            long endTime = (long)timeSpan.TotalSeconds;

            var tournament = new Tournament
            {
                ID = id.ToString(),
                ContextID = contextID.ToString(),
                Title = "Mock Tournament",
                EndTime = endTime,
                Payload = new Dictionary<string, object>
                {
                    { "referral_id", "1234" },
                    { "bonus_coins", "100" },
                },
            };

            return tournament;
        }

        #endregion Debug Helpers
    }
}
