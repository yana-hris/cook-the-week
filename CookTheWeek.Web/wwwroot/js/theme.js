﻿(function () {
    // JavaScript snippet handling Dark/Light mode switching
    const getStoredTheme = () => localStorage.getItem('theme');
    const setStoredTheme = theme => localStorage.setItem('theme', theme);
    const forcedTheme = document.documentElement.getAttribute('data-bss-forced-theme');

    const getPreferredTheme = () => {

        if (forcedTheme) return forcedTheme;

        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }

        const pageTheme = document.documentElement.getAttribute('data-bs-theme');

        if (pageTheme) {
            return pageTheme;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    const isInHowItWorksView = () => {
        return document.getElementById('how-it-works') !== null;
    };

    const updateImages = theme => {
        const images = document.querySelectorAll('[data-light][data-dark]');
        images.forEach(img => {
            const newSrc = theme === 'dark' ? img.getAttribute('data-dark') : img.getAttribute('data-light');
            img.setAttribute('src', newSrc);
        });
    };

    const setTheme = theme => {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark');
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme);
        }
        if (isInHowItWorksView()) {
            updateImages(theme);
        }
    };

    

    setTheme(getPreferredTheme());

    

    const showActiveTheme = (theme, focus = false) => {
        const themeSwitchers = [].slice.call(document.querySelectorAll('.theme-switcher'));

        if (!themeSwitchers.length) return;

        document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
            element.classList.remove('active');
            element.setAttribute('aria-pressed', 'false');
        });

        for (const themeSwitcher of themeSwitchers) {

            const btnToActivate = themeSwitcher.querySelector('[data-bs-theme-value="' + theme + '"]');

            if (btnToActivate) {
                btnToActivate.classList.add('active');
                btnToActivate.setAttribute('aria-pressed', 'true');
            }
        }
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme();
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme());
        }
    });

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme());

        document.querySelectorAll('[data-bs-theme-value]')
            .forEach(toggle => {
                toggle.addEventListener('click', (e) => {
                    e.preventDefault();
                    const theme = toggle.getAttribute('data-bs-theme-value');
                    setStoredTheme(theme);
                    setTheme(theme);
                    showActiveTheme(theme);
                })
            })
    });
})();