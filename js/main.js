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