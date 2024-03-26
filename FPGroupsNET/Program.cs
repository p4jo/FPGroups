int vertexCount = 5;    
int edgeCount = 12;
double proportionOfGenerators = 0.5;
while (true)
{
    
    while (true)
    {
        Console.WriteLine($"Enter the number of vertices: [{vertexCount}]");
        string vertexCountString = Console.ReadLine() ?? string.Empty;
        Console.WriteLine($"Enter the number of edges: [{edgeCount}]");
        string edgeCountString = Console.ReadLine() ?? string.Empty;
        Console.WriteLine($"Enter the proportion of Generators (i.e. #Generators = max. Degree of graph * this; >= 0.5): [{proportionOfGenerators}]");
        string proportionOfGeneratorsString = Console.ReadLine() ?? string.Empty;
        if (
            ( string.IsNullOrWhiteSpace(vertexCountString) || int.TryParse(vertexCountString, out vertexCount)   )
            && 
            ( string.IsNullOrWhiteSpace(edgeCountString) || int.TryParse(edgeCountString, out edgeCount) )
            &&
            ( string.IsNullOrWhiteSpace(proportionOfGeneratorsString) || double.TryParse(proportionOfGeneratorsString?.Replace('.',','), out proportionOfGenerators)) 
        )  break;
        Console.WriteLine("Invalid input. Please enter valid numbers.");
    }

    if (edgeCount > 30)
    {
        Console.WriteLine("This might crash the program. We don't recommend more than 30 edges. Continue? y/n");
        if (Console.ReadLine()?.ToLower() != "y") continue;
    }

    var (gens, rels) = RandomGroups.RandomPresentation(vertexCount, edgeCount, proportionOfGenerators) ;
    if (rels.Length > 1000)
    {
        Console.WriteLine($"There are {rels.Length} relators. Reducing them might not work. Continue? y/n");
        if (Console.ReadLine()?.ToLower() != "y") continue;
    }
    Console.WriteLine("Unreduced group:\n<" + string.Join(", ", gens) + " | " + string.Join(", ", rels) + ">");
    var (optGens, optRels, optMap) = await FpGroups.OptimizePresentation(gens, rels);
    Console.WriteLine("Reduced group:\n<" + string.Join(", ", optGens) + " | " + string.Join(", ", optRels) + ">\nwith " + string.Join(", ",
        from g in gens select g + " -> " + optMap[g]
    ));
}