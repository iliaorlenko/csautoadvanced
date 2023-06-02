using Xml;

var config = new XmlRepository().GetConfig();

//// Write config to DB using code-first approach
new SqlRepository_CodeFirst.SqlRepository().WriteConfig(config);

//// Get Config .net object using EF after DB scaffolding
config = new SqlRepository_DbFirst.SqlRepository().GetConfig();

//// Get Config .net object using ADO.NET 
config = new SqlRepository_AdoNet.SqlRepository().GetConfig();

//// Write Config .net object using ADO.NET 
new SqlRepository_AdoNet.SqlRepository().WriteConfig(config);

Console.ReadKey();
