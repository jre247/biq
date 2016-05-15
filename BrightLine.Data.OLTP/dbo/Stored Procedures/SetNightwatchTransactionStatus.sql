CREATE PROCEDURE [dbo].[SetNightwatchTransactionStatus]
	@TransactionId uniqueidentifier
AS
	update NightwatchTransaction
	set Status = 'Completed'
	where TransactionId = @TransactionId

RETURN 0

