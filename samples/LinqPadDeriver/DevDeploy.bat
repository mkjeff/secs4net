rem
rem  You can simplify development by updating this batch file and then calling it from the 
rem  project's post-build event.
rem
rem  It copies the output .DLL (and .PDB) to LINQPad's drivers folder, so that LINQPad
rem  picks up the drivers immediately (without needing to click 'Add Driver').
rem
rem  The final part of the directory is the name of the assembly plus its public key token in brackets.

xcopy /i/y Secs4Net.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
xcopy /i/y Secs4Net.pdb "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"

xcopy /i/y Secs4Net.Rx.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
xcopy /i/y Secs4Net.Rx.pdb "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"

xcopy /i/y Secs4Net.LinqPadDriver.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
xcopy /i/y Secs4Net.LinqPadDriver.pdb "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"

xcopy /i/y Secs4Net.Json.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
xcopy /i/y Secs4Net.Json.pdb "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"

xcopy /i/y Newtonsoft.Json.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
xcopy /i/y System.Runtime.CompilerServices.Unsafe.dll "C:\Users\mkjef\AppData\Local\LINQPad\Drivers\DataContext\4.6\Secs4Net.LinqPadDriver (no-strong-name)\"
