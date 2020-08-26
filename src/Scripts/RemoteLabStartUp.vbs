'RemoteLab Startup script - this script records the startup event for RemoteLab

on error resume next
dim con, sql , Network, networkAddress, oShell

set con = createobject("adodb.connection")
set Network = createobject("wscript.network")
Set oShell = CreateObject("WScript.Shell")

networkAddress = Replace(oShell.Exec("powershell -Command ""[System.Net.Dns]::GetHostEntry($(hostname)).Hostname""").StdOut.ReadAll, vbNewLine, "")
sql = "exec dbo.P_remotelabdb_startup '" & ucase(network.ComputerName) & "', '" & poolName & "', '" & networkAddress & "'"
con.connectionstring = connstr

con.open
con.execute sql
con.Close 

set con = nothing
set network = nothing