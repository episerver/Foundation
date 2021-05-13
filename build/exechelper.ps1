function exec
{
    [CmdletBinding()]
    Param([parameter(Mandatory=$true)]$executable,[parameter()]$arguments)

    Invoke-Expression "& `"$executable`" $arguments" -ErrorAction Stop
    if ($LastExitCode -ne 0) {
        throw "$executable $arguments return exit code $LastExitCode"
    }
}