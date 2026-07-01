#!/bin/bash

# Configuración
TARGET_FRAMEWORK="net10.0-ios"         # Cambia según tu proyecto
RUNTIME_ID="iossimulator-x64"         # iossimulator-arm64 para M1/M2
SIMULATOR_NAME="iPhone 15"            # Nombre del simulador que quieres usar

# 1. Listar simuladores disponibles
echo "Simuladores disponibles:"
xcrun simctl list devices

# 2. Buscar UUID del simulador por nombre
SIM_UUID=$(xcrun simctl list devices | grep "$SIMULATOR_NAME" | grep -oE "[A-F0-9\-]{36}" | head -n 1)

if [ -z "$SIM_UUID" ]; then
  echo "Simulador '$SIMULATOR_NAME' no encontrado."
  exit 1
fi

echo "Usando simulador $SIMULATOR_NAME ($SIM_UUID)"

# 3. Arrancar simulador si no está iniciado
SIM_STATE=$(xcrun simctl list devices | grep "$SIM_UUID" | grep -oE "(Booted|Shutdown)")
if [ "$SIM_STATE" == "Shutdown" ]; then
  echo "Arrancando simulador..."
  xcrun simctl boot "$SIM_UUID"
  sleep 5
fi

# 4. Ejecutar dotnet watch con Hot Reload
echo "Iniciando dotnet watch con Hot Reload..."
dotnet watch Samples/Nivaes.App.Cross.Sample.UIKit.iOS  -f $TARGET_FRAMEWORK run --property:RuntimeIdentifier=$RUNTIME_ID
