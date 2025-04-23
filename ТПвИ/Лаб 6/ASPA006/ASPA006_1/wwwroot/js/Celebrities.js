let celebrities = [];

fetch('https://localhost:7203/api/celebrities')
    .then(response => response.json())
    .then(data => {
        celebrities = data;
        const grid = document.getElementById('photoGrid');
        data.forEach(c => {
            const img = document.createElement('img');
            img.src = `/api/Celebrities/photo/${c.reqPhotoPath}`;
            img.id = c.id;
            img.alt = c.fullName;
            img.onload = function () { onPhotoLoad(this, 200, 0); };
            img.onclick = () => onPhotoClick(c.id);
            grid.appendChild(img);
        });
    })
    .catch(err => console.error("Ошибка загрузки:", err));


function onPhotoLoad(e, h, w) {

    let ratio = e.naturalWidth / e.naturalHeight;

    if (h != 0 && w == 0) {
        e.width = Math.round(ratio * h);
    }
    if (h == 0 && w != 0) {
        e.height = Math.round(w / ratio);
    }
}

function onPhotoClick(id) {

    const table = document.getElementById('celebrityTable');
    table.style.display = 'table';

    const celebrity = celebrities.find(c => c.id === id);

    if (!celebrity) {
        console.error("Знаменитость с ID", id, "не найдена");
        return;
    }

    fetch(`https://localhost:7203/api/Lifeevents/Celebrities/${id}`)
        .then(response => response.json())
        .then(data => {
            const tbody = document.getElementById('celebrityTable').querySelector('tbody');
            tbody.innerHTML = ''; 

            if (data.length === 0) {
                const row = document.createElement('tr');
                row.innerHTML = '<td colspan="3">Нет данных о событиях</td>';
                tbody.appendChild(row);
                return;
            }

            data.forEach(l => {
                const row = document.createElement('tr');

                row.innerHTML = `
                    <td>${celebrity.fullName}</td>
                    <td>${l.date ? new Date(l.date).toLocaleDateString('ru-RU') : ''}</td>
                    <td>${l.description}</td>
                `;
                tbody.appendChild(row);
            });
        })
        .catch(err => {
            console.error("Ошибка загрузки событий:", err);
            const tbody = document.getElementById('celebrityTable').querySelector('tbody');
            tbody.innerHTML = `<tr><td colspan="3">Ошибка загрузки данных: ${err.message}</td></tr>`;
        });
}


