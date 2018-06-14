

--Declare @PLP_ID as int
--Set @PLP_ID=10

select 
	PLP_ID,
	Assembler,
	isnull(Assemble_Started,'') as Assemble_Started,
	Assemble_Ended,
	Status,
	Percent_Complete,
	isnull(LastStepNote,'') as LastStepNote
FROM PLP_Assemble
WHERE PLP_ID=@PLP_ID