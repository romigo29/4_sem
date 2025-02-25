document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("form").addEventListener("submit", function (event) {
        validateForm(event);
    });
});

function validateForm(event) {
    event.preventDefault();
    clearErrors();
    let isValid = true;

    const regName = /^[A-Za-zА-Яа-яЁё]{1,20}$/;
    const regEmail = /^[^\s@]+@[A-Za-z]{2,5}\.[A-Za-z]{2,3}$/;
    const regPhone = /^\(0\d{2}\)\d{3}-\d{2}-\d{2}$/;
    const regAbout = /^.{1,250}$/;

    let lastName = document.getElementById("last_name");
    let firstName = document.getElementById("name");
    let email = document.getElementById("email");
    let phone = document.getElementById("phone");
    let about = document.getElementById("about");

    if (!regName.test(lastName.value)) {
        showError(lastName.parentNode, "Фамилия должна содержать только буквы (до 20 символов)");
        isValid = false;
    }

    if (!regName.test(firstName.value)) {
        showError(firstName.parentNode, "Имя должно содержать только буквы (до 20 символов)");
        isValid = false;
    }

    if (!regEmail.test(email.value)) {
        showError(email.parentNode, "Неверный формат e-mail (пример: user@site.com)");
        isValid = false;
    }

    if (!regPhone.test(phone.value)) {
        showError(phone.parentNode, "Формат номера: (0xx)xxx-xx-xx");
        isValid = false;
    }

    if (!regAbout.test(about.value)) {
        showError(about.parentNode, "Текст не должен превышать 250 символов");
        isValid = false;
    }

    const courseSelected = document.querySelector(".radio-group input:checked");
    if (!courseSelected) {
        showError(document.querySelector(".radio-group"), "Выберите курс");
        isValid = false;
    }

    let city = document.getElementById("city");
    let course = document.getElementsByName("course");
    let isbstu = document.getElementById("isbstu");

    if (city.value != "Минск" || course.value != 3 || !isbstu.checked) {
        var confirmed = confirm("Вы уверены?");
    }

    if (isValid) {
        if (confirmed) {
            alert("Форма успешно отправлена!");
            document.getElementById("form").submit();
        }
        else {
            event.preventDefault();
        }
    }
}

function showError(element, message) {
    if (!element) return;
    const error = document.createElement("div");
    error.className = "error";
    error.style.color = "red";
    error.style.fontSize = "14px";
    error.textContent = message;
    element.insertAdjacentElement("afterend", error);
}

function clearErrors() {
    document.querySelectorAll(".error").forEach(error => error.remove());
}
