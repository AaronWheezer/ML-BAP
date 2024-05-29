﻿// This file was auto-generated by ML.NET Model Builder.
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
public partial class MLModel1
{
    /// <summary>
    /// model input class for MLModel1.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [LoadColumn(0)]
        [ColumnName(@"ItemID")]
        public string ItemID { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"Weight")]
        public float Weight { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"Size")]
        public float Size { get; set; }

        [LoadColumn(3)]
        [ColumnName(@"Fragility")]
        public string Fragility { get; set; }

        [LoadColumn(4)]
        [ColumnName(@"DestinationWarehouse")]
        public string DestinationWarehouse { get; set; }

    }

    #endregion

    /// <summary>
    /// model output class for MLModel1.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName(@"ItemID")]
        public float[] ItemID { get; set; }

        [ColumnName(@"Weight")]
        public float Weight { get; set; }

        [ColumnName(@"Size")]
        public float Size { get; set; }

        [ColumnName(@"Fragility")]
        public float[] Fragility { get; set; }

        [ColumnName(@"DestinationWarehouse")]
        public uint DestinationWarehouse { get; set; }

        [ColumnName(@"Features")]
        public float[] Features { get; set; }

        [ColumnName(@"PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName(@"Score")]
        public float[] Score { get; set; }

    }

    #endregion

    private static string MLNetModelPath = Path.GetFullPath("MLModel1.mlnet");

    public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);


    private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
        return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
    }

    /// <summary>
    /// Use this method to predict scores for all possible labels.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels(ModelInput input)
    {
        var predEngine = PredictEngine.Value;
        var result = predEngine.Predict(input);
        return GetSortedScoresWithLabels(result);
    }

    /// <summary>
    /// Map the unlabeled result score array to the predicted label names.
    /// </summary>
    /// <param name="result">Prediction to get the labeled scores from.</param>
    /// <returns>Ordered list of label and score.</returns>
    /// <exception cref="Exception"></exception>
    public static IOrderedEnumerable<KeyValuePair<string, float>> GetSortedScoresWithLabels(ModelOutput result)
    {
        var unlabeledScores = result.Score;
        var labelNames = GetLabels(result);

        Dictionary<string, float> labledScores = new Dictionary<string, float>();
        for (int i = 0; i < labelNames.Count(); i++)
        {
            // Map the names to the predicted result score array
            var labelName = labelNames.ElementAt(i);
            labledScores.Add(labelName.ToString(), unlabeledScores[i]);
        }

        return labledScores.OrderByDescending(c => c.Value);
    }

    /// <summary>
    /// Get the ordered label names.
    /// </summary>
    /// <param name="result">Predicted result to get the labels from.</param>
    /// <returns>List of labels.</returns>
    /// <exception cref="Exception"></exception>
    private static IEnumerable<string> GetLabels(ModelOutput result)
    {
        var schema = PredictEngine.Value.OutputSchema;

        var labelColumn = schema.GetColumnOrNull("DestinationWarehouse");
        if (labelColumn == null)
        {
            throw new Exception("DestinationWarehouse column not found. Make sure the name searched for matches the name in the schema.");
        }

        // Key values contains an ordered array of the possible labels. This allows us to map the results to the correct label value.
        var keyNames = new VBuffer<ReadOnlyMemory<char>>();
        labelColumn.Value.GetKeyValues(ref keyNames);
        return keyNames.DenseValues().Select(x => x.ToString());
    }

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput input)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input);
    }

}
