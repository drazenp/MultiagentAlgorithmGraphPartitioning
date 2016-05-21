	/* Initialize graph file paths.*/
    create table _GraphFilePaths(GraphFilePath nvarchar(150), NumberOfPartitions integer, ColoringProbability float, MovingProbability float, NumberOfVerticesForBalance integer, TimesToRun integer);
    insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, ColoringProbability, MovingProbability, NumberOfVerticesForBalance, TimesToRun) values ('myciel3.col', 2, 0.9, 0.85, 6, 20);
	insert into  _GraphFilePaths (GraphFilePath, NumberOfPartitions, ColoringProbability, MovingProbability, NumberOfVerticesForBalance, TimesToRun) values ('myciel4.col', 2, 0.9, 0.85, 12, 20);

	/* Initialize number of ants.*/
	create table _NumberOfAnts(NumberOfAnts integer);
	insert into _NumberOfAnts(NumberOfAnts)values(1);
	insert into _NumberOfAnts(NumberOfAnts)values(2);
	insert into _NumberOfAnts(NumberOfAnts)values(3);
	insert into _NumberOfAnts(NumberOfAnts)values(4);
	insert into _NumberOfAnts(NumberOfAnts)values(5);
	insert into _NumberOfAnts(NumberOfAnts)values(6);
	insert into _NumberOfAnts(NumberOfAnts)values(7);
	insert into _NumberOfAnts(NumberOfAnts)values(8);

	create table _NumberOfIterations(NumberOfIterations integer);
	insert into _NumberOfIterations(NumberOfIterations)values(100);
	insert into _NumberOfIterations(NumberOfIterations)values(500);
	insert into _NumberOfIterations(NumberOfIterations)values(1000);
	insert into _NumberOfIterations(NumberOfIterations)values(3000);
	insert into _NumberOfIterations(NumberOfIterations)values(5000);

	select *
	from _GraphFilePaths, _NumberOfAnts, _NumberOfIterations
	
	delete from AnalyzeData;
	delete from AnalyzeResults;
	
	/*GraphFilePath, NumberOfPartitions, ColoringProbability, MovingProbability, NumberOfVerticesForBalance, TimesToRun*/
	insert into AnalyzeData(GraphFilePath, NumberOfAnts, NumberOfPartitions, ColoringProbability, MovingProbability, NumberOfVerticesForBalance, NumberOfIterations, TimesToRun)
	select g.GraphFilePath, a.NumberOfAnts, g.NumberOfPartitions, g.ColoringProbability, g.MovingProbability, g.NumberOfVerticesForBalance, i.NumberOfIterations, g.TimesToRun
	from _GraphFilePaths as g, _NumberOfAnts as a, _NumberOfIterations as i;

    drop table _GraphFilePaths;
	drop table _NumberOfAnts;
	drop table _NumberOfIterations;