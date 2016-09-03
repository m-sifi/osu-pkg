# osu! package manager
A simple tool that allows you to backup your beatmaplist into a file and restore them.

## Warning: This tool assumes that the install folder for osu! is at ``` /Appdata/Local/osu!/```, the program will prompt you to enter in your custom Osu! install folder when running for the first time

You can change the install location anytime in ```osu.conf``` by editing it with InstallLocation=[OsuInstallPath]

![List Beatmaps](/screenshots/query.gif)
![Download Beatmaps](/screenshots/download.gif)

### Available commands
- ``` -h ``` or ```--help``` 
- ``` -q ``` or ```--query```
- ``` -d ``` or ```--download```

### Commands Usage
- ``` -h ``` or ```--help``` 
    - Displays available commands
- ``` -q ``` or ```--query```
    - Lists all beatmaps installed
- ``` -d ``` or ```--download```
    - Downloads the beatmap using the list of beatmaps provided

### Note that downloading beatmaps requires you to be logged into osu! on your main browser
