#!/bin/bash
# This script will only be run if Assets/CroquetJS/package-lock.json
# is different from the package's original package-lock.json

# $1 will be the path to the node engine from CroquetSettings.pathToNode
export PATH="$1:$PATH"

# for debugging paths to node, npm, npx
# echo "From runNPM.sh PATH=$PATH"
# which npm
# which npx
# which node

# this expects npm to be available in the CroquetSettings.pathToNode
npm ci
