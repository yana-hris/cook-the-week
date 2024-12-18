$(document).ready(function () {
    const deleteModal = $('#confirmDeleteModal');
    debugger;
    if (deleteModal.length) {
        deleteModal.on('show.bs.modal', function (event) {
            const triggerBtn = event.relatedTarget;
            // Extraxt info from data-* attributes
            const controller = triggerBtn.getAttribute('data-controller');
            const action = triggerBtn.getAttribute('data-action');
            const id = triggerBtn.getAttribute('data-id');
            const returnUrl = triggerBtn.getAttribute('data-returnurl');

            let url = `/${controller}/${action}`;

            // Append id only if it exists
            if (id) {
                url += `/${id}`;
            }

            // Append returnUrl as a query parameter if it exists
            if (returnUrl) {
                url += `?returnUrl=${encodeURIComponent(returnUrl)}`;
            }

            const button = document.getElementById('confirmDeleteBtn');
            button.href = url;

        });
    }

    initializeModalFix();
    
});
// Move modals to <body> dynamically when opened
const initializeModalFix = function() {    
    document.addEventListener('show.bs.modal', function (event) {
        const modal = event.target; // The modal being opened
        if (modal && modal.parentElement !== document.body) {
            document.body.appendChild(modal); // Move modal to <body>
        }
    });
}

