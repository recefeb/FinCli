const themeKey = 'theme';
const toggleButton = document.getElementById('themeToggle');

function applyTheme(theme) {
    document.documentElement.setAttribute('data-bs-theme', theme);

    if (toggleButton) {
        const label = theme === 'dark' ? 'Switch to light theme' : 'Switch to dark theme';

        toggleButton.innerHTML = theme === 'dark'
            ? '<i class="bi bi-sun"></i>'
            : '<i class="bi bi-moon-stars"></i>';
        toggleButton.setAttribute('aria-label', label);
        toggleButton.title = label;
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