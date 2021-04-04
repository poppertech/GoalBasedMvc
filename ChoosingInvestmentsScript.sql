SELECT * FROM Network
SELECT * FROM [Node]
SELECT * FROM [Distribution]
SELECT * FROM CashFlow

DELETE FROM Network WHERE Id = 3

SET IDENTITY_INSERT Network ON

	INSERT INTO Network(Id, [Name], [Url]) 
	VALUES(3, 'Goal-Based Investment Comparison', 'goal-based-investment-comparison')

SET IDENTITY_INSERT Network OFF

SET IDENTITY_INSERT [Node] ON 

	INSERT INTO [Node](Id, [Name], NetworkId, ParentId, InitialInvestment, InitialPrice, IsPortfolioComponent)
	VALUES(3, 'S&P 500', 3, NULL, 0.01, 100, 1)

SET IDENTITY_INSERT [Node] OFF

SET IDENTITY_INSERT [Distribution] ON

	INSERT INTO [Distribution](Id, NodeId, [Index], Minimum, Worst, Likely, Best, Maximum)
	SELECT 3, 3, [Index], Minimum, Worst, Likely, Best, Maximum
	FROM [Distribution]
	WHERE Id = 1

SET IDENTITY_INSERT [Distribution] OFF

SET IDENTITY_INSERT CashFlow ON 

INSERT INTO CashFlow(Id, NetworkId, Cost)
SELECT Id + 21, 3, Cost
FROM CashFlow
WHERE NetworkId = 2

SET IDENTITY_INSERT CashFlow OFF