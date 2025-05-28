// Открыть модальное окно
function openModal() {
  document.getElementById('modal').style.display = 'flex';
}

// Закрыть модальное окно
function closeModal() {
  document.getElementById('modal').style.display = 'none';
}

// Закрытие модального окна при клике вне контента
window.onclick = function(event) {
  const modal = document.getElementById('modal');
  if (event.target === modal) {
    closeModal();
  }
}

// Обработка отправки формы
document.getElementById('signupForm').addEventListener('submit', function(event) {
  event.preventDefault(); // Отменяем стандартное поведение формы

  // Получаем значения полей
  const phone = document.getElementById('phone').value.trim();
  const fullname = document.getElementById('fullname').value.trim();

  if (!phone || !fullname) {
    alert('Пожалуйста, заполните все поля.');
    return;
  }

  // Здесь можно добавить отправку данных на сервер через fetch/ajax

  alert(`Спасибо за запись, ${fullname}! Мы свяжемся с вами по номеру ${phone}.`);
  this.reset();
  closeModal();
});
