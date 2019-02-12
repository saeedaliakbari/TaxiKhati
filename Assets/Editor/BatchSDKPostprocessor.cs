using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
//using UnityEditor.iOS.Xcode;

public class BatchSDKPostprocessor {
    //[PostProcessBuildAttribute(1)]
    //public static void OnPostprocessBuild(BuildTarget unityTarget, string path) {
    //    if (unityTarget == BuildTarget.iOS) {
    //        string xcodeprojPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

    //        PBXProject project = new PBXProject();
    //        project.ReadFromFile(xcodeprojPath);

    //        string targetGuid = project.TargetGuidByName("Unity-iPhone");

    //        // What we need to add:
    //        // - CoreTelephony.framework
    //        // - Security.framework
    //        // - libsqlite3.dylib (or .tbd ?)
    //        // - "-ObjC" flag to OTHER_LD_FLAGS

    //        project.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
    //        project.AddFrameworkToProject(targetGuid, "Security.framework", false);

    //        // Adding a library that is not a framework needs to be done in two steps
    //        string sqliteGuid = project.AddFile("/usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk);
    //        project.AddFileToBuild(targetGuid, sqliteGuid);

    //        project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");

    //        project.WriteToFile(xcodeprojPath);
    //    }
    //}

}
