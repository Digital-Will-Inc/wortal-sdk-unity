using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalTournament : IWortalTournament
    {
        public bool IsSupported => false;

        public void GetCurrent(Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[iOS Platform] IWortalTournament.GetCurrent() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetCurrent implementation"
            });
        }

        public void GetAll(Action<Tournament[]> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[iOS Platform] IWortalTournament.GetAll() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetAll implementation"
            });
        }

        public void PostScore(int score, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalTournament.PostScore({score}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "PostScore implementation"
            });
        }

        public void Create(CreateTournamentPayload payload, Action<Tournament> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalTournament.Create({payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "Create implementation"
            });
        }

        public void Share(ShareTournamentPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalTournament.Share({payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "Create implementation"
            });
        }

        public void Join(string tournamentID, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalTournament.Join({tournamentID}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "Join implementation"
            });
        }
    }
}