public static class Test{
public async static void Run(){
    System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    Console.WriteLine("[");
    int RETRIES = 10;
    double ratio = 1.0/RETRIES;
    for (var proportionOfGenerators = 0.5; proportionOfGenerators <= 1.0; proportionOfGenerators += 0.15)
    for (var edgeCount = 3; edgeCount <= 12; edgeCount += 2)
    for (var vertexCount = 3; vertexCount <= 6; vertexCount++){
        PresentationStats res = new() { edgeCount=edgeCount, vertexCount = vertexCount, proportionOfGenerators = proportionOfGenerators};

        for (int _ = 0; _ < RETRIES; _++)
        {
            (string[] gens, string[] rels) = RandomGroups.RandomPresentation(vertexCount, edgeCount, proportionOfGenerators) ;
            (string[] optGens, string[] optRels, Dictionary<string, string> optMap) = await FpGroups.OptimizePresentation(gens, rels);
            res.GenCountSum += gens.Length;
            res.RelCountSum += rels.Length;
            res.RelLengthSum += rels.Sum(r => r.Length);
            res.GenCountOptSum += optGens.Length;
            res.RelCountOptSum += optRels.Length;
            res.RelLengthOptSum += optRels.Sum(r => r.Length);
        }
        // Console.WriteLine(res.ToString(ratio));
        Console.WriteLine(res.ToJSONString(ratio) + ",");
    }
    Console.WriteLine("]");
}
}


class PresentationStats{
    public double proportionOfGenerators;
    public int vertexCount;
    public int edgeCount;
    public int RelCountSum = 0;
    public int GenCountSum = 0;
    public int RelLengthSum = 0;
    public int GenCountOptSum = 0;
    public int RelCountOptSum = 0;
    public int RelLengthOptSum = 0;

    // To JSON String
    public string ToJSONString(double ratio) => $"{{\"VertexCount\": {vertexCount} , \"EdgeCount\": {edgeCount} , \"ProportionOfGenerators\": {proportionOfGenerators} , \"RelCount\": {RelCountSum* ratio}, \"GenCount\": {GenCountSum* ratio}, \"RelLength\": {RelLengthSum* ratio}, \"GenCountOpt\": {GenCountOptSum* ratio}, \"RelCountOpt\": {RelCountOptSum* ratio}, \"RelLengthOpt\": {RelLengthOptSum* ratio}}}";
    public string ToString(double ratio) => $"VertexCount: {vertexCount} - EdgeCount: {edgeCount} - ProportionOfGenerators: {proportionOfGenerators} :\n RelCountSum: {RelCountSum* ratio}, GenCountSum: {GenCountSum* ratio}, RelLengthSum: {RelLengthSum* ratio}, GenCountOptSum: {GenCountOptSum* ratio}, RelCountOptSum: {RelCountOptSum* ratio}, RelLengthOptSum: {RelLengthOptSum* ratio}";
}
