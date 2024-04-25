#!/bin/sh
export DOTNET_ROOT=/home/abdullah/.dotnet
export PATH=$PATH:/home/abdullah/.dotnet:/home/abdullah/.dotnet/tools
set -e  # Exit immediately if any command fails
cd AspireProject/AspireProject.AppHost
echo "BUILD MANIFEST"
aspirate build --non-interactive -m ./manifest.json
echo "GENERATE KUBERNETES FILES"
aspirate generate --non-interactive -m ./manifest.json --skip-build --secret-password "$ASPIRE_PASSWORD" --image-pull-policy IfNotPresent --include-dashboard true
echo "APPLYING CONFIG"
aspirate apply --non-interactive --secret-password "$ASPIRE_PASSWORD" -k docker-desktop
