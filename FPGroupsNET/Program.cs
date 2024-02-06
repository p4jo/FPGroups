// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
while (true)
{
    
    // read the parameters vertexCount, edgeCount from the command line, retrying if it couln't be converted to int
    int vertexCount = 5;    
    int edgeCount = 12;
    double proportionOfGenerators = 0.5;
    while (true)
    {
        Console.WriteLine("Enter the number of vertices: ");
        string vertexCountString = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter the number of edges: ");
        string edgeCountString = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter the proportion of Generators (i.e. #Generators = max. Degree of graph * this; >= 0.5): ");
        string proportionOfGeneratorsString = Console.ReadLine() ?? string.Empty;
        if (int.TryParse(vertexCountString, out vertexCount) && int.TryParse(edgeCountString, out edgeCount) && double.TryParse(proportionOfGeneratorsString?.Replace('.',','), out proportionOfGenerators))
            break;
        Console.WriteLine("Invalid input. Please enter a valid integer.");
    }


    var (gens, rels) = RandomGroups.RandomPresentation(vertexCount, edgeCount, proportionOfGenerators) ;
    Console.WriteLine("<" + string.Join(", ", gens) + " | " + string.Join(", ", rels) + ">");
}