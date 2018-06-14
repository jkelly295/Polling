

UPDATE PLP_Assemble SET
	Assemble_Ended=getDate(),
	Status=@Status,
	Percent_Complete=100,
	LastStepNote='Finished Assembling'
WHERE PLP_ID=@PLP_ID