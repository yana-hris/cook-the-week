$(document).ready(function () {
    const deleteModal = $('#confirmDeleteModal');

    if (deleteModal.length) {
        deleteModal.on('show.bs.modal', function (event) {
            const triggerBtn = event.relatedTarget;
            debugger;
            // Extraxt info from data-* attributes
            const controller = triggerBtn.getAttribute('data-controller');
            const action = triggerBtn.getAttribute('data-action');
            const id = triggerBtn.getAttribute('data-id');
            const returnUrl = triggerBtn.getAttribute('data-returnurl');

            const url = `/${controller}/${action}/${id}`;

            if (returnUrl) {
                url += `?returnUrl=${encodeURIComponent(returnUrl)}`;
            }

            const button = document.getElementById('confirmDeleteBtn');
            button.href = url;

        });
    }
    
});

