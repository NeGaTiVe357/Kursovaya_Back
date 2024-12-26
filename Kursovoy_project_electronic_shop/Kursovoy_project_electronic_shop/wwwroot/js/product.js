async function getAllProducts() {
    try {
        const response = await fetch('https://localhost:7210/api/Product/GetAllProducts', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const productTable = document.getElementById('productTable');
            productTable.innerHTML = '';

            data.forEach(product => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${product.productUid}</td>
                <td>${product.name}</td>
                <td>${product.manufacturers}</td>
                <td>${product.types}</td>
                <td>${product.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${product.productUid}"></td>`;
                productTable.appendChild(row);
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

async function getProductsInfo() {
    try {
        const response = await fetch('https://localhost:7210/api/Product/GetProductsInfo', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const productContainer = document.getElementById('container');
            productContainer.innerHTML = '<h1 class="text-center" style="margin-top: 5%">Товары</h1><br><br>';

            let row = document.createElement('div');
            row.classList.add('row');

            data.forEach(product => {
                let card = document.createElement('div');
                card.classList.add('col-md-4', 'mb-5');

                card.innerHTML = `
                    <div class="card">
                        <img src="${product.image}" class="card-img-top" style="height: 200px; object-fit: contain;" alt="${product.name}">
                        <div class="card-body">
                            <h5 class="card-title">${product.name}</h5>
                            <p class="card-text">Производитель: ${product.manufacturers}</p>
                            <p class="card-text">Тип: ${product.types}</p>
                            <p class="card-text">Цена: ${product.price} ₽</p>
                            <input type="number" class="form-control mb-2" placeholder="Quantity" id="quantity-${product.productUid}">
                            <button class="btn btn-primary" value="${product.productUid}" id="createOrder-${product.productUid}" onClick="createOrder(event)">В корзину</button>
                        </div>
                    </div>`;

                row.appendChild(card);

                if (row.children.length === 3 || data.indexOf(product) === data.length - 1) {
                    productContainer.appendChild(row);
                    row = document.createElement('div');
                    row.classList.add('row');
                }

            });
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function createProduct() {
    const productName = document.getElementById('productName').value;
    const productPrice = document.getElementById('productPrice').value;
    const productManufacturers = document.getElementById("productManufacturers").value;
    const productTypes = document.getElementById("productTypes").value;
    const productImage = document.getElementById("productImage").value;

    if (!productName || !productPrice) {
        alert('Заполните необходимые поля');
        return;
    }

    const productInfo = {
        name: productName,
        price: productPrice,
        manufacturers: productManufacturers.split(", "),
        types: productTypes.split(", "),
        image: productImage
    };

    try {
        const response = await fetch(`https://localhost:7210/api/Product/CreateProduct`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
            body: JSON.stringify(productInfo)
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);;
            getAllProducts();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getSingleProduct() {
    const name = document.getElementById('productName').value

    if (!name) {
        alert('Введите название товара');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7210/api/Product/GetSingleProduct?productName=${name}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const productTable = document.getElementById('productTable');
            productTable.innerHTML = '';

            const row = document.createElement('tr');
            row.innerHTML = `
            <td>${data.productUid}</td>
            <td>${data.name}</td>
            <td>${data.manufacturers}</td>
            <td>${data.types}</td>
            <td>${data.price}</td>
            <td style="text-align: center;"><input type="checkbox" value="${data.productUid}"></td>`;
            productTable.appendChild(row);
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateProduct() {
    const productName = document.getElementById('productName').value;
    const productPrice = document.getElementById('productPrice').value;
    const productManufacturers = document.getElementById("productManufacturers").value;
    const productTypes = document.getElementById("productTypes").value;
    const productImage = document.getElementById("productImage").value;

    const selectedCheckboxes = document.querySelectorAll('#productTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите один товар');
        return;
    }

    if (!productName || !productPrice) {
        alert('Заполните необходимые поля');
        return;
    }

    const productInfo = {
        name: productName,
        price: productPrice,
        manufacturers: productManufacturers.split(", "),
        types: productTypes.split(", "),
        image: productImage
    };

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7210/api/Product/UpdateProduct?productUid=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(productInfo)
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getAllProducts();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить товар');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteProduct() {
    const selectedCheckboxes = document.querySelectorAll('#productTable input[type="checkbox"]:checked');

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
            const response = await fetch(`https://localhost:7210/api/Product/DeleteProduct?productUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getAllProducts();

            } else {
                console.log(data);
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    });
}