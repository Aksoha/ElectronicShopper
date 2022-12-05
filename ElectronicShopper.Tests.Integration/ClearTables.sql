Set nocount on

Exec sp_MSforeachtable 'Alter Table ? NoCheck Constraint All'

Exec sp_MSforeachtable
     '
     If ObjectProperty(Object_ID(''?''), ''TableHasForeignRef'')=1
     Begin
     Delete From ?
     End
     Else
     Begin
     Truncate Table ?
     End
     '

EXEC sp_MSforeachtable 'DBCC CHECKIDENT(''?'', RESEED, 0)'

Exec sp_MSforeachtable 'Alter Table ? Check Constraint All'