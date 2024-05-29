document.addEventListener("DOMContentLoaded", (event) => {
    getWarehouse();
});


function getWarehouse() {
    const loadingBars = document.getElementById('loadingBars');
    loadingBars.innerHTML = ""
    fetch("https://localhost:60988/getWarehouseData", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        },
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            const warehouses = ['Warehouse_A', 'Warehouse_B', 'Warehouse_C'];
            const maxItemsPerWarehouse = 10;

            const loadingBars = document.getElementById('loadingBars');

            warehouses.forEach(warehouse => {
                const warehouseItems = data.filter(item => item.predictedLabel === warehouse);
                console.log(warehouseItems)
                const percentage = (warehouseItems.length / maxItemsPerWarehouse) * 100;

                const loadingBarContainer = document.createElement('div');
                loadingBarContainer.classList.add('warehouse-loading-bar');

                // Add warehouse label above the loading bar
                const warehouseLabel = document.createElement('div');
                warehouseLabel.classList.add('warehouse-label');
                warehouseLabel.innerText = warehouse;
                loadingBarContainer.appendChild(warehouseLabel);

                const loadingBar = document.createElement('div');
                loadingBar.classList.add('progress', 'mb-3');

                const progressBar = document.createElement('div');
                progressBar.classList.add('progress-bar');
                progressBar.setAttribute('role', 'progressbar');
                progressBar.setAttribute('aria-valuenow', percentage);
                progressBar.setAttribute('aria-valuemin', '0');
                progressBar.setAttribute('aria-valuemax', '100');
                progressBar.style.width = `${percentage}%`;
                progressBar.innerText = `${percentage}%`;

                loadingBar.appendChild(progressBar);
                loadingBarContainer.appendChild(loadingBar);

                loadingBars.appendChild(loadingBarContainer);
            });
        })
        .catch(error => console.error('Error fetching warehouse data:', error));
}

document.getElementById("palletForm").addEventListener("submit", function (event) {
    event.preventDefault();

    var formData = {
        weight: document.getElementById("weight").value,
        size: document.getElementById("size").value,
        fragility: document.getElementById("fragility").value,
        storedIn: document.getElementById("storedIn").value
    };

    fetch("https://localhost:60988/predict", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(formData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(data => {
            document.getElementById("result").textContent = "Pallet added successfully. Destination warehouse: " + data.predictedLabel;
            console.log(data);
            console.log(formData);
        })
        .catch(error => {
            document.getElementById("result").textContent = "Error: " + error.message;
        });
     getWarehouse();
});

