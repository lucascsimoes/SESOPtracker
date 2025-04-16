document.addEventListener('DOMContentLoaded', function () {
    const path = window.location.pathname

    document.querySelectorAll('#menu button').forEach(element => element.classList.remove('current'))

    if (path == "/") {
        document.getElementById("nav-home").classList.add('current')
    } else if (path.includes('Salas')) {
        document.getElementById("nav-salas").classList.add('current')
    } else if (path.includes('Situacoes')) {
        document.getElementById("nav-situacoes").classList.add('current')
    } else if (path.includes('Relatorios')) {
        document.getElementById("nav-relatorios").classList.add('current')
    }

    // ATRIBUIR COR A LINHA DO HISTÓRICO

    var elementColored = document.querySelectorAll("[data-color]");
    elementColored.forEach(function (element) {
        var color = element.getAttribute("data-color");
        element.style.setProperty("--color", color);
    });

    const themeBtn = document.getElementById("changeTheme")
    themeBtn.addEventListener("click", () => {
        const theme = document.querySelector("html").getAttribute("data-bs-theme")
        if (theme == "light") {
            document.querySelector("html").setAttribute("data-bs-theme", "dark")
        } else {
            document.querySelector("html").setAttribute("data-bs-theme", "light")
        }
        
    })
});