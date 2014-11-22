PLIM_BAT
========



##Deployment requirements:

###OS 

* Microsoft Windows 8/8.1(64x, Pro edition or above)

###RAM

* 4 GB or more

###Environment 

* [Microsoft Visual Studio Express 2013 (or Professional, Ultimate)](http://www.visualstudio.com/fr-fr/downloads#d-express-windows-8)

* [Windows Phone SDK 8.0](http://www.microsoft.com/en-us/download/details.aspx?id=35471)

###Virtualization

* Hyper-V : must be on

* Hardware-assisted virtualization.

How to get started
========	

1. Download the latest [release](https://github.com/Akami7/PLIM_BAT/releases)
2. Unzip the file
3. Open the test.sln file in /PLIM_BAT/test
4. Install the packages: in the menu go to TOOLS > NuGet Package Manager > NuGet Manager Console, and enter these command:
```
  PM>Install-Package Sparrow.Chart.WP8
  PM>Install-Package Sparrow.BulletGraph.WP8
```
5. Connect the device(phone) with the USB cable
6. In the tools bar click on play "Device"
7. Wait for the opening of the emulateur(can take several minutes depending on your configuration)
8. Enjoy !!!
