IF EXIST bin\debug\Bot.dll (bin\debug\mono-assembly-injector.exe -dll bin\debug\Bot.dll -target hearthstone.exe -namespace Bot -class Loader -method Unload)

bin\debug\mono-assembly-injector.exe -dll bin\debug\Bot.dll -target hearthstone.exe -namespace Bot -class Loader -method Load