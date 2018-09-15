set xact_abort on

BEGIN TRAN

exec sp_addmessage @msgnum = 50001,  
				   @severity = 11,  
				   @msgtext =  N'%s',
				   @replace='REPLACE';  
GO  

COMMIT TRAN