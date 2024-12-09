// Открытие модального окна
const openModalBtn = document.getElementById('openModalBtn');
const modal = document.getElementById('uploadModal');
const closeModalBtn = document.getElementById('closeModalBtn');

openModalBtn.addEventListener('click', () => {
    modal.style.display = 'block';
});

// Закрытие модального окна
closeModalBtn.addEventListener('click', () => {
    modal.style.display = 'none';
});

// Закрытие окна при клике на фон
window.addEventListener('click', (event) => {
    if (event.target === modal) {
        modal.style.display = 'none';
    }
});

// Обработка формы (пример отправки)
const uploadForm = document.getElementById('uploadForm');
uploadForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const fileInput = document.getElementById('fileInput');
    if (fileInput.files.length === 0) {
        alert('Выберите файл!');
        return;
    }
    alert(`Файл "${fileInput.files[0].name}" загружен!`);
    modal.style.display = 'none';
});