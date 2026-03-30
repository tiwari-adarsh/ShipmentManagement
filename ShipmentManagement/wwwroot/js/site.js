// ── Sidebar Toggle (Mobile) ─────────────────
function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('open');
}

// ── Auto-hide Alerts after 4s ───────────────
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert-toast');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            alert.style.transition = 'opacity 0.5s';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 500);
        }, 4000);
    });
});