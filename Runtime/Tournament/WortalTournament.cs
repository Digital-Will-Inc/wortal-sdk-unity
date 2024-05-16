using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Tournament API
    /// </summary>
    public class WortalTournament
    {
        private static Action<Tournament> _getCurrentCallback;
        private static Action<Tournament[]> _getAllCallback;
        private static Action _postScoreCallback;
        private static Action<Tournament> _createCallback;
        private static Action _shareCallback;
        private static Action _joinCallback;

#region Public API

        /// <summary>
        /// Fetch the tournament out of the current context the user is playing. This will reject if there is no
        /// tournament linked to the current context. The tournament returned can be either active or expired
        /// (A tournament is expired if its end time is in the past). For each tournament, there is only one unique context
        /// ID linked to it, and that ID doesn't change.
        /// </summary>
        /// <param name="callback">Callback with the current tournament. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.GetCurrent(
        ///     tournament => Debug.Log("Tournament ID: " + tournament.ID),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>PENDING_REQUEST</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>TOURNAMENT_NOT_FOUND</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void GetCurrent(Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            _getCurrentCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentGetCurrentJS(TournamentGetCurrentCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Tournament.GetCurrent()");
            _getCurrentCallback?.Invoke(GetMockTournament());
#endif
        }

        /// <summary>
        /// Returns a list of eligible tournaments that can be surfaced in-game, including tournaments:
        ///
        /// - The player has created
        /// - The player is participating in
        /// - The player's friends (who granted permission) are participating in
        ///
        /// The tournaments returned are active. A tournament is expired if its end time is in the past.
        /// For each tournament, there is only one unique context ID linked to it, and that ID doesn't change.
        /// </summary>
        /// <param name="callback">Callback with an array of active tournaments. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.GetAll(
        ///     tournaments => Debug.Log(tournaments.Length),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NETWORK_FAILURE</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void GetAll(Action<Tournament[]> callback, Action<WortalError> errorCallback)
        {
            _getAllCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentGetAllJS(TournamentGetAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Tournament.GetAll()");
            Tournament[] tournaments = new Tournament[Random.Range(1, 5)];
            for (int i = 0; i < tournaments.Length; i++)
            {
                tournaments[i] = GetMockTournament();
            }
            _getAllCallback?.Invoke(tournaments);
#endif
        }

        /// <summary>
        /// Posts a player's score. This API should only be called within a tournament context at the end of an
        /// activity (example: when the player doesn't have "lives" to continue the game). This API will be rate-limited when
        /// called too frequently. Scores posted using this API should be consistent and comparable across game sessions.
        /// For example, if Player A achieves 200 points in a session, and Player B achieves 320 points in a session, those
        /// two scores should be generated from activities where the scores are fair to be compared and ranked against each other.
        /// </summary>
        /// <param name="score">An integer value representing the player's score at the end of an activity.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.PostScore(200,
        ///     () => Debug.Log("Score posted!"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// <li>TOURNAMENT_NOT_FOUND</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void PostScore(int score, Action callback, Action<WortalError> errorCallback)
        {
            _postScoreCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentPostScoreJS(score, TournamentPostScoreCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.PostScore({score})");
            _postScoreCallback?.Invoke();
#endif
        }

        /// <summary>
        /// Opens the tournament creation dialog if the player is not currently in a tournament session.
        /// </summary>
        /// <param name="payload">Payload that defines the tournament configuration.</param>
        /// <param name="callback">Callback that contains the created tournament. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.Create(createPayload,
        ///     tournament => Debug.Log("Tournament ID: " + tournament.ID),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>DUPLICATE_POST</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void Create(CreateTournamentPayload payload, Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentCreateJS(JsonConvert.SerializeObject(payload), TournamentCreateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Create({payload})");
            _createCallback?.Invoke(new Tournament(payload));
#endif
        }

        /// <summary>
        /// Opens the share tournament dialog if the player is currently in a tournament session.
        /// </summary>
        /// <param name="payload">Specifies share content.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.Share(sharePayload,
        ///     () => Debug.Log("Tournament shared!"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>DUPLICATE_POST</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void Share(ShareTournamentPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _shareCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentShareJS(JsonConvert.SerializeObject(payload), TournamentShareCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Share({payload})");
            _shareCallback?.Invoke();
#endif
        }

        /// <summary>
        /// Request a switch into a specific tournament context. If the player is not a participant of the tournament, or there
        /// are not any connected players participating in the tournament, this will reject. Otherwise, the promise will resolve
        /// when the game has switched into the specified context.
        /// </summary>
        /// <param name="tournamentID">ID of the desired tournament context to switch into.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.
        /// This is completed after the player is switched into the desired tournament context.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Tournament.Join("123456789",
        ///     () => Debug.Log("Switched into tournament!"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>TOURNAMENT_NOT_FOUND</li>
        /// <li>SAME_CONTEXT</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>USER_INPUT</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void Join(string tournamentID, Action callback, Action<WortalError> errorCallback)
        {
            _joinCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentJoinJS(tournamentID, TournamentJoinCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Tournament.Join({tournamentID})");
            _joinCallback?.Invoke();
#endif
        }

#endregion
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern void TournamentGetCurrentJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentGetAllJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentPostScoreJS(int score, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentCreateJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentShareJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentJoinJS(string tournamentID, Action callback, Action<string> errorCallback);

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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
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

#endregion
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
