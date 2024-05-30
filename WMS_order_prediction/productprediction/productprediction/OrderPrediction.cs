using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using NPoco;

namespace OrderPrediction
{
    class OrderData
    {
        [LoadColumn(0), ColumnName("Date"), ColumnType(typeof(DateTime))]
        public DateTime Date { get; set; }

        [LoadColumn(1), ColumnName("Orders")]
        public float Orders { get; set; }
    }

    class OrderFeatures
    {
        public float Year { get; set; }
        public float Month { get; set; }
        public float DayOfWeek { get; set; }
    }

    class OrderPrediction
    {
        [ColumnName("Score")]
        public float PredictedOrders { get; set; }

        public DateTime Date { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext(seed: 0);

            string dataPath = @"C:\Users\aaron\Desktop\thesis\produc\productprediction\productprediction\orders.csv";
            IDataView dataView = mlContext.Data.LoadFromTextFile<OrderData>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = mlContext.Transforms.Conversion.ConvertType("Year", "Date", DataKind.Single)
                .Append(mlContext.Transforms.Conversion.ConvertType("Month", "Date", DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType("DayOfWeek", "Date", DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features", "Year", "Month", "DayOfWeek"))
                .Append(mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Orders"))
                .Append(mlContext.Regression.Trainers.FastTree());

            // Train model
            var model = pipeline.Fit(dataView);
            var predictions2024 = new List<OrderPrediction>();
            for (DateTime date = new DateTime(2024, 1, 1); date <= new DateTime(2024, 12, 31); date = date.AddDays(1))
            {
                var prediction = mlContext.Model.CreatePredictionEngine<OrderData, OrderPrediction>(model)
                                                .Predict(new OrderData { Date = date });
                predictions2024.Add(prediction);
            }

            // Output predictions for 2024
            foreach (var prediction in predictions2024)
            {
                Console.WriteLine($"Date: {prediction.Date.ToShortDateString()}, Predicted Orders: {prediction.PredictedOrders}");
            }
        }
    }
}
