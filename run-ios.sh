#!/bin/bash

# Configuración
TARGET_FRAMEWORK="net10.0-ios"         # Cambia según tu proyecto
RUNTIME_ID="iossimulator-x64"         # iossimulator-arm64 para M1/M2
SIMULATOR_NAME="iPhone 15"            # Nombre del simulador que quieres usar

# 1. Listar simuladores disponibles
echo "Simuladores disponibles:"
xcrun simctl list devices

# 2. Buscar el UUID del simulador por nombre
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

# 4. Compilar el proyecto
echo "Compilando proyecto para iOS..."
dotnet build samples/Nivaes.App.Cross.Sample.UIKit.iOS -f $TARGET_FRAMEWORK -p:RuntimeIdentifier=$RUNTIME_ID

# 5. Ejecutar la app en el simulador
APP_PATH=$(find ./samples/Nivaes.App.Cross.Sample.UIKit.iOS/bin/Debug -name "*.app" | head -n 1)
if [ -z "$APP_PATH" ]; then
  echo "No se encontró la app compilada."
  exit 1
fi

echo "Instalando app en simulador..."
xcrun simctl install "$SIM_UUID" "$APP_PATH"

echo "Lanzando app..."
BUNDLE_ID=$(defaults read "$APP_PATH/Info" CFBundleIdentifier)
xcrun simctl launch "$SIM_UUID" "$BUNDLE_ID"

echo "¡App ejecutada en $SIMULATOR_NAME!"