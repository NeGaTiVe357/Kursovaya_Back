async function login() {
    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    const credentials = {
        login: login,
        password: password
    };

    try {
        const response = await fetch('https://localhost:7210/api/User/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(credentials)
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('userToken', data.token);

            var decodedToken = JSON.parse(atob(data.token.split('.')[1]));
            const userRole = decodedToken.role;

            if (userRole === 'Admin') {
                window.location.href = 'admin/user.html';
            } else if (userRole === 'User') {
                window.location.href = 'user/product.html';
            } else {
                console.log(await response.text());
                alert('Произошла ошибка при авторизации');
            }
        } else {
            console.log(await response.text());
            throw new Error('Неправильный логин или пароль');
        }
    } catch (error) {
        console.error(error);
        alert('Неправильный логин или пароль');
    }
}

async function register() {
    const name = document.getElementById('name').value;
    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    if (!name || !login || !password) {
        alert('Заполните необходимые поля');
        return;
    }

    const credentials = {
        name: name,
        login: login,
        password: password
    };

    try {
        const response = await fetch('https://localhost:7210/api/User/Register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(credentials)
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('userToken', data.token);

            alert('Успешная регистрация');
            window.location.href = 'user/product.html';
        } else {
            console.log(await response.text());
            throw new Error('Ошибка регистрации');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка регистрации');
    }
}

async function updateUser() {
    const name = document.getElementById('name').value;
    const login = document.getElementById('login').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmedPassword = document.getElementById('confirmedPassword').value;

    if (!name || !login || !password || !confirmedPassword) {
        alert('Заполните необходимые поля');
        return;
    }

    const userUpdate = {
        name: name,
        login: login,
        email: email,
        password: password,
        confirmedPassword: confirmedPassword
    };

    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7210/api/User/UpdateUser?userUid=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(userUpdate)
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            alert('Данные обновлены');
            location.reload(true);
        } else {
            console.log(data);
            throw new Error('Не удалось обновить данные');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getUserInfo() {
    document.getElementById('password').value = '';
    document.getElementById('confirmedPassword').value = '';

    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7210/api/User/GetUserInfo?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        if (response.ok) {
            const data = await response.json();

            document.getElementById('name').value = data.name;
            document.getElementById('login').value = data.login;
            document.getElementById('email').value = data.email;
        } else {
            console.log(await response.text());
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function getAllUsers() {
    try {
        const response = await fetch('https://localhost:7210/api/User/GetAllUsers', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const userTable = document.getElementById('userTable');
            userTable.innerHTML = '';

            data.forEach(user => {
                let email;

                if (!user.email) {
                    email = '';
                } else {
                    email = user.email;
                }

                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${user.userUid}</td>
                <td>${user.name}</td>
                <td>${user.login}</td>
                <td>${email}</td>
                <td>${user.isAdmin}</td>
                <td style="text-align: center;"><input type="checkbox" value="${user.userUid}"></td>`;
                userTable.appendChild(row);
            });
        } else {
            console.log(await response.text());
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getSingleUser() {
    const uid = document.getElementById('userUid').value;

    if (!uid) {
        alert("Введите uid пользователя");
        return;
    }

    try {
        const response = await fetch(`https://localhost:7210/api/User/GetSingleUser?userUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const userTable = document.getElementById('userTable');
            userTable.innerHTML = '';

            let email;
            if (!data.email) {
                email = '';
            } else {
                email = data.email;
            }

            const row = document.createElement('tr');
            row.innerHTML = `
            <td>${data.userUid}</td>
            <td>${data.name}</td>
            <td>${data.login}</td>
            <td>${email}</td>
            <td>${data.isAdmin}</td>
            <td style="text-align: center;"><input type="checkbox" value="${data.userUid}"></td>`;
            userTable.appendChild(row);
        } else {
            console.log(await response.text());
            throw new Error('Что-то пошло не так');
        }

    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteUser() {
    const selectedCheckboxes = document.querySelectorAll('#userTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одного пользователя');
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
            const response = await fetch(`https://localhost:7210/api/User/DeleteUser?userUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getAllUsers();
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