import random
import datetime
import csv

def generate_orders_csv(days=6000, filename="warehouse_orders.csv"):
    """Generates random daily orders for a toy warehouse with holiday spikes,
    saving the data to a CSV file.

    Args:
        days: Number of days to generate data for (default 6000).
        filename: Name of the CSV file to save the data (default "warehouse_orders.csv").
    """

    base_order_range = (20, 50)  # Range for random base order on non-holiday days
    spike_duration = datetime.timedelta(days=14)  # Spike duration (2 weeks)

    with open(filename, 'w', newline='') as csvfile:
        writer = csv.writer(csvfile)
        writer.writerow(["Date", "Orders"])  # Header row

        for day in range(days):
            date = datetime.date.today() - datetime.timedelta(days=days - day - 1)
            orders = random.randint(*base_order_range)  # Random base order within the range

            # Check if the current date is around Christmas, Easter, or New Year's
            if date.month == 12 and (date.day >= 15 and date.day <= 31):  # Christmas spike
                spike_multiplier = random.uniform(2.0, 5.0)
                orders = round(orders * spike_multiplier)
            elif date.month == 1 and (date.day >= 1 and date.day <= 15):  # New Year's spike
                spike_multiplier = random.uniform(1.5, 3.0)
                orders = round(orders * spike_multiplier)
            elif date.month == 4 and (date.day >= 1 and date.day <= 15):  # Easter spike
                spike_multiplier = random.uniform(1.2, 2.5)
                orders = round(orders * spike_multiplier)

            writer.writerow([date.strftime("%Y-%m-%d"), orders])

if __name__ == "__main__":
    generate_orders_csv()  # Call the function to generate and save data
