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

    var asides = document.querySelectorAll("aside[data-color]");
    asides.forEach(function (aside) {
        var color = aside.getAttribute("data-color");
        aside.style.setProperty("--color", color);
    });

    // DELETE MODAL

    //var modal = document.getElementById("modal");
    //var btn = document.getElementById("openModal");
    //var cancelButton = document.getElementById("closeModal");

    //if (btn != null) {
    //    btn.onclick = function () {
    //        modal.style.display = "flex";
    //    }

    //    cancelButton.onclick = function () {
    //        modal.style.display = "none";
    //    }

    //    window.onclick = function (event) {
    //        if (event.target == modal) {
    //            modal.style.display = "none";
    //        }
    //    }
    //}
});