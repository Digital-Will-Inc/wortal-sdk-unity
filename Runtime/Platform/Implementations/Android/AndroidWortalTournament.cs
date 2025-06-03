using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalTournament : IWortalTournament
    {
        public bool IsSupported => false;

        public void GetCurrent(Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[Android Platform] IWortalTournament.GetCurrent() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetCurrent implementation"
            });
        }

        public void GetAll(Action<Tournament[]> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[Android Platform] IWortalTournament.GetAll() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetAll implementation"
            });
        }

        public void PostScore(int score, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalTournament.PostScore({score}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "PostScore implementation"
            });
        }

        public void Create(CreateTournamentPayload payload, Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalTournament.Create({payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Create implementation"
            });
        }

        public void Share(ShareTournamentPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalTournament.Share({payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Create implementation"
            });
        }

        public void Join(string tournamentID, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalTournament.Join({tournamentID}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Join implementation"
            });
        }
    }
}