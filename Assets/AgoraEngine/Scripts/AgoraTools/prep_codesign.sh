#!/bin/bash
################################################################################
#   Script:  prep_codesign.sh
#
#   Synosis: prep_codesign.sh <App>
#
#   Description
#	Use for Unity Mac build only.  When an Agora Unity Project gets built,
#   the framework library symbolic link structure gets lost.  This script will
#   restore the symlink structure inside the AgoraRTCKit frame work.
#	
#	For a reference to the original framework structure, you may also take
#   a look at the online download zipped version and look inside the bundle.
#	
#   Usage:
#   *	Use for preparation for codesign and distribution that requires Apple 
#	notarization.
#
#   *	Run this from the build directory where your Mac build ("YourApp.app") is.
#	Build the Unity Project and execute this script, for example:
#	    ./prep_codesign.sh YourApp.app
#   *   A entitlement file "App.entitlements" will be created.  Use that for your
#	codesign --entitlements option.
#	
################################################################################

if [ "$1" == "" ] || [ $# -lt 1 ]; then
   echo "Please enter the app location"
   exit 1
fi

echo "--------------------------------------"
echo "start restructure framework links..."
echo "--------------------------------------"
APP="$PWD/$1"
ENTITLEMENT="App.entitlements"
AGORA_FRAMEWORKS="$APP/Contents/PlugIns/agoraSdkCWrapper.bundle/Contents/Frameworks"
AGORA_CLIB="$APP/Contents/Plugins/agoraSdkCWrapper.bundle/Contents/MacOS/agoraSdkCWrapper"

shopt -s extglob


function create_entitlement {
echo "Writing entitlement to $ENTITLEMENT ..."

echo "
<?xml version=\"1.0\" encoding=\"UTF-8\"?>
<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">
<plist version=\"1.0\">
<dict>
	<key>com.apple.security.cs.allow-unsigned-executable-memory</key>
	<true/>
	<key>com.apple.security.device.audio-input</key>
	<true/>
	<key>com.apple.security.device.camera</key>
	<true/>
	<key>com.apple.security.network.client</key>
	<true/>
	<key>com.apple.security.network.server</key>
	<true/>
</dict>
</plist>
" > $ENTITLEMENT
}

function relink {
    # remove everything except versions
    echo "removing duplicate framework files... in $PWD"
    rm -rf !(Versions)
    rm -rf Versions/Current*
    
    cd Versions
    ln -s A Current
    cd ..
    
    for filename in Versions/Current/*; do
        if [ ${filename: -5} != ".meta" ]; then
          echo "linking $filename"
          ln -s $filename .
        fi
    done
}

function signhelp {

echo ""
echo "Make sure you code sign the following items in addition to the App itself:"
echo "        $AGORA_CLIB"
for framework in "$AGORA_FRAMEWORKS/*"; do
    echo "        $framework" 
done
echo "Or use the signcode.sh script to help your code-signing"
}

# remove all meta files
find "$APP" -type f -name "*.meta" -delete

# re-estasbish version symlinks inside the frameworks
for framework in $AGORA_FRAMEWORKS/*; do
    (cd $framework && relink) 2>/dev/null
done

create_entitlement

echo "--------------------------------------"
echo "done."
signhelp 
echo "--------------------------------------"
