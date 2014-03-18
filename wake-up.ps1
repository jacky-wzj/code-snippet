# enable local ps execute => Set-ExecutionPolicy RemoteSigned
# Last Booted Time
Get-WmiObject -Class Win32_OperatingSystem –ComputerName localhost | Select-Object -Property CSName,@{n=”Last Booted”;e={[Management.ManagementDateTimeConverter]::ToDateTime($_.LastBootUpTime)}}

# WinAPI wrapper
$script:nativeMethods = @();
function Register-NativeMethod([string]$dll, [string]$methodSignature)
{
    $script:nativeMethods += [PSCustomObject]@{ Dll = $dll; Signature = $methodSignature; }
}
function Add-NativeMethods()
{
    $nativeMethodsCode = $script:nativeMethods | % { "
        [DllImport(`"$($_.Dll)`")]
        public static extern $($_.Signature);
    " }

    Add-Type @"
        using System;
        using System.Runtime.InteropServices;
        public static class SetThreadExecutionStateWrapper {
            $nativeMethodsCode
        }
"@
}
Register-NativeMethod "kernel32.dll" "uint SetThreadExecutionState(uint esFlags)"
Add-NativeMethods


function wakeUpOS() {
    [SetThreadExecutionStateWrapper]::SetThreadExecutionState(0x00000000 -bor 0x00000001 -bor 0x00000002)
}

while(1){
    wakeUpOS
    [System.Threading.Thread]::Sleep(5000)
}