%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe bin\Debug\TLJ_LoginService.exe
Net Start TLJ_LoginService
sc config TLJ_LoginService start= auto

pause