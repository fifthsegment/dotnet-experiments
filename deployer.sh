#!/bin/sh
password=$ENV_KUBERNETES_PASSWORD
cd AspireProject/AspireProject.AppHost
aspirate build --non-interactive -m ./manifest.json
aspirate generate --non-interactive -m ./manifest.json --skip-build --secret-password "$password" --image-pull-policy IfNotPresent --include-dashboard true
aspirate apply --non-interactive --secret-password "$password" -k docker-desktop
