<div align="center">
  <img src="https://github.com/bsautermeister/uwpcore.framework/blob/master/Assets/phonekit.png" alt="PhoneKit Framework"><br>
</div>
-----------------

# PhoneKit Framework for Windows Phone 8 (Silverlight)

The PhoneKit Framework is a development acceleration library for Windows Phone 8 Silverlight based projects. It is a collection of helper classes to simplify the development of mobile apps.

### Overview of all helper classes

The following list gives a short overview and a description about every included helper class, seperated by each module. The framework and all modules are separated in two project, namely `PhoneKit.Framework.Core` and `PhoneKit.Framework`. The reason for this is that a specific subset of classes cannot be used within a background task. For this reason, all backgrond task and shared projects only reference the *Core* project, while the app-project references both projects of the framework.

#### Advertising
- User controls to simplify the use of ad-constrols
- It offers implementations for Microsoft Advertising with AdDuplex fallback, Google DoubleClick, as well as offline advertisment using local or hosted images

#### Audo
- Helper class to play simple sound effects

#### Collections
- Classes that can be used to implement a grouped ListView
- List extions methods, e.g. to shuffle a list

#### Controls
- Reusable controls, such as for the about page, data backup using OneDrive, in-app store, etc.

#### Conversion
- Commonly used converter classes, that can be used in XAML
- Examples: BooleanToVisiblityConverter, BooleanNegationConverter, ToUpperCaseConverter, ...

#### Graphics
- Offers a robust picture implementation, which wraps the `Microsoft.Xna.Framework.Media.Picture` implementation to ensure proper orientation on all devices
- Classes to render images from a given user control. This enables the ability to create an amazing Live Tile experience!

#### InAppPurchase
- Helper class to handle the IAPs, such as product activation, product listing, etc.

#### LockScreen
- Helper class to simplify lock screen functionality

#### MVVM
- Provides a base class for view models
- Offers a tiny delegate command implementation

#### Net
- Helper class to check the internet and WiFi availability
- Helper class to manage long running downloads

#### OS
- Offers OS and device and display information
- Classes that enable device capabilities, such as vibration or gestures

#### Storage
- Helper classes for state storage, that it only persistent during the lifetime of the app including tombstoning
- Helper class to use OneDrive
- Simplifies the read and write access to files
- Offers an easy to use persistent variable implementation for app settings

#### Support
- Fedback and error logging mechanisms
- Helper class that triggers action after a specified number of app launches

#### Tasks
- Helper functions to manage background tasks

#### Theming
- Offers helper classes to change the global app's theme color in code

#### Tile
- Helper classes to pin and update Live Tiles

#### Voice
- Helper class to use recognizer and synthesis functionality
