async function getAllOrders() {
    try {
        const response = await fetch('https://localhost:7210/api/Order/GetAllOrders', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const orderTable = document.getElementById('orderTable');
            orderTable.innerHTML = '';

            data.forEach(order => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${order.orderUid}</td>
                <td>${order.userLogin}</td>
                <td>${order.productName}</td>
                <td>${order.productManufacturer}</td>
                <td>${order.productType}</td>
                <td>${order.productPrice}</td>
                <td style="text-align: center;"><input type="checkbox" value="${order.orderUid}"></td>`;
                orderTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            location.reload(true);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getUserOrders() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7210/api/Order/GetUserOrders?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const orderTable = document.getElementById('orderTable');
            orderTable.innerHTML = '';

            data.forEach(order => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${order.productName}</td>
                <td>${order.productManufacturer}</td>
                <td>${order.productType}</td>
                <td>${order.productPrice}</td>
                <td style="text-align: center;"><input name = "check" type="checkbox" value="${order.orderUid}"></td>`;
                orderTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
} 

async function getPurchasedUserOrders() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7210/api/Order/GetPurchasedUserOrders?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const orderTable = document.getElementById('orderTable');
            orderTable.innerHTML = '';

            data.forEach(order => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${order.productName}</td>
                <td>${order.productManufacturer}</td>
                <td>${order.productType}</td>
                <td>${order.productPrice}</td>
                <td style="text-align: center;"><input name = "checkPurchasesOrders" type="checkbox" value="${order.orderUid}"></td>`;
                orderTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getProductOrders() {
    const uid = document.getElementById('productUid').value;

    try {
        const response = await fetch(`https://localhost:7210/api/Order/GetProductOrders?productUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const orderTable = document.getElementById('orderTable');
            orderTable.innerHTML = '';

            data.forEach(order => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${order.orderUid}</td>
                <td>${order.userLogin}</td>
                <td>${order.productName}</td>
                <td>${order.productManufacturer}</td>
                <td>${order.productType}</td>
                <td>${order.productPrice}</td>
                <td style="text-align: center;"><input type="checkbox" value="${order.orderUid}"></td>`;
                orderTable.appendChild(row);
            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function createOrder(event) {
    const productUid = event.target.value;
    let quantity = document.getElementById('quantity-' + productUid).value;

    if (!quantity) {
        quantity = 1;
    }

    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const userUid = decodedToken.nameid;

    for (let i = 0; i < quantity; i++) {
        try {
            const response = await fetch(`https://localhost:7210/api/Order/CreateOrder?userUid=${userUid}&productUid=${productUid}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                //location.reload(true);
            } else {
                console.log(data)
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            location.reload(true);
        }
    }

    document.getElementById('quantity-' + productUid).value = '';
}


async function updateOrderStatus() {
    const selectedCheckboxes = document.querySelectorAll('#orderTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один товар');
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
            const response = await fetch(`https://localhost:7210/api/Order/UpdateOrderStatus?orderUid=${uid}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                location.reload(true);
            } else {
                console.log(data)
                throw new Error('Не удалось купить товар');
            }
        } catch (error) {
            console.error(error);
            //alert('Ошибка');
        }
    });
}

async function deleteOrder() {
    const selectedCheckboxes = document.querySelectorAll('#orderTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один заказ');
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
            const response = await fetch(`https://localhost:7210/api/Order/DeleteOrder?orderUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                if (window.location.href === 'https://localhost:7210/html/user/cart.html' || window.location.href === 'https://localhost:7210/html/user/purchases.html') {
                    console.log(data);
                    location.reload(true);
                } else if (window.location.href === 'https://localhost:7210/html/admin/order.html') {
                    console.log(data);
                    getAllOrders();
                }
            } else {
                console.log(data)
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);

        }
    });
}