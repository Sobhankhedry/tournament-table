$(window).scroll(function(){
    let position = $(this).scrollTop();
    if(position >= 200 ){
        $('.nav-menu').addClass('custom-navbar')
    }else{
        $('.nav-menu').removeClass('custom-navbar')
    }
}) 

const feedbacks = [
    { id: 1, name: "علی محمدی", email: "ali@example.com", feedback: "این وبسایت عالی است! من واقعاً از طراحی آن لذت می‌برم." },
    { id: 2, name: "مریم احمدی", email: "maryam@example.com", feedback: "لطفاً گزینه‌های بیشتری اضافه کنید." },
    { id: 3, name: "رضا کریمی", email: "reza@example.com", feedback: "سرعت بارگذاری صفحات می‌تواند بهتر باشد." },
];

function populateTable() {
    const tableBody = document.getElementById('feedbackTableBody');
    tableBody.innerHTML = '';

    feedbacks.forEach(feedback => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${feedback.name}</td>
            <td>${feedback.email}</td>
            <td>
                <button class="btn btn-sm text-white" onclick="showFeedback(${feedback.id})">
                    <i class="fa-solid fa-comments" style="color: #ffffff;"></i>
                </button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function showFeedback(id) {
    const feedback = feedbacks.find(f => f.id === id);
    if (feedback) {
        document.getElementById('modalFeedbackContent').textContent = feedback.feedback;
        const modal = new bootstrap.Modal(document.getElementById('feedbackModal'));
        modal.show();
    }
}

document.addEventListener('DOMContentLoaded', populateTable);


const users = [
    { id: 1, firstName: "علی", lastName: "محمدی", email: "ali@example.com", confirmed: false },
    { id: 2, firstName: "مریم", lastName: "احمدی", email: "maryam@example.com", confirmed: false },
    { id: 3, firstName: "رضا", lastName: "کریمی", email: "reza@example.com", confirmed: false },
];

function displayUsers() {
    const tableBody = document.getElementById('userTableBody');
    tableBody.innerHTML = '';

    users.forEach(user => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${user.firstName}</td>
            <td>${user.lastName}</td>
            <td>${user.email}</td>
            <td>
                <button class="btn btn-confirm ${user.confirmed ? 'confirmed btn-danger' : 'btn-primary'}" 
                        onclick="toggleConfirmUser(${user.id})">
                    ${user.confirmed ? 'لغو' : 'تایید'}
                </button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function toggleConfirmUser(userId) {
    const user = users.find(u => u.id === userId);
    if (user) {
        user.confirmed = !user.confirmed; 
        displayUsers(); 
    }
}

window.onload = displayUsers;