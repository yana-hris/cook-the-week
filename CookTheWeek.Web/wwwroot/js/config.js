const mainContainer = document.getElementById('main-container');

export const AppConfig = {
    currentView: mainContainer.dataset.currentView || null,
    currentUserId: mainContainer.dataset.currentUserId || null,
    hasActiveMealplan: mainContainer.dataset.hasActiveMealplan === 'true'
};




