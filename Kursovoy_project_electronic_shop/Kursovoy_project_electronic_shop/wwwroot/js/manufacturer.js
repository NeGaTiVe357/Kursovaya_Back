async function getManufacturers() {
    try {
        const response = await fetch(
            "https://localhost:7210/api/Manufacturer/GetManufacturers",
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("userToken")}`,
                },
            }
        );

        if (response.ok) {
            const data = await response.json();

            const manufacturerTable = document.getElementById("manufacturerTable");
            manufacturerTable.innerHTML = "";

            data.forEach((manufacturer) => {
                const row = document.createElement("tr");
                row.innerHTML = `
                <td>${manufacturer.manufacturerUid}</td>
                <td>${manufacturer.name}</td>
                <td style="text-align: center;"><input type="checkbox" value="${manufacturer.manufacturerUid}"></td>
            `;
                manufacturerTable.appendChild(row);
            });
        } else {
            console.log(await response.text());
            location.reload(true);
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
    }
}

async function createManufacturer() {
    const manufacturerName = document.getElementById("manufacturerName").value;

    if (!manufacturerName) {
        alert("Введите название производителя");
        return;
    }

    try {
        const response = await fetch(
            `https://localhost:7210/api/Manufacturer/CreateManufacturer?name=${manufacturerName}`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("userToken")}`,
                },
            }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getManufacturers();
        } else {
            console.log(data);
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function updateManufacturer() {
    const manufacturerName = document.getElementById("manufacturerName").value;
    const selectedCheckboxes = document.querySelectorAll(
        '#manufacturerTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length !== 1) {
        alert("Выберите одного производителя");
        return;
    }

    if (!manufacturerName) {
        alert("Введите название производителя");
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(
            `https://localhost:7210/api/Manufacturer/UpdateManufacturer?manufacturerUid=${uid}&name=${manufacturerName}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("userToken")}`,
                },
            }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getManufacturers();
        } else {
            console.log(data);
            throw new Error("Не удалось изменить производителя");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function deleteManufacturer() {
    const selectedCheckboxes = document.querySelectorAll(
        '#manufacturerTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length === 0) {
        alert("Выберите хотя бы одного производителя");
        return;
    }

    let uids = [];
    for (let i = 0; i < selectedCheckboxes.length; i++) {
        if (selectedCheckboxes[i].checked) {
            uids.push(selectedCheckboxes[i].value);
        }
    }

    uids.forEach(async (uid) => {
        try {
            const response = await fetch(
                `https://localhost:7210/api/Manufacturer/DeleteManufacturer?manufacturerUid=${uid}`,
                {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${localStorage.getItem("userToken")}`,
                    },
                }
            );

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getManufacturers();
            } else {
                console.log(data);
                throw new Error("Что-то пошло не так");
            }
        } catch (error) {
            console.error(error);
            alert("Ошибка");
        }
    });
}

