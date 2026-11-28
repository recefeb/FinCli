const themeKey = 'theme';
const toggleButton = document.getElementById('themeToggle');

function applyTheme(theme) {
    document.documentElement.setAttribute('data-bs-theme', theme);

    if (toggleButton) {
        toggleButton.textContent = theme === 'dark' ? 'Light' : 'Dark';
    }

    document.dispatchEvent(new Event('themechange'));
}

let savedTheme = 'light';

try {
    savedTheme = localStorage.getItem(themeKey) || 'light';
} catch (e) {
    savedTheme = 'light';
}

applyTheme(savedTheme);

if (toggleButton) {
    toggleButton.addEventListener('click', () => {
        const current = document.documentElement.getAttribute('data-bs-theme');
        const next = current === 'dark' ? 'light' : 'dark';

        applyTheme(next);

        try {
            localStorage.setItem(themeKey, next);
        } catch (e) {
        }
    });
}