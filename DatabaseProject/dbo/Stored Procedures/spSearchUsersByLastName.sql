CREATE PROCEDURE [dbo].[spSearchUsersByLastName]
	@lastName varchar(50)
AS
BEGIN
	SELECT * FROM [User] WHERE LastName = @lastName
END
