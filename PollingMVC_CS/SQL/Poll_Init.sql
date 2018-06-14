--Declare @PLP_ID as int
--Declare @UserID as varchar(50)

--Set @PLP_ID=10
--Set @UserID='jkelly'


--Init
IF  EXISTS(Select PLP_ID FROM PLP_Assemble WHERE PLP_ID=@PLP_ID)
	BEGIN
		UPDATE PLP_Assemble SET 
		                        Assembler=@UserID,
								Assemble_Started=getdate(),
								Assemble_Ended  = null,
								Status=@Status,
								Percent_Complete=0
	    WHERE PLP_ID=@PLP_ID;
	END
ELSE
	BEGIN
		INSERT INTO PLP_Assemble (PLP_ID,Assembler,Assemble_Started,Status,Percent_Complete)
		                            VALUES
								(@PLP_ID,@UserID,getDate(),@Status,0);

	END