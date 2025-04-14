document.addEventListener("DOMContentLoaded", () => {
    const checkAllCheckboxes = document.querySelectorAll(".checkAll")

    Array.from(checkAllCheckboxes).forEach(checkbox => {
        checkbox.addEventListener("change", function () {
            let checked = checkbox.checked
            const rows = checkbox.closest("table").querySelectorAll("tbody tr")

            rows.forEach(row => {
                row.querySelector("input[type=checkbox]").checked = checked
            })

            countRows()
        })
    })
})

export function countRows() {
    const count = document.querySelectorAll("table tbody input[type=checkbox]:checked").length
    document.querySelector(".modal-footer > p").textContent = `${count} ${count === 1 ? "equipamento selecionado" : "equipamentos selecionados"}`

    if (count > 0) {
        document.querySelector(".modal-footer > .btn-default").removeAttribute("disabled")
    } else {
        document.querySelector(".modal-footer > .btn-default").setAttribute("disabled", "disabled")
    }
}