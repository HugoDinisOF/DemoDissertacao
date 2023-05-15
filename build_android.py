import time
import os
import shutil
import subprocess
#import command
import sys

def remove():
    if not os.path.exists("./TempBuildFiles"):
        os.mkdir("./TempBuildFiles")
    
    try:
        shutil.move("./Packages/com.unity.xr.core-utils~", "./TempBuildFiles/com.unity.xr.core-utils~")
        shutil.move("./Packages/com.unity.xr.interaction.toolkit~", "./TempBuildFiles/com.unity.xr.interaction.toolkit~")
        shutil.move("./Packages/com.unity.xr.legacyinputhelpers~", "./TempBuildFiles/com.unity.xr.legacyinputhelpers~")
        shutil.move("./Packages/com.unity.xr.management~", "./TempBuildFiles/com.unity.xr.management~")
        shutil.move("./Packages/com.unity.xr.oculus~", "./TempBuildFiles/com.unity.xr.oculus~")
        shutil.move("./Packages/com.unity.xr.openxr~", "./TempBuildFiles/com.unity.xr.openxr~")

        shutil.move("./Assets/Samples/XR Interaction Toolkit", "./TempBuildFiles/XR Interaction Toolkit")
    except:
        pass

def bringback():
    #os.remove("./Packages/com.unity.xr.core-utils~")
    #os.remove("./Packages/com.unity.xr.interaction.toolkit~")
    #os.remove("./Packages/com.unity.xr.legacyinputhelpers~")
    #os.remove("./Packages/com.unity.xr.management~")
    #os.remove("./Packages/com.unity.xr.oculus~")
    #os.remove("./Packages/com.unity.xr.openxr~")

    #os.remove("./Assets/Samples/XR Interaction Toolkit")

    def debugger(text):     
        print(text)
    
    #res = command.run(["'C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.6f1\\Editor\\Unity.exe' -projectPath . -quit -batchmode -nographics -buildTarget Android"], debug=debugger) 

    #print(res.output) 
    #print(res.exit)
    # This is our shell command, executed by Popen.
    #print(f"C:\\'Program Files'\\Unity\\Hub\\Editor\\2021.3.6f1\\Editor\\Unity.exe -projectPath . -quit -batchmode -nographics -buildTarget Android")
    #p = subprocess.Popen(f"C:\\'Program Files'\\Unity\\Hub\\Editor\\2021.3.6f1\\Editor\\Unity.exe -projectPath . -quit -batchmode -nographics -buildTarget Android -executeMethod BuildAndroidScript.MyBuild",
    #    stdout=subprocess.PIPE, shell=True,
    #    executable="C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe")

    #print(p.communicate())
    try:
        shutil.move("./TempBuildFiles/com.unity.xr.core-utils~", "./Packages/com.unity.xr.core-utils~")
        shutil.move("./TempBuildFiles/com.unity.xr.interaction.toolkit~", "./Packages/com.unity.xr.interaction.toolkit~")
        shutil.move("./TempBuildFiles/com.unity.xr.legacyinputhelpers~", "./Packages/com.unity.xr.legacyinputhelpers~")
        shutil.move("./TempBuildFiles/com.unity.xr.management~", "./Packages/com.unity.xr.management~")
        shutil.move("./TempBuildFiles/com.unity.xr.openxr~", "./Packages/com.unity.xr.openxr~")
        shutil.move("./TempBuildFiles/com.unity.xr.oculus~", "./Packages/com.unity.xr.oculus~")
    except:
        pass
    shutil.move("./TempBuildFiles/XR Interaction Toolkit", "./Assets/Samples")


arg = sys.argv[1]

if arg == "-r":
    remove()
    print("removed")
elif arg == "-b":
    bringback()
    print("brought back")
else:
    print("No valid argument given")
