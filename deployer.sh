#!/bin/sh
export DOTNET_ROOT=/home/abdullah/.dotnet
export PATH=$PATH:/home/abdullah/.dotnet:/home/abdullah/.dotnet/tools
set -e  # Exit immediately if any command fails
cd AspireProject/AspireProject.AppHost
aspirate build --non-interactive -m ./manifest.json
aspirate generate --non-interactive -m ./manifest.json --skip-build --secret-password "$ASPIRE_PASSWORD" --image-pull-policy IfNotPresent --include-dashboard true
aspirate apply --non-interactive --secret-password "$ASPIRE_PASSWORD" -k docker-desktop
