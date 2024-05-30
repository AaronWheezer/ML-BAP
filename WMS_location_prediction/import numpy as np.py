import numpy as np
import pandas as pd
import random
import string


def generate_random_string(length):
    letters = string.ascii_letters
    return ''.join(random.choice(letters) for _ in range(length))


num_samples = 100000

length = np.random.uniform(10, 50, num_samples)
width = np.random.uniform(5, 30, num_samples)
height = np.random.uniform(2, 20, num_samples)

weight = np.random.uniform(0.5, 25, num_samples)

fragility = np.random.randint(1, 6, num_samples)

priority = np.random.randint(0, 2, num_samples)

contents = [generate_random_string(random.randint(5, 20))
            for _ in range(num_samples)]

temperature_sensitivity = np.random.choice(['Yes', 'No'], num_samples)

expiration_dates = ['{}-{}-{}'.format(random.randint(2023, 2025), random.randint(1, 12), random.randint(1, 28)) for _ in range(num_samples)]


value = np.random.randint(50, 2000, num_samples)

fragile_item = np.random.choice(['Yes', 'No'], num_samples)

hazardous = np.random.choice(['Yes', 'No'], num_samples)

fragrance = np.random.choice(['Yes', 'No'], num_samples)

special_handling = np.random.choice(
    ['None', 'Fragile', 'Refrigeration', 'None'], num_samples)

zones = np.random.choice(['Zone 1', 'Zone 2', 'Zone 3'], num_samples)

colors = [generate_random_string(random.randint(4, 10))
          for _ in range(num_samples)]

materials = [generate_random_string(random.randint(6, 12))
             for _ in range(num_samples)]

manufacturers = [generate_random_string(
    random.randint(8, 15)) for _ in range(num_samples)]


origin_countries = [generate_random_string(
    random.randint(5, 10)) for _ in range(num_samples)]

shelf_life = np.random.randint(1, 30, num_samples)


perishable = np.random.choice(['Yes', 'No'], num_samples)


liquid = np.random.choice(['Yes', 'No'], num_samples)

fragrance_notes = [generate_random_string(
    random.randint(4, 15)) for _ in range(num_samples)]

shapes = np.random.choice(['Rectangle', 'Round', 'Square'], num_samples)

weight_capacity = np.random.randint(1, 50, num_samples)


def determine_destination(fragility, value, contents, perishable, temperature_sensitivity, hazardous, liquid, special_handling):
    if hazardous == 'Yes':
        return 'Hazardous Materials Area'
    elif fragility >= 4 or special_handling == 'Fragile':
        return 'Storage Area'
    elif perishable == 'Yes' and temperature_sensitivity == 'Yes':
        return 'Refrigerated Area'
    elif perishable == 'Yes':
        return 'Perishable Storage Area'
    elif liquid == 'Yes':
        return 'Liquid Storage Area'
    else:
        return "Shelf"


# Apply the function to determine destinations
destinations = [determine_destination(f, v, c, p, t, h, l, s) for f, v, c, p, t, h, l, s in zip(
    fragility, value, contents, perishable, temperature_sensitivity, hazardous, liquid, special_handling)]



# Create a DataFrame to store the synthetic data
data = pd.DataFrame({
    'Parcel ID': range(1, num_samples + 1),
    'Length (cm)': length,
    'Width (cm)': width,
    'Height (cm)': height,
    'Weight (kg)': weight,
    'Fragility (1-5)': fragility,
    'Priority (0 or 1)': priority,
    'Destination': destinations,
    'Contents': contents,
    'Temperature Sensitivity': temperature_sensitivity,
    'Expiration Date': expiration_dates,
    'Value ($)': value,
    'Fragile Item': fragile_item,
    'Hazardous': hazardous,
    'Fragrance': fragrance,
    'Special Handling': special_handling,
    'Zone': zones,
    'Color': colors,
    'Material': materials,
    'Manufacturer': manufacturers,
    'Origin Country': origin_countries,
    'Shelf Life (days)': shelf_life,
    'Perishable': perishable,
    'Liquid': liquid,
    'Fragrance Notes': fragrance_notes,
    'Shape': shapes,
    'Weight Capacity (kg)': weight_capacity
})


# check if fixed position if true = location = fixed position name


# Save the synthetic data to an Excel file
data.to_excel('warehouse.xlsx', index=False)
