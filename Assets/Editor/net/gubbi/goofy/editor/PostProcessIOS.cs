#if UNITY_IOS
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Editor.net.gubbi.goofy.editor {
    public class PostProcessIOS {
        
        
        [PostProcessBuild(Int32.MaxValue)]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            // Performs any post build processes that we need done
            if( buildTarget == BuildTarget.iOS )
            {
                // PList modifications
                {
                    // Get plist
                    string plistPath = pathToBuiltProject + "/Info.plist";
                    var plist = new PlistDocument();
                    plist.ReadFromString(File.ReadAllText(plistPath));
     
                    // Get root
                    var rootDict = plist.root;
     
                    // Add export compliance for TestFlight builds

                    var requiredCapArray = rootDict.CreateArray("UIRequiredDeviceCapabilities");
                    requiredCapArray.AddString("armv7" );
                   
                    // Write to file
                    File.WriteAllText( plistPath , plist.WriteToString() );
                }
            }
        }        
        
    }
}
#endif