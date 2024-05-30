import csv
import random

def generate_dataset(file_path, num_items):
    with open(file_path, 'w', newline='') as csvfile:
        fieldnames = ['ItemID', 'Weight', 'Size', 'Fragility', 'storedIn', 'DestinationWarehouse']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)

        writer.writeheader()

        for i in range(num_items):
            item_id = f'Item_{i+1}'
            
            # Generate weight with a higher probability of being heavier for Warehouse A
            if random.random() < 0.7:
                weight = round(random.uniform(10, 50), 2)  
            else:
                weight = round(random.uniform(0.1, 10), 2) 
                
            size = round(random.uniform(0.1, 5), 2)
            
            storage_unit_type = random.choice(['Pallet', 'Shelf', 'Bin'])
            
            fragility = random.choices(['High', 'Medium', 'Low'], weights=[0.1, 0.3, 0.6])[0]
            
            if storage_unit_type == 'Pallet':
                destination_warehouse = 'Warehouse_A'
            elif storage_unit_type == 'Shelf':
                if weight <= 10:
                    destination_warehouse = 'Warehouse_C'
                else:
                    destination_warehouse = 'Warehouse_B'
            else:
                if weight <= 5:
                    destination_warehouse = 'Warehouse_C'
                else:
                    destination_warehouse = 'Warehouse_A'

            writer.writerow({
                'ItemID': item_id,
                'Weight': weight,
                'Size': size,
                'Fragility': fragility,
                'storedIn': storage_unit_type,
                'DestinationWarehouse': destination_warehouse
            })

if __name__ == "__main__":
    file_path = "warehouse_dataset_advanced2.csv"
    num_items = 100000 
    generate_dataset(file_path, num_items)
    print(f"Dataset generated and saved to {file_path}")
