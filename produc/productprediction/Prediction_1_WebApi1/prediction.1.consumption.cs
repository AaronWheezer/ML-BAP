﻿// This file was auto-generated by ML.NET Model Builder.

using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML.Transforms.TimeSeries;

public partial class Prediction_1
{
    /// <summary>
    /// model input class for Prediction_1.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [LoadColumn(1)]
        [ColumnName(@"Orders")]
        public float Orders { get; set; }

    }

    #endregion

    /// <summary>
    /// model output class for Prediction_1.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName(@"Orders")]
        public float[] Orders { get; set; }

        [ColumnName(@"Orders_LB")]
        public float[] Orders_LB { get; set; }

        [ColumnName(@"Orders_UB")]
        public float[] Orders_UB { get; set; }

    }

    #endregion

    private static string MLNetModelPath = Path.GetFullPath(@"prediction.1.mlnet");

    public static readonly Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput? input = null, int? horizon = null)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input, horizon);
    }

    private static TimeSeriesPredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var schema);
        return mlModel.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
    }
}

