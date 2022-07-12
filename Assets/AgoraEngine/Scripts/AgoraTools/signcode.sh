#!/bin/bash
################################################################################
# Notarize Step #1 - Code signing
#  Required envirnmenta variable "$SIGNATURE"
#  possible command line: 
#     (export SIGNATURE="my apple developer identity (xxxxx)"; ./codesign.sh MyApp.app)
#  To find available signatures, use 
#	security find-identity -v -p codesigning
################################################################################

if [ "$1" == "" ] || [ $# -lt 1 ]; then
   echo "Please enter the app name!"
   exit 1
fi

if [ "" == "$SIGNATURE" ]; then
    echo "You must provide signature for codesign!"
    echo possible command line: 
    echo '  (export SIGNATURE="my apple developer identity (xxxxx)";' $0 $1 ")"
    echo To find available signatures, running:
    echo '  security find-identity -v -p codesigning'
    security find-identity -v -p codesigning
    exit 2
fi

ENTITLEMENT="App.entitlements"
APP="$1"

AGORA_FRAMEWORKS="$APP/Contents/PlugIns/agoraSdkCWrapper.bundle/Contents/Resources"
AGORA_CLIB="$APP/Contents/Plugins/agoraSdkCWrapper.bundle/Contents/MacOS/agoraSdkCWrapper"
PROJ_BIN="$APP/Contents/MacOS"

# with option the executable can't be run before notarization
OPTIONS="-o runtime"

if [ ! -e $ENTITLEMENT ]; then
    echo "$ENTITLEMENT is not found! quit..."
    exit 1
fi

function CodeSign {
    target="$1"
    echo "codesigning $target" 
    codesign $OPTIONS -f -v --timestamp --deep -s "$SIGNATURE" --entitlements $ENTITLEMENT $target
}

#set -x
chmod -R a+xr $APP
#read $b

echo ""
echo "==== frameworks"
for framework in $AGORA_FRAMEWORKS/*; do
    CodeSign $framework
done

echo "==== bin "
for bin in $PROJ_BIN/*; do
    CodeSign "$bin"
done

CodeSign $AGORA_CLIB
CodeSign $APP

# verify
echo ""
echo "Code sign is done. next, verify..."
codesign -v --strict --deep --verbose=2 $APP

# after notarize
# spctl --assess -vv TestMacSign.app
# After this, run the build, it should still runs
#set +x
