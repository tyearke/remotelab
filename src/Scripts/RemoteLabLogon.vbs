' Remote Lab Logon Script

on error resume next
dim con, sql , Network, oShell, username, dbusername

set con = createobject("adodb.connection")
set rs = createobject("adodb.recordset")
set Network = createobject("wscript.network")
Set oShell = CreateObject("WScript.Shell") 

Function ValueInIterable(value, iterable)
	For Each entry In iterable
		If entry = value Then
			ValueInIterable = True
			Exit Function
		End If
	Next
	ValueInIterable = False
End Function

sql = "select username from Computers where computername = '" & ucase(network.ComputerName) & "'"
username = lcase(network.username) 
con.connectionstring = connstr
con.Open
Set rs = con.Execute(sql)
dbusername = lcase(rs(0))
If IsNull(dbusername) Then dbusername = ""

' Check if user is in group that may bypass logon check
'
' Source: https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-powershell-1.0/ff730963(v=technet.10)
userInExceptionGroup = False
Set cnRegex = New RegExp
cnRegex.Pattern = "(^|.*,)CN=([^,]*).*"
For Each groupDN In GetObject("LDAP://" & CreateObject("ADSystemInfo").UserName).memberOf
	groupCN = cnRegex.Replace(groupDN, "$2")
	If ValueInIterable(groupCN, exceptionGroups) Then
		userInExceptionGroup = True
		Exit For
	End If
Next

If (username <> remotepowershelluser) And (Not userInExceptionGroup) Then
	If Not (dbusername = username) Then 
		oShell.Popup "You must use the web interface to access Remote Lab.", 5, "RemoteLab Error", 80000
		oShell.Run "%comspec% /c shutdown /l", , True
	Else
		sql2 = "exec dbo.P_remotelabdb_logon '" & ucase(network.ComputerName) & "','" & lcase(username) & "'"
		set rs = con.Execute(sql2) 
	End If	
End If

con.Close 
Set con = nothing
set network = Nothing



