#!/bin/sh
export DOTNET_ROOT=/home/abdullah/.dotnet
export PATH=$PATH:/home/abdullah/.dotnet:/home/abdullah/.dotnet/tools
set -e  # Exit immediately if any command fails
read -r password < ./temp_pass.txt
rm -rf temp_pass.txt
cd AspireProject/AspireProject.AppHost
echo "READING CREDENTIALS"
echo "BUILD MANIFEST"
aspirate build --non-interactive -m ./manifest.json
echo "GENERATE KUBERNETES FILES"
aspirate generate --non-interactive -m ./manifest.json --skip-build --secret-password "$password" --image-pull-policy IfNotPresent --include-dashboard true
echo "APPLYING CONFIG"
aspirate apply --non-interactive --secret-password "$password" -k docker-desktop
