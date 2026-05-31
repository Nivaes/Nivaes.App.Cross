# Depurar app iOS

En el mac

Mirar los valores de

```xml
<ApplicationTitle>NivaesCross</ApplicationTitle>
<ApplicationId>com.nivaes.cross.ios.sample</ApplicationId>
```

Ejemplos

```shell
log stream --predicate 'process contains "Nivaes"'
```

```shell
log stream --process NivaesCross
```
