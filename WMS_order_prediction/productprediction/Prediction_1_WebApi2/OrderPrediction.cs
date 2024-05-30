using Microsoft.ML.Data;
using Microsoft.ML;
using System;
using System.IO;

namespace Prediction_1_WebApi2
{
    public class OrderPrediction
    {
        class Program
        {
            static void Main(string[] args)
            {
                // Set up the ML.NET environment
                var mlContext = new MLContext();

                // Load the data
                var data = mlContext.Data.LoadFromTextFile<OrderData>("orders.csv", separatorChar: ',');

                // Define the training pipeline
                var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                    .Append(mlContext.Transforms.Concatenate("Features", "Date"))
                    .Append(mlContext.Regression.Trainers.FastTree());

                // Train the model
                var model = pipeline.Fit(data);

                // Make a prediction for a specific date
                var predictionEngine = mlContext.Model.CreatePredictionEngine<OrderData, OrderPredictionTest>(model);

                // Replace "2024-05-17" with the date you want to predict for
                var dateToPredict = new DateTime(2024, 5, 17);
                var prediction = predictionEngine.Predict(new OrderData { Date = dateToPredict });

                Console.WriteLine($"Predicted number of orders for {dateToPredict.ToShortDateString()}: {prediction.Orders}");

                Console.ReadLine();
            }
        }

        // Define the data classes
        public class OrderData
        {
            [LoadColumn(0), ColumnName("Date")]
            public DateTime Date { get; set; }

            [LoadColumn(1), ColumnName("Orders")]
            public float Orders { get; set; }
        }

        public class OrderPredictionTest
        {
            [ColumnName("Score")]
            public float Orders { get; set; }
        }
    }
}
