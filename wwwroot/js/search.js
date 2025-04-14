export function countRows() {
    const accordion = document.querySelectorAll(".accordion-item")
    if (accordion == null) {
        return;
    }

    Array.from(accordion).forEach(item => {
        const rows = item.querySelectorAll("tbody tr")
        const count = Array.from(rows).filter(item => window.getComputedStyle(item).display == "table-row").length
        item.querySelector(".count-rows").textContent = count

        if (count == 0) {
            item.querySelector("table").style.display = "none"
        } else {
            item.querySelector("table").style.display = "table"
        }
    })
}

document.addEventListener("DOMContentLoaded", function () {
    const input = document.querySelector(".searchbar fieldset input")
    const tableRows = document.querySelectorAll("table tbody tr")

    input.onkeyup = () => {
        tableRows.forEach((element) => {
            let matches = 0
            element.querySelectorAll("td").forEach((value) => {
                if (value.innerText.toLowerCase().trim().includes(input.value.toLowerCase().trim())) {
                    matches++
                }
            })

            if (matches > 0) {
                element.style.display = "table-row"
            } else {
                element.style.display = "none"
            }

        })

        const quantity = Array.from(tableRows).filter(item => window.getComputedStyle(item).display == "table-row").length
        if (document.getElementById("equipmentQuantity") != null) {
            document.getElementById("equipmentQuantity").innerHTML = `Há ${ quantity } equipamentos listados`
        }
        countRows()
    }
})