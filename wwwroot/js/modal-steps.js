let step = 1

export function nextStep() {
    step++
    changeModal()
}

export function previousStep() {
    step--
    changeModal()
}


function changeModal() {
    const modals = document.querySelectorAll(".modal-content")
    Array.from(modals).forEach(modal => {
        modal.style.display = "none"
    })

    document.querySelector(`.modal-content[data-current="${ step }"]`).style.display = "block"
}

document.addEventListener("DOMContentLoaded", () => changeModal())