#
using System;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;


List<Vector<double>> FindDistributedVectors(int n) {
    var objective = ObjectiveFunction.Scalar(ComputeObjective);
    var initialGuess = (from i in Enumerable.Range(0, (n-1) * 2) select Random.NextDouble() * 2 - 1.0).ToArray();

    var optimizer = new NelderMeadSimplex(1e-9, 10000);
    var result = optimizer.FindMinimum(objective, initialGuess).MinimizingPoint;
    var resultAsVectors = (
        from i in Enumerable.Range(0, n)
        let v = new Vector<double>(new double[]{ (float)result[i * 2], (float)result[i * 2 + 1], 1 })
        select v.normalized
    ).ToList();
    resultAsVectors.Append(
        new Vector<double>(new double[] { 1, 0, 0 })
    );

    double ComputeObjective(double[] x) {
        // length = 2*(n-1)

        double[] normSquared = (from i in Enumerable.Range(0, n-1) select x[2*i] * x[2*i] + x[2*i+1] * x[2*i+1] + 1).ToArray();

        double sumSquaredDotProducts = 0;
        for (int i = 0; i < n-1; i++) {
            for (int j = 0; j < i; j++) {
                double dotProduct = x[2*i] * x[2*j] + x[2*i+1] * x[2*j+1] + 1;
                sumSquaredDotProducts += dotProduct * dotProduct / (normSquared[i] * normSquared[j]);
            }
            sumSquaredDotProducts += x[2*i] * x[2*i] / normSquared[i];
        }

        return sumSquaredDotProducts;
    }
}

print(FindDistributedVectors(5));