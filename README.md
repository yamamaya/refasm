# refasm
Add your assembly to "Add Reference dialog box" of VisualStudio.

## Usage
refasm /i .NET-Version ProductName [SearchPath]
/i : Add assembly search path.
/u : Remove assembly search path.
.NET-Version : Specify .NET framework version, such as 'v4.5'.
ProductName : Product name to register.
SearchPath : Specify a directory to search assemblies, defualt is current directory.

## Example
refasm /i v4.5 HogeLib "c:\foo\bar\lib"
refasm /u v4.5 HogeLib
