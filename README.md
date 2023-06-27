# NativeAndroidUnity
- Distribute [nativeplugin](https://github.com/CSaratakij/AndroidNativePlugins) with [External dependency manager format (EDM)](https://github.com/googlesamples/unity-jar-resolver)

# Getting Started
1) Build android library from submodule
2) Put '.aar' to 'PluginName/Android/Plugings/libs'
3) Convert to EDM package
4) Enable 'Custom Main Gradle Template' in Unity PlayerSetting
5) Force resolve jar package with EDM
