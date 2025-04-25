document.addEventListener("DOMContentLoaded", () => {
    const checkAllCheckboxes = document.querySelectorAll(".checkAll")

    Array.from(checkAllCheckboxes).forEach(checkbox => {
        checkbox.addEventListener("change", function () {
            let checked = checkbox.checked
            let rows = checkbox.closest("table").querySelectorAll("tbody tr")
            rows = Array.from(rows).filter(item => window.getComputedStyle(item).display == "table-row")

            rows.forEach(row => {
                row.querySelector("input[type=checkbox]").checked = checked
            })

            countRows(checkbox.closest("table"))
        })
    })
})

export function countRows(table) {
    const count = table.querySelectorAll("tbody input[type=checkbox]:checked").length
    console.log(table.querySelectorAll("tbody input[type=checkbox]:checked"))
    document.querySelector(".modal-footer > p").textContent = `${count} ${count === 1 ? "equipamento selecionado" : "equipamentos selecionados"}`

    if (count > 0) {
        document.querySelector(".modal-footer > .btn-default").removeAttribute("disabled")
    } else {
        document.querySelector(".modal-footer > .btn-default").setAttribute("disabled", "disabled")
    }
}