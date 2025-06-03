using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DigitalWill.WortalEditor
{
    [InitializeOnLoad]
    public class DependencyInstaller : EditorWindow
    {
        private static AddRequest addRequest;
        private static ListRequest listRequest;
        private static bool isInstalling = false;
        private static bool hasCheckedDependencies = false;

        // EDM4U package info
        private const string EDM4U_PACKAGE_NAME = "com.google.external-dependency-manager";
        private const string EDM4U_GIT_URL = "https://github.com/googlesamples/unity-jar-resolver.git?path=upm";

        static DependencyInstaller()
        {
            // Auto-check dependencies when Unity starts up
            EditorApplication.delayCall += CheckDependenciesOnStartup;
        }

        private static void CheckDependenciesOnStartup()
        {
            if (!hasCheckedDependencies)
            {
                hasCheckedDependencies = true;
                CheckAndInstallDependencies();
            }
        }

        [MenuItem("Wortal/Install Dependencies", false, 1)]
        public static void InstallDependenciesManual()
        {
            CheckAndInstallDependencies();
        }

        [MenuItem("Wortal/Check Dependencies Status", false, 2)]
        public static void CheckDependenciesStatus()
        {
            Debug.Log("Checking Wortal SDK dependencies...");
            CheckInstalledPackages();
        }

        [MenuItem("Wortal/About", false, 100)]
        public static void ShowAbout()
        {
            EditorUtility.DisplayDialog(
                "Wortal SDK",
                "Wortal SDK for Unity\nVersion: 6.2.2\n\nUnified SDK for WebGL, Android, and iOS game services.\n\n© Digital Will Inc",
                "OK"
            );
        }

        private static void CheckAndInstallDependencies()
        {
            if (isInstalling)
            {
                Debug.Log("Wortal SDK: Dependency installation already in progress...");
                return;
            }

            Debug.Log("Wortal SDK: Checking dependencies...");
            CheckInstalledPackages();
        }

        private static void CheckInstalledPackages()
        {
            listRequest = Client.List();
            EditorApplication.update += CheckListProgress;
        }

        private static void CheckListProgress()
        {
            if (!listRequest.IsCompleted) return;

            EditorApplication.update -= CheckListProgress;

            if (listRequest.Status == StatusCode.Success)
            {
                var packages = listRequest.Result;
                bool hasEDM4U = packages.Any(p => p.name == EDM4U_PACKAGE_NAME);

                if (hasEDM4U)
                {
                    Debug.Log("Wortal SDK: All dependencies are installed! ✓");
                    OnAllDependenciesInstalled();
                }
                else
                {
                    Debug.Log("Wortal SDK: Missing External Dependency Manager. Installing...");
                    InstallEDM4U();
                }
            }
            else
            {
                Debug.LogError($"Wortal SDK: Failed to check package list: {listRequest.Error.message}");
            }

            listRequest = null;
        }

        private static void InstallEDM4U()
        {
            if (isInstalling) return;

            isInstalling = true;
            Debug.Log("Wortal SDK: Installing External Dependency Manager...");

            addRequest = Client.Add(EDM4U_GIT_URL);
            EditorApplication.update += CheckInstallProgress;
        }

        private static void CheckInstallProgress()
        {
            if (!addRequest.IsCompleted) return;

            EditorApplication.update -= CheckInstallProgress;
            isInstalling = false;

            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Wortal SDK: External Dependency Manager installed successfully! ✓");

                // Show success notification
                if (EditorUtility.DisplayDialog(
                    "Wortal SDK Setup Complete",
                    "External Dependency Manager has been installed successfully!\n\nYour Wortal SDK is now ready to use on all platforms (WebGL, Android, iOS).",
                    "OK",
                    "Open Documentation"))
                {
                    // User clicked OK
                }
                else
                {
                    // User wants to see documentation
                    Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity");
                }

                OnAllDependenciesInstalled();
            }
            else
            {
                Debug.LogError($"Wortal SDK: Failed to install External Dependency Manager: {addRequest.Error.message}");

                // Show error dialog with manual installation option
                if (EditorUtility.DisplayDialog(
                    "Wortal SDK Installation Error",
                    "Failed to automatically install dependencies.\n\nWould you like to install manually?",
                    "Open Manual Instructions",
                    "Cancel"))
                {
                    Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity#installation");
                }
            }

            addRequest = null;
        }

        private static void OnAllDependenciesInstalled()
        {
            // Just trigger the dependency processor to copy files
            // Don't try to resolve immediately - let the processor handle it properly
            EditorApplication.delayCall += () =>
            {
                try
                {
                    DependencyProcessor.RefreshDependencies();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Wortal SDK: Could not refresh dependencies: {e.Message}");
                }
            };
        }

        // Menu validation - disable if installation is in progress
        [MenuItem("Wortal/Install Dependencies", true)]
        private static bool ValidateInstallDependencies()
        {
            return !isInstalling;
        }
    }
}