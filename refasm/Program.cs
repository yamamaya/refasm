using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace refasm {
    class Program {
        private enum RunMode {
            NA,
            Install,
            Uninstall
        }

        static void Main( string[] args ) {
            bool ArgError = true;
            RunMode mode = RunMode.NA;
            string NETVersion = null;
            string ProductName = null;
            string SearchPath = null;
            if ( args.Length >= 1 ) {
                string s = args[ 0 ].ToLower();
                if ( s == "/i" ) {
                    if ( args.Length >= 3 ) {
                        mode = RunMode.Install;
                        NETVersion = args[ 1 ];
                        ProductName = args[ 2 ];
                        if ( args.Length == 4 ) {
                            SearchPath = args[ 3 ];
                        } else {
                            SearchPath = System.IO.Directory.GetCurrentDirectory();
                        }
                        ArgError = false;
                    }
                } else if ( s == "/u" ) {
                    if ( args.Length == 3 ) {
                        mode = RunMode.Uninstall;
                        NETVersion = args[ 1 ];
                        ProductName = args[ 2 ];
                        ArgError = false;
                    }
                }
            }
            if ( NETVersion != null && !Regex.IsMatch( NETVersion, @"^v\d+(\.\d+)*$" ) ) {
                ArgError = true;
            }
            if ( SearchPath != null && !Regex.IsMatch( SearchPath, @"\\$" ) ) {
                SearchPath += @"\";
            }
            if ( ProductName != null && ( !Regex.IsMatch( ProductName, @"^\w+" ) || Regex.IsMatch( ProductName, @"\\" ) ) ) {
                ArgError = true;
            }
            if ( ArgError ) {
                Console.WriteLine( "Usage:" );
                Console.WriteLine( " refasm /i .NET-Version ProductName [SearchPath]" );
                Console.WriteLine( " refasm /u .NET-Version ProductName" );
                Console.WriteLine( " /i : Add assembly search path." );
                Console.WriteLine( " /u : Remove assembly search path." );
                Console.WriteLine( " .NET-Version : Specify .NET framework version, such as 'v4.5'." );
                Console.WriteLine( " ProductName : Product name to register." );
                Console.WriteLine( " SearchPath : Specify a directory to search assemblies, defualt is current directory." );
                return;
            }

            try {
                string regpath = string.Format( @"SOFTWARE\Microsoft\.NETFramework\{0}\AssemblyFoldersEx\{1}", NETVersion, ProductName );
                if ( mode == RunMode.Install ) {
                    using ( RegistryKey regkey = Registry.LocalMachine.CreateSubKey( regpath ) ) {
                        regkey.SetValue( null, SearchPath );
                    }
                } else if ( mode == RunMode.Uninstall ) {
                    Registry.LocalMachine.DeleteSubKeyTree( regpath );
                }
            } catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }
        }
    }
}
