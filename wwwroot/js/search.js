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

export function checkMatch(element, text) {
    let matches = 0
    element.querySelectorAll("td").forEach((value) => {
        if (value.innerText.toLowerCase().trim().includes(text.toLowerCase().trim()) && window.getComputedStyle(element).display != "none") {
            matches++
        }
    })

    if (matches > 0) {
        element.style.display = "table-row"
    } else {
        element.style.display = "none"
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const input = document.querySelector(".searchbar fieldset input")
    const tableRows = document.querySelectorAll("#container section table tbody tr")

    if (input != null) {
        input.onkeyup = () => {
            tableRows.forEach((element) => checkMatch(element, input.value))

            const quantity = Array.from(tableRows).filter(item => window.getComputedStyle(item).display == "table-row").length

            if (document.getElementById("quantityResult") != null) {
                document.getElementById("quantityResult").innerHTML = quantity
            }

            countRows()
        }
    }
})