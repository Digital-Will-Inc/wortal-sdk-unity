using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace DigitalWill.WortalEditor
{
    public class IOSBuildProcessor
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                ProcessiOSBuild(pathToBuiltProject);
            }
        }

        private static void ProcessiOSBuild(string pathToBuiltProject)
        {
#if UNITY_IOS
            string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            string plistPath = pathToBuiltProject + "/Info.plist";

            // Modify Xcode project
            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);

            string target = project.GetUnityMainTargetGuid();
            string frameworkTarget = project.GetUnityFrameworkTargetGuid();

            // Add required frameworks
            project.AddFrameworkToProject(target, "GameKit.framework", false);
            project.AddFrameworkToProject(target, "Foundation.framework", false);
            project.AddFrameworkToProject(target, "UIKit.framework", false);

            // Add Game Center capability
            string entitlementsPath = pathToBuiltProject + "/Unity-iPhone.entitlements";
            var entitlements = new PlistDocument();
            
            if (File.Exists(entitlementsPath))
            {
                entitlements.ReadFromFile(entitlementsPath);
            }
            else
            {
                entitlements.Create();
            }

            // Add Game Center entitlement
            entitlements.root.SetBoolean("com.apple.developer.game-center", true);
            entitlements.WriteToFile(entitlementsPath);

            // Set code signing entitlements
            project.SetBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", "Unity-iPhone.entitlements");

            // Write changes to project
            project.WriteToFile(projectPath);

            // Modify Info.plist
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Add Game Center usage description
            plist.root.SetString("NSGameCenterUsageDescription", 
                "This app uses Game Center to provide leaderboards, achievements, and multiplayer features.");

            // Set minimum iOS version if needed
            plist.root.SetString("MinimumOSVersion", "12.0");

            // Write changes to plist
            plist.WriteToFile(plistPath);

            Debug.Log("Wortal SDK: iOS build post-processing completed - Game Center configured");
#endif
        }
    }
}